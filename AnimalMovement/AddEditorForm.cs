using System;
#if ! NO_ACTIVE_DIRECTORY
using System.Data.SqlClient;
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
        private string CurrentUser { get; set; }
        private bool LockSelector { get; set; }
        private Project Project { get; set; }
        private ProjectInvestigator Investigator { get; set; }
        private bool IsProjectInvestigator { get; set; }
        internal event EventHandler DatabaseChanged;

        internal AddEditorForm(Project project, ProjectInvestigator investigator = null, bool lockSelector = false)
        {
            InitializeComponent();
            RestoreWindow();
            Project = project;
            Investigator = investigator;
            LockSelector = lockSelector;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            EnableControls();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Project and Investigator are in a different DataContext, get them in this DataContext
            if (Project != null)
                Project = Database.Projects.FirstOrDefault(p => p.ProjectId == Project.ProjectId);
            if (Investigator != null)
                Investigator = Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == Investigator.Login);
            if (Project == null && Investigator == null)
                throw new InvalidOperationException("Add Editor Form not provided a valid Project or Project Investigator.");
            if (Project != null && Investigator != null)
                throw new InvalidOperationException("Add Editor Form cannot have both a valid Project AND Project Investigator.");

            IsProjectInvestigator = Database.ProjectInvestigators.Any(pi => pi.Login == CurrentUser);
            SetupControls();
        }

        private void SetupControls()
        {
            if (Investigator != null)
            {
                //no one can add editors (assistants) to another PI, so a selector is not appropriate
                LockSelector = true;
                SelectorComboBox.Items.Add(Investigator);
                SelectorComboBox.SelectedItem = Investigator;
                SelectorComboBox.DisplayMember = "Name";
                SelectorComboBox.Enabled = false;
                ProjectLabel.Visible = false;
                InvestigatorLabel.Visible = true;
            }
            if (Project != null)
            {
                if (LockSelector)
                    SelectorComboBox.Items.Add(Project);
                else
                    SelectorComboBox.DataSource = Database.Projects.Where(p => p.ProjectInvestigator == CurrentUser);
                SelectorComboBox.SelectedItem = Project;
                SelectorComboBox.DisplayMember = "ProjectName";
                SelectorComboBox.Enabled = !LockSelector;
                ProjectLabel.Visible = true;
                InvestigatorLabel.Visible = false;
            }
        }

        private void EnableControls()
        {
            CreateButton.Enabled = IsProjectInvestigator && SelectorComboBox.SelectedItem != null && ResultsListBox.SelectedItem != null;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            string domainName = ((EditorListItem)ResultsListBox.SelectedItem).DomainName;
            if (Investigator != null)
                CreateAssistant(domainName);
            if (Project != null)
                CreateEditor(domainName);
        }

        private void CreateEditor(string domainName)
        {
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
            if (!SubmitChanges())
            {
                Database.ProjectEditors.DeleteOnSubmit(editor);
                EditorTextBox.Focus();
                CreateButton.Enabled = false;
                return;
            }
            OnDatabaseChanged();
            Close();
        }

        private void CreateAssistant(string domainName)
        {
            if (Database.ProjectInvestigatorAssistants.Any(a => a.ProjectInvestigator1 == Investigator && a.Assistant == domainName))
            {
                MessageBox.Show("The assistant is already helping the investigator.  Try again", "Database Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                EditorTextBox.Focus();
                CreateButton.Enabled = false;
                return;
            }
            var assistant = new ProjectInvestigatorAssistant
            {
                ProjectInvestigator1 = Investigator,
                Assistant = domainName,
            };
            Database.ProjectInvestigatorAssistants.InsertOnSubmit(assistant);
            if (!SubmitChanges())
            {
                Database.ProjectInvestigatorAssistants.DeleteOnSubmit(assistant);
                EditorTextBox.Focus();
                CreateButton.Enabled = false;
                return;
            }
            OnDatabaseChanged();
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

        private void ProjectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void ResultsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // public accessors are used by the control when these classes are accessed through the Datasource
        private class EditorListItem
        {
            public string DisplayName { get; set; }
            public string DomainName { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local


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
            EnableControls(); //If the Datasource is empty, then SelectedIndexChanged is not called.
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
