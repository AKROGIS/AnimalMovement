using System;
using System.Data.SqlClient;
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
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        public AddCollarParametersForm(Collar collar)
        {
            InitializeComponent();
            Collar = collar;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            LoadDefaultFormContents();  //Called before events are triggered
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Collar is in a different data context, get one in this Datacontext
            if (Collar != null)
                Collar =
                    Database.Collars.FirstOrDefault(
                        c => c.CollarManufacturer == Collar.CollarManufacturer && c.CollarId == Collar.CollarId);
            if (Collar == null)
                throw new InvalidOperationException("Add Collar Parameters Form not provided a valid Collar.");

            //Todo - put check in database function to get assistants as well
            IsEditor = string.Equals(Collar.Manager.Normalize(), CurrentUser.Normalize(),
                                     StringComparison.OrdinalIgnoreCase);
        }

        private void LoadDefaultFormContents()
        {
            CollarTextBox.Text = Collar.ToString();
            Gen3Label.Visible = Collar.CollarModel == "Gen3";
            Gen3PeriodTextBox.Visible = Collar.CollarModel == "Gen3";
            Gen3TimeUnitComboBox.Visible = Collar.CollarModel == "Gen3";
            ClearFileButton.Visible = Collar.CollarModel == "Gen3";
            Gen3TimeUnitComboBox.SelectedIndex = 0;
            FileComboBox.Size = new System.Drawing.Size(Collar.CollarModel == "Gen3" ? 119 : 172, FileComboBox.Size.Height);
            LoadFileComboBox();
            if (Collar.CollarModel == "Gen3")
                FileComboBox.SelectedItem = null;
        }

        private void LoadFileComboBox()
        {
            //TODO - We can use anyones parameter file, but should we limit the list to just ours?
            //TODO - should we show the inactive files?
            IQueryable<FileItem> fileQuery;
            switch (Collar.CollarModel)
            {
                case "Gen3":
                    fileQuery = from file in Database.CollarParameterFiles
                                where file.Format == 'B' && file.Status == 'A'
                                select new FileItem(file.FileId, file.FileName);
                    break;
                case "Gen4":
                    //TODO limit to TPF files with this collar
                    fileQuery = from file in Database.CollarParameterFiles
                                where file.Format == 'A' && file.Status == 'A'
                                select new FileItem(file.FileId, file.FileName);
                    break;
                default:
                    fileQuery = from file in Database.CollarParameterFiles
                                where file.Format != 'A' && file.Format != 'B' && file.Status == 'A'
                                select new FileItem(file.FileId, file.FileName);
                    break;
            }
            FileComboBox.DataSource = fileQuery.ToList();
            FileComboBox.DisplayMember = "Name";
            FileComboBox.ValueMember = "FileId";
        }

        private void EnableFormControls()
        {
            if (!IsEditor)
            {
                CreateButton.Enabled = false;
                FileComboBox.Enabled = false;
                StartDateTimePicker.Enabled = false;
                EndDateTimePicker.Enabled = false;
                ValidationTextBox.Text = "You do not have permission to edit this collar.";
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
            CreateButton.Enabled = error == null;
            FixItButton.Visible = error != null;
        }

        private string ValidateError()
        {
            //We must have a file or a period for Gen3
            var hasFile = FileComboBox.SelectedItem != null;
            if (Collar.CollarModel == "Gen3" && !hasFile && String.IsNullOrEmpty(Gen3PeriodTextBox.Text))
                return "You must provide a file or a time period for this collar";
            //We must have a file or all others
            if (Collar.CollarModel != "Gen3" && !hasFile)
                return "You must provide a file for this collar";

            var start = StartDateTimePicker.Checked ? StartDateTimePicker.Value.ToUniversalTime() : DateTime.MinValue;
            var end = EndDateTimePicker.Checked ? EndDateTimePicker.Value.ToUniversalTime() : DateTime.MaxValue;
            if (end < start)
                return "The end date must be after the start date";

            //A collar cannot have multiple Parameters at the same time
            if (Collar.CollarParameters.Any(param =>
                                            DatesOverlap(param.StartDate ?? DateTime.MinValue,
                                                         param.EndDate ?? DateTime.MaxValue, start, end)))
                return "This collar has another set of parameters during your date range.";

            //TODO - for Gen4 TPF files, validate start date and collar match

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

        private bool AddParameters()
        {
            //TODO add warning about PPF files
            int? period = String.IsNullOrEmpty(Gen3PeriodTextBox.Text)
                              ? (int?) null
                              : Int32.Parse(Gen3PeriodTextBox.Text)*
                                (((string) Gen3TimeUnitComboBox.SelectedItem) == "Hours" ? 60 : 1);
            var param = new CollarParameter
            {
                Collar = Collar,
                Gen3Period = period,
                FileId = FileComboBox.SelectedItem == null ? (int?)null : (int)FileComboBox.SelectedValue,
                StartDate = StartDateTimePicker.Checked ? StartDateTimePicker.Value.ToUniversalTime() : (DateTime?)null,
                EndDate = EndDateTimePicker.Checked ? EndDateTimePicker.Value.ToUniversalTime() : (DateTime?)null
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
            //TODO - implement FIxIt code
            MessageBox.Show("Not Implemented Yet.");
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
