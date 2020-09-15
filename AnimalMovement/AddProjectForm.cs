using DataModel;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace AnimalMovement
{
    internal partial class AddProjectForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private ProjectInvestigator Investigator { get; set; }
        internal event EventHandler DatabaseChanged;

        internal AddProjectForm()
        {
            InitializeComponent();
            RestoreWindow();
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpForm();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            Investigator = Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == CurrentUser);
        }

        private void SetUpForm()
        {
            var investigators = from pi in Database.ProjectInvestigators
                                where pi.Login == CurrentUser ||
                                      pi.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser)
                                select pi;
            ProjectInvestigatorComboBox.DataSource = investigators;
            ProjectInvestigatorComboBox.DisplayMember = "Name";
            ProjectInvestigatorComboBox.SelectedItem = Investigator;
            EnableControls();
        }

        private void EnableControls()
        {
            CreateButton.Enabled = ProjectInvestigatorComboBox.SelectedItem != null &&
                                   !string.IsNullOrEmpty(ProjectIdTextBox.Text) &&
                                   !string.IsNullOrEmpty(ProjectNameTextBox.Text);
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

        private Project GetValidProject()
        {
            if (Database.Projects.Any(p => p.ProjectId == ProjectIdTextBox.Text))
            {
                MessageBox.Show("The project Id is not unique.  Try again", "Database Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                ProjectIdTextBox.Focus();
                CreateButton.Enabled = false;
                return null;
            }
            return new Project
            {
                ProjectId = ProjectIdTextBox.Text,
                ProjectName = ProjectNameTextBox.Text.NullifyIfEmpty(),
                ProjectInvestigator1 = (ProjectInvestigator)ProjectInvestigatorComboBox.SelectedItem,
                UnitCode = UnitCodeTextBox.Text.NullifyIfEmpty(),
                Description = DescriptionTextBox.Text.NullifyIfEmpty()
            };
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var project = GetValidProject();
            if (project == null)
            {
                return;
            }

            Database.Projects.InsertOnSubmit(project);
            if (SubmitChanges())
            {
                OnDatabaseChanged();
                Close();
            }
            ProjectIdTextBox.Focus();
            CreateButton.Enabled = false;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CodeTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls();
        }
    }
}
