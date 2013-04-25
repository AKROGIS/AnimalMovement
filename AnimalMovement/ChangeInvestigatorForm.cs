using System;
using System.Windows.Forms;
using DataModel;
using System.Linq;

namespace AnimalMovement
{
    internal partial class ChangeInvestigatorForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private string ProjectId { get; set; }
        private ProjectInvestigator ProjectInvestigator { get; set; }
        private Project Project { get; set; }

        internal ChangeInvestigatorForm(string projectId)
        {
            InitializeComponent();
            RestoreWindow();
            ProjectId = projectId;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            Project = Database.Projects.FirstOrDefault(p => p.ProjectId == ProjectId);
            ProjectInvestigator = Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == CurrentUser);
            LeadComboBox.DataSource = Database.ProjectInvestigators;
            LeadComboBox.ValueMember = "Login";
            LeadComboBox.DisplayMember = "Name";
            LeadComboBox.SelectedItem = ProjectInvestigator;
        }

        private void ChangeInvestigatorForm_Load(object sender, EventArgs e)
        {
            if (Project == null)
            {
                MessageBox.Show("The project ID '" + ProjectId + "' was not found in the database.", "Invalid Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("You '" + CurrentUser + "' are not the Project Investigator for the project '" + ProjectId +"'", "Invalid Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel; //Closes form
            }
            EnableChangeButton();
        }

        private void LeadComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableChangeButton();
        }

        private void EnableChangeButton()
        {
            ChangeButton.Enabled = LeadComboBox.SelectedItem != Project.ProjectInvestigator1;
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            try
            {
                Project.ProjectInvestigator1 = (ProjectInvestigator)LeadComboBox.SelectedItem;
                Database.SubmitChanges();
                DialogResult = DialogResult.OK; //Closes form
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable To Update Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
