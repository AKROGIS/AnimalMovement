using System;
using System.Linq;
using System.Windows.Forms;
using DataModel;

//FIXME - Code column is not resizable

namespace AnimalMovement
{
    internal partial class ProjectsForm : BaseForm
    {
        private string CurrentUser { get; set; }

        internal ProjectsForm(string user)
        {
            InitializeComponent();
            RestoreWindow();
            CurrentUser = user;
        }

        private void ReviewProjectsForm_Load(object sender, EventArgs e)
        {
            ReloadFromDatabase();
        }

        private void ReloadFromDatabase()
        {
            var db = new AnimalMovementDataContext();
            SelectProjectFilter();
            if (ShowHideButton.Text == "Show All Projects")
            {
                //show only my projects
                var myProjects = from p in db.Projects
                                 where p.ProjectInvestigator == CurrentUser ||
                                       p.ProjectEditors.Any(u => u.Editor == CurrentUser)
                                 select new
                                 {
                                     Code = p.ProjectId,
                                     Name = p.ProjectName,
                                     Lead = p.ProjectInvestigator1.Name,
                                     Unit = p.UnitCode,
                                     p.Description,
                                     CanDelete = p.ProjectInvestigator == CurrentUser && !p.Animals.Any() && !p.CollarFiles.Any()
                                 };
                ProjectsGridView.DataSource = myProjects;
            }
            else
            {
                // show all projects
                var allProjects = from p in db.Projects
                                  select new
                                  {
                                      Code = p.ProjectId,
                                      Name = p.ProjectName,
                                      Lead = p.ProjectInvestigator1.Name,
                                      Unit = p.UnitCode,
                                      p.Description,
                                      CanDelete = p.ProjectInvestigator == CurrentUser && !p.Animals.Any() && !p.CollarFiles.Any()
                                  };
                ProjectsGridView.DataSource = allProjects;
            }

            AddProjectButton.Enabled = db.ProjectInvestigators.Any(pi => pi.Login == CurrentUser);
        }

        void SelectProjectFilter()
        {
            ShowHideButton.Text = Settings.GetDefaultProjectFilter() ? "Show All Projects" : "Show Only My Projects";
        }

        private void ShowHideButton_Click(object sender, EventArgs e)
        {
            ShowHideButton.Text = ShowHideButton.Text == "Show Only My Projects" ? "Show All Projects" : "Show Only My Projects";
            Settings.SetDefaultProjectFilter(ShowHideButton.Text == "Show All Projects");
            ReloadFromDatabase();
        }

        private void AddProjectButton_Click(object sender, EventArgs e)
        {
            var form = new AddProjectForm(CurrentUser);
            if (form.ShowDialog(this) != DialogResult.Cancel)
                ReloadFromDatabase();
        }

        private void DeleteProjectButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in ProjectsGridView.SelectedRows.Cast<DataGridViewRow>().Where(row => (bool)row.Cells["columnCanDelete"].Value))
            {
                var projectId = (string)row.Cells["columnCode"].Value;
                var projectName = (string)row.Cells["columnName"].Value;
                try
                {
                    var database = new AnimalMovementDataContext();
                    database.Project_Delete(projectId);
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
            ReloadFromDatabase();
        }

        private void InfoProjectButton_Click(object sender, EventArgs e)
        {
            if (ProjectsGridView.CurrentRow == null)
                return; //This buttton is only enabled when Current Row is not not null
            var projectId = (string)ProjectsGridView.CurrentRow.Cells["columnCode"].Value;
            var form = new ProjectDetailsForm(projectId, CurrentUser);
            form.DatabaseChanged += (o, args) => { ReloadFromDatabase();
                                              SelectedRow(projectId);};
            form.Show(this);
        }

        private void SelectedRow(string projectId)
        {
            foreach (DataGridViewRow row in ProjectsGridView.Rows)
                if ((string)row.Cells["columnCode"].Value == projectId)
                    ProjectsGridView.CurrentCell = row.Cells["columnCode"];
        }

        private void ProjectsGridView_SelectionChanged(object sender, EventArgs e)
        {
            DeleteProjectButton.Enabled = false;
            if (ProjectsGridView.SelectedRows.Cast<DataGridViewRow>().Any(row => (bool)row.Cells["columnCanDelete"].Value))
                DeleteProjectButton.Enabled = true;
            InfoProjectButton.Enabled = ProjectsGridView.SelectedRows.Count == 1;
        }
    }
}
