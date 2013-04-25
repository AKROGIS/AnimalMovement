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
    internal partial class ProjectDetailsForm : BaseForm
    {

        private AnimalMovementDataContext Database { get; set; }
        private AnimalMovementFunctions DatabaseFunctions { get; set; }
        private string CurrentUser { get; set; }
        private string ProjectId { get; set; }
        private Project Project { get; set; }
        private bool IsProjectInvestigator { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // public accessors are used by the control when these classes are accessed through the Datasource
        class AnimalListItem
        {
            public string Name { get; set; }
            public bool CanDelete { get; set; }
            public Animal Animal { get; set; }
        }

        class FileListItem
        {
            public string Name { get; set; }
            public bool CanDelete { get; set; }
            public CollarFile File { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        internal ProjectDetailsForm(string projectId, string user)
        {
            ProjectId = projectId;
            CurrentUser = user;
            SetupForm();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Properties.Settings.Default.ProjectDetailsFormActiveTab = ProjectTabs.SelectedIndex;
            Properties.Settings.Default.ProjectDetailsFormShowEmailFiles = ShowEmailFilesCheckBox.Checked;
            Properties.Settings.Default.ProjectDetailsFormShowDownloadFiles = ShowDownloadFilesCheckBox.Checked;
            Properties.Settings.Default.ProjectDetailsFormShowDerivedFiles = ShowDerivedFilesCheckBox.Checked;
        }

        private void SetupForm()
        {
            InitializeComponent();
            RestoreWindow();
            LoadDataContext();
            ShowEmailFilesCheckBox.Checked = Properties.Settings.Default.ProjectDetailsFormShowEmailFiles;
            ShowDownloadFilesCheckBox.Checked = Properties.Settings.Default.ProjectDetailsFormShowDownloadFiles;
            ShowDerivedFilesCheckBox.Checked = Properties.Settings.Default.ProjectDetailsFormShowDerivedFiles;
            ProjectTabs.SelectedIndex = Properties.Settings.Default.ProjectDetailsFormActiveTab;
            if (ProjectTabs.SelectedIndex == 0)
                ProjectTabs_SelectedIndexChanged(null, null);
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            DatabaseFunctions = new AnimalMovementFunctions();
            Project = Database.Projects.FirstOrDefault(p => p.ProjectId == ProjectId);
            if (Database == null || Project == null || ProjectId == null || CurrentUser == null)
            {
                MessageBox.Show("Insufficent information to initialize form.", "Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            IsProjectInvestigator = IsCurrentUserTheInvestigatorForThisProject();
            IsEditor = DatabaseFunctions.IsProjectEditor(Project.ProjectId, CurrentUser) ?? false;
            ProjectCodeTextBox.Text = Project.ProjectId;
            DescriptionTextBox.Text = Project.Description;
            UnitTextBox.Text = Project.UnitCode;
            ProjectNameTextBox.Text = Project.ProjectName;
            InvestigatorTextBox.Text = Project.ProjectInvestigator;
            //Whenever we load the datasource, we should reenable the form (we may not have the permissions we did)
            EnableForm();
        }

        private void SetEditorList()
        {
            //Using the EntitySet seems to mostly work, but it does not get refreshed when we submit changes
            //var editors = Project.ProjectEditors;
            //This query does not always create something DataSource can accept (IList<>)
            //var editors = from editor in Project.ProjectEditors select editor;
            //This query goes directly to the database, and seems to always work.
            var editors = from editor in Database.ProjectEditors
                          where editor.Project == Project
                          select editor;
            EditorsListBox.DataSource = editors;
            EditorsListBox.DisplayMember = "Editor";
        }

        private void SetAnimalList()
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
            var name   = currentCollar == null
                       ? animal.AnimalId
                       : animal.AnimalId + " (" + currentCollar.Collar + ")";
            if (animal.MortalityDate != null)
                name = String.Format("{0} (mort:{1:M/d/yy})", name, animal.MortalityDate.Value.ToLocalTime());
            return name;
        }

        private void SetFileList()
        {
            var query =  from file in Database.CollarFiles
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

        private bool CanDeleteAnimal(Animal animal)
        {
            //An animal can't have any locations without a deployment
            return !Database.CollarDeployments.Any(d => d.Animal == animal);
        }

        private void UpdateDataSource()
        {
            Project.Description = DescriptionTextBox.Text;
            Project.UnitCode = UnitTextBox.Text;
            Project.ProjectName = ProjectNameTextBox.Text;
        }

        private void EnableForm()
        {
            EditInvestigatorButton.Enabled = IsProjectInvestigator;
            EditSaveButton.Enabled = IsEditor || IsProjectInvestigator;
            SetEditingControls();
        }

        private void SetEditingControls()
        {
            bool editModeEnabled = EditSaveButton.Text == "Save";
            ProjectNameTextBox.Enabled = editModeEnabled;
            DescriptionTextBox.Enabled = editModeEnabled;
            UnitTextBox.Enabled = editModeEnabled;

            EditInvestigatorButton.Enabled = !editModeEnabled && IsProjectInvestigator;
            InvestigatorDetailsButton.Enabled = !editModeEnabled;

            AddAnimalButton.Enabled = !editModeEnabled && IsEditor;
            AddFilesButton.Enabled = !editModeEnabled && IsEditor;
            AddEditorButton.Enabled = !editModeEnabled && IsProjectInvestigator;
            //Set the Delete/Info buttons based on what is selected
            EditorsListBox_SelectedIndexChanged(null, null);
            AnimalsListBox_SelectedIndexChanged(null, null);
            FilesListBox_SelectedIndexChanged(null, null);
        }

        private bool IsCurrentUserTheInvestigatorForThisProject()
        {
            return Database.Projects.Any(p => p.ProjectId == Project.ProjectId && p.ProjectInvestigator == CurrentUser);
        }


        #region form control events

        private void EditInvestigatorButton_Click(object sender, EventArgs e)
        {
            var form = new ChangeInvestigatorForm(Project.ProjectId);
            if (form.ShowDialog(this) == DialogResult.Cancel)
                return;
            OnDatabaseChanged();
            LoadDataContext();
        }

        private void InvestigatorDetailsButton_Click(object sender, EventArgs e)
        {
            var form = new InvestigatorForm(Project.ProjectInvestigator, CurrentUser);
            form.DatabaseChanged += (o, args) => { OnDatabaseChanged(); LoadDataContext(); };
            form.Show(this);
        }

        private void AddEditorButton_Click(object sender, EventArgs e)
        {
            //Adding editors in this context is not supported.
            //var form = new AddEditorForm(Database, Project, CurrentUser);
            var form = new AddEditorForm(ProjectId, CurrentUser);
            if (form.ShowDialog(this) == DialogResult.Cancel)
                return;
            OnDatabaseChanged();
            LoadDataContext();
        }

        private void DeleteEditorButton_Click(object sender, EventArgs e)
        {
            //If we have permission, We can always delete any/all project editors
            foreach (ProjectEditor editor in EditorsListBox.SelectedItems)
                Database.ProjectEditors.DeleteOnSubmit(editor);
            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                string msg = "Unable to delete one or more of the selected editors\n" +
                                "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            OnDatabaseChanged();
            SetEditorList();
            EditorsListBox_SelectedIndexChanged(null, null);
        }

        private void AddFilesButton_Click(object sender, EventArgs e)
        {
            //The add happens in a new context, so we need to reload this context if changes were made
            var form = new UploadFilesForm(Project);
            form.DatabaseChanged += (o, args) => { OnDatabaseChanged(); LoadDataContext(); };
            form.Show(this);
        }

        private void DeleteFilesButton_Click(object sender, EventArgs e)
        {
            foreach (FileListItem item in FilesListBox.SelectedItems.Cast<FileListItem>().Where(item => item.CanDelete))
                Database.CollarFiles.DeleteOnSubmit(item.File);
            //Deleting an active file takes time to remove the locations; assume at least one file is active
            Cursor.Current = Cursors.WaitCursor;
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
            Cursor.Current = Cursors.Default;
            OnDatabaseChanged();
            SetFileList();
            FilesListBox_SelectedIndexChanged(null, null);
        }

        private void InfoFilesButton_Click(object sender, EventArgs e)
        {
            //If the user make changes in the info dialog, they happen in a different context, so we need to reload this context if changes were made
            var file = ((FileListItem)FilesListBox.SelectedItem).File;
            var form = new FileDetailsForm(file.FileId, CurrentUser);
            form.DatabaseChanged += (o, args) => { OnDatabaseChanged(); LoadDataContext(); };
            form.Show(this);
        }

        private void AddAnimalButton_Click(object sender, EventArgs e)
        {
            //The add happens in a new context, so we need to reload this context if changes were made
            var form = new AddAnimalForm(ProjectId, CurrentUser);
            //Adding animals in this context is not supported.
            //var form = new AddAnimalForm(Database, Project, CurrentUser);
            if (form.ShowDialog(this) == DialogResult.Cancel)
                return;
            OnDatabaseChanged();
            LoadDataContext();
        }

        private void DeleteAnimalsButton_Click(object sender, EventArgs e)
        {
            foreach (AnimalListItem item in AnimalsListBox.SelectedItems.Cast<AnimalListItem>().Where(item => item.CanDelete))
                Database.Animals.DeleteOnSubmit(item.Animal);
            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                string msg = "Unable to delete one or more of the selected animals\n" +
                             "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            OnDatabaseChanged();
            SetAnimalList();
            AnimalsListBox_SelectedIndexChanged(null, null);
        }

        private void InfoAnimalsButton_Click(object sender, EventArgs e)
        {
            //If the user make changes in the info dialog, they happen in a different context, so we need to reload this context if changes were made
            var animal = ((AnimalListItem)AnimalsListBox.SelectedItem).Animal;
            var form = new AnimalDetailsForm(ProjectId, animal.AnimalId, CurrentUser);
            form.DatabaseChanged += (o, args) => { OnDatabaseChanged(); LoadDataContext(); };
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
                SetEditingControls();
            }
            else
            {
                //User is saving
                UpdateDataSource();
                try
                {
                    Database.SubmitChanges();
                }
                catch (Exception ex)
                {
                    string msg = "Unable to save all the changes.\n" +
                                 "Error message:\n" + ex.Message;
                    MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                OnDatabaseChanged();
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

        private void EditorsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DeleteEditorButton.Enabled = false;
            if (EditSaveButton.Text == "Save" || !IsProjectInvestigator)
                return;
            if (EditorsListBox.SelectedItems.Count > 0)
                DeleteEditorButton.Enabled = true;
        }

        private void AnimalsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InfoAnimalsButton.Enabled = false;
            DeleteAnimalsButton.Enabled = false;
            if (EditSaveButton.Text == "Save")
                return;
            InfoAnimalsButton.Enabled = AnimalsListBox.SelectedItems.Count == 1;
            if (IsEditor && AnimalsListBox.SelectedItems.Cast<AnimalListItem>().Any(item => item.CanDelete))
                DeleteAnimalsButton.Enabled = true;
        }

        private void FilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            InfoFilesButton.Enabled = false;
            DeleteFilesButton.Enabled = false;
            if (EditSaveButton.Text == "Save")
                return;
            InfoFilesButton.Enabled = FilesListBox.SelectedItems.Count == 1;
            if (IsEditor && FilesListBox.SelectedItems.Cast<FileListItem>().Any(item => item.CanDelete))
                DeleteFilesButton.Enabled = true;
        }


        private void ProjectTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ProjectTabs.SelectedIndex)
            {
                case 0:
                    SetAnimalList();
                    break;
                case 1:
                    SetFileList();
                    break;
                case 2:
                    SetEditorList();
                    break;
            }
        }


        private void ShowFilesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetFileList();
        }

        #endregion

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this,EventArgs.Empty);
        }
    }

}
