using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class AnimalDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string ProjectId { get; set; }
        private string AnimalId { get; set; }
        private string CurrentUser { get; set; }
        private Animal Animal { get; set; }
        private bool IsEditor { get; set; }
        private bool IsEditMode { get; set; }
        internal event EventHandler DatabaseChanged;

        internal AnimalDetailsForm(string projectId, string animalId, string user)
        {
            InitializeComponent();
            RestoreWindow();
            ProjectId = projectId;
            AnimalId = animalId;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Animal is in a different data context, get them in this DataContext
            Animal = Database.Animals.FirstOrDefault(a => a.ProjectId == ProjectId && a.AnimalId == AnimalId);
            if (Animal == null)
            {
                MessageBox.Show("Animal not found.", "Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            var functions = new AnimalMovementFunctions();
            IsEditor = functions.IsProjectEditor(Animal.ProjectId, CurrentUser) ?? false;

            ProjectTextBox.Text = Animal.Project.ProjectName;
            AnimalIdTextBox.Text = Animal.AnimalId;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AnimalTabsControl.SelectedIndex = Properties.Settings.Default.AnimalDetailsFormActiveTab;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Properties.Settings.Default.AnimalDetailsFormActiveTab = AnimalTabsControl.SelectedIndex;
        }

        private void AnimalTabsControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (AnimalTabsControl.SelectedIndex)
            {
                default:
                    SetupGeneralTab();
                    break;
                case 1:
                    SetupCollarsTab();
                    break;
                case 2:
                    SetupLocationsTab();
                    break;
            }
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


        #region General Tab

        private void SetupGeneralTab()
        {
            SpeciesComboBox.DataSource = Database.LookupSpecies;
            SpeciesComboBox.DisplayMember = "Species";
            SpeciesComboBox.SelectedItem = Animal.LookupSpecies;
            GenderComboBox.DataSource = Database.LookupGenders;
            GenderComboBox.DisplayMember = "Sex";
            GenderComboBox.SelectedItem = Animal.LookupGender;
            GroupTextBox.Text = Animal.GroupName;
            DescriptionTextBox.Text = Animal.Description;
            SetupMortalityDateTimePicker();
            EnableGeneralTab();
        }

        private void SetupMortalityDateTimePicker()
        {
            if (Animal.MortalityDate == null)
            {
                var now = DateTime.Now;
                MortatlityDateTimePicker.Value = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0);
                MortatlityDateTimePicker.Checked = false;
                MortatlityDateTimePicker.CustomFormat = " ";
            }
            else
            {
                MortatlityDateTimePicker.Checked = true;
                MortatlityDateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm";
                MortatlityDateTimePicker.Value = Animal.MortalityDate.Value.ToLocalTime();
            }
        }

        private void EnableGeneralTab()
        {
            EditSaveButton.Enabled = IsEditor;
            SetEditingControls();
        }

        private void SetEditingControls()
        {
            IsEditMode = EditSaveButton.Text == "Save";
            SpeciesComboBox.Enabled = IsEditMode;
            GenderComboBox.Enabled = IsEditMode;
            GroupTextBox.Enabled = IsEditMode;
            DescriptionTextBox.Enabled = IsEditMode;
            MortatlityDateTimePicker.Enabled = IsEditMode;
        }

        private void UpdateDataSource()
        {
            Animal.LookupSpecies = (LookupSpecies)SpeciesComboBox.SelectedItem;
            Animal.LookupGender = (LookupGender)GenderComboBox.SelectedItem;
            Animal.GroupName = GroupTextBox.Text;
            Animal.Description = DescriptionTextBox.Text;
            if (MortatlityDateTimePicker.Checked)
                Animal.MortalityDate = MortatlityDateTimePicker.Value.ToUniversalTime();
            else
                Animal.MortalityDate = null;
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

        private void MortatlityDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            MortatlityDateTimePicker.CustomFormat = MortatlityDateTimePicker.Checked ? "yyyy-MM-dd HH:mm" : " ";
        }

        #endregion


        #region Collar Deployment Tab

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

        private void SetupCollarsTab()
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
                    DeploymentDate = d.DeploymentDate.ToLocalTime(),
                    RetrievalDate = d.RetrievalDate.HasValue ? d.RetrievalDate.Value.ToLocalTime() : (DateTime?)null,
                    Deployment = d,
                };
            EnableCollarControls();
        }

        private void EnableCollarControls()
        {
            AddDeploymentButton.Enabled = !IsEditMode && IsEditor;
            DeleteDeploymentButton.Enabled = !IsEditMode && IsEditor && DeploymentDataGridView.SelectedRows.Count > 0;
            EditDeploymentButton.Enabled = !IsEditMode && IsEditor && DeploymentDataGridView.SelectedRows.Count == 1;
            InfoCollarButton.Enabled = !IsEditMode && DeploymentDataGridView.SelectedRows.Count == 1;
        }

        private void CollarDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetupCollarsTab();
        }

        private void AddDeploymentButton_Click(object sender, EventArgs e)
        {
            var form = new AddCollarDeploymentForm(null, Animal);
            form.DatabaseChanged += (o, x) => CollarDataChanged();
            form.Show(this);
        }

        private void DeleteDeploymentButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DeploymentDataGridView.SelectedRows)
            {
                var deployment = (DeploymentDataItem)row.DataBoundItem;
                Database.CollarDeployments.DeleteOnSubmit(deployment.Deployment);
            }
            if (SubmitChanges())
                CollarDataChanged();
        }

        private void EditDeploymentButton_Click(object sender, EventArgs e)
        {
            if (DeploymentDataGridView.CurrentRow == null)
                return;
            var item = DeploymentDataGridView.CurrentRow.DataBoundItem as DeploymentDataItem;
            if (item == null)
                return;
            var form = new CollarDeploymentDetailsForm(item.Deployment.DeploymentId, false, true);
            form.DatabaseChanged += (o, x) => CollarDataChanged();
            form.Show(this);
        }

        private void InfoCollarButton_Click(object sender, EventArgs e)
        {
            if (DeploymentDataGridView.CurrentRow == null)
                return;
            var item = DeploymentDataGridView.CurrentRow.DataBoundItem as DeploymentDataItem;
            if (item == null)
                return;
            var form = new CollarDetailsForm(item.Deployment.CollarManufacturer, item.CollarId, CurrentUser);
            form.DatabaseChanged += (o, x) => CollarDataChanged();
            form.Show(this);
        }

        private void DeploymentDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            EnableCollarControls();
        }

        #endregion


        #region Locations Tab

        //TODO - Add additional location info: hidden locations, centroid, MCP area, average speed

        private void SetupLocationsTab()
        {
            if (Animal == null)
                return;
            var views = new AnimalMovementViews();
            var summary = views.AnimalLocationSummary(Animal.ProjectId, Animal.AnimalId).FirstOrDefault();
            if (summary == null)
            {
                SummaryLabel.Text = "There are NO locations.";
                TopTextBox.Text = String.Empty;
                BottomTextBox.Text = String.Empty;
                LeftTextBox.Text = String.Empty;
                RightTextBox.Text = String.Empty;
            }
            else
            {
                SummaryLabel.Text = String.Format("{0} fixes between {1:yyyy-MM-dd} and {2:yyyy-MM-dd}.", summary.Count, summary.First, summary.Last);
                TopTextBox.Text = String.Format("{0}", summary.Top);
                BottomTextBox.Text = String.Format("{0}", summary.Bottom);
                LeftTextBox.Text = String.Format("{0}", summary.Left);
                RightTextBox.Text = String.Format("{0}", summary.Right);
            }
        }

        #endregion

    }
}
