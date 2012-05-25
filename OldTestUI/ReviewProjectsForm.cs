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
    public partial class ReviewProjectsForm : Form
    {
        public ReviewProjectsForm()
        {
            InitializeComponent();
        }

        public string CurrentUser { get; set; }

        private void ReviewProjectsForm_Load(object sender, EventArgs e)
        {
            LoadBindingSources();
        }

        private void LoadBindingSources()
        {
            Project.Refresh();
            ProjectsGridView.DataSource = Project.Projects;
            //this.ProjectsTableAdapter.Fill(this.animal_MovementDataSet.AllProjects);
            //this.myProjectsTableAdapter.Fill(this.animal_MovementDataSet.MyProjects, CurrentUser);
        }

        private void ShowHideButton_Click(object sender, EventArgs e)
        {
            if (ShowHideButton.Text == "Show Only My Projects")
            {
                ShowHideButton.Text = "Show All Projects";
                ProjectsGridView.DataSource = Project.Projects;
            }
            else
            {
                ShowHideButton.Text = "Show Only My Projects";
                ProjectsGridView.DataSource = Project.Projects;
            }
        }

        private void AddProjectButton_Click(object sender, EventArgs e)
        {
            if (!Investigator.IsInvestigator(CurrentUser))
            {
                MessageBox.Show("You are not permitted to create projects.", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            var form = new AddProjectForm();
            form.ShowDialog(this);
            LoadBindingSources();
        }

        private void DeleteProjectButton_Click(object sender, EventArgs e)
        {
            if (ProjectsGridView.SelectedRows.Count < 1)
            {
                if (ProjectsGridView.CurrentRow != null)
                    ProjectsGridView.Rows[ProjectsGridView.CurrentRow.Index].Selected = true;
            }
            if (ProjectsGridView.SelectedRows.Count < 1)
            {
                string msg = "You must select one or more rows";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            foreach (DataGridViewRow row in ProjectsGridView.SelectedRows)
            {
                var projectCode = (string)row.Cells["dataGridViewColumnCode"].Value;
                var projectName = (string)row.Cells["dataGridViewColumnName"].Value;
                try
                {
                    AML.DeleteProject(projectCode);
                    LoadBindingSources();
                }
                catch (Exception ex)
                {
                    string msg = "Unable to delete project '" + projectName + "'.  " +
                                 "Projects can only be deleted by the project investigator " + 
                                 "and they cannot contain any files or animals.  " +
                                 "Please review the project properties.\n\n" +
                                 "Error message:\n" + ex.Message;
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InfoProjectButton_Click(object sender, EventArgs e)
        {
            if (ProjectsGridView.SelectedRows.Count < 1 && ProjectsGridView.CurrentRow != null)
                ProjectsGridView.Rows[ProjectsGridView.CurrentRow.Index].Selected = true;

            if (ProjectsGridView.CurrentRow == null || ProjectsGridView.SelectedRows.Count > 1)
            {
                MessageBox.Show("You must first select a single project");
                return;
            }

            var projectCode = (string)ProjectsGridView.CurrentRow.Cells["dataGridViewColumnCode"].Value;
            var form = new ProjectDetailsForm {ProjectId = projectCode, CurrentUser = CurrentUser};
            form.Show(this);
            LoadBindingSources();

        }

    }
}
