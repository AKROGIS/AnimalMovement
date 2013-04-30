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
        private bool IsEditMode { get; set; }
        private bool LockCollar { get; set; }
        private bool LockFile { get; set; }
        internal event EventHandler DatabaseChanged;

        public CollarParametersDetailsForm(CollarParameter collarParameter, bool lockCollar = false, bool lockFile = false)
        {
            InitializeComponent();
            CollarParameter = collarParameter;
            LockCollar = lockCollar;
            LockFile = lockFile;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            LoadDefaultFormContents();  //Called before events are triggered
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
            Gen3Label.Visible = CollarParameter.Collar.CollarModel == "Gen3";
            Gen3PeriodTextBox.Visible = CollarParameter.Collar.CollarModel == "Gen3";
            Gen3TimeUnitComboBox.Visible = CollarParameter.Collar.CollarModel == "Gen3";
            ClearFileButton.Visible = CollarParameter.Collar.CollarModel == "Gen3";
            FileComboBox.Size = new System.Drawing.Size(CollarParameter.Collar.CollarModel == "Gen3" ? 119 : 172, FileComboBox.Size.Height);
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
            IQueryable<FileItem> fileQuery;
            switch (CollarParameter.Collar.CollarModel)
            {
                case "Gen3":
                    fileQuery = from file in Database.CollarParameterFiles
                                where file.Format == 'B' && file.Status == 'A' &&
                                      (file.Owner == CurrentUser ||
                                       file.ProjectInvestigator.ProjectInvestigatorAssistants.Any(
                                           a => a.Assistant == CurrentUser))
                                select new FileItem(file.FileId, file.FileName);
                    break;
                case "Gen4":
                    //TODO limit to TPF files with this collar
                    fileQuery = from file in Database.CollarParameterFiles
                                where file.Format == 'A' && file.Status == 'A' &&
                                      (file.Owner == CurrentUser ||
                                       file.ProjectInvestigator.ProjectInvestigatorAssistants.Any(
                                           a => a.Assistant == CurrentUser))
                                select new FileItem(file.FileId, file.FileName);
                    break;
                default:
                    fileQuery = from file in Database.CollarParameterFiles
                                where file.Format != 'A' && file.Format != 'B' && file.Status == 'A' &&
                                      (file.Owner == CurrentUser ||
                                       file.ProjectInvestigator.ProjectInvestigatorAssistants.Any(
                                           a => a.Assistant == CurrentUser))
                                select new FileItem(file.FileId, file.FileName);
                    break;
            }
            FileComboBox.DataSource = fileQuery;
            FileComboBox.DisplayMember = "Name";
            FileComboBox.ValueMember = "FileId";
            if (CollarParameter.FileId == null)
                FileComboBox.SelectedItem = null;
            else
                FileComboBox.SelectedValue = CollarParameter.FileId;
        }

        private void LoadCollarComboBox()
        {
            var collarQuery = from collar in Database.Collars
                              where collar.Manager == CollarParameter.Collar.Manager
                              select collar;
            var collars = collarQuery.ToList();
            CollarComboBox.DataSource = collars;
            CollarComboBox.SelectedItem =
                collars.First(c =>
                              c.CollarManufacturer == CollarParameter.Collar.CollarManufacturer &&
                              c.CollarId == CollarParameter.Collar.CollarId);
        }

        private void LoadDatePickers()
        {
            StartDateTimePicker.Value = CollarParameter.StartDate == null ? DateTime.Now.Date : CollarParameter.StartDate.Value.ToLocalTime();
            StartDateTimePicker.Checked = CollarParameter.StartDate != null;
            StartDateTimePicker.CustomFormat = CollarParameter.StartDate != null ? "MMM-d-yyyy" : " ";
            EndDateTimePicker.Value = CollarParameter.EndDate == null ? DateTime.Now.Date : CollarParameter.EndDate.Value.ToLocalTime();
            EndDateTimePicker.Checked = CollarParameter.EndDate != null;
            EndDateTimePicker.CustomFormat = CollarParameter.EndDate != null ? "MMM-d-yyyy" : " ";
        }

        private void EnableFormControls()
        {
            EditSaveButton.Enabled = IsEditor;
            IsEditMode = EditSaveButton.Text == "Save";
            EditSaveButton.Enabled = IsEditMode;
            CollarComboBox.Enabled = IsEditMode && !LockCollar;
            FileComboBox.Enabled = IsEditMode && !LockFile;
            StartDateTimePicker.Enabled = IsEditMode;
            EndDateTimePicker.Enabled = IsEditMode;
            Gen3PeriodTextBox.Enabled = IsEditMode;
            ClearFileButton.Enabled = IsEditMode;
            BrowseButton.Enabled = IsEditMode;
            Gen3TimeUnitComboBox.Enabled = IsEditMode;
            ValidateForm();
        }

        private void ValidateForm()
        {
            var error = ValidateError();
            if (error != null)
                ValidationTextBox.Text = error;
            ValidationTextBox.Visible = error != null;
            ValidationTextBox.ForeColor = Color.Red;
            EditSaveButton.Enabled = (IsEditMode && error == null) || (!IsEditMode && IsEditor);
            FixItButton.Visible = error != null;
            if (error != null)
                return;
            var warning = ValidateWarning();
            if (warning != null)
                ValidationTextBox.Text = warning;
            ValidationTextBox.Visible = warning != null;
            ValidationTextBox.ForeColor = Color.DodgerBlue;
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
            var collar = (Collar)CollarComboBox.SelectedItem;
            if (collar.CollarModel == "Gen3" && FileComboBox.SelectedItem != null)
                return "Warning: Argos data for collars with a PPF file cannot be automatically processed";

            //TODO - for Gen4 TPF files, validate start date and collar match

            return null;
        }

        private bool UpdateParameters()
        {
            var newCollar = (Collar)CollarComboBox.SelectedItem;
            if (CollarParameter.Collar.CollarManufacturer != newCollar.CollarManufacturer || CollarParameter.Collar.CollarId != newCollar.CollarId)
                CollarParameter.Collar = newCollar;

            var newFile = FileComboBox.SelectedItem == null ? null : Database.CollarParameterFiles.FirstOrDefault(f => f.FileId == (int)FileComboBox.SelectedValue);
            if ((newFile == null && CollarParameter.FileId != null) || (newFile != null && CollarParameter.FileId != newFile.FileId))
                CollarParameter.CollarParameterFile = newFile;

            int? period = String.IsNullOrEmpty(Gen3PeriodTextBox.Text)
                              ? (int?) null
                              : Int32.Parse(Gen3PeriodTextBox.Text)*
                                (((string) Gen3TimeUnitComboBox.SelectedItem) == "Hours" ? 60 : 1);
            CollarParameter.Gen3Period = period;

            CollarParameter.StartDate = StartDateTimePicker.Checked ? StartDateTimePicker.Value.ToUniversalTime() : (DateTime?) null;
            CollarParameter.EndDate = EndDateTimePicker.Checked ? EndDateTimePicker.Value.ToUniversalTime() : (DateTime?) null;

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
            //TODO - Specify that we only want PPF or TPF files
            //The add happens in a new context, so we need to reload this context if changes were made
            var form = new AddCollarParameterFileForm(CurrentUser);
            form.DatabaseChanged += (o, args) => { OnDatabaseChanged(); LoadDataContext(); LoadDefaultFormContents(); };
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

        private void EditSaveButton_Click(object sender, EventArgs e)
        {
            //This button is not enabled unless editing is permitted 
            if (EditSaveButton.Text == "Edit")
            {
                // The user wants to edit, Enable form
                EditSaveButton.Text = "Save";
                DoneCancelButton.Text = "Cancel";
                EnableFormControls();
            }
            else
            {
                if (UpdateParameters())
                {
                    EditSaveButton.Text = "Edit";
                    DoneCancelButton.Text = "Done";
                    EnableFormControls();
                }
            }
        }

        private void DoneCancelButton_Click(object sender, EventArgs e)
        {
            if (DoneCancelButton.Text == "Cancel")
            {
                DoneCancelButton.Text = "Done";
                EditSaveButton.Text = "Edit";
                EnableFormControls();
                //Reset state from database
                LoadDefaultFormContents();
            }
            else
            {
                Close();
            }
        }

        #endregion

        private struct FileItem
        {
            public FileItem(int fileId, string name)
                : this()
            {
                FileId = fileId;
                Name = name;
            }
            // ReSharper disable MemberCanBePrivate.Local
            // ReSharper disable UnusedAutoPropertyAccessor.Local
            // Members are accessed by reflection in ThreadExceptionDialog FileComboBox
            public int FileId { get; private set; }
            public string Name { get; private set; }
            // ReSharper restore MemberCanBePrivate.Local
            // ReSharper restore UnusedAutoPropertyAccessor.Local
        }

    }
}
