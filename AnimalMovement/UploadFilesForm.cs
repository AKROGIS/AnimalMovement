using System;
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
            SetupForm();
        }

        private void SetupForm()
        {
            InitializeComponent();
            RestoreWindow();
            LoadDataContext();
            EnableUpload();
            FileRadioButton.Checked = true;
            ProjectRadioButton.Checked = Project != null;
            InvestigatorRadioButton.Checked = !ProjectRadioButton.Checked;
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            LoadProjectList();
            LoadInvestigatorList();
        }

        private void LoadProjectList()
        {
            //The Project I was given does not have object equality with projects in this data context
            var projects = (from p in Database.Projects
                            where p.ProjectInvestigator == CurrentUser ||
                                  p.ProjectEditors.Any(u => u.Editor == CurrentUser)
                            select p).ToList();
            var selectedProject =
                projects.FirstOrDefault(
                    p =>
                    Project == null ? p.ProjectId == Settings.GetDefaultProject() : p.ProjectId == Project.ProjectId);

            ProjectComboBox.DataSource = projects;
            ProjectComboBox.SelectedItem = selectedProject;
            ProjectComboBox.DisplayMember = "ProjectName";
        }

        private void LoadInvestigatorList()
        {
            //The Investigator I was given does not have object equality with investigators in this data context
            var investigators = Database.ProjectInvestigators.ToList();
            var selectedInvestigator =
                investigators.FirstOrDefault(
                    i => Investigator == null ? i.Login == CurrentUser : i.Login == Investigator.Login);
            InvestigatorComboBox.DataSource = investigators;
            InvestigatorComboBox.SelectedItem = selectedInvestigator;
            InvestigatorComboBox.DisplayMember = "Name";
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

            //FIXME - Revise Fileloader to use events we can subscribe to - this will require using instances instead of a static method
            //FIXME - Revise Fileloader to have hooks that can allow it to be canceled.
            //FIXME - Run FileLoader on a background thread
            //FIXME - Display a progress bar updated by events from the FileLoader
            FileLoader.LoadPath(path, HandleException,
                                ProjectRadioButton.Checked ? (Project) ProjectComboBox.SelectedItem : null,
                                InvestigatorRadioButton.Checked
                                    ? (ProjectInvestigator) InvestigatorComboBox.SelectedItem
                                    : null,
                                (Collar) CollarComboBox.SelectedItem,
                                StatusActiveRadioButton.Checked ? 'A' : 'I', AllowDuplicatesCheckBox.Checked);
            
            //FIXME - OnDatabaseChanged() should be called in response to a FileLoader FinishedLoading event
            //for now, we will assume that at least one file loaded and we need to update the caller
            OnDatabaseChanged();

            //FIXME - This should not be done here, but in some FileLoader Done event
            Cursor.Current = Cursors.Default;
            UploadButton.Enabled = true;
            UploadButton.Text = "Close";
        }

        private static void HandleException(Exception ex, string path, Project project, ProjectInvestigator manager)
        {
            //FIXME - disctinguish between loading and processing (loaded fine) errors
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
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileNameTextBox.Text = string.Join(";", openFileDialog.FileNames);
                CollarIsRequired = openFileDialog.FileNames.Length == 1 &&
                                   new FileLoader(FileNameTextBox.Text).CollarIsRequired;
                EnableUpload();
            }
        }

        private void FolderBrowserButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                FolderNameTextBox.Text = folderBrowserDialog.SelectedPath;
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
            if(UploadButton.Text == "Close")
                Close();
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
            var project = ProjectComboBox.SelectedItem as Project;
            if (project != null)
                Settings.SetDefaultProject(project.ProjectId);
            EnableUpload();
            RefreshCollarComboBox();
        }

        private void InvestigatorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Investigator = InvestigatorComboBox.SelectedItem as ProjectInvestigator;
            //if (Investigator != null)
            //    Settings.SetDefaultProject(Project.ProjectId);
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
