using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class AddCollarParametersForm : Form
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private Collar Collar { get; set; }
        private CollarParameterFile File { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        public AddCollarParametersForm(Collar collar = null, CollarParameterFile file = null)
        {
            InitializeComponent();
            Collar = collar;
            File = file;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            LoadDefaultFormContents();  //Called before events are triggered
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Collar and File are in a different DataContext, get them in this DataContext
            if (Collar != null)
                Collar =
                    Database.Collars.FirstOrDefault(
                        c => c.CollarManufacturer == Collar.CollarManufacturer && c.CollarId == Collar.CollarId);
            if (File != null)
                File = Database.CollarParameterFiles.FirstOrDefault(f => f.FileId == File.FileId);
            //Both Collar and File can be null or non-null.  if either is non-null, then it is locked.

            IsEditor = CanEditCollar(Collar);
        }

        private bool CanEditCollar(Collar collar)
        {
            if (collar == null)
                return Database.ProjectInvestigators.Any(
                        pi =>
                        pi.Login == CurrentUser || pi.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser));
            var functions = new AnimalMovementFunctions();
            return functions.IsInvestigatorEditor(collar.Manager, CurrentUser) ?? false;
        }

        private void LoadDefaultFormContents()
        {
            LoadCollarComboBox();
            LoadFileComboBox();
            Gen3Label.Visible = Collar == null || Collar.CollarModel == "Gen3";
            Gen3PeriodTextBox.Visible = Collar == null || Collar.CollarModel == "Gen3";
            Gen3TimeUnitComboBox.Visible = Collar == null || Collar.CollarModel == "Gen3";
            ClearFileButton.Visible = Collar == null || Collar.CollarModel == "Gen3";
            Gen3TimeUnitComboBox.SelectedIndex = 0;
            FileComboBox.Size = new Size((Collar == null || Collar.CollarModel == "Gen3") ? 119 : 172, FileComboBox.Size.Height);
        }

        private void LoadCollarComboBox()
        {
            if (Collar != null)
                CollarComboBox.DataSource = new[] {Collar};
            else
                CollarComboBox.DataSource = from collar in Database.Collars
                                            where collar.Manager == CurrentUser ||
                                                  collar.ProjectInvestigator.ProjectInvestigatorAssistants.Any(
                                                      a => a.Assistant == CurrentUser)
                                            select collar;
            CollarComboBox.Enabled = Collar == null;
            CollarComboBox.SelectedItem = Collar;
        }

        private void LoadFileComboBox()
        {
            if (File != null)
                FileComboBox.DataSource = new[] { File };
            else
            {
                if (Collar == null)
                    FileComboBox.DataSource = from file in Database.CollarParameterFiles
                                              where file.Status == 'A' &&
                                                    (file.Owner == CurrentUser ||
                                                     file.ProjectInvestigator
                                                         .ProjectInvestigatorAssistants.Any(
                                                             a => a.Assistant == CurrentUser))
                                              select file;
                else
                {
                    switch (Collar.CollarModel)
                    {
                        case "Gen3":
                            FileComboBox.DataSource = from file in Database.CollarParameterFiles
                                                      where file.Format == 'B' && file.Status == 'A' &&
                                                            (file.Owner == CurrentUser ||
                                                             file.ProjectInvestigator.ProjectInvestigatorAssistants.Any(
                                                                 a => a.Assistant == CurrentUser))
                                                      select new FileItem(file.FileId, file.FileName);
                            break;
                        case "Gen4":
                            FileComboBox.DataSource = from file in Database.CollarParameterFiles
                                                      where file.Format == 'A' && file.Status == 'A' &&
                                                            (file.Owner == CurrentUser ||
                                                             file.ProjectInvestigator.ProjectInvestigatorAssistants.Any(
                                                                 a => a.Assistant == CurrentUser))
                                                      select new FileItem(file.FileId, file.FileName);
                            break;
                        default:
                            FileComboBox.DataSource = from file in Database.CollarParameterFiles
                                                      where
                                                          file.Format != 'A' && file.Format != 'B' && file.Status == 'A' &&
                                                          (file.Owner == CurrentUser ||
                                                           file.ProjectInvestigator.ProjectInvestigatorAssistants.Any(
                                                               a => a.Assistant == CurrentUser))
                                                      select new FileItem(file.FileId, file.FileName);
                            break;
                    }
                }
            }
            FileComboBox.Enabled = File == null;
            ClearFileButton.Enabled = File == null;
            BrowseButton.Enabled = File == null;
            FileComboBox.DisplayMember = "Name";
            FileComboBox.ValueMember = "FileId";
        }

        private void EnableFormControls()
        {
            if (!IsEditor)
            {
                CollarComboBox.Enabled = false;
                FileComboBox.Enabled = false;
                ClearFileButton.Enabled = false;
                BrowseButton.Enabled = false;
                StartDateTimePicker.Enabled = false;
                EndDateTimePicker.Enabled = false;
                CreateButton.Enabled = false;
                ValidationTextBox.Text = "You do not have permission to create this deployment.";
                return;
            }
            ValidateForm();
        }

        private void ValidateForm()
        {
            var error = ValidateError();
            if (error != null)
                ValidationTextBox.Text = error;
            ValidationTextBox.Visible = error != null;
            ValidationTextBox.ForeColor = Color.Red;
            CreateButton.Enabled = error == null;
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
            if (Collar == null)
                return "You must select a collar";
            //We must have a file or a period for Gen3
            var hasFile = FileComboBox.SelectedItem != null;
            if (Collar.CollarModel == "Gen3" && !hasFile && String.IsNullOrEmpty(Gen3PeriodTextBox.Text))
                return "You must provide a file or a time period";
            if (Collar.CollarModel == "Gen3" && hasFile && !String.IsNullOrEmpty(Gen3PeriodTextBox.Text))
                return "You must provide a file OR a time period, not both";
            //We must have a file or all others
            if (Collar.CollarModel != "Gen3" && !hasFile)
                return "You must provide a file for this collar";

            var start = StartDateTimePicker.Checked ? StartDateTimePicker.Value.Date.ToUniversalTime() : DateTime.MinValue;
            var end = EndDateTimePicker.Checked ? EndDateTimePicker.Value.Date.ToUniversalTime() : DateTime.MaxValue;
            if (end < start)
                return "The end date must be after the start date";

            //A collar cannot have multiple Parameters at the same time
            if (Collar.CollarParameters.Any(param =>
                                            DatesOverlap(param.StartDate ?? DateTime.MinValue,
                                                         param.EndDate ?? DateTime.MaxValue, start, end)))
                return "This collar has another set of parameters during your date range.";

            //Check Gen3 Period
            int period;
            if (Collar.CollarModel == "Gen3" &&
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
            if (Collar.CollarModel == "Gen3" && FileComboBox.SelectedItem != null)
                return "Warning: Argos data for collars with a PPF file cannot be automatically processed";
            return null;
        }

        private bool AddParameters()
        {
            int? period = String.IsNullOrEmpty(Gen3PeriodTextBox.Text)
                              ? (int?) null
                              : Int32.Parse(Gen3PeriodTextBox.Text)*
                                (((string) Gen3TimeUnitComboBox.SelectedItem) == "Hours" ? 60 : 1);
            var param = new CollarParameter
            {
                Collar = Collar,
                Gen3Period = period,
                FileId = FileComboBox.SelectedItem == null ? (int?)null : (int)FileComboBox.SelectedValue,
                StartDate = StartDateTimePicker.Checked ? StartDateTimePicker.Value.Date.ToUniversalTime() : (DateTime?)null,
                EndDate = EndDateTimePicker.Checked ? EndDateTimePicker.Value.Date.ToUniversalTime() : (DateTime?)null
            };
            Database.CollarParameters.InsertOnSubmit(param);
            if (SubmitChanges())
                return true;
            // The collar now thinks it has a parameter, deleteOnSubmit does not clear it
            LoadDataContext();
            return false;
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

        private void CollarComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFileComboBox();
            IsEditor = CanEditCollar((Collar)CollarComboBox.SelectedItem);
            EnableFormControls();
        }

        private void AddCollarParametersForm_Load(object sender, EventArgs e)
        {
            EnableFormControls();
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
            var form = new AddCollarParameterFileForm(Collar);
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

        private void CreateButton_Click(object sender, EventArgs e)
        {
            if (AddParameters())
                Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
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
