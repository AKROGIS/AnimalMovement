using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using DataModel;
using System.Linq;

namespace AnimalMovement
{
    internal partial class ChangeInvestigatorForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        //private string ProjectId { get; set; }
        private ProjectInvestigator ProjectInvestigator { get; set; }
        private Project Project { get; set; }
        internal event EventHandler DatabaseChanged;

        internal ChangeInvestigatorForm(Project project)
        {
            InitializeComponent();
            RestoreWindow();
            Project = project;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpControls();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Project is in a different DataContext, get one in this DataContext
            if (Project != null)
                Project = Database.Projects.FirstOrDefault(p => p.ProjectId == Project.ProjectId);
            ProjectInvestigator = Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == CurrentUser);
        }

        private void SetUpControls()
        {
            LeadComboBox.DataSource = Database.ProjectInvestigators;
            LeadComboBox.ValueMember = "Login";
            LeadComboBox.DisplayMember = "Name";
            LeadComboBox.SelectedItem = ProjectInvestigator;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Project == null)
            {
                MessageBox.Show("The Change Investigator Form was not provide a valid project.", "Invalid Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel; //Closes form
                return;
            }
            if (ProjectInvestigator == null)
            {
                MessageBox.Show("You '" + CurrentUser + "' are not a Project Investigator in the database.", "Invalid Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel; //Closes form
                return;
            }
            if (ProjectInvestigator != Project.ProjectInvestigator1)
            {
                MessageBox.Show(Project.ProjectInvestigator1.Name + " is the Project Investigator for the project '" + Project.ProjectName + "' not you.", "Invalid Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel; //Closes form
            }
            EnableControls();
        }

        private void LeadComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void EnableControls()
        {
            ChangeButton.Enabled = LeadComboBox.SelectedItem != Project.ProjectInvestigator1;
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            Project.ProjectInvestigator1 = (ProjectInvestigator)LeadComboBox.SelectedItem;
            if (SubmitChanges())
            {
                OnDatabaseChanged();
                Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
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


    }
}
