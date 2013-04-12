using System;
using System.Drawing;
using System.Windows.Forms;
using DataModel;
using System.Linq;

/*
 * I wanted the changes to the projects and collars list to occur in the main datacontext,
 * so that it all edits could be 'canceled' or saved together.
 * unfortunately, the datasources of the list controls query the datacontext, which by
 * design does not return the transient state.  Therefore I cannot see the current state
 * of the lists until I submit changes to the database.
 * 
 * I have therefore reworked the logic on the form.  the edit button enables the text fields
 * and disables the edit controls on the lists.  The edit controls on the lists are only enabled
 * when the form is not in edit mode.
 */

namespace AnimalMovement
{
    internal partial class InvestigatorForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private string InvestigatorLogin { get; set; }
        private ProjectInvestigator Investigator { get; set; }
        private bool IsMyProfile { get; set; }
        internal event EventHandler DatabaseChanged;

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // public accessors are used by the control when these classes are accessed through the Datasource
        class ProjectListItem
        {
            public string Name { get; set; }
            public bool CanDelete { get; set; }
            public Project Project { get; set; }
        }

        class CollarListItem
        {
            public string Name { get; set; }
            public bool CanDelete { get; set; }
            public Collar Collar { get; set; }
        }

        class CollarFileListItem
        {
            public string Name { get; set; }
            public bool CanDelete { get; set; }
            public CollarFile File { get; set; }
        }

        class ParameterFileListItem
        {
            public string Name { get; set; }
            public bool CanDelete { get; set; }
            public CollarParameterFile File { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        internal InvestigatorForm(string investigator, string user)
        {
            InitializeComponent();
            RestoreWindow();
            CurrentUser = user;
            InvestigatorLogin = investigator;
            IsMyProfile = String.Equals(user, investigator, StringComparison.InvariantCultureIgnoreCase);
            EditSaveButton.Enabled = IsMyProfile;
            SetEditingControls();
        }

        private void InvestigatorForm_Load(object sender, EventArgs e)
        {
            LoadDataContext();
            ShowEmailFilesCheckBox.Checked = Properties.Settings.Default.InvestigatorFormShowEmailFiles;
            ShowDownloadFilesCheckBox.Checked = Properties.Settings.Default.InvestigatorFormShowDownloadFiles;
            ShowDerivedFilesCheckBox.Checked = Properties.Settings.Default.InvestigatorFormShowDerivedFiles;
            ProjectInvestigatorTabs.SelectedIndex = Properties.Settings.Default.InvestigatorFormActiveTab;
            if (ProjectInvestigatorTabs.SelectedIndex == 0)
                ProjectInvestigatorTabs_SelectedIndexChanged(null, null);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Properties.Settings.Default.InvestigatorFormActiveTab = ProjectInvestigatorTabs.SelectedIndex;
            Properties.Settings.Default.InvestigatorFormShowEmailFiles = ShowEmailFilesCheckBox.Checked;
            Properties.Settings.Default.InvestigatorFormShowDownloadFiles = ShowDownloadFilesCheckBox.Checked;
            Properties.Settings.Default.InvestigatorFormShowDerivedFiles = ShowDerivedFilesCheckBox.Checked;
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            Investigator = Database.ProjectInvestigators.First(pi => pi.Login == InvestigatorLogin);
            LoginTextBox.Text = InvestigatorLogin;
            NameTextBox.Text = Investigator.Name;
            EmailTextBox.Text = Investigator.Email;
            PhoneTextBox.Text = Investigator.Phone;
        }

        private bool CanDeleteCollar(Collar collar)
        {
            return !Database.CollarDeployments.Any(cd => cd.Collar == collar) &&
                   !Database.CollarFixes.Any(cd => cd.Collar == collar);
        }

        private string BuildCollarText(Collar collar)
        {
            string name = collar.ToString();
            var animals = from deployment in Database.CollarDeployments
                          where deployment.Collar == collar && deployment.RetrievalDate == null
                          select deployment.Animal;
            var animal = animals.FirstOrDefault();
            if (animal != null)
                name += " on " + animal;
            if (collar.DisposalDate != null)
                name = String.Format("{0} (disp:{1:M/d/yy})", name, collar.DisposalDate.Value.ToLocalTime());
            return name;
        }


        private void UpdateDataSource()
        {
            Investigator.Name = NameTextBox.Text;
            Investigator.Email = EmailTextBox.Text;
            Investigator.Phone = PhoneTextBox.Text;
        }


        private void EditSaveButton_Click(object sender, EventArgs e)
        {
            //This button is not enabled unless editing is permitted 
            if (EditSaveButton.Text == "Edit")
            {
                // The user wants to edit, Enable form
                EditSaveButton.Text = "Save";
                DoneCancelButton.Text = "Cancel";
                SetEditingControls();
            }
            else
            {
                //User is saving
                try
                {
                    UpdateDataSource();
                    Database.SubmitChanges();
                    OnDatabaseChanged();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to save changes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                EditSaveButton.Text = "Edit";
                DoneCancelButton.Text = "Done";
                SetEditingControls();
            }
        }


        private void DoneCancelButton_Click(object sender, EventArgs e)
        {
            if (DoneCancelButton.Text == "Cancel")
            {
                DoneCancelButton.Text = "Done";
                EditSaveButton.Text = "Edit";
                SetEditingControls();
                //Reset state from database
                LoadDataContext();
            }
            else
            {
                Close();
            }
        }


        private void SetEditingControls()
        {
            bool editModeEnabled = EditSaveButton.Text == "Save";
            NameTextBox.Enabled = editModeEnabled;
            EmailTextBox.Enabled = editModeEnabled;
            PhoneTextBox.Enabled = editModeEnabled;
            AddCollarButton.Enabled = !editModeEnabled && IsMyProfile;
            AddProjectButton.Enabled = !editModeEnabled && IsMyProfile;
            AddParameterFileButton.Enabled = !editModeEnabled && IsMyProfile;
            //Set the Delete/Info buttons based on what is selected
            CollarsListBox_SelectedIndexChanged(null, null);
            ProjectsListBox_SelectedIndexChanged(null, null);
            ParameterFilesListBox_SelectedIndexChanged(null, null);
        }

        #region Project List

        private void LoadProjectList()
        {
            var query = from project in Database.Projects
                        where project.ProjectInvestigator1 == Investigator
                        select new ProjectListItem
                        {
                            Project = project,
                            Name = project.ProjectName + " (" + project.ProjectId + ")",
                            CanDelete = !project.Animals.Any() && !project.CollarFiles.Any()
                        };
            var sortedList = query.OrderBy(p => p.Name).ToList();
            ProjectsListBox.DataSource = sortedList;
            ProjectsListBox.DisplayMember = "Name";
            ProjectsTab.Text = sortedList.Count < 5 ? "Projects" : String.Format("Projects ({0})", sortedList.Count);
        }

        private void AddProjectButton_Click(object sender, EventArgs e)
        {
            var form = new AddProjectForm(CurrentUser);
            //Adding projects in this context is not supported.
            //var form = newAddProjectForm(Database, CurrentUser);
            if (form.ShowDialog(this) != DialogResult.Cancel)
                LoadProjectList();
        }

        private void DeleteProjectsButton_Click(object sender, EventArgs e)
        {
            foreach (ProjectListItem item in ProjectsListBox.SelectedItems.Cast<ProjectListItem>().Where(item => item.CanDelete))
                Database.Projects.DeleteOnSubmit(item.Project);
            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                string msg = "Unable to delete one or more of the selected projects\n" +
                                "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadDataContext();
        }

        private void InfoProjectButton_Click(object sender, EventArgs e)
        {
            var project = ((ProjectListItem)ProjectsListBox.SelectedItem).Project;
            var form = new ProjectDetailsForm(project.ProjectId, CurrentUser);
            form.DatabaseChanged += (o, args) => { OnDatabaseChanged(); LoadDataContext(); };
            form.Show(this);
        }

        private void ProjectsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InfoProjectButton.Enabled = false;
            DeleteProjectsButton.Enabled = false;
            if (EditSaveButton.Text == "Save")
                return;
            InfoProjectButton.Enabled = ProjectsListBox.SelectedItems.Count == 1;
            if (IsMyProfile && ProjectsListBox.SelectedItems.Cast<ProjectListItem>().Any(item => item.CanDelete))
                DeleteProjectsButton.Enabled = true;

        }

        #endregion


        #region Collar List

        private void LoadCollarList()
        {
            var query = from collar in Database.Collars
                        where collar.ProjectInvestigator == Investigator
                        //orderby collar.CollarManufacturer , collar.CollarId
                        select new CollarListItem
                        {
                            Collar = collar,
                            Name = BuildCollarText(collar),
                            CanDelete = CanDeleteCollar(collar)
                        };
            var sortedList = query.OrderBy(c => c.Collar.DisposalDate != null).ThenBy(c => c.Collar.CollarManufacturer).ThenBy(c => c.Collar.CollarId).ToList();
            CollarsListBox.DataSource = sortedList;
            CollarsListBox.DisplayMember = "Name";
            CollarsListBox.ClearItemColors();
            for (int i = 0; i < sortedList.Count; i++)
            {
                if (sortedList[i].Collar.DisposalDate != null)
                    CollarsListBox.SetItemColor(i, Color.DarkGray);
            }
            CollarsTab.Text = sortedList.Count < 5 ? "Collars" : String.Format("Collars ({0})", sortedList.Count);
        }

        private void AddCollarButton_Click(object sender, EventArgs e)
        {
            var form = new AddCollarForm(CurrentUser);
            //Adding collars in this context is not supported.
            //var form = new AddCollarForm(Database, CurrentUser);
            if (form.ShowDialog(this) != DialogResult.Cancel)
                LoadCollarList();
        }

        private void DeleteCollarsButton_Click(object sender, EventArgs e)
        {
            foreach (CollarListItem item in CollarsListBox.SelectedItems.Cast<CollarListItem>().Where(item => item.CanDelete))
                Database.Collars.DeleteOnSubmit(item.Collar);
            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                string msg = "Unable to delete one or more of the selected collars\n" +
                                "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadDataContext();
        }

        private void InfoCollarButton_Click(object sender, EventArgs e)
        {
            var collar = ((CollarListItem)CollarsListBox.SelectedItem).Collar;
            var form = new CollarDetailsForm(collar.CollarManufacturer, collar.CollarId, CurrentUser);
            form.DatabaseChanged += (o, args) => { OnDatabaseChanged(); LoadDataContext(); };
            form.Show(this);
        }

        private void CollarsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InfoCollarButton.Enabled = false;
            DeleteCollarsButton.Enabled = false;
            if (EditSaveButton.Text == "Save")
                return;
            InfoCollarButton.Enabled = CollarsListBox.SelectedItems.Count == 1;
            if (IsMyProfile && CollarsListBox.SelectedItems.Cast<CollarListItem>().Any(item => item.CanDelete))
                DeleteCollarsButton.Enabled = true;
        }

        #endregion


        #region Collar File List

        private void LoadCollarFileList()
        {
            var query = from file in Database.CollarFiles
                        where file.ProjectInvestigator == Investigator &&
                              (ShowDerivedFilesCheckBox.Checked || file.ParentFileId == null) &&
                              (ShowEmailFilesCheckBox.Checked || file.Format != 'E') &&
                              (ShowDownloadFilesCheckBox.Checked || file.Format != 'F')
                        select new CollarFileListItem
                        {
                            File = file,
                            Name = file.FileName,
                            CanDelete = file.ParentFileId == null && !file.ArgosDownloads.Any()
                        };
            var sortedList = query.OrderBy(f => f.Name).ToList();
            CollarFilesListBox.DataSource = sortedList;
            CollarFilesListBox.DisplayMember = "Name";
            CollarFilesListBox.ClearItemColors();
            for (int i = 0; i < sortedList.Count; i++)
            {
                if (sortedList[i].File.ParentFileId != null)
                    CollarFilesListBox.SetItemColor(i, Color.Brown);
                if (sortedList[i].File.Format == 'E')
                    CollarFilesListBox.SetItemColor(i, Color.MediumBlue);
                if (sortedList[i].File.Format == 'F')
                    CollarFilesListBox.SetItemColor(i, Color.DarkMagenta);
                if (sortedList[i].File.Status == 'I')
                {
                    //Dim color of inactive files
                    var c = CollarFilesListBox.GetItemColor(i);
                    CollarFilesListBox.SetItemColor(i, ControlPaint.Light(c, 1.4f));
                }
            }
            CollarFilesTab.Text = sortedList.Count < 5 ? "Collar Files" : String.Format("Collar Files ({0})", sortedList.Count);
        }

        private void AddCollarFileButton_Click(object sender, EventArgs e)
        {
            var form = new AddFileForm(CurrentUser);
            //Adding projects in this context is not supported.
            //var form = newAddProjectForm(Database, CurrentUser);
            if (form.ShowDialog(this) != DialogResult.Cancel)
                LoadCollarFileList();
        }

        private void DeleteCollarFilesButton_Click(object sender, EventArgs e)
        {
            foreach (CollarFileListItem item in CollarFilesListBox.SelectedItems.Cast<CollarFileListItem>().Where(item => item.CanDelete))
                Database.CollarFiles.DeleteOnSubmit(item.File);
            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                string msg = "Unable to delete one or more of the selected files\n" +
                                "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadDataContext();
        }

        private void InfoCollarFileButton_Click(object sender, EventArgs e)
        {
            var file = ((CollarFileListItem)CollarFilesListBox.SelectedItem).File;
            var form = new FileDetailsForm(file.FileId, CurrentUser);
            form.DatabaseChanged += (o, args) => { OnDatabaseChanged(); LoadDataContext(); };
            form.Show(this);
        }

        private void CollarFilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InfoCollarFileButton.Enabled = false;
            DeleteCollarFilesButton.Enabled = false;
            if (EditSaveButton.Text == "Save")
                return;
            InfoCollarFileButton.Enabled = CollarFilesListBox.SelectedItems.Count == 1;
            if (IsMyProfile && CollarFilesListBox.SelectedItems.Cast<CollarFileListItem>().Any(item => item.CanDelete))
                DeleteCollarFilesButton.Enabled = true;
        }

        #endregion


        #region Parameter File List

        private void LoadParameterFileList()
        {
            var query = from file in Database.CollarParameterFiles
                        where file.ProjectInvestigator == Investigator
                        select new ParameterFileListItem
                        {
                            File = file,
                            Name = file.FileName,
                            CanDelete = true
                        };
            var sortedList = query.OrderBy(f => f.Name).ToList();
            ParameterFilesListBox.DataSource = sortedList;
            ParameterFilesListBox.DisplayMember = "Name";
            ParameterFilesListBox.ClearItemColors();
            for (int i = 0; i < sortedList.Count; i++)
            {
                if (sortedList[i].File.Status == 'I')
                    ParameterFilesListBox.SetItemColor(i, Color.DarkGray);
            }
            ParameterFilesTab.Text = sortedList.Count < 5 ? "Parameter Files" : String.Format("Parameter Files ({0})", sortedList.Count);
        }

        private void AddParameterFileButton_Click(object sender, EventArgs e)
        {
            var form = new AddCollarParameterFileForm(CurrentUser);
            //Adding projects in this context is not supported.
            //var form = newAddProjectForm(Database, CurrentUser);
            if (form.ShowDialog(this) != DialogResult.Cancel)
                LoadParameterFileList();
        }

        private void DeleteParameterFilesButton_Click(object sender, EventArgs e)
        {
            foreach (ParameterFileListItem item in ParameterFilesListBox.SelectedItems.Cast<ParameterFileListItem>().Where(item => item.CanDelete))
                Database.CollarParameterFiles.DeleteOnSubmit(item.File);
            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                string msg = "Unable to delete one or more of the selected files\n" +
                                "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadDataContext();
        }

        private void InfoParameterFileButton_Click(object sender, EventArgs e)
        {
            var file = ((ParameterFileListItem)ParameterFilesListBox.SelectedItem).File;
            var form = new CollarParameterFileDetailsForm(file.FileId, CurrentUser);
            form.DatabaseChanged += (o, args) => { OnDatabaseChanged(); LoadDataContext(); };
            form.Show(this);
        }

        private void ParameterFilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InfoParameterFileButton.Enabled = false;
            DeleteParameterFilesButton.Enabled = false;
            if (EditSaveButton.Text == "Save")
                return;
            InfoParameterFileButton.Enabled = ParameterFilesListBox.SelectedItems.Count == 1;
            if (IsMyProfile && ParameterFilesListBox.SelectedItems.Cast<ParameterFileListItem>().Any(item => item.CanDelete))
                DeleteParameterFilesButton.Enabled = true;

        }

        #endregion


        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }


        private void ProjectInvestigatorTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ProjectInvestigatorTabs.SelectedIndex)
            {
                case 0:
                    LoadProjectList();
                    break;
                case 1:
                    LoadCollarList();
                    break;
                case 2:
                    LoadCollarFileList();
                    break;
                case 3:
                    LoadParameterFileList();
                    break;
            }
        }


        private void ShowFilesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadCollarFileList();
        }

    }
}
