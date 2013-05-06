using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class AddDeploymentCsvForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private ProjectInvestigator Investigator { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        public AddDeploymentCsvForm(ProjectInvestigator investigator = null)
        {
            InitializeComponent();
            Investigator = investigator;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpControls();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Investigator is in a different DataContext, get one in this DataContext
            if (Investigator != null)
                Investigator = Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == Investigator.Login);

            //Validate Program and Editor on load, so we can show a messagebox.
        }

        private void SetUpControls()
        {
            SetUpInvestigatorComboBox();
            EnableControls();
        }

        private void SetUpInvestigatorComboBox()
        {
            //If given a Investigator, set that and lock it.
            //else, set list to all projects I can edit, and select null per the constructor request
            if (Investigator != null)
                InvestigatorComboBox.Items.Add(Investigator);
            else
            {
                InvestigatorComboBox.DataSource =
                    Database.ProjectInvestigators.Where(pi => pi.Login == CurrentUser ||
                     pi.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser));
            }
            InvestigatorComboBox.Enabled = Investigator == null;
            InvestigatorComboBox.SelectedItem = Investigator;
            InvestigatorComboBox.DisplayMember = "Name";
        }

        private void EnableControls()
        {
            ProcessButton.Enabled = IsEditor && InvestigatorComboBox.SelectedItem != null &&
                                   !string.IsNullOrEmpty(FileNameTextBox.Text);
        }

        private void ValidateEditor()
        {
            //If Investigator is provided, it will be locked, so you must be the pi or an assistant
            if (Investigator != null && Investigator.Login != CurrentUser &&
                Investigator.ProjectInvestigatorAssistants.All(a => a.Assistant != CurrentUser))
            {
                MessageBox.Show(
                    "You do not have permission to load deployments for this investigator.", "No Permission",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsEditor = false;
                return;
            }
            //If Investigator is not provided, make sure there is a investigator (we can edit) to pick from
            if (Investigator == null && InvestigatorComboBox.Items.Count == 0)
            {
                MessageBox.Show(
                    "You can't load deployments unless you are a PI or a PI's assistant.", "No Permission",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsEditor = false;
                return;
            }
            IsEditor = true; //Hope for the best
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

        #region Form Control Events

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ValidateEditor();
            EnableControls();
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            //do The work
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
                FileNameTextBox.Text = openFileDialog1.FileName;
        }

        private void InvestigatorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void FileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        #endregion
    }
}
