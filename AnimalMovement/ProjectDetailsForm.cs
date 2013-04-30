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
    internal partial class ProjectDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private Project Project { get; set; }
        private bool IsInvestigator { get; set; }
        private bool IsEditor { get; set; }
        private bool IsEditMode { get; set; }
        internal event EventHandler DatabaseChanged;

        internal ProjectDetailsForm(Project project)
        {
            InitializeComponent();
            RestoreWindow();
            Project = project;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            SetDefaultPropertiesBeforeFormLoad();
            LoadDataContext();
            SetUpGeneral();
        }

        private void SetDefaultPropertiesBeforeFormLoad()
        {
            ShowEmailFilesCheckBox.Checked = Properties.Settings.Default.ProjectDetailsFormShowEmailFiles;
            ShowDownloadFilesCheckBox.Checked = Properties.Settings.Default.ProjectDetailsFormShowDownloadFiles;
            ShowDerivedFilesCheckBox.Checked = Properties.Settings.Default.ProjectDetailsFormShowDerivedFiles;
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Project is in a different DataContext, get one in this DataContext
            if (Project != null)
                Project = Database.Projects.FirstOrDefault(p => p.ProjectId == Project.ProjectId);
            if (Project == null)
                throw new InvalidOperationException("Project Details Form not provided a valid project.");

            var functions = new AnimalMovementFunctions();
            IsInvestigator = Database.Projects.Any(p => p == Project && p.ProjectInvestigator == CurrentUser);
            IsEditor = functions.IsProjectEditor(Project.ProjectId, CurrentUser) ?? false;
        }

        
        #region Form Control

        protected override void OnLoad(EventArgs e)
        {
            ProjectTabs.SelectedIndex = Properties.Settings.Default.ProjectDetailsFormActiveTab;
            if (ProjectTabs.SelectedIndex == 0)
                //if new index is zero, index changed event will not fire, so fire it manually
                ProjectTabs_SelectedIndexChanged(null, null);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Properties.Settings.Default.ProjectDetailsFormActiveTab = ProjectTabs.SelectedIndex;
            Properties.Settings.Default.ProjectDetailsFormShowEmailFiles = ShowEmailFilesCheckBox.Checked;
            Properties.Settings.Default.ProjectDetailsFormShowDownloadFiles = ShowDownloadFilesCheckBox.Checked;
            Properties.Settings.Default.ProjectDetailsFormShowDerivedFiles = ShowDerivedFilesCheckBox.Checked;
        }

        private void ProjectTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ProjectTabs.SelectedIndex)
            {
                case 0:
                    SetUpAnimalTab();
                    break;
                case 1:
                    SetUpFileTab();
                    break;
                case 2:
                    SetUpEditorsTab();
                    break;
                case 3:
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


        #region General Tab (Header)

        private void SetUpGeneral()
        {
            ProjectCodeTextBox.Text = Project.ProjectId;
            DescriptionTextBox.Text = Project.Description;
            UnitTextBox.Text = Project.UnitCode;
            ProjectNameTextBox.Text = Project.ProjectName;
            InvestigatorTextBox.Text = Project.ProjectInvestigator;
            EnableGeneralControls();
        }

        private void EnableGeneralControls()
        {
            EditSaveButton.Enabled = IsEditor;
            IsEditMode = EditSaveButton.Text == "Save";
            ProjectNameTextBox.Enabled = IsEditMode;
            DescriptionTextBox.Enabled = IsEditMode;
            UnitTextBox.Enabled = IsEditMode;
            EditInvestigatorButton.Enabled = !IsEditMode && IsInvestigator;
            InvestigatorDetailsButton.Enabled = !IsEditMode;
        }

        private void GeneralDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpGeneral();
        }

        private void EditInvestigatorButton_Click(object sender, EventArgs e)
        {
            var form = new ChangeInvestigatorForm(Project);
            form.DatabaseChanged += (o, args) => GeneralDataChanged();
            form.Show(this);
        }

        private void InvestigatorDetailsButton_Click(object sender, EventArgs e)
        {
            var form = new InvestigatorForm(Project.ProjectInvestigator1);
            form.DatabaseChanged += (o, args) => GeneralDataChanged();
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
            Project.Description = DescriptionTextBox.Text;
            Project.UnitCode = UnitTextBox.Text;
            Project.ProjectName = ProjectNameTextBox.Text;
        }

        #endregion


        #region Animal List

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // public accessors are used by the control when these classes are accessed through the Datasource
        class AnimalListItem
        {
            public string Name { get; set; }
            public bool CanDelete { get; set; }
            public Animal Animal { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        private void SetUpAnimalTab()
        {
            var query = (from animal in Database.Animals
                         where animal.Project == Project
                         //orderby animal.MortalityDate , animal.AnimalId
                         select new AnimalListItem
                         {
                             Animal = animal,
                             Name = GetName(animal),
                             CanDelete = CanDeleteAnimal(animal)
                         });
            var sortedList = query.OrderBy(a => a.Animal.MortalityDate != null).ThenBy(a => a.Animal.AnimalId).ToList();
            AnimalsListBox.DataSource = sortedList;
            AnimalsListBox.DisplayMember = "Name";
            AnimalsListBox.ClearItemColors();
            for (int i = 0; i < sortedList.Count; i++)
            {
                if (sortedList[i].Animal.MortalityDate != null)
                    AnimalsListBox.SetItemColor(i, Color.DarkGray);
            }
            AnimalsTabPage.Text = sortedList.Count < 5 ? "Animals" : String.Format("Animals ({0})", sortedList.Count);
        }

        private static string GetName(Animal animal)
        {
            var currentCollar = animal.CollarDeployments.FirstOrDefault(cd => cd.RetrievalDate == null);
            var name = currentCollar == null
                       ? animal.AnimalId
                       : animal.AnimalId + " (" + currentCollar.Collar + ")";
            if (animal.MortalityDate != null)
                name = String.Format("{0} (mort:{1:M/d/yy})", name, animal.MortalityDate.Value.ToLocalTime());
            return name;
        }

        private bool CanDeleteAnimal(Animal animal)
        {
            //An animal can't have any locations without a deployment
            return !Database.CollarDeployments.Any(d => d.Animal == animal);
        }

        private void EnableAnimalControls()
        {
            AddAnimalButton.Enabled = !IsEditMode && IsEditor;
            DeleteAnimalsButton.Enabled = !IsEditMode && IsEditor &&
                                          AnimalsListBox.SelectedItems.Cast<AnimalListItem>()
                                                        .Any(item => item.CanDelete);
            InfoAnimalsButton.Enabled = !IsEditMode && AnimalsListBox.SelectedItems.Count == 1;
        }

        private void AnimalDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpAnimalTab();
        }

        private void AddAnimalButton_Click(object sender, EventArgs e)
        {
            var form = new AddAnimalForm(Project);
            form.DatabaseChanged += (o, x) => AnimalDataChanged();
            form.Show(this);
        }

        private void DeleteAnimalsButton_Click(object sender, EventArgs e)
        {
            foreach (AnimalListItem item in AnimalsListBox.SelectedItems.Cast<AnimalListItem>().Where(item => item.CanDelete))
                Database.Animals.DeleteOnSubmit(item.Animal);
            if (SubmitChanges())
                AnimalDataChanged();
        }

        private void InfoAnimalButton_Click(object sender, EventArgs e)
        {
            var animal = ((AnimalListItem)AnimalsListBox.SelectedItem).Animal;
            var form = new AnimalDetailsForm(animal);
            form.DatabaseChanged += (o, args) => AnimalDataChanged();
            form.Show(this);
        }

        private void AnimalsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableAnimalControls();
        }

        #endregion


        #region File List

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // public accessors are used by the control when these classes are accessed through the Datasource
        class FileListItem
        {
            public string Name { get; set; }
            public bool CanDelete { get; set; }
            public CollarFile File { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        private void SetUpFileTab()
        {
            var query = from file in Database.CollarFiles
                        where file.Project == Project &&
                 (ShowDerivedFilesCheckBox.Checked || file.ParentFileId == null) &&
                 (ShowEmailFilesCheckBox.Checked || file.Format != 'E') &&
                 (ShowDownloadFilesCheckBox.Checked || file.Format != 'F')
                        select new FileListItem
                        {
                            File = file,
                            Name = file.FileName + (file.Status == 'I' ? " (Inactive)" : ""),
                            CanDelete = file.ParentFileId == null && !file.ArgosDownloads.Any()
                        };
            var sortedList = query.OrderBy(f => f.File.Status)
                                  .ThenByDescending(f => f.File.ParentFileId ?? f.File.FileId)
                                  .ThenByDescending(f => f.File.FileId)
                                  .ToList();
            FilesListBox.DataSource = sortedList;
            FilesListBox.DisplayMember = "Name";
            FilesListBox.ClearItemColors();
            for (int i = 0; i < sortedList.Count; i++)
            {
                if (sortedList[i].File.ParentFileId != null)
                    FilesListBox.SetItemColor(i, Color.Brown);
                if (sortedList[i].File.Format == 'E')
                    FilesListBox.SetItemColor(i, Color.MediumBlue);
                if (sortedList[i].File.Format == 'F')
                    FilesListBox.SetItemColor(i, Color.DarkMagenta);
                if (sortedList[i].File.Status == 'I')
                {
                    //Dim color of inactive files
                    var c = FilesListBox.GetItemColor(i);
                    FilesListBox.SetItemColor(i, ControlPaint.Light(c, 1.4f));
                }
            }
            FilesTabPage.Text = sortedList.Count < 5 ? "Files" : String.Format("Files ({0})", sortedList.Count);
        }

        private void EnableFileControls()
        {
            AddFilesButton.Enabled = !IsEditMode && IsEditor;
            DeleteFilesButton.Enabled = !IsEditMode && IsEditor &&
                                          FilesListBox.SelectedItems.Cast<FileListItem>()
                                                        .Any(item => item.CanDelete);
            InfoFilesButton.Enabled = !IsEditMode && FilesListBox.SelectedItems.Count == 1;
        }

        private void FileDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpFileTab();
        }

        private void AddFilesButton_Click(object sender, EventArgs e)
        {
            var form = new UploadFilesForm(Project);
            form.DatabaseChanged += (o, x) => FileDataChanged();
            form.Show(this);
        }

        private void DeleteFilesButton_Click(object sender, EventArgs e)
        {
            foreach (FileListItem item in AnimalsListBox.SelectedItems.Cast<FileListItem>().Where(item => item.CanDelete))
                Database.CollarFiles.DeleteOnSubmit(item.File);
            if (SubmitChanges())
                FileDataChanged();
        }

        private void InfoFileButton_Click(object sender, EventArgs e)
        {
            var file = ((FileListItem)FilesListBox.SelectedItem).File;
            var form = new FileDetailsForm(file);
            form.DatabaseChanged += (o, args) => FileDataChanged();
            form.Show(this);
        }

        private void FilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableFileControls();
        }

        private void ShowFilesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Visible)
                SetUpFileTab();
        }

        #endregion


        #region Editors List

        private void SetUpEditorsTab()
        {
            var editors = Project.ProjectEditors;
            EditorsListBox.DataSource = editors;
            EditorsListBox.DisplayMember = "Editor";
            EnableEditorControls();
        }

        private void EnableEditorControls()
        {
            AddEditorButton.Enabled = !IsEditMode && IsInvestigator;
            DeleteEditorButton.Enabled = !IsEditMode && EditorsListBox.SelectedItems.Count > 0 &&
                                            (IsInvestigator ||
                                             (IsEditor && EditorsListBox.SelectedItems.Count == 1 &&
                                              String.Equals(((ProjectEditor)EditorsListBox.SelectedItem).Editor.Normalize(),
                                                            CurrentUser.Normalize(), StringComparison.OrdinalIgnoreCase)));
        }

        private void EditorDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpEditorsTab();
        }

        private void AddEditorButton_Click(object sender, EventArgs e)
        {
            var form = new AddEditorForm(Project);
            form.DatabaseChanged += (o, x) => EditorDataChanged();
            form.Show(this);
        }

        private void DeleteEditorButton_Click(object sender, EventArgs e)
        {
            foreach (var item in EditorsListBox.SelectedItems)
                Database.ProjectEditors.DeleteOnSubmit((ProjectEditor)item);
            if (SubmitChanges())
                EditorDataChanged();
        }

        private void EditorsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableEditorControls();
        }

        #endregion


        #region QC Reports

        private XDocument _queryDocument;

        private void SetUpReportsTab()
        {
            var xmlFilePath = Properties.Settings.Default.ProjectReportsXml;
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
                                           r => ((string)r.Element("name")).Trim() == (string)ReportComboBox.SelectedItem);
            ReportDescriptionTextBox.Text = report == null ? null : (string)report.Element("description");
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
            command.Parameters.Add(new SqlParameter("@Project", SqlDbType.NVarChar) { Value = Project.ProjectId });
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
