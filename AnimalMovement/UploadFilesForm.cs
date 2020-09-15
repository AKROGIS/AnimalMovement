using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DataModel;
using FileLibrary;

namespace AnimalMovement
{
    internal partial class UploadFilesForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        //private string ProjectId { get; set; }
        private Project Project { get; set; }
        private ProjectInvestigator Investigator { get; set; }
        private bool CollarIsRequired { get; set; }
        internal event EventHandler DatabaseChanged;


        internal UploadFilesForm(Project project = null, ProjectInvestigator pi = null)
        {
            Project = project;
            Investigator = pi;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            InitializeComponent();
            RestoreWindow();
            LoadDataContext();
            SetUpForm();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Project and Investigator are in a different DataContext, get them in this DataContext
            if (Project != null)
                Project = Database.Projects.FirstOrDefault(p => p.ProjectId == Project.ProjectId);
            if (Investigator != null)
                Investigator = Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == Investigator.Login);
            if (Project == null && Investigator == null)
                // Try the current user as the PI
                Investigator = Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == CurrentUser);
            // legal to open the form without a project or PI, but cannot upload unless one is specified
            if (Project != null && Investigator != null)
                throw new InvalidOperationException("Upload Files Form cannot have both a valid Project AND Project Investigator.");

            //Permission to upload a file is dependent on the project or PI that this fill will upload to.
            //The picklist will only show the allowable.  IF there are no allowable the upload button will be disabled.
        }

        private void SetUpForm()
        {
            LoadProjectList();
            LoadInvestigatorList();
            FileRadioButton.Checked = true;
            ProjectRadioButton.Checked = Project != null;
            InvestigatorRadioButton.Checked = !ProjectRadioButton.Checked;
            EnableUpload();
        }

        private void LoadProjectList()
        {
            var projects = from p in Database.Projects
                           where p.ProjectInvestigator == CurrentUser ||
                                 p.ProjectEditors.Any(u => u.Editor == CurrentUser)
                           select p;
            ProjectComboBox.DataSource = projects;
            ProjectComboBox.DisplayMember = "ProjectName";
            ProjectComboBox.SelectedItem = Project;
        }

        private void LoadInvestigatorList()
        {
            var investigators = from pi in Database.ProjectInvestigators
                                where pi.Login == CurrentUser ||
                                      pi.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser)
                                select pi;
            InvestigatorComboBox.DataSource = investigators;
            InvestigatorComboBox.DisplayMember = "Name";
            InvestigatorComboBox.SelectedItem = Investigator;
        }

        private void EnableUpload()
        {
            UploadButton.Text = "Upload";
            var haveDataSource = (FileRadioButton.Checked && !String.IsNullOrEmpty(FileNameTextBox.Text)) ||
                                 (FolderRadioButton.Checked && !String.IsNullOrEmpty(FolderNameTextBox.Text));
            var haveAssociation = (ProjectRadioButton.Checked && ProjectComboBox.SelectedItem != null) ||
                                  (InvestigatorRadioButton.Checked && InvestigatorComboBox.SelectedItem != null);
            var haveCollar = CollarComboBox.SelectedItem != null || !CollarIsRequired;
            UploadButton.Enabled = haveDataSource && haveAssociation && haveCollar;
        }

        private void RefreshCollarComboBox()
        {
            var currentCollar = CollarComboBox.SelectedItem;
            if (ProjectRadioButton.Checked)
            {
                var project = (Project) ProjectComboBox.SelectedItem;
                CollarComboBox.DataSource = from collar in Database.Collars
                                            where collar.ProjectInvestigator == project.ProjectInvestigator1
                                            select collar;
            }
            else
            {
                var investigator = (ProjectInvestigator)InvestigatorComboBox.SelectedItem;
                CollarComboBox.DataSource = from collar in Database.Collars
                                            where collar.ProjectInvestigator == investigator
                                            select collar;
            }
            CollarComboBox.SelectedItem = currentCollar;
        }

        private void UploadPath(string path)
        {
            UploadButton.Text = "Working...";
            UploadButton.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            //TODO - Revise Fileloader to use events we can subscribe to - this will require using instances instead of a static method
            //TODO - Revise Fileloader to have hooks that can allow it to be canceled.
            //TODO - Run FileLoader on a background thread
            //TODO - Display a progress bar updated by events from the FileLoader
            FileLoader.LoadPath(path, HandleException,
                                ProjectRadioButton.Checked ? (Project) ProjectComboBox.SelectedItem : null,
                                InvestigatorRadioButton.Checked
                                    ? (ProjectInvestigator) InvestigatorComboBox.SelectedItem
                                    : null,
                                (Collar) CollarComboBox.SelectedItem,
                                StatusActiveRadioButton.Checked ? 'A' : 'I', AllowDuplicatesCheckBox.Checked);

            //TODO - OnDatabaseChanged() should be called in response to a FileLoader FinishedLoading event
            //for now, we will assume that at least one file loaded and we need to update the caller
            OnDatabaseChanged();

            //TODO - This should not be done here, but in some FileLoader Done event
            Cursor.Current = Cursors.Default;
            UploadButton.Enabled = true;
            UploadButton.Text = "Close";
        }

        private static void HandleException(Exception ex, string path, Project project, ProjectInvestigator manager)
        {
            //TODO - distinguish between loading and processing (loaded fine) errors
            var msg = String.Format("Unable to load file: {0} for project: {1} or manager: {2} reason: {3}", path,
                project == null ? "<null>" : project.ProjectId, manager == null ? "<null>" : manager.Login, ex.Message);
            MessageBox.Show(msg, "File Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }


        #region Form Events

        private void FileBrowserButton_Click(object sender, EventArgs e)
        {
            openFileDialog.FilterIndex = Properties.Settings.Default.UploadFileFileFilterIndex;
            openFileDialog.InitialDirectory = Properties.Settings.Default.UploadFileFolderPath;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileNameTextBox.Text = string.Join(";", openFileDialog.FileNames);
                CollarIsRequired = openFileDialog.FileNames.Length == 1 &&
                                   new FileLoader(FileNameTextBox.Text).CollarIsRequired;
                Properties.Settings.Default.UploadFileFileFilterIndex = openFileDialog.FilterIndex;
                if (openFileDialog.FileNames.Length > 0)
                {
                    var folder = Path.GetDirectoryName(openFileDialog.FileNames[0]);
                    Properties.Settings.Default.UploadFileFolderPath = folder;                    
                }
                EnableUpload();
            }
        }

        private void FolderBrowserButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = Properties.Settings.Default.UploadFileFolderPath;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                FolderNameTextBox.Text = folderBrowserDialog.SelectedPath;
                Properties.Settings.Default.UploadFileFolderPath = folderBrowserDialog.SelectedPath;
                CollarIsRequired = false;
                EnableUpload();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            if (UploadButton.Text == "Close")
            {
                Close();
                return;
            }
            if (FolderRadioButton.Checked)
                UploadPath(FolderNameTextBox.Text);
            else
                foreach (var path in FileNameTextBox.Text.Split(';'))
                    UploadPath(path);
        }

        private void FileRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            FileNameTextBox.Enabled = FileRadioButton.Checked;
            FileBrowserButton.Enabled = FileRadioButton.Checked;
            FolderNameTextBox.Enabled = FolderRadioButton.Checked;
            FolderBrowserButton.Enabled = FolderRadioButton.Checked;
            EnableUpload();
        }

        private void ProjectRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ProjectComboBox.Enabled = ProjectRadioButton.Checked;
            InvestigatorComboBox.Enabled = InvestigatorRadioButton.Checked;
            EnableUpload();
            RefreshCollarComboBox();
        }

        private void ProjectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProjectComboBox.SelectedItem is Project project)
                Settings.SetDefaultProject(project.ProjectId);
            EnableUpload();
            RefreshCollarComboBox();
        }

        private void InvestigatorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableUpload();
            RefreshCollarComboBox();
        }

        private void ClearCollarButton_Click(object sender, EventArgs e)
        {
            CollarComboBox.SelectedItem = null;
        }

        private void FileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableUpload();
        }

        private void FolderNameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableUpload();
        }

        private void CollarComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableUpload();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Help not available yet");
        }

        #endregion

    }
}
