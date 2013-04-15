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
        private string ProjectId { get; set; }
        private Project Project { get; set; }
        private ProjectInvestigator Investigator { get; set; }
        private bool CollarIsRequired { get; set; }
        internal event EventHandler DatabaseChanged;


        internal UploadFilesForm(string projectId = null)
        {
            ProjectId = projectId;
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
            ProjectRadioButton.Checked = true;
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
            var query = from p in Database.Projects
                        where p.ProjectInvestigator == CurrentUser ||
                              p.ProjectEditors.Any(u => u.Editor == CurrentUser)
                        select p;
            var myProjects = query.ToList();
            ProjectComboBox.DataSource = myProjects;

            if (ProjectId == null)
                ProjectId = Settings.GetDefaultProject();
            Project = ProjectId == null
                    ? myProjects.FirstOrDefault()
                    : Project = myProjects.FirstOrDefault(p => p.ProjectId == ProjectId);
            ProjectComboBox.DisplayMember = "ProjectName";
            ProjectComboBox.SelectedItem = Project;
        }

        private void LoadInvestigatorList()
        {
            //var investigators = Database.ProjectInvestigators.ToList();
            InvestigatorComboBox.DataSource = Database.ProjectInvestigators;
            InvestigatorComboBox.DisplayMember = "Name";
            InvestigatorComboBox.SelectedItem = Database.ProjectInvestigators.FirstOrDefault(i => i.Login == CurrentUser);
        }


        private void EnableUpload()
        {
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
                if (openFileDialog.FileNames.Length == 1)
                {
                    char? format;
                    try
                    {
                        format = new FileLoader(openFileDialog.FileNames[0]).Format.Value;
                    }
                    catch (Exception)
                    {
                        CollarIsRequired = false;
                        return;
                    }
                    if (format == 'A' || format == 'D')
                        CollarIsRequired = true;
                }
                else
                    CollarIsRequired = false;
            }
        }

        private void FolderBrowserButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                FolderNameTextBox.Text = folderBrowserDialog.SelectedPath;
                CollarIsRequired = false;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            OnDatabaseChanged();
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
            Project = ProjectComboBox.SelectedItem as Project;
            if (Project != null)
                Settings.SetDefaultProject(Project.ProjectId);
            EnableUpload();
            RefreshCollarComboBox();
        }

        private void InvestigatorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Investigator = InvestigatorComboBox.SelectedItem as ProjectInvestigator;
            if (Investigator != null)
                Settings.SetDefaultProject(Project.ProjectId);
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
