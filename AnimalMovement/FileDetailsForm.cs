using DataModel;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

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
            {
                File = Database.CollarFiles.FirstOrDefault(f => f.FileId == File.FileId);
            }

            if (File == null)
            {
                throw new InvalidOperationException("File Details Form not provided a valid File.");
            }

            DatabaseViews = new AnimalMovementViews();
            var functions = new AnimalMovementFunctions();
            IsFileEditor = (functions.IsProjectEditor(File.ProjectId, CurrentUser) ?? false) ||
                           (functions.IsInvestigatorEditor(File.Owner, CurrentUser) ?? false);
        }

        private void SetUpControls()
        {
            SetUpHeaderControls();
            SetTabVisibility();
            EnableControls();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoneCancelButton.Focus();
            FileTabControl_SelectedIndexChanged(null, null);
        }


        private void SetTabVisibility()
        {
            //TabControl does not support setting TabPage visibility, so we will remove invisible pages
            var fileCouldHaveChildren = File.LookupCollarFileFormat.ArgosData == 'Y' || File.Format == 'H';
            var fileHasChildren = Database.CollarFiles.Any(f => f.ParentFileId == File.FileId);
            var fileHasArgosFixes = Database.CollarFixes.Any(f => f.CollarFile == File && !f.Collar.HasGps);

            FileTabControl.TabPages.Clear();
            if (File.LookupCollarFileFormat.ArgosData == 'Y')
            {
                FileTabControl.TabPages.AddRange(new[] { ArgosTabPage });
            }

            if (!fileHasChildren && File.Collar != null && File.Collar.HasGps)
            {
                FileTabControl.TabPages.AddRange(new[] { GpsFixesTabPage });
            }

            if (fileHasArgosFixes)
            {
                FileTabControl.TabPages.AddRange(new[] { ArgosFixesTabPage });
            }

            if (fileCouldHaveChildren)
            {
                FileTabControl.TabPages.AddRange(new[] { ProcessingIssuesTabPage, DerivedFilesTabPage });
            }
        }

        private void EnableControls()
        {
            IsEditMode = EditSaveButton.Text == "Save";
            SourceFileButton.Enabled = !IsEditMode;
            FileNameTextBox.Enabled = IsEditMode;
            ProjectComboBox.Enabled = IsEditMode && File.ParentFile == null;
            OwnerComboBox.Enabled = IsEditMode && File.ParentFile == null;
            CollarComboBox.Enabled = IsEditMode && File.ParentFile == null && File.LookupCollarFileFormat.ArgosData != 'Y';
            StatusComboBox.Enabled = IsEditMode && (File.ParentFile == null || File.ParentFile.Status == 'A');
            EditSaveButton.Enabled = (IsFileEditor && !IsEditMode) || (IsEditMode && ValidateForm());
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
            DatabaseChanged?.Invoke(this, EventArgs.Empty);
        }

        private void FileTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FileTabControl.TabCount == 0)
            {
                return;
            }

            switch ((string)FileTabControl.SelectedTab.Tag)
            {
                case "Argos":
                    SetUpArgosTab();
                    break;
                case "GpsFixes":
                    SetUpGpsFixesTab();
                    break;
                case "ArgosFixes":
                    SetUpArgosFixesTab();
                    break;
                case "DerivedFiles":
                    SetUpDerivedFilesTab();
                    break;
                case "ProcessingIssues":
                    SetUpProcessingIssuesTab();
                    break;
            }
        }


        #region File Header Controls

        private void SetUpHeaderControls()
        {
            FileIdTextBox.Text = File.FileId.ToString(CultureInfo.CurrentCulture);
            FormatTextBox.Text = File.LookupCollarFileFormat.Name;
            UserNameTextBox.Text = File.UserName;
            UploadDateTextBox.Text = File.UploadDate.ToString("g");
            FileNameTextBox.Text = File.FileName;
            SetUpCollarComboBox();
            SetUpProjectAndOwnerComboBoxes();
            SetUpStatusComboBox();
            SourceFileButton.Visible = File.ParentFileId != null;
            SourceFileLabel.Visible = File.ParentFileId != null;
        }

        private void SetUpCollarComboBox()
        {
            //Show only my collars and collars that belonging to PI that I can assist
            CollarComboBox.DataSource = from c in Database.Collars
                                        where c == File.Collar ||
                                              c.Manager == CurrentUser ||
                                              c.ProjectInvestigator.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser)
                                        select c;
            CollarComboBox.SelectedItem = File.Collar;
        }

        private void SetUpProjectAndOwnerComboBoxes()
        {
            //I have to do these together because Setting the datasource triggers selectedindex changed, so the last one wins

            //Show only my projects and projects that I can edit and projects belonging to PIs I can assist.
            ProjectComboBox.DataSource = from p in Database.Projects
                                         where p == File.Project ||
                                               p.ProjectInvestigator == CurrentUser ||
                                               p.ProjectEditors.Any(u => u.Editor == CurrentUser) ||
                                               p.ProjectInvestigator1.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser)
                                         select p;
            ProjectComboBox.DisplayMember = "ProjectName";

            // Show me (if I'm a PI), and any PIs that I can assist
            OwnerComboBox.DataSource = from pi in Database.ProjectInvestigators
                                       where pi == File.ProjectInvestigator ||
                                             pi.Login == CurrentUser ||
                                             pi.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser)
                                       select pi;
            OwnerComboBox.DisplayMember = "Name";

            if (File.Project == null)
            {
                ProjectComboBox.SelectedItem = null;
                OwnerComboBox.SelectedItem = File.ProjectInvestigator;
            }
            else
            {
                OwnerComboBox.SelectedItem = null;
                ProjectComboBox.SelectedItem = File.Project;
            }
        }

        private void SetUpStatusComboBox()
        {
            StatusComboBox.DataSource = Database.LookupFileStatus;
            StatusComboBox.DisplayMember = "Name";
            StatusComboBox.SelectedItem = File.LookupFileStatus;
        }

        private bool ValidateForm()
        {
            string error = null;
            if (OwnerComboBox.SelectedItem == null && ProjectComboBox.SelectedItem == null)
            {
                error = "One of Owner or Project must be set";
            }
            else if (OwnerComboBox.SelectedItem != null && ProjectComboBox.SelectedItem != null)
            {
                error = "One of Owner or Project must be empty";
            }
            else if (CollarComboBox.SelectedItem != File.Collar && File.Status != 'I')
            {
                error = "You cannot change the collar of an active file";
            }
            else if (CollarComboBox.SelectedItem != File.Collar && StatusComboBox.SelectedItem != File.LookupFileStatus)
            {
                error = "You cannot change the collar and the status at the same time";
            }
            else if (CollarComboBox.SelectedItem == null && File.LookupCollarFileFormat.RequiresCollar == 'Y')
            {
                error = "This kind of file must have a collar assignment";
            }

            ValidationTextBox.Text = error;
            ValidationTextBox.Visible = error != null;
            return error == null;
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
            {
                OwnerComboBox.SelectedItem = null;
            }

            EnableControls();
        }

        private void OwnerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OwnerComboBox.SelectedItem != null)
            {
                ProjectComboBox.SelectedItem = null;
            }

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
            {
                return;
            }

            var form = new FileDetailsForm(File.ParentFile);
            form.DatabaseChanged += (o, args) => FileDataChanged();
            form.Show(this);
        }

        private void ChildFilesDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Do not open (and potentially edit) a child file if we are editing the parent
            if (IsEditMode)
            {
                return;
            }
            //I can't use the DataSource here, because it is an anoymous type.
            var file = (CollarFile)DerivedFilesDataGridView.SelectedRows[0].Cells[0].Value;
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

        #endregion


        #region Argos Tab

        private void SetUpArgosTab()
        {
            var query = from argos in Database.ArgosFilePlatformDates
                        where argos.CollarFile == File
                        select new { ArgosId = argos.PlatformId, argos.FirstTransmission, argos.LastTransmission };
            ArgosPlatformsDataGridView.DataSource = query;
        }

        #endregion


        #region GPS Fixes Tab

        private void SetUpGpsFixesTab()
        {
            var data = from fix in DatabaseViews.AnimalFixesByFiles
                       where fix.FileId == File.FileId
                       select fix;
            GpsFixesDataGridView.DataSource = data;
        }

        #endregion


        #region Argos Fixes Tab

        private void SetUpArgosFixesTab()
        {
            var data = from fix in DatabaseViews.AnimalFixesByFiles
                       where fix.FileId == File.FileId
                       select fix;
            ArgosFixesDataGridView.DataSource = data;
        }

        #endregion


        #region Derived Files

        private void SetUpDerivedFilesTab()
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
            DerivedFilesDataGridView.DataSource = data;
            DerivedFilesDataGridView.Columns[0].Visible = false;
        }

        #endregion


        #region Processing Issues Tab

        private void SetUpProcessingIssuesTab()
        {
            ProcessingIssuesDataGridView.DataSource =
                Database.ArgosFileProcessingIssues.Where(i => i.CollarFile == File)
                        .Select(i => new { i.Issue, ArgosId = i.PlatformId, i.Collar });
        }

        #endregion

    }
}
