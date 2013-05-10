using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class CollarParametersDetailsForm : Form
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private CollarParameter CollarParameter { get; set; }
        private bool IsEditor { get; set; }
        private bool LockCollar { get; set; }
        private bool LockFile { get; set; }
        internal event EventHandler DatabaseChanged;

        public CollarParametersDetailsForm(CollarParameter collarParameter, bool lockCollar = false,
                                           bool lockFile = false)
        {
            InitializeComponent();
            CollarParameter = collarParameter;
            LockCollar = lockCollar;
            LockFile = lockFile;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            LoadDefaultFormContents(); //Called before events are triggered
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //CollarParameter is in a different DataContext, get one in this DataContext
            if (CollarParameter != null)
                CollarParameter =
                    Database.CollarParameters.FirstOrDefault(p => p.ParameterId == CollarParameter.ParameterId);
            if (CollarParameter == null)
                throw new InvalidOperationException("Collar Parameters Form not provided a valid Collar Parameter Id.");

            var functions = new AnimalMovementFunctions();
            IsEditor = functions.IsInvestigatorEditor(CollarParameter.Collar.Manager, CurrentUser) ?? false;
        }

        private void LoadDefaultFormContents()
        {
            var showGen3 = CollarParameter.Collar.CollarModel == "Gen3" &&
                           (!LockFile || CollarParameter.CollarParameterFile == null);
            Gen3Label.Visible = showGen3;
            Gen3PeriodTextBox.Visible = showGen3;
            Gen3TimeUnitComboBox.Visible = showGen3;
            ClearFileButton.Visible = showGen3;
            FileComboBox.Size = new Size(showGen3 ? 119 : 172, FileComboBox.Size.Height);
            LoadFileComboBox();
            LoadCollarComboBox();
            LoadDatePickers();
            Gen3TimeUnitComboBox.SelectedIndex = 1; // minutes
            Gen3PeriodTextBox.Text = CollarParameter.Gen3Period.ToString();
            if (CollarParameter.Gen3Period != null && CollarParameter.Gen3Period%60 == 0) // use hours
            {
                Gen3PeriodTextBox.Text = (CollarParameter.Gen3Period/60).ToString();
                Gen3TimeUnitComboBox.SelectedIndex = 0;
            }
        }

        private void LoadFileComboBox()
        {
            if (LockFile)
                FileComboBox.DataSource = new[] {CollarParameter.CollarParameterFile};
            else
                switch (CollarParameter.Collar.CollarModel)
                {
                    case "Gen3":
                        FileComboBox.DataSource = from file in Database.CollarParameterFiles
                                                  where file.Format == 'B' && file.Status == 'A' &&
                                                        (file.Owner == CurrentUser ||
                                                         file.ProjectInvestigator.ProjectInvestigatorAssistants.Any(
                                                             a => a.Assistant == CurrentUser))
                                                  select file;
                        break;
                    case "Gen4":
                        FileComboBox.DataSource = from file in Database.CollarParameterFiles
                                                  where file.Format == 'A' && file.Status == 'A' &&
                                                        (file.Owner == CurrentUser ||
                                                         file.ProjectInvestigator.ProjectInvestigatorAssistants.Any(
                                                             a => a.Assistant == CurrentUser))
                                                  select file;
                        break;
                    default:
                        FileComboBox.DataSource = from file in Database.CollarParameterFiles
                                                  where
                                                      file.Format != 'A' && file.Format != 'B' && file.Status == 'A' &&
                                                      (file.Owner == CurrentUser ||
                                                       file.ProjectInvestigator.ProjectInvestigatorAssistants.Any(
                                                           a => a.Assistant == CurrentUser))
                                                  select file;
                        break;
                }
            FileComboBox.SelectedItem = CollarParameter.CollarParameterFile;
        }

        private void LoadCollarComboBox()
        {
            if (LockCollar)
                CollarComboBox.DataSource = new[] {CollarParameter.Collar};
            else
                CollarComboBox.DataSource = from collar in Database.Collars
                                            where (collar.Manager == CurrentUser ||
                                                   collar.ProjectInvestigator.ProjectInvestigatorAssistants.Any(
                                                       a => a.Assistant == CurrentUser)) &&
                                                  (CollarParameter.CollarParameterFile == null ||
                                                   (collar.CollarModel == "Gen4" &&
                                                    CollarParameter.CollarParameterFile.Format == 'A') ||
                                                   (collar.CollarModel == "Gen3" &&
                                                    CollarParameter.CollarParameterFile.Format == 'B') ||
                                                   (collar.CollarModel != "Gen3" && collar.CollarModel != "Gen4"))
                                            select collar;
            CollarComboBox.SelectedItem = CollarParameter.Collar;
        }

        private void LoadDatePickers()
        {
            StartDateTimePicker.Value = CollarParameter.StartDate == null
                                            ? DateTime.Now.Date
                                            : CollarParameter.StartDate.Value.ToLocalTime();
            StartDateTimePicker.Checked = CollarParameter.StartDate != null;
            StartDateTimePicker.CustomFormat = CollarParameter.StartDate != null ? "MMM-d-yyyy" : " ";
            EndDateTimePicker.Value = CollarParameter.EndDate == null
                                          ? DateTime.Now.Date
                                          : CollarParameter.EndDate.Value.ToLocalTime();
            EndDateTimePicker.Checked = CollarParameter.EndDate != null;
            EndDateTimePicker.CustomFormat = CollarParameter.EndDate != null ? "MMM-d-yyyy" : " ";
        }

        private void EnableFormControls()
        {
            SaveButton.Enabled = IsEditor;
            SaveButton.Enabled = IsEditor;
            CollarComboBox.Enabled = IsEditor && !LockCollar;
            FileComboBox.Enabled = IsEditor && !LockFile;
            StartDateTimePicker.Enabled = IsEditor;
            EndDateTimePicker.Enabled = IsEditor;
            Gen3PeriodTextBox.Enabled = IsEditor;
            ClearFileButton.Enabled = IsEditor && !LockFile;
            BrowseButton.Enabled = IsEditor && !LockFile;
            Gen3TimeUnitComboBox.Enabled = IsEditor;
            ValidateForm();
        }

        private void ValidateForm()
        {
            var error = ValidateError();
            if (error != null)
                ValidationTextBox.Text = error;
            ValidationTextBox.Visible = error != null;
            ValidationTextBox.ForeColor = Color.Red;
            FixItButton.Visible = error != null;
            SaveButton.Enabled = IsEditor && error == null && ParameterChanged();
            if (error != null)
                return;
            var warning = ValidateWarning();
            if (warning != null)
                ValidationTextBox.Text = warning;
            ValidationTextBox.Visible = warning != null;
            ValidationTextBox.ForeColor = Color.DodgerBlue;
        }

        private bool ParameterChanged()
        {
            return CollarComboBox.SelectedItem as Collar != CollarParameter.Collar ||
                   FileComboBox.SelectedItem as CollarParameterFile != CollarParameter.CollarParameterFile ||
                   (CollarParameter.StartDate == null && StartDateTimePicker.Checked) ||
                   (CollarParameter.StartDate != null &&
                    StartDateTimePicker.Value != CollarParameter.StartDate.Value.ToLocalTime()) ||
                   (CollarParameter.EndDate == null && EndDateTimePicker.Checked) ||
                   (CollarParameter.EndDate != null &&
                    EndDateTimePicker.Value != CollarParameter.EndDate.Value.ToLocalTime());
        }

        private string ValidateError()
        {
            //We must have a collar
            var collar = CollarComboBox.SelectedItem as Collar;
            if (collar == null)
                return "No collar selected.";

            //We must have a file or a period for Gen3
            var hasFile = FileComboBox.SelectedItem != null;
            if (collar.CollarModel == "Gen3" && !hasFile && String.IsNullOrEmpty(Gen3PeriodTextBox.Text))
                return "You must provide a file or a time period";
            if (collar.CollarModel == "Gen3" && hasFile && !String.IsNullOrEmpty(Gen3PeriodTextBox.Text))
                return "You must provide a file OR a time period, not both";
            //We must have a file or all others
            if (collar.CollarModel != "Gen3" && !hasFile)
                return "You must provide a file for this collar";

            var start = StartDateTimePicker.Checked ? StartDateTimePicker.Value.ToUniversalTime() : DateTime.MinValue;
            var end = EndDateTimePicker.Checked ? EndDateTimePicker.Value.ToUniversalTime() : DateTime.MaxValue;
            if (end < start)
                return "The end date must be after the start date";

            //A collar cannot have multiple Parameters at the same time
            if (collar.CollarParameters.Any(param =>
                                            param.ParameterId != CollarParameter.ParameterId &&
                                            DatesOverlap(param.StartDate ?? DateTime.MinValue,
                                                         param.EndDate ?? DateTime.MaxValue, start, end)))
                return "This collar has another set of parameters during your date range.";

            //Check Gen3 Period
            int period;
            if (CollarParameter.Collar.CollarModel == "Gen3" &&
                !String.IsNullOrEmpty(Gen3PeriodTextBox.Text) &&
                !Int32.TryParse(Gen3PeriodTextBox.Text, out period))
                return "The time period must be a whole number";
            return null;
        }

        private static bool DatesOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            //touching is not considered overlapping.
            return (start2 < end1 && start1 < end2);
        }

        private string ValidateWarning()
        {
            var collar = (Collar) CollarComboBox.SelectedItem;
            if (collar.CollarModel == "Gen3" && FileComboBox.SelectedItem != null)
                return "Warning: Argos data for collars with a PPF file cannot be automatically processed";
            return null;
        }

        private bool UpdateParameters()
        {
            CollarParameter.Collar = (Collar)CollarComboBox.SelectedItem;
            CollarParameter.CollarParameterFile = (CollarParameterFile)FileComboBox.SelectedItem;
            CollarParameter.Gen3Period = String.IsNullOrEmpty(Gen3PeriodTextBox.Text)
                                             ? (int?) null
                                             : Int32.Parse(Gen3PeriodTextBox.Text)*
                                               (((string) Gen3TimeUnitComboBox.SelectedItem) == "Hours" ? 60 : 1);
            CollarParameter.StartDate = StartDateTimePicker.Checked
                                            ? StartDateTimePicker.Value.ToUniversalTime()
                                            : (DateTime?) null;
            CollarParameter.EndDate = EndDateTimePicker.Checked
                                          ? EndDateTimePicker.Value.ToUniversalTime()
                                          : (DateTime?) null;
            return SubmitChanges();
        }

        private bool SubmitChanges()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Database.SubmitChanges();
            }
            catch (SqlException ex)
            {
                string msg = "Unable to submit changes to the database.\n" +
                             "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            OnDatabaseChanged();
            return true;
        }

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }

        #region Form Control Events

        private void CollarParametersDetailsForm_Load(object sender, EventArgs e)
        {
            EnableFormControls();
        }

        private void CollarComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void FileComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void ClearFileButton_Click(object sender, EventArgs e)
        {
            FileComboBox.SelectedItem = null;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            var form = new AddCollarParameterFileForm();
            form.DatabaseChanged += (o, args) =>
                {
                    OnDatabaseChanged();
                    LoadDataContext();
                    LoadDefaultFormContents();
                };
            form.Show(this);
        }

        private void Gen3PeriodTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void StartDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            StartDateTimePicker.CustomFormat = StartDateTimePicker.Checked ? "MMM-d-yyyy" : " ";
            ValidateForm();
        }

        private void EndDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            EndDateTimePicker.CustomFormat = EndDateTimePicker.Checked ? "MMM-d-yyyy" : " ";
            ValidateForm();
        }

        private void FixItButton_Click(object sender, EventArgs e)
        {
            //TODO - implement FixIt code
            MessageBox.Show("You must fix it manually", "Not Implemented");
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (UpdateParameters())
                Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

    }
}
