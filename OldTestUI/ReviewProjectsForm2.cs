using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataModel;

namespace TestUI
{
    public partial class ReviewProjectsForm2 : Form
    {
        public ReviewProjectsForm2()
        {
            InitializeComponent();
        }

        private AnimalMovementDataContext _db;

        public string CurrentUser { get; set; }

        private void ReviewProjectsForm_Load(object sender, EventArgs e)
        {
            _db = new AnimalMovementDataContext();
            LoadProjects();
        }

        private void LoadProjects()
        {
            if (ShowHideButton.Text == "Show All Projects")
            {
                //show only my projects
                var myProjects = from p in _db.Projects
                                 where p.ProjectInvestigator == CurrentUser ||
                                       p.ProjectEditors.Any(u => u.Editor == CurrentUser)
                                 select new
                                 {
                                     Code = p.ProjectId,
                                     Name = p.ProjectName,
                                     Lead = "",
                                     Unit = p.UnitCode,
                                     p.Description,
                                     Project = p //hidden on UI
                                 };
                ProjectsGridView.DataSource = myProjects;
            }
            else
            {
                // show all projects
                var allProjects = from p in _db.Projects
                                  select new
                                  {
                                      Code = p.ProjectId,
                                      Name = p.ProjectName,
                                      Lead = p.ProjectInvestigator1.Name,
                                      Unit = p.UnitCode,
                                      p.Description,
                                      Project = p // hidden on UI
                                  };
                ProjectsGridView.DataSource = allProjects;
            }
            ProjectsGridView.Columns["columnProject"].Visible = false;
        }

        private void ShowHideButton_Click(object sender, EventArgs e)
        {
            if (ShowHideButton.Text == "Show Only My Projects")
            {
                ShowHideButton.Text = "Show All Projects";
                ProjectsGridView.Columns["columnLead"].Visible = false;
            }
            else
            {
                ShowHideButton.Text = "Show Only My Projects";
                ProjectsGridView.Columns["columnLead"].Visible = true;
            }
            LoadProjects();
        }

        private void AddProjectButton_Click(object sender, EventArgs e)
        {
            if (!_db.ProjectInvestigators.Any(pi => pi.Login == CurrentUser))
            {
                MessageBox.Show("You are not permitted to create projects.", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            var form = new AddProjectForm();
            form.ShowDialog(this);
            LoadProjects();
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
                var project = (Project)row.Cells["columnProject"].Value;
                //var projectName = (string)row.Cells["dataGridViewColumnName"].Value;
                try
                {
                    //AML.DeleteProject(projectCode);
                    _db.Projects.DeleteOnSubmit(project);
                    _db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    string msg = "Unable to delete project '" + project.ProjectName + "'.  " +
                                 "Projects can only be deleted by the project investigator " + 
                                 "and they cannot contain any files or animals.  " +
                                 "Please review the project properties.\n\n" +
                                 "Error message:\n" + ex.Message;
                    MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            LoadProjects();
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
            LoadProjects();
        }

    }
}
