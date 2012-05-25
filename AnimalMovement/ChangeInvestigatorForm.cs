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
        private Project Project { get; set; }

        internal ChangeInvestigatorForm(string projectId, string user)
        {
            InitializeComponent();
            RestoreWindow();
            CurrentUser = user;
            LoadFormFromDatabase(projectId);
        }

        private void LoadFormFromDatabase(string projectId)
        {
            Database = new AnimalMovementDataContext();
            Project = Database.Projects.FirstOrDefault(p => p.ProjectId == projectId);
            if (Project == null)
            {
                MessageBox.Show("The project ID '" + projectId +"' was not found in the database.", "Invalid Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
            }
            LeadComboBox.DataSource = Database.ProjectInvestigators;
            LeadComboBox.ValueMember = "Login";
            LeadComboBox.DisplayMember = "Name";
            LeadComboBox.SelectedItem = LeadComboBox.Items.Cast<ProjectInvestigator>().First(pi => 
                string.Equals(pi.Login, CurrentUser, StringComparison.OrdinalIgnoreCase));
        }

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            if (string.Equals(Project.ProjectInvestigator, (string)LeadComboBox.SelectedValue,StringComparison.OrdinalIgnoreCase))
                DialogResult = DialogResult.Cancel;

            try
            {
                Project.ProjectInvestigator1 = (ProjectInvestigator)LeadComboBox.SelectedItem;
                Database.SubmitChanges();
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable To Update Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
