using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DataModel;

//TODO - Allow editing of deployment and retrieval dates
//TODO - Delay population of hidden tabs until displayed.
//TODO - Move Info and Delete buttons onto the data grid.
//TODO - Conflicting Fixes needs better support (don't show fixes hidden by store on board)

namespace AnimalMovement
{
    internal partial class CollarDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string ManufacturerId { get; set; }
        private string CollarId { get; set; }
        private string CurrentUser { get; set; }
        private Collar Collar { get; set; }
        private bool Deployed { get; set; }
        private bool IsCollarOwner { get; set; }
        internal event EventHandler DatabaseChanged;

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // public accessors are used by the control when these classes are accessed through the Datasource
        class DeploymentDataItem
        {
            public string Manufacturer { get; set; }
            public string CollarId { get; set; }
            public string Project { get; set; }
            public string AnimalId { get; set; }
            public DateTime? DeploymentDate { get; set; }
            public DateTime? RetrievalDate { get; set; }
            public CollarDeployment Deployment { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        internal CollarDetailsForm(string mfgrId, string collarId, string user)
        {
            InitializeComponent();
            RestoreWindow();
            ManufacturerId = mfgrId;
            CollarId = collarId;
            CurrentUser = user;
            LoadDataContext();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            Collar = Database.Collars.FirstOrDefault(c => c.CollarManufacturer == ManufacturerId && c.CollarId == CollarId);
            if (Collar == null)
            {
                MessageBox.Show("Collar not found.", "Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            IsCollarOwner = string.Equals(Collar.Manager, CurrentUser, StringComparison.OrdinalIgnoreCase);
            ManufacturerTextBox.Text = Collar.LookupCollarManufacturer.Name;
            CollarIdTextBox.Text = Collar.CollarId;
            ManagerComboBox.DataSource = Database.ProjectInvestigators;
            ManagerComboBox.DisplayMember = "Name";
            ManagerComboBox.SelectedItem = Collar.ProjectInvestigator;
            ModelComboBox.DataSource = Database.LookupCollarModels;
            ModelComboBox.DisplayMember = "CollarModel";
            ModelComboBox.SelectedItem = Collar.LookupCollarModel;
            AlternativeIdTextBox.Text = Collar.AlternativeId;
            OwnerTextBox.Text = Collar.Owner;
            SerialNumberTextBox.Text = Collar.SerialNumber;
            FrequencyTextBox.Text = Collar.Frequency.HasValue ?  Collar.Frequency.Value.ToString(CultureInfo.InvariantCulture) : null;
            DownloadInfoTextBox.Text = Collar.DownloadInfo;
            NotesTextBox.Text = Collar.Notes;
            SetDeploymentDataGrid();
            SetFixesGrid();
            SetFilesGrid();
            Deployed = Collar.CollarDeployments.Any(d => d.RetrievalDate == null);
            EnableForm();
        }

        private void SetFilesGrid()
        {
            if (Collar == null)
                return;
            var db = new AnimalMovementViewsDataContext();
            FilesDataGridView.DataSource = db.CollarFixesByFile(Collar.CollarManufacturer, Collar.CollarId);
        }

        private void SetFixesGrid()
        {
            if (Collar == null)
                return;
            var db = new AnimalMovementViewsDataContext();
            FixConflictsDataGridView.DataSource = db.ConflictingFixes(Collar.CollarManufacturer, Collar.CollarId);
            var summary = db.CollarFixSummary(Collar.CollarManufacturer, Collar.CollarId).FirstOrDefault();
            SummaryLabel.Text = summary == null
                              ? "There are NO fixes."
                              : String.Format("{0} fixes between {1:yyyy-MM-dd} and {2:yyyy-MM-dd}.", summary.Count, summary.First, summary.Last);
        }

        private void SetDeploymentDataGrid()
        {
            DeploymentDataGridView.DataSource =
                from d in Database.CollarDeployments
                where d.Collar == Collar
                orderby d.DeploymentDate
                select new DeploymentDataItem
                        {
                            Manufacturer = d.Collar.LookupCollarManufacturer.Name,
                            CollarId = d.CollarId,
                            Project = d.Animal.Project.ProjectName,
                            AnimalId = d.AnimalId,
                            DeploymentDate = d.DeploymentDate,
                            RetrievalDate = d.RetrievalDate,
                            Deployment = d,
                        };
        }

        private void UpdateDataSource()
        {
            Collar.ProjectInvestigator = (ProjectInvestigator)ManagerComboBox.SelectedItem;
            Collar.LookupCollarModel = (LookupCollarModel)ModelComboBox.SelectedItem;
            Collar.AlternativeId = AlternativeIdTextBox.Text;
            Collar.Owner = OwnerTextBox.Text;
            Collar.SerialNumber = SerialNumberTextBox.Text;
            Collar.Frequency = FrequencyTextBox.Text.DoubleOrNull() ?? 0;
            Collar.DownloadInfo = DownloadInfoTextBox.Text;
            Collar.Notes = NotesTextBox.Text;
        }

        private void EnableForm()
        {
            EditSaveButton.Enabled = IsCollarOwner;
            DeployRetrieveButton.Text = Deployed ? "Retrieve" : "Deploy";
            SetEditingControls();
            EnableUnideFixButton();
        }

        private void SetEditingControls()
        {
            bool editModeEnabled = EditSaveButton.Text == "Save";
            ManagerComboBox.Enabled = editModeEnabled;
            ModelComboBox.Enabled = editModeEnabled;
            AlternativeIdTextBox.Enabled = editModeEnabled;
            OwnerTextBox.Enabled = editModeEnabled;
            SerialNumberTextBox.Enabled = editModeEnabled;
            FrequencyTextBox.Enabled = editModeEnabled;
            DownloadInfoTextBox.Enabled = editModeEnabled;
            NotesTextBox.Enabled = editModeEnabled;

            AnimalInfoButton.Enabled = !editModeEnabled && DeploymentDataGridView.RowCount > 0;
            DeleteDeploymentButton.Enabled = !editModeEnabled && IsCollarOwner && DeploymentDataGridView.RowCount > 0;
            DeployRetrieveButton.Enabled = !editModeEnabled && IsCollarOwner;
        }

        private void DeleteDeploymentButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DeploymentDataGridView.SelectedRows)
            {
                var deployment = (DeploymentDataItem)row.DataBoundItem;
                Database.CollarDeployments.DeleteOnSubmit(deployment.Deployment);
            }
            //when we delete a deployment, we may delete locations, which takes time.
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                string msg = "Unable to delete one or more of the selected deployments\n" +
                                "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
            OnDatabaseChanged();
            SetDeploymentDataGrid();
            Deployed = Collar.CollarDeployments.Any(d => d.RetrievalDate == null);
            EnableForm();
        }

        private void DeployRetrieveButton_Click(object sender, EventArgs e)
        {
            if (DeployRetrieveButton.Text == "Retrieve")
            {
                CollarDeployment deployment = Collar.CollarDeployments.First(d => d.RetrievalDate == null);
                var form = new RetrieveCollarForm(deployment);
                if (form.ShowDialog(this) == DialogResult.Cancel)
                    return;
            }
            else  //Deploy
            {
                var form = new DeployCollarForm(Database, Collar, CurrentUser);
                if (form.ShowDialog(this) == DialogResult.Cancel)
                    return;
            }
            //when we deploy or retrieve a collar, we may add or delete locations, which takes time.
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                string msg = "Unable to save the changes.\n" +
                             "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
            OnDatabaseChanged();
            LoadDataContext();
        }

        private void EditSaveButton_Click(object sender, EventArgs e)
        {
            //This button is not enabled unless editing is permitted 
            if (EditSaveButton.Text == "Edit")
            {
                // The user wants to edit, Enable form
                EditSaveButton.Text = "Save";
                DoneCancelButton.Text = "Cancel";
                SetEditingControls();
            }
            else
            {
                //User is saving
                UpdateDataSource();
                try
                {
                    Database.SubmitChanges();
                }
                catch (Exception ex)
                {
                    string msg = "Unable to save all the changes.\n" +
                                 "Error message:\n" + ex.Message;
                    MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                OnDatabaseChanged();
                EditSaveButton.Text = "Edit";
                DoneCancelButton.Text = "Done";
                SetEditingControls();
            }

        }

        private void DoneCancelButton_Click(object sender, EventArgs e)
        {
            if (DoneCancelButton.Text == "Cancel")
            {
                DoneCancelButton.Text = "Done";
                EditSaveButton.Text = "Edit";
                SetEditingControls();
                //Reset state from database
                LoadDataContext();
            }
            else
            {
                Close();
            }
        }

        private void AnimalInfoButton_Click(object sender, EventArgs e)
        {
            if (DeploymentDataGridView.CurrentRow == null)
                return;
            var item = DeploymentDataGridView.CurrentRow.DataBoundItem as DeploymentDataItem;
            if (item == null)
                return;
            var form = new AnimalDetailsForm(item.Deployment.ProjectId, item.AnimalId, CurrentUser);
            form.Show(this);
        }

        private void FileInfoButton_Click(object sender, EventArgs e)
        {
            if (FilesDataGridView.CurrentRow == null)
                return;
            var item = FilesDataGridView.CurrentRow.DataBoundItem as CollarFixesByFileResult;
            if (item == null)
                return;
            var form = new FileDetailsForm(item.FileId, CurrentUser);
            form.Show(this);

        }

        private void FixConflictsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            EnableUnideFixButton();
        }

        private void EnableUnideFixButton()
        {
            if (FixConflictsDataGridView.CurrentRow == null)
            {
                UnhideFixButton.Enabled = false;
                return;
            }
            var selectedFix = FixConflictsDataGridView.CurrentRow.DataBoundItem as ConflictingFixesResult;
            if (selectedFix == null)
            {
                UnhideFixButton.Enabled = false;
                return;
            }
            bool isEditor = Database.IsFixEditor(selectedFix.FixId, CurrentUser) ?? false;
            bool isHidden = selectedFix.HiddenBy != null;
            UnhideFixButton.Enabled = isEditor && isHidden;
        }

        private void UnhideFixButton_Click(object sender, EventArgs e)
        {
            if (FixConflictsDataGridView.CurrentRow == null)
                return;
            var selectedFix = FixConflictsDataGridView.CurrentRow.DataBoundItem as ConflictingFixesResult;
            if (selectedFix == null)
                return;
            try
            {
                Database.CollarFixes_UpdateUnhideFix(selectedFix.FixId);
            }
            catch (Exception ex)
            {
                string msg = "Unable to update the fix.\n" +
                             "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            SetFixesGrid();
        }

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }
    }
}
