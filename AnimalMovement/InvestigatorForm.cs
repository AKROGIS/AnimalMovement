using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
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
        private ProjectInvestigator Investigator { get; set; }
        private bool IsInvestigator { get; set; }
        private bool IsEditor { get; set; }
        private bool IsEditMode { get; set; }
        internal event EventHandler DatabaseChanged;

        internal InvestigatorForm(ProjectInvestigator investigator)
        {
            InitializeComponent();
            RestoreWindow();
            Investigator = investigator;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            SetDefaultPropertiesBeforeFormLoad();
            LoadDataContext();
            SetUpGeneral();
        }

        private void SetDefaultPropertiesBeforeFormLoad()
        {
            ShowEmailFilesCheckBox.Checked = Properties.Settings.Default.InvestigatorFormShowEmailFiles;
            ShowDownloadFilesCheckBox.Checked = Properties.Settings.Default.InvestigatorFormShowDownloadFiles;
            ShowDerivedFilesCheckBox.Checked = Properties.Settings.Default.InvestigatorFormShowDerivedFiles;
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Investigator is in a different DataContext, get one in this DataContext
            if (Investigator != null)
                Investigator = Database.ProjectInvestigators.First(pi => pi.Login == Investigator.Login);
            if (Investigator == null)
                throw new InvalidOperationException("Investigator Form not provided a valid investigator.");

            var functions = new AnimalMovementFunctions();
            IsInvestigator = Investigator == Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == CurrentUser);
            IsEditor = functions.IsInvestigatorEditor(Investigator.Login, CurrentUser) ?? false;
        }


        #region Form Control

        protected override void OnLoad(EventArgs e)
        {
            ProjectInvestigatorTabs.SelectedIndex = Properties.Settings.Default.InvestigatorFormActiveTab;
            if (ProjectInvestigatorTabs.SelectedIndex == 0)
                //if new index is zero, index changed event will not fire, so fire it manually
                ProjectInvestigatorTabs_SelectedIndexChanged(null,null);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Properties.Settings.Default.InvestigatorFormActiveTab = ProjectInvestigatorTabs.SelectedIndex;
            Properties.Settings.Default.InvestigatorFormShowEmailFiles = ShowEmailFilesCheckBox.Checked;
            Properties.Settings.Default.InvestigatorFormShowDownloadFiles = ShowDownloadFilesCheckBox.Checked;
            Properties.Settings.Default.InvestigatorFormShowDerivedFiles = ShowDerivedFilesCheckBox.Checked;
        }

        private void ProjectInvestigatorTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ProjectInvestigatorTabs.SelectedIndex)
            {
                case 0:
                    SetUpProjectTab();
                    break;
                case 1:
                    SetUpCollarsTab();
                    break;
                case 2:
                    SetUpArgosTab();
                    break;
                case 3:
                    SetUpCollarFilesTab();
                    break;
                case 4:
                    SetUpParameterFilesTab();
                    break;
                case 5:
                    SetUpAssistantsTab();
                    break;
                case 6:
                    SetUpReportsTab();
                    break;
            }
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

        #endregion


        #region General

        private void SetUpGeneral()
        {
            LoginTextBox.Text = Investigator.Login;
            NameTextBox.Text = Investigator.Name;
            EmailTextBox.Text = Investigator.Email;
            PhoneTextBox.Text = Investigator.Phone;
            EnableGeneralControls();
        }

        private void EnableGeneralControls()
        {
            EditSaveButton.Enabled = IsEditor;
            IsEditMode = EditSaveButton.Text == "Save";
            NameTextBox.Enabled = IsEditMode;
            EmailTextBox.Enabled = IsEditMode;
            PhoneTextBox.Enabled = IsEditMode;
        }

        private void EditSaveButton_Click(object sender, EventArgs e)
        {
            //This button is not enabled unless editing is permitted 
            if (EditSaveButton.Text == "Edit")
            {
                // The user wants to edit, Enable form
                EditSaveButton.Text = "Save";
                DoneCancelButton.Text = "Cancel";
                EnableGeneralControls();
            }
            else
            {
                //User is saving
                UpdateDataSource();
                if (SubmitChanges())
                {
                    OnDatabaseChanged();
                    EditSaveButton.Text = "Edit";
                    DoneCancelButton.Text = "Done";
                    EnableGeneralControls();
                }
            }
        }

        private void DoneCancelButton_Click(object sender, EventArgs e)
        {
            if (DoneCancelButton.Text == "Cancel")
            {
                DoneCancelButton.Text = "Done";
                EditSaveButton.Text = "Edit";
                EnableGeneralControls();
                //Reset state from database
                LoadDataContext();
                SetUpGeneral();
            }
            else
            {
                Close();
            }
        }

        private void UpdateDataSource()
        {
            Investigator.Name = NameTextBox.Text;
            Investigator.Email = EmailTextBox.Text;
            Investigator.Phone = PhoneTextBox.Text;
        }

        #endregion


        #region Project List

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // public accessors are used by the control when these classes are accessed through the Datasource
        class ProjectListItem
        {
            public string Name { get; set; }
            public bool CanDelete { get; set; }
            public Project Project { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        private void SetUpProjectTab()
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

        private void EnableProjectControls()
        {
            AddProjectButton.Enabled = !IsEditMode && IsEditor;
            DeleteProjectsButton.Enabled = !IsEditMode && IsEditor &&
                                          ProjectsListBox.SelectedItems.Cast<ProjectListItem>()
                                                        .Any(item => item.CanDelete);
            InfoProjectButton.Enabled = !IsEditMode && ProjectsListBox.SelectedItems.Count == 1;
        }

        private void ProjectDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpProjectTab();
        }

        private void AddProjectButton_Click(object sender, EventArgs e)
        {
            var form = new AddProjectForm();
            form.DatabaseChanged += (o, x) => ProjectDataChanged();
            form.Show(this);
        }

        private void DeleteProjectsButton_Click(object sender, EventArgs e)
        {
            foreach (ProjectListItem item in ProjectsListBox.SelectedItems.Cast<ProjectListItem>().Where(item => item.CanDelete))
                Database.Projects.DeleteOnSubmit(item.Project);
            if (SubmitChanges())
                ProjectDataChanged();
        }

        private void InfoProjectButton_Click(object sender, EventArgs e)
        {
            var project = ((ProjectListItem)ProjectsListBox.SelectedItem).Project;
            var form = new ProjectDetailsForm(project);
            form.DatabaseChanged += (o, args) => ProjectDataChanged();
            form.Show(this);
        }

        private void ProjectsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableProjectControls();
        }

        #endregion


        #region Collar List

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // public accessors are used by the control when these classes are accessed through the Datasource

        class CollarListItem
        {
            public string Name { get; set; }
            public bool CanDelete { get; set; }
            public Collar Collar { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        private void SetUpCollarsTab()
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
            EnableCollarControls();
        }

        private void EnableCollarControls()
        {
            AddCollarButton.Enabled = !IsEditMode && IsEditor;
            DeleteCollarsButton.Enabled = !IsEditMode && IsEditor &&
                                          CollarsListBox.SelectedItems.Cast<CollarListItem>()
                                                        .Any(item => item.CanDelete);
            InfoCollarButton.Enabled = !IsEditMode;
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

        private void CollarDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpCollarsTab();
        }

        private void AddCollarButton_Click(object sender, EventArgs e)
        {
            var form = new AddCollarForm(Investigator);
            form.DatabaseChanged += (o, x) => CollarDataChanged();
            form.Show(this);
        }

        private void DeleteCollarsButton_Click(object sender, EventArgs e)
        {
            foreach (CollarListItem item in CollarsListBox.SelectedItems.Cast<CollarListItem>().Where(item => item.CanDelete))
                Database.Collars.DeleteOnSubmit(item.Collar);
            if (SubmitChanges())
                CollarDataChanged();
        }

        private void InfoCollarButton_Click(object sender, EventArgs e)
        {
            var collar = ((CollarListItem)CollarsListBox.SelectedItem).Collar;
            var form = new CollarDetailsForm(collar);
            form.DatabaseChanged += (o, args) => CollarDataChanged();
            form.Show(this);
        }

        private void CollarsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableCollarControls();
        }

        #endregion


        #region Argos Programs

        private void SetUpArgosTab()
        {
            //TODO - provide implementation
            EnableArgosControls();
        }

        private void EnableArgosControls()
        {

        }

        private void ArgosDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpArgosTab();
        }

        private void AddPlatformButton_Click(object sender, EventArgs e)
        {
            var prog = Database.ArgosPrograms.FirstOrDefault(p => p.ProgramId == "14559");
            var form = new AddArgosPlatformForm(prog);
            form.DatabaseChanged += (o, x) => ArgosDataChanged();
            form.Show(this);
        }

        #endregion


        #region Collar File List

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // public accessors are used by the control when these classes are accessed through the Datasource
        class CollarFileListItem
        {
            public string Name { get; set; }
            public bool CanDelete { get; set; }
            public CollarFile File { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        private void SetUpCollarFilesTab()
        {
            var query = from file in Database.CollarFiles
                        where file.ProjectInvestigator == Investigator &&
                              (ShowDerivedFilesCheckBox.Checked || file.ParentFileId == null) &&
                              (ShowEmailFilesCheckBox.Checked || file.Format != 'E') &&
                              (ShowDownloadFilesCheckBox.Checked || file.Format != 'F')
                        select new CollarFileListItem
                        {
                            File = file,
                            Name = file.FileName + (file.Status == 'I' ? " (Inactive)" : ""),
                            CanDelete = file.ParentFileId == null && !file.ArgosDownloads.Any()
                        };
            var sortedList = query.OrderBy(f => f.File.Status)
                                  .ThenByDescending(f => f.File.ParentFileId ?? f.File.FileId)
                                  .ThenByDescending(f => f.File.FileId)
                                  .ToList();
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
            EnableCollarFilesControls();
        }

        private void EnableCollarFilesControls()
        {
            AddCollarFileButton.Enabled = !IsEditMode && IsEditor;
            DeleteCollarFilesButton.Enabled = !IsEditMode && IsEditor &&
                                              CollarFilesListBox.SelectedItems.Cast<CollarFileListItem>()
                                                                .Any(item => item.CanDelete);
            InfoCollarFileButton.Enabled = !IsEditMode;
        }

        private void CollarFileDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpCollarFilesTab();
        }

        private void AddCollarFileButton_Click(object sender, EventArgs e)
        {
            var form = new UploadFilesForm(null, Investigator);
            form.DatabaseChanged += (o, x) => CollarFileDataChanged();
            form.Show(this);
        }

        private void DeleteCollarFilesButton_Click(object sender, EventArgs e)
        {
            foreach (CollarFileListItem item in CollarFilesListBox.SelectedItems.Cast<CollarFileListItem>().Where(item => item.CanDelete))
                Database.CollarFiles.DeleteOnSubmit(item.File);
            if (SubmitChanges())
                CollarFileDataChanged();
        }

        private void InfoCollarFileButton_Click(object sender, EventArgs e)
        {
            var file = ((CollarFileListItem)CollarFilesListBox.SelectedItem).File;
            var form = new FileDetailsForm(file);
            form.DatabaseChanged += (o, args) => CollarFileDataChanged();
            form.Show(this);
        }

        private void CollarFilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableCollarFilesControls();
        }

        private void ShowFilesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Visible)
                SetUpCollarFilesTab();
        }

        #endregion


        #region Parameter File List

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // public accessors are used by the control when these classes are accessed through the Datasource
        class ParameterFileListItem
        {
            public string Name { get; set; }
            public bool CanDelete { get; set; }
            public CollarParameterFile File { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        private void SetUpParameterFilesTab()
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
            EnableParameterFilesControls();
        }

        private void EnableParameterFilesControls()
        {
            AddParameterFileButton.Enabled = !IsEditMode && IsEditor;
            DeleteParameterFilesButton.Enabled = !IsEditMode && IsEditor &&
                                              ParameterFilesListBox.SelectedItems.Cast<ParameterFileListItem>()
                                                                .Any(item => item.CanDelete);
            InfoParameterFileButton.Enabled = !IsEditMode;
        }

        private void ParameterFileDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpParameterFilesTab();
        }

        private void AddParameterFileButton_Click(object sender, EventArgs e)
        {
            var form = new AddCollarParameterFileForm(null);
            form.DatabaseChanged += (o, x) => ParameterFileDataChanged();
            form.Show(this);
        }

        private void DeleteParameterFilesButton_Click(object sender, EventArgs e)
        {
            foreach (ParameterFileListItem item in ParameterFilesListBox.SelectedItems.Cast<ParameterFileListItem>().Where(item => item.CanDelete))
                Database.CollarParameterFiles.DeleteOnSubmit(item.File);
            if (SubmitChanges())
                ParameterFileDataChanged();
        }

        private void InfoParameterFileButton_Click(object sender, EventArgs e)
        {
            var file = ((ParameterFileListItem)ParameterFilesListBox.SelectedItem).File;
            var form = new CollarParameterFileDetailsForm(file);
            form.DatabaseChanged += (o, args) => ParameterFileDataChanged();
            form.Show(this);
        }

        private void ParameterFilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableParameterFilesControls();
        }

        #endregion


        #region Assistants

        private void SetUpAssistantsTab()
        {
            var assistants = Investigator.ProjectInvestigatorAssistants;
            AssistantsListBox.DataSource = assistants;
            AssistantsListBox.DisplayMember = "Assistant";
            EnableAssistantControls();
        }

        private void EnableAssistantControls()
        {
            AddAssistantButton.Enabled = !IsEditMode && IsInvestigator;
            DeleteAssistantButton.Enabled = !IsEditMode && AssistantsListBox.SelectedItems.Count > 0 &&
                                            (IsInvestigator ||
                                             (IsEditor && AssistantsListBox.SelectedItems.Count == 1 &&
                                              String.Equals(((ProjectInvestigatorAssistant)AssistantsListBox.SelectedItem).Assistant.Normalize(),
                                                            CurrentUser.Normalize(), StringComparison.OrdinalIgnoreCase)));
        }

        private void AssistantDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpAssistantsTab();
        }

        private void AddAssistantButton_Click(object sender, EventArgs e)
        {
            var form = new AddEditorForm(null, Investigator);
            form.DatabaseChanged += (o, x) => AssistantDataChanged();
            form.Show(this);
        }

        private void DeleteAssistantButton_Click(object sender, EventArgs e)
        {
            foreach (var item in AssistantsListBox.SelectedItems)
                Database.ProjectInvestigatorAssistants.DeleteOnSubmit((ProjectInvestigatorAssistant)item);
            if (SubmitChanges())
                AssistantDataChanged();
        }

        private void AssistantsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableAssistantControls();
        }

        #endregion


        #region QC Reports

        private XDocument _queryDocument;

        private void SetUpReportsTab()
        {
            var xmlFilePath = Properties.Settings.Default.InvestigatorReportsXml;
            string error = null;
            try
            {
                _queryDocument = XDocument.Load(xmlFilePath);
            }
            catch (Exception ex)
            {
                error = String.Format("Unable to load '{0}': {1}", xmlFilePath, ex.Message);
                _queryDocument = null;
            }
            if (error != null)
            {
                ReportDescriptionTextBox.Text = error;
                MessageBox.Show(error, "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ReportComboBox.DataSource = null;
                return;
            }
            var names = new List<string>{"Pick a report"};
            names.AddRange(_queryDocument.Descendants("name").Select(i => i.Value.Trim()));
            ReportComboBox.DataSource = names;
        }

        private void ReportComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var report = _queryDocument.Descendants("report")
                                       .FirstOrDefault(
                                           r => ((string) r.Element("name")).Trim() == (string) ReportComboBox.SelectedItem);
            ReportDescriptionTextBox.Text = report == null ? null : (string) report.Element("description");
            FillDataGrid(report == null ? null : (string)report.Element("query"));
        }

        private void FillDataGrid(string sql)
        {
            if (String.IsNullOrEmpty(sql))
            {
                ReportDataGridView.DataSource = null;
                return;
            }
            var command = new SqlCommand(sql, (SqlConnection)Database.Connection);
            command.Parameters.Add(new SqlParameter("@PI", SqlDbType.NVarChar) { Value = Investigator.Login });
            var dataAdapter = new SqlDataAdapter(command);
            var table = new DataTable();
            try
            {
                dataAdapter.Fill(table);
            }
            catch (Exception ex)
            {
                table.Columns.Add("Error");
                table.Rows.Add(ex.Message);
            }
            ReportDataGridView.DataSource = new BindingSource { DataSource = table };
        }

        #endregion

    }
}
