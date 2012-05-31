using System;
using System.Linq;
using System.Windows.Forms;
using DataModel;

//TODO - Move Delete, Retrieve and Info button onto the data grid.
//TODO - Add Location information on form below deployments
//TODO - Include spatial properties, centroid, bounding box, MCP
//TODO - Add Info button to get Collar details (for those who do not know to double click)

namespace AnimalMovement
{
    internal partial class AnimalDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string ProjectId { get; set; }
        private string AnimalId { get; set; }
        private string CurrentUser { get; set; }
        private Animal Animal { get; set; }
        private bool Collared { get; set; }
        private bool IsAnimalEditor { get; set; }
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

        internal AnimalDetailsForm(string projectId, string animalId, string user)
        {
            InitializeComponent();
            RestoreWindow();
            ProjectId = projectId;
            AnimalId = animalId;
            CurrentUser = user;
            LoadDataContext();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            Animal = Database.Animals.FirstOrDefault(a => a.ProjectId == ProjectId && a.AnimalId == AnimalId);
            if (Animal == null)
            {
                MessageBox.Show("Animal not found.", "Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            IsAnimalEditor = Database.IsEditor(Animal.ProjectId, CurrentUser) ?? false;
            ProjectTextBox.Text = Animal.Project.ProjectName;
            AnimalIdTextBox.Text = Animal.AnimalId;
            SpeciesComboBox.DataSource = Database.LookupSpecies;
            SpeciesComboBox.DisplayMember = "Species";
            SpeciesComboBox.SelectedItem = Animal.LookupSpecies;
            GenderComboBox.DataSource = Database.LookupGenders;
            GenderComboBox.DisplayMember = "Sex";
            GenderComboBox.SelectedItem = Animal.LookupGender;
            GroupTextBox.Text = Animal.GroupName;
            DescriptionTextBox.Text = Animal.Description;
            SetDeploymentDataGrid();
            Collared = Animal.CollarDeployments.Any(d => d.RetrievalDate == null);
            EnableForm();
        }

        private void SetDeploymentDataGrid()
        {
            DeploymentDataGridView.DataSource =
                from d in Database.CollarDeployments
                where d.Animal == Animal
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
            Animal.LookupSpecies = (LookupSpecies)SpeciesComboBox.SelectedItem;
            Animal.LookupGender = (LookupGender)GenderComboBox.SelectedItem;
            Animal.GroupName = GroupTextBox.Text.NullifyIfEmpty();
            Animal.Description = DescriptionTextBox.Text.NullifyIfEmpty();
        }

        private void EnableForm()
        {
            EditSaveButton.Enabled = IsAnimalEditor;
            DeployRetrieveButton.Text = Collared ? "Retrieve" : "Deploy";
            SetEditingControls();
        }

        private void SetEditingControls()
        {
            bool editModeEnabled = EditSaveButton.Text == "Save";
            SpeciesComboBox.Enabled = editModeEnabled;
            GenderComboBox.Enabled = editModeEnabled;
            GroupTextBox.Enabled = editModeEnabled;
            DescriptionTextBox.Enabled = editModeEnabled;

            DeleteDeploymentButton.Enabled = !editModeEnabled && IsAnimalEditor && DeploymentDataGridView.RowCount > 0;
            DeployRetrieveButton.Enabled = !editModeEnabled && IsAnimalEditor;
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
            Collared = Animal.CollarDeployments.Any(d => d.RetrievalDate == null);
            EnableForm();
        }

        private void DeployRetrieveButton_Click(object sender, EventArgs e)
        {
            if (DeployRetrieveButton.Text == "Retrieve")
            {
                CollarDeployment deployment = Animal.CollarDeployments.First(d => d.RetrievalDate == null);
                var form = new RetrieveCollarForm(deployment);
                if (form.ShowDialog(this) == DialogResult.Cancel)
                    return;
            }
            else
            {
                var form = new CollarAnimalForm(Database, Animal, CurrentUser);
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

        private void DeploymentDataGridView_DoubleClick(object sender, EventArgs e)
        {
            if (DeploymentDataGridView.CurrentRow == null)
                return;
            var item = DeploymentDataGridView.CurrentRow.DataBoundItem as DeploymentDataItem;
            if (item == null)
                return;
            var form = new CollarDetailsForm(item.Deployment.CollarManufacturer, item.CollarId, CurrentUser);
            form.Show(this);
        }

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }

    }
}
