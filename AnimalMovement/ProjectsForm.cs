using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class ProjectsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;
        private object _myProjects;
        private object _allProjects;

        internal ProjectsForm()
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
            IsEditor =
                Database.ProjectInvestigators.Any(
                    pi =>
                    pi.Login == CurrentUser || pi.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser));
            MakeLists();
        }

        private void MakeLists()
        {
            _myProjects = (from p in Database.Projects
                          where p.ProjectInvestigator == CurrentUser ||
                                p.ProjectEditors.Any(u => u.Editor == CurrentUser)
                          select new
                              {
                                  p.ProjectId,
                                  p.ProjectName,
                                  Lead = p.ProjectInvestigator1.Name,
                                  p.UnitCode,
                                  p.Description,
                                  CanDelete =
                                     (p.ProjectInvestigator == CurrentUser || 
                                      p.ProjectInvestigator1.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser))
                                     && !p.Animals.Any() && !p.CollarFiles.Any(),
                                  Project = p
                              }).ToList();
            _allProjects = (from p in Database.Projects
                           select new
                               {
                                   p.ProjectId,
                                   p.ProjectName,
                                   Lead = p.ProjectInvestigator1.Name,
                                   p.UnitCode,
                                   p.Description,
                                   CanDelete =
                                     (p.ProjectInvestigator == CurrentUser ||
                                      p.ProjectInvestigator1.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser))
                                     && !p.Animals.Any() && !p.CollarFiles.Any(),
                                   Project = p
                               }).ToList();
        }

        private void SetUpForm()
        {
            SelectProjectFilter();
            ProjectsGridView.DataSource = ShowHideButton.Text == "Show All Projects" ? _myProjects : _allProjects;
            EnableControls();
        }

        private void EnableControls()
        {
            ProjectsGridView.Columns[6].Visible = false;
            AddProjectButton.Enabled = IsEditor;
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

        private void ProjectDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpForm();
        }

        void SelectProjectFilter()
        {
            ShowHideButton.Text = Settings.GetDefaultProjectFilter() ? "Show All Projects" : "Show Only My Projects";
        }

        private void ShowHideButton_Click(object sender, EventArgs e)
        {
            ShowHideButton.Text = ShowHideButton.Text == "Show Only My Projects" ? "Show All Projects" : "Show Only My Projects";
            Settings.SetDefaultProjectFilter(ShowHideButton.Text == "Show All Projects");
            SetUpForm();
        }

        private void AddProjectButton_Click(object sender, EventArgs e)
        {
            var form = new AddProjectForm();
            form.DatabaseChanged += (o, x) => ProjectDataChanged();
            form.Show(this);
        }

        private void DeleteProjectButton_Click(object sender, EventArgs e)
        {
            foreach (Project project in
                ProjectsGridView.SelectedRows.Cast<DataGridViewRow>()
                                .Where(row => (bool) row.Cells["CanDelete"].Value)
                                .Select(row => row.Cells["Project"].Value))
                Database.Projects.DeleteOnSubmit(project);
            if (SubmitChanges())
                ProjectDataChanged();

        }

        private void InfoProjectButton_Click(object sender, EventArgs e)
        {
            if (ProjectsGridView.CurrentRow == null)
                return; //This buttton is only enabled when Current Row is not not null
            var project = (Project)ProjectsGridView.CurrentRow.Cells["Project"].Value;
            var form = new ProjectDetailsForm(project);
            form.DatabaseChanged += (o, args) =>
                {
                    ProjectDataChanged();
                    SelectProjectRow(project.ProjectId);
                };
            form.Show(this);
        }

        private void SelectProjectRow(string projectId)
        {
            foreach (DataGridViewRow row in ProjectsGridView.Rows)
                if ((string)row.Cells["ProjectId"].Value == projectId)
                    ProjectsGridView.CurrentCell = row.Cells["ProjectId"];
        }

        private void ProjectsGridView_SelectionChanged(object sender, EventArgs e)
        {
            DeleteProjectButton.Enabled = ProjectsGridView.SelectedRows.Cast<DataGridViewRow>().Any(row => (bool)row.Cells["CanDelete"].Value);
            InfoProjectButton.Enabled = ProjectsGridView.SelectedRows.Count == 1;
        }

        private void ProjectsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && InfoProjectButton.Enabled)
                InfoProjectButton_Click(sender, e);
        }
    }
}
