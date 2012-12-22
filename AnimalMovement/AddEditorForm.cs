using System;
#if ! NO_ACTIVE_DIRECTORY
using System.DirectoryServices.AccountManagement;
#endif
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class AddEditorForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private bool IndependentContext { get; set; }
        private string CurrentUser { get; set; }
        private string ProjectId { get; set; }
        private Project Project { get; set; }
        private bool IsProjectInvestigator { get; set; }
        internal event EventHandler DatabaseChanged;

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // public accessors are used by the control when these classes are accessed through the Datasource
        private class EditorListItem
        {
            public string DisplayName { get; set; }
            public string DomainName { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        internal AddEditorForm(string projectId, string user)
        {
            IndependentContext = true;
            ProjectId = projectId;
            CurrentUser = user;
            SetupForm();
        }

        internal AddEditorForm(AnimalMovementDataContext database, Project project, string user)
        {
            IndependentContext = false;
            Database = database;
            Project = project ;
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
                //Weirdness: Project points into our datacontext, which gets changed
                //when we requery the projects table, so setting it here doen't work
                //Project = Database.Projects.FirstOrDefault(p => p.ProjectId == ProjectId);   
            }
            if (Database == null || ProjectId == null || CurrentUser == null)
            {
                MessageBox.Show("Insufficent information to initialize form.", "Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            IsProjectInvestigator = Database.ProjectInvestigators.Any(pi => pi.Login == CurrentUser);
            ProjectComboBox.DataSource = Database.Projects.Where(p => p.ProjectInvestigator == CurrentUser);
            Project = Database.Projects.FirstOrDefault(p => p.ProjectId == ProjectId);
            ProjectComboBox.DisplayMember = "ProjectName";
            ProjectComboBox.SelectedItem = Project;
        }

        private void EnableCreate()
        {
            CreateButton.Enabled = IsProjectInvestigator && Project != null && ResultsListBox.SelectedItem != null;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            string domainName = ((EditorListItem)ResultsListBox.SelectedItem).DomainName;

            if (Database.ProjectEditors.Any(pe => pe.Project == Project && pe.Editor == domainName))
            {
                MessageBox.Show("The editor is already on the project.  Try again", "Database Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                EditorTextBox.Focus();
                CreateButton.Enabled = false;
                return;
            }
            var editor = new ProjectEditor
            {
                Project = Project,
                Editor = domainName,
            };
            Database.ProjectEditors.InsertOnSubmit(editor);
            if (IndependentContext)
            {
                try
                {
                    Database.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Database.ProjectEditors.DeleteOnSubmit(editor);
                    MessageBox.Show(ex.Message, "Unable to create the new editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EditorTextBox.Focus();
                    CreateButton.Enabled = false;
                    return;
                }
            }
            OnDatabaseChanged();
            DialogResult = DialogResult.OK;
        }

        private void ProjectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Project = ProjectComboBox.SelectedItem as Project;
            EnableCreate();
        }

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }

        private void ResultsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableCreate();
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
#if NO_ACTIVE_DIRECTORY
			MessageBox.Show("Active Directory Search Disabled.", "Sorry", MessageBoxButtons.OK,
			                MessageBoxIcon.Information);
#else
			var context = new PrincipalContext(ContextType.Domain, "nps", "OU=AKR,DC=nps,DC=doi,DC=net");
            string search = EditorTextBox.Text + "*";
            var principal = new UserPrincipal(context) { Surname = search, Enabled = true };
            var searcher = new PrincipalSearcher { QueryFilter = principal };
            var query = from Principal p in searcher.FindAll()
                        orderby p.DisplayName
                        select new EditorListItem
                                   {
                                       DisplayName = p.Name + " (" + p.Description + ") - NPS\\" + p.SamAccountName,
                                       DomainName = "NPS\\" + p.SamAccountName,
                                   };
            ResultsListBox.DisplayMember = "DisplayName";
            var data = query.ToList();
            ResultsListBox.DataSource = data;
            EnableCreate(); //If the Datasource is empty, then SelectedIndexChanged is not called.
            if (data.Count > 0)
                ResultsListBox.Focus();
            else
            {
                MessageBox.Show("Nobody found with that name.", "Try again", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                EditorTextBox.Focus();
            }
#endif
        }

        private void EditorTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            AcceptButton = null;
            if (e.KeyCode != Keys.Enter)
                return;
            e.Handled = true;
            FindButton.PerformClick();
            AcceptButton = CreateButton;
        }

    }}
