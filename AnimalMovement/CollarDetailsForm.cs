using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DataModel;

//FIXME - disable 'Delete' deployment button when there are no deployments
//TODO - Add list of files to with data for this collar to form

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
            Deployed = Collar.CollarDeployments.Any(d => d.RetrievalDate == null);
            EnableForm();
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
            Collar.AlternativeId = AlternativeIdTextBox.Text.NullifyIfEmpty();
            Collar.Owner = DownloadInfoTextBox.Text.NullifyIfEmpty();
            Collar.SerialNumber = DownloadInfoTextBox.Text.NullifyIfEmpty();
            Collar.Frequency = DownloadInfoTextBox.Text.DoubleOrNull();
            Collar.DownloadInfo = DownloadInfoTextBox.Text.NullifyIfEmpty();
            Collar.Notes = NotesTextBox.Text.NullifyIfEmpty();
        }

        private void EnableForm()
        {
            EditSaveButton.Enabled = IsCollarOwner;
            DeployRetrieveButton.Text = Deployed ? "Retrieve" : "Deploy";
            SetEditingControls();
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

            DeleteDeploymentButton.Enabled = !editModeEnabled && IsCollarOwner;
            DeployRetrieveButton.Enabled = !editModeEnabled && IsCollarOwner;
        }

        private void DeleteDeploymentButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DeploymentDataGridView.SelectedRows)
            {
                var deployment = (DeploymentDataItem)row.DataBoundItem;
                Database.CollarDeployments.DeleteOnSubmit(deployment.Deployment);
            }
            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                string msg = "Unable to delete one or more of the selected deploymnets\n" +
                                "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            else
            {
                var form = new DeployCollarForm(Database, Collar, CurrentUser);
                if (form.ShowDialog(this) == DialogResult.Cancel)
                    return;
            }
            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                string msg = "Unable to save the changes.\n" +
                             "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }

    }
}
