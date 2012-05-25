using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AnimalMovementLibrary;

namespace TestUI
{
    public partial class ChangeInvestigatorForm : Form
    {
        public ChangeInvestigatorForm(string user, string project)
        {
            InitializeComponent();
            CurrentUser = user;
            ProjectId = project;
            AML.Connect();
            _project = Project.FromName(project);
            LeadComboBox.DataSource = Investigator.Investigators;
            LeadComboBox.ValueMember = "Login";
            LeadComboBox.DisplayMember = "Name";
        }

        public string CurrentUser { get; set; }
        public string ProjectId { get; set; }
        private readonly Project _project;

        private void ChangeButton_Click(object sender, EventArgs e)
        {
            if (_project == null)
            {
                Close();
                return;
            }
            if (_project.Investigator.Login == (string)LeadComboBox.SelectedValue)
            {
                Close();
                return;
            }
            try
            {
                _project.UpdateProject(null, (string)LeadComboBox.SelectedValue, null, null);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable To Update Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
