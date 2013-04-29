using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class FileDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private AnimalMovementViews DatabaseViews { get; set; }
        private string CurrentUser { get; set; }
        private CollarFile File { get; set; }
        private bool IsFileEditor { get; set; }
        private bool IsEditMode { get; set; }
        internal event EventHandler DatabaseChanged;

        internal FileDetailsForm(CollarFile file)
        {
            InitializeComponent();
            RestoreWindow();
            File = file;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpControls();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //File is in a different DataContext, get one in this DataContext
            if (File != null)
                File = Database.CollarFiles.FirstOrDefault(f => f.FileId == File.FileId);
            if (File == null)
                throw new InvalidOperationException("File Details Form not provided a valid File.");

            DatabaseViews = new AnimalMovementViews();
            var functions = new AnimalMovementFunctions();
            IsFileEditor = (functions.IsProjectEditor(File.ProjectId, CurrentUser) ?? false) ||
                           (functions.IsInvestigatorEditor(File.Owner, CurrentUser) ?? false);
        }

        private void SetUpControls()
        {
            FileIdTextBox.Text = File.FileId.ToString(CultureInfo.CurrentCulture);
            FormatTextBox.Text = File.LookupCollarFileFormat.Name;
            UserNameTextBox.Text = File.UserName;
            UploadDateTextBox.Text = File.UploadDate.ToString("g");
            FileNameTextBox.Text = File.FileName;
            SetUpCollarComboBox();
            SetUpProjectComboBox();
            SetUpOwnerComboBox();
            SetUpStatusComboBox();
            SetParentChildFiles();
            EnableControls();
            DoneCancelButton.Focus();
        }

        private void SetUpCollarComboBox()
        {
            //TODO - Correctly Limit the lists in the ComboBoxes
            CollarComboBox.DataSource = Database.Collars.Where(c => c.Manager == CurrentUser);
                //Add collars belonging to PI I assist
            CollarComboBox.SelectedItem = File.Collar;
        }

        private void SetUpProjectComboBox()
        {
            ProjectComboBox.DataSource = Database.Projects; //only projects I can edit
            ProjectComboBox.DisplayMember = "ProjectName";
            ProjectComboBox.SelectedItem = File.Project;
        }

        private void SetUpOwnerComboBox()
        {
            OwnerComboBox.DataSource = Database.ProjectInvestigators; //Give a file away to anyone.
            OwnerComboBox.DisplayMember = "Name";
            OwnerComboBox.SelectedItem = File.ProjectInvestigator;
        }

        private void SetUpStatusComboBox()
        {
            StatusComboBox.DataSource = Database.LookupFileStatus;
            StatusComboBox.DisplayMember = "Name";
            StatusComboBox.SelectedItem = File.LookupFileStatus;
        }

        private void SetParentChildFiles()
        {
            var fileHasChildren = Database.CollarFiles.Any(f => f.ParentFileId == File.FileId);
            var fileHasParent = File.ParentFileId != null;
            SourceFileButton.Visible = fileHasParent;
            SourceFileLabel.Visible = fileHasParent;
            GridViewLabel.Text = fileHasChildren ? "Files derived from this file" : "Summary of fixes in file";
            ChildFilesDataGridView.Visible = fileHasChildren;
            FixInfoDataGridView.Visible = !fileHasChildren;
            if (fileHasChildren)
            {
                var data = from file in Database.CollarFiles
                           where file.ParentFileId == File.FileId
                           select new
                               {
                                   file,
                                   file.FileId,
                                   file.FileName,
                                   Format = file.LookupCollarFileFormat.Name,
                                   Status = file.LookupFileStatus.Name,
                                   file.Collar
                               };
                ChildFilesDataGridView.DataSource = data;
                ChildFilesDataGridView.Columns[0].Visible = false;
            }
            else
            {
                var data = from fix in DatabaseViews.AnimalFixesByFiles
                           where fix.FileId == File.FileId
                           select fix;
                FixInfoDataGridView.DataSource = data;
            }
        }

        private void EnableControls()
        {
            IsEditMode = EditSaveButton.Text == "Save";
            FileNameTextBox.Enabled = IsEditMode;
            ProjectComboBox.Enabled = IsEditMode && File.ParentFile == null;
            OwnerComboBox.Enabled = IsEditMode && File.ParentFile == null;
            CollarComboBox.Enabled = IsEditMode && File.ParentFile == null && File.LookupCollarFileFormat.ArgosData != 'Y';
            StatusComboBox.Enabled = IsEditMode && (File.ParentFile == null || File.ParentFile.Status == 'A');
            EditSaveButton.Enabled = (IsFileEditor && !IsEditMode) || (IsEditMode && ValidateForm());
        }

        private bool ValidateForm()
        {
            string error = null;
            if (OwnerComboBox.SelectedItem == null && ProjectComboBox.SelectedItem == null)
                error = "One of Owner or Project must be set";
            else if (OwnerComboBox.SelectedItem != null && ProjectComboBox.SelectedItem != null)
                error = "One of Owner or Project must be empty";
            else if (CollarComboBox.SelectedItem != File.Collar && File.Status != 'I')
                error = "You cannot change the collar of an active file";
            else if (CollarComboBox.SelectedItem != File.Collar && StatusComboBox.SelectedItem != File.LookupFileStatus)
                error = "You cannot change the collar and the status at the same time";
            else if (CollarComboBox.SelectedItem == null && File.LookupCollarFileFormat.ArgosData != 'Y')
                error = "File must have a collar assignment (unless it has Argos Data)";
            else if (CollarComboBox.SelectedItem == null && File.LookupCollarFileFormat.ArgosData != 'Y')
                error = "If the parent file is inactive, the child must be inactive";
            ValidationTextBox.Text = error;
            ValidationTextBox.Visible = error != null; 
            return error == null;
        }

        private void UpdateFile()
        {
            File.FileName = FileNameTextBox.Text.NullifyIfEmpty();
            File.Project = (Project)ProjectComboBox.SelectedItem;
            File.ProjectInvestigator = (ProjectInvestigator)OwnerComboBox.SelectedItem;
            File.Collar = (Collar)CollarComboBox.SelectedItem;
            File.LookupFileStatus = (LookupFileStatus)StatusComboBox.SelectedItem;
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
            return true;
        }

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }

        private void FileDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpControls();
        }


        private void ProjectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProjectComboBox.SelectedItem != null)
                OwnerComboBox.SelectedItem = null;
            EnableControls();
        }

        private void OwnerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OwnerComboBox.SelectedItem != null)
                ProjectComboBox.SelectedItem = null;
            EnableControls();
        }

        private void CollarComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void StatusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void ShowContentsButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var form = new FileContentsForm(File.Contents.ToArray(), File.FileName);
            Cursor.Current = Cursors.Default;
            form.Show(this);
        }

        private void SourceFileButton_Click(object sender, EventArgs e)
        {
            if (File.ParentFileId == null)
                return;
            var form = new FileDetailsForm(File.ParentFile);
            form.DatabaseChanged += (o, args) => FileDataChanged();
            form.Show(this);
        }

        private void ChildFilesDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //I can't use the DataSource here, because it is an anoymous type.
            var file = (CollarFile)ChildFilesDataGridView.SelectedRows[0].Cells[0].Value;
            var form = new FileDetailsForm(file);
            form.DatabaseChanged += (o, args) => FileDataChanged();
            form.Show(this);
        }

        private void EditSaveButton_Click(object sender, EventArgs e)
        {
            //This button is not enabled unless editing is permitted 
            if (EditSaveButton.Text == "Edit")
            {
                // The user wants to edit, Enable form
                EditSaveButton.Text = "Save";
                DoneCancelButton.Text = "Cancel";
                EnableControls();
            }
            else
            {
                //User is saving
                UpdateFile();
                if (SubmitChanges())
                {
                    OnDatabaseChanged();
                    EditSaveButton.Text = "Edit";
                    DoneCancelButton.Text = "Done";
                    EnableControls();
                }
            }
        }

        private void DoneCancelButton_Click(object sender, EventArgs e)
        {
            if (DoneCancelButton.Text == "Cancel")
            {
                DoneCancelButton.Text = "Done";
                EditSaveButton.Text = "Edit";
                //Reset state from database
                LoadDataContext();
                SetUpControls();
            }
            else
            {
                Close();
            }
        }

    }
}
