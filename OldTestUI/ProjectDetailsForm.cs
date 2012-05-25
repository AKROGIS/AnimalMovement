using System;
using System.Data;
using System.Windows.Forms;
using AnimalMovementLibrary;

namespace TestUI
{
    public partial class ProjectDetailsForm : Form
    {
        public ProjectDetailsForm()
        {
            InitializeComponent();
        }

        public string CurrentUser { get; set; }

        public string ProjectId
        { 
            get { return _projectId; }
            set
            {
                if (_projectId == value)
                    return;
                _projectId = value;
                UpdateForm(_projectId);
            }
        }

        private string _projectId;
        private Project _project; 

        private void UpdateForm(string project)
        {
            ProjectCodeTextBox.Text = project;
            LoadBindingSources();
        }

        private void EditInvestigatorButton_Click(object sender, EventArgs e)
        {
            if (ProjectId == null)
                return;

            if (!Project.IsInvestigator(CurrentUser, ProjectId))
            {
                MessageBox.Show("You are not the investigator of the project.", "Permission denied", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            var form = new ChangeInvestigatorForm(CurrentUser, ProjectId);
            form.ShowDialog(this);
            LoadBindingSources();
        }

        private void InvestigatorDetailsButton_Click(object sender, EventArgs e)
        {
            var form = new InvestigatorDetailsForm(_project.Investigator.Login);
            form.ShowDialog(this);
        }

        private void AddEditorButton_Click(object sender, EventArgs e)
        {
            if (ProjectId == null)
                return;

            if (!Project.IsInvestigator(CurrentUser, ProjectId))
            {
                MessageBox.Show("You are not the investigator of the project.", "Permission denied", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            var form = new AddEditorForm(CurrentUser) { Project = ProjectId };
            form.ShowDialog(this);
            LoadBindingSources();
        }

        private void DeleteEditorButton_Click(object sender, EventArgs e)
        {
            if (ProjectId == null)
                return;

            if (FilesListBox.SelectedItems.Count < 1)
            {
                const string msg = "You must select one or more editors";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            foreach (DataRowView row in EditorsListBox.SelectedItems)
            {
                var editor = (string)row[0];
                try
                {
                    AML.DeleteEditor(ProjectId, editor);
                    LoadBindingSources();
                }
                catch (Exception ex)
                {
                    string msg = "Unable to delete editor '" + editor + "'.  " +
                                 "Error message:\n" + ex.Message;
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        //FIXME - need to manipulate FileId, but show FileName
        //FIXME - show inactive files differently
        //FIXME - provide interface to change file status

        private void AddFilesButton_Click(object sender, EventArgs e)
        {
            if (ProjectId == null)
                return;

            if (!AML.IsEditor(CurrentUser, ProjectId))
            {
                MessageBox.Show("You are not permitted to add files to this project.", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            var form = new AddFileForm(CurrentUser) {Project = ProjectId};
            form.ShowDialog(this);
            LoadBindingSources();
        }

        private void DeleteFilesButton_Click(object sender, EventArgs e)
        {
            if (ProjectId == null)
                return;

            if (FilesListBox.SelectedItems.Count < 1)
            {
                const string msg = "You must select one or more files";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            foreach (DataRowView row in FilesListBox.SelectedItems)
            {
                var fileId = (int)row["FileId"];
                try
                {
                    AML.DeleteFile(fileId);
                    LoadBindingSources();
                }
                catch (Exception ex)
                {
                    string msg = "Unable to delete file '" + fileId + "'.  " +
                                 "Error message:\n" + ex.Message;
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InfoFilesButton_Click(object sender, EventArgs e)
        {
            if (ProjectId == null)
                return;

            if (FilesListBox.SelectedItems.Count < 1 || FilesListBox.SelectedItems.Count > 1)
            {
                MessageBox.Show("You must first select a single file");
                return;
            }
            var row = (DataRowView)FilesListBox.SelectedItem;
            var fileId = (int)row["FileId"];
            var form = new FileDetailsForm(fileId);
            form.Show(this);
        }

        private void AddAnimalButton_Click(object sender, EventArgs e)
        {
            if (ProjectId == null)
                return;

            if (!AML.IsEditor(CurrentUser, ProjectId))
            {
                MessageBox.Show("You are not permitted to add animals in this project.", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            var form = new AddAnimalForm(ProjectId);
            form.ShowDialog(this);
            LoadBindingSources();
        }

        private void DeleteAnimalsButton_Click(object sender, EventArgs e)
        {
            if (ProjectId == null)
                return;

            if (AnimalsListBox.SelectedItems.Count < 1)
            {
                const string msg = "You must select one or more animals";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            foreach (DataRowView row in AnimalsListBox.SelectedItems)
            {
                var animalId = (string)row[0]; 
                try
                {
                    AML.DeleteAnimal(ProjectId, animalId);
                    LoadBindingSources();
                }
                catch (Exception ex)
                {
                    string msg = "Unable to delete animal '" + animalId + "'.  " +
                                 "Animals can only be deleted by a project editor " +
                                 "and they cannot have any associated location data.  " +
                                 "Please review the animal properties.\n\n" +
                                 "Error message:\n" + ex.Message;
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InfoAnimalsButton_Click(object sender, EventArgs e)
        {
            if (ProjectId == null)
                return;

            if (AnimalsListBox.SelectedItems.Count < 1 || AnimalsListBox.SelectedItems.Count > 1)
            {
                MessageBox.Show("You must first select a single animal");
                return;
            }
            var row = (DataRowView)AnimalsListBox.SelectedItem;
            var animalId = (string)row[0];
            var form = new AnimalDetailsForm(ProjectId, animalId);
            form.Show(this);
        }

        private void LoadBindingSources()
        {
            Project.Refresh();
            _project = Project.FromName(_projectId);
            ProjectNameTextBox.Text = _project.Name;
            DescriptionTextBox.Text = _project.Description;
            UnitTextBox.Text = _project.Unit;
            ProjectNameTextBox.Text = _project.Name;
            //InvestigatorTextBox.Text = _project.Investigator;
            InvestigatorTextBox.Text = _project.Investigator.Name;
            SetEditorList();
            SetAnimalList();
            SetFileList();
        }

        private void SetFileList()
        {
            FilesListBox.DataSource = _project.Files;
            FilesListBox.ValueMember = "FileId";
            FilesListBox.DisplayMember = "FileName";
        }

        private void SetEditorList()
        {
            EditorsListBox.DataSource = _project.Editors;
            EditorsListBox.ValueMember = "Editor";
            EditorsListBox.DisplayMember = "Editor";
        }

        private void SetAnimalList()
        {
            AnimalsListBox.DataSource = _project.Animals;
            AnimalsListBox.ValueMember = "AnimalId";
            AnimalsListBox.DisplayMember = "AnimalId";
        }

        private void ProjectNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_project == null)
                return;
            try
            {
                _project.UpdateProject(ProjectNameTextBox.Text, null, null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable To Update Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DescriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_project == null)
                return;
            try
            {
                _project.UpdateProject(null, null, null, DescriptionTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable To Update Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void UnitTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_project == null)
                return;
            try
            {
                _project.UpdateProject(null, null, UnitTextBox.Text, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable To Update Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
