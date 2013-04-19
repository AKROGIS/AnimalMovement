using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class CollarDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private AnimalMovementViews DatabaseViews { get; set; }
        private AnimalMovementFunctions DatabaseFunctions { get; set; }
        private string ManufacturerId { get; set; }
        private string CollarId { get; set; }
        private string CurrentUser { get; set; }
        private Collar Collar { get; set; }
        private bool Deployed { get; set; }
        private bool IsCollarOwner { get; set; }
        private bool IsEditMode { get; set; }
        internal event EventHandler DatabaseChanged;

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
            //Database.Log = Console.Out;
            DatabaseFunctions = new AnimalMovementFunctions();
            DatabaseViews = new AnimalMovementViews();
            Collar = Database.Collars.FirstOrDefault(c => c.CollarManufacturer == ManufacturerId && c.CollarId == CollarId);
            if (Collar == null)
            {
                MessageBox.Show("Collar not found.", "Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            IsCollarOwner = string.Equals(Collar.Manager.Normalize(), CurrentUser.Normalize(), StringComparison.OrdinalIgnoreCase);
            Deployed = Collar.CollarDeployments.Any(d => d.RetrievalDate == null);
            ManufacturerTextBox.Text = Collar.LookupCollarManufacturer.Name;
            CollarIdTextBox.Text = Collar.CollarId;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CollarTabs.SelectedIndex = Properties.Settings.Default.CollarDetailsFormActiveTab;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Properties.Settings.Default.CollarDetailsFormActiveTab = CollarTabs.SelectedIndex;
        }

        private void CollarTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (CollarTabs.SelectedIndex)
            {
                default:
                    SetupGeneralTab();
                    break;
                case 1:
                    SetupAnimalsTab();
                    break;
                case 2:
                    SetupArgosTab();
                    break;
                case 3:
                    SetupParametersTab();
                    break;
                case 4:
                    SetupFilesTab();
                    break;
                case 5:
                    SetupFixesTab();
                    break;
            }

        }


        #region General Tab

        private void SetupGeneralTab()
        {
            ManagerComboBox.DataSource = Database.ProjectInvestigators;
            ManagerComboBox.DisplayMember = "Name";
            ManagerComboBox.SelectedItem = Collar.ProjectInvestigator;
            ModelComboBox.DataSource = Database.LookupCollarModels.Where(m => m.CollarManufacturer == Collar.CollarManufacturer);
            ModelComboBox.DisplayMember = "CollarModel";
            ModelComboBox.SelectedItem = Collar.LookupCollarModel;
            HasGpsCheckBox.Checked = Collar.HasGps;
            OwnerTextBox.Text = Collar.Owner;
            SerialNumberTextBox.Text = Collar.SerialNumber;
            FrequencyTextBox.Text = Collar.Frequency.HasValue ? Collar.Frequency.Value.ToString(CultureInfo.InvariantCulture) : null;
            NotesTextBox.Text = Collar.Notes;
            EditSaveButton.Enabled = IsCollarOwner;
            ConfigureDatePicker();
            SetEditingControls();
        }

        private void ConfigureDatePicker()
        {
            if (Collar.DisposalDate == null)
            {
                var now = DateTime.Now;
                DisposalDateTimePicker.Value = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0);
                DisposalDateTimePicker.Checked = false;
                DisposalDateTimePicker.CustomFormat = " ";
            }
            else
            {
                DisposalDateTimePicker.Checked = true;
                DisposalDateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm";
                DisposalDateTimePicker.Value = Collar.DisposalDate.Value.ToLocalTime();
            }
        }

        private void SetEditingControls()
        {
            IsEditMode = EditSaveButton.Text == "Save";
            ManagerComboBox.Enabled = IsEditMode;
            ModelComboBox.Enabled = IsEditMode;
            HasGpsCheckBox.Enabled = IsEditMode;
            OwnerTextBox.Enabled = IsEditMode;
            SerialNumberTextBox.Enabled = IsEditMode;
            FrequencyTextBox.Enabled = IsEditMode;
            DisposalDateTimePicker.Enabled = IsEditMode;
            NotesTextBox.Enabled = IsEditMode;

            AnimalInfoButton.Enabled = !IsEditMode && DeploymentDataGridView.RowCount > 0;
            DeleteDeploymentButton.Enabled = !IsEditMode && IsCollarOwner && DeploymentDataGridView.RowCount > 0;
            DeployRetrieveButton.Enabled = !IsEditMode && IsCollarOwner;
            ChangeFileStatusButton.Enabled = !IsEditMode;
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
                SetupGeneralTab();
            }
            else
            {
                Close();
            }
        }

        private void DisposalDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DisposalDateTimePicker.CustomFormat = DisposalDateTimePicker.Checked ? "yyyy-MM-dd HH:mm" : " ";
        }

        private void UpdateDataSource()
        {
            Collar.ProjectInvestigator = (ProjectInvestigator)ManagerComboBox.SelectedItem;
            Collar.LookupCollarModel = (LookupCollarModel)ModelComboBox.SelectedItem;
            Collar.HasGps = HasGpsCheckBox.Checked;
            Collar.Owner = OwnerTextBox.Text;
            Collar.SerialNumber = SerialNumberTextBox.Text;
            Collar.Frequency = FrequencyTextBox.Text.DoubleOrNull() ?? 0;
            Collar.Notes = NotesTextBox.Text;
            if (DisposalDateTimePicker.Checked)
                Collar.DisposalDate = DisposalDateTimePicker.Value.ToUniversalTime();
            else
                Collar.DisposalDate = null;
        }

        #endregion


        #region Animals Tab

        //FIXME - Allow editing of deployment and retrieval dates
        //TODO - Move Info and Delete buttons onto the data grid.

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

        private void SetupAnimalsTab()
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
                    DeploymentDate = d.DeploymentDate.ToLocalTime(),
                    RetrievalDate = d.RetrievalDate.HasValue ? d.RetrievalDate.Value.ToLocalTime() : (DateTime?)null,
                    Deployment = d,
                };
            DeployRetrieveButton.Text = Deployed ? "Retrieve" : "Deploy";
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
            LoadDataContext();
            SetupAnimalsTab();
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
            SetupAnimalsTab();
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

        #endregion


        #region Argos Tab

        private void SetupArgosTab()
        {
            ArgoDataGridView.DataSource =
                Collar.ArgosDeployments.Select(a => new
                {
                    Argos_Id = a.PlatformId,
                    Start = a.StartDate == null ? "Long ago" : a.StartDate.Value.ToString("d"),
                    End = a.EndDate == null ? "Never" : a.EndDate.Value.ToString("d")
                }).ToList();
        }

        #endregion


        #region Parameters Tab

        //TODO - View parameter file info on the Collar Dialog
        //TODO - Identify collars with multiple parameter files, so dates can be added

        private void SetupParametersTab()
        {
            switch (Collar.CollarModel)
            {
                case "Gen3":
                    ParametersDataGridView.DataSource =
                        Collar.CollarParameters.Select(
                            p =>
                            new
                                {
                                    Period = p.Gen3Period < 60 ? p.Gen3Period + " min" : p.Gen3Period/60 + " hrs",
                                    File = p.CollarParameterFile == null ? null : p.CollarParameterFile.FileName,
                                    Start = p.StartDate == null ? "Long ago" : p.StartDate.Value.ToString("d"),
                                    End = p.EndDate == null ? "Never" : p.EndDate.Value.ToString("d")
                                }).ToList();
                    break;
                case "Gen4":
                    ParametersDataGridView.DataSource =
                        Collar.CollarParameters.Select(p => new {p.CollarParameterFile.FileName, p.StartDate, p.EndDate})
                              .ToList();
                    break;
            }
        }

        #endregion


        #region Files Tab

        private void SetupFilesTab()
        {
            if (Collar == null)
                return;
            FilesDataGridView.DataSource = DatabaseViews.CollarFixesByFile(Collar.CollarManufacturer, Collar.CollarId);
            EnableFileButtons();
        }

        private void EnableFileButtons()
        {
            FileInfoButton.Enabled = FilesDataGridView.CurrentRow != null && !IsEditMode && FilesDataGridView.SelectedRows.Count == 1;
            ChangeFileStatusButton.Enabled = FilesDataGridView.CurrentRow != null && !IsEditMode;
            if (FilesDataGridView.SelectedRows.Count > 1)
            {
                var firstRowStatus = (string)FilesDataGridView.SelectedRows[0].Cells["Status"].Value;
                ChangeFileStatusButton.Text = firstRowStatus != "Active" ? "Activate" : "Deactivate";
                if (FilesDataGridView.SelectedRows.Cast<DataGridViewRow>()
                                     .Any(row => (string)row.Cells["Status"].Value != firstRowStatus))
                    ChangeFileStatusButton.Enabled = false;
            }
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

        private void FilesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            EnableFileButtons();
        }

        private void ChangeFileStatusButton_Click(object sender, EventArgs e)
        {
            //If the button is labeled/enabled correctly, then the user wants to change the status of all the selected files
            var newStatus = Database.LookupFileStatus.First(s => s.Code == 'A');
            if (ChangeFileStatusButton.Text == "Deactivate")
                newStatus = Database.LookupFileStatus.First(s => s.Code == 'I');

            var fileIds = FilesDataGridView.SelectedRows.Cast<DataGridViewRow>()
                                           .Select(row => (int)row.Cells["FileId"].Value).ToList();
            var files = Database.CollarFiles.Where(f => fileIds.Contains(f.FileId));
            foreach (var collarFile in files)
                collarFile.LookupFileStatus = newStatus;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                string msg = "Unable to change the status.\n" +
                             "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
            OnDatabaseChanged();
            LoadDataContext();
            SetupFilesTab();
        }

        #endregion


        #region Fixes Tab

        private void SetupFixesTab()
        {
            if (Collar == null)
                return;
            FixConflictsDataGridView.DataSource = DatabaseViews.ConflictingFixes(Collar.CollarManufacturer, Collar.CollarId, 36500); //last 100 years
            var summary = DatabaseViews.CollarFixSummary(Collar.CollarManufacturer, Collar.CollarId).FirstOrDefault();
            SummaryLabel.Text = summary == null
                              ? "There are NO fixes."
                              : (summary.Count == summary.Unique
                                 ? String.Format("{0} fixes between {1:yyyy-MM-dd} and {2:yyyy-MM-dd}.", summary.Count, summary.First, summary.Last)
                                 : String.Format("{3}/{0} unique/total fixes between {1:yyyy-MM-dd} and {2:yyyy-MM-dd}.", summary.Count, summary.First, summary.Last, summary.Unique));
            FixConflictsDataGridView_SelectionChanged(null, null);
        }

        private void FixConflictsDataGridView_SelectionChanged(object sender, EventArgs e)
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
            bool isEditor = DatabaseFunctions.IsFixEditor(selectedFix.FixId, CurrentUser) ?? false;
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
                return;
            }
            OnDatabaseChanged();
            LoadDataContext();
            SetupFixesTab();
        }


        #endregion


        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }

    }
}
