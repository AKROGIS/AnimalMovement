using System;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class AddProjectForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private bool IndependentContext { get; set; }
        private string CurrentUser { get; set; }
        private bool IsProjectInvestigator { get; set; }
        internal event EventHandler DatabaseChanged;

        internal AddProjectForm(string user)
        {
            IndependentContext = true;
            CurrentUser = user;
            SetupForm();
        }

        internal AddProjectForm(AnimalMovementDataContext database, string user)
        {
            IndependentContext = false;
            Database = database;
            CurrentUser = user;
            SetupForm();
        }

        private void SetupForm()
        {
            InitializeComponent();
            RestoreWindow();
            LoadDataContext();
            EnableCreate();
        }

        private void LoadDataContext()
        {
            if (IndependentContext)
            {
                Database = new AnimalMovementDataContext();
            }
            if (Database == null || CurrentUser == null)
            {
                MessageBox.Show("Insufficent information to initialize form.", "Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            IsProjectInvestigator = Database.ProjectInvestigators.Any(pi => pi.Login == CurrentUser);
            ProjectInvestigatorComboBox.DataSource = Database.ProjectInvestigators;
            ProjectInvestigatorComboBox.DisplayMember = "Name";
            ProjectInvestigatorComboBox.SelectedItem =
                Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == CurrentUser);
        }

        private void EnableCreate()
        {
            CreateButton.Enabled = IsProjectInvestigator &&
                                   !string.IsNullOrEmpty(ProjectIdTextBox.Text) &&
                                   !string.IsNullOrEmpty(ProjectNameTextBox.Text);
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            string projectId = ProjectIdTextBox.Text;
            if (Database.Projects.Any(p => p.ProjectId == projectId))
            {
                MessageBox.Show("The project Id is not unique.  Try again", "Database Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                ProjectIdTextBox.Focus();
                CreateButton.Enabled = false;
                return;
            }
            var project = new Project
            {
                ProjectId = projectId,
                ProjectName = ProjectNameTextBox.Text.NullifyIfEmpty(),
                ProjectInvestigator1 = (ProjectInvestigator)ProjectInvestigatorComboBox.SelectedItem,
                UnitCode = UnitCodeTextBox.Text.NullifyIfEmpty(),
                Description = DescriptionTextBox.Text.NullifyIfEmpty()
            };
            Database.Projects.InsertOnSubmit(project);
            if (IndependentContext)
            {
                try
                {
                    Database.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Database.Projects.DeleteOnSubmit(project);
                    MessageBox.Show(ex.Message, "Unable to create new project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ProjectIdTextBox.Focus();
                    CreateButton.Enabled = false;
                    return;
                }
            }
            OnDatabaseChanged();
            DialogResult = DialogResult.OK;
        }

        private void CodeTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableCreate();
        }

        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableCreate();
        }

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
