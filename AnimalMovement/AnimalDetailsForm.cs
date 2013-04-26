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
        private string CurrentUser { get; set; }
        private Animal Animal { get; set; }
        private bool IsEditor { get; set; }
        private bool IsEditMode { get; set; }
        internal event EventHandler DatabaseChanged;

        internal AnimalDetailsForm(Animal animal)
        {
            InitializeComponent();
            RestoreWindow();
            Animal = animal;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Animal is in a different DataContext, get one in this DataContext
            if (Animal != null)
                Animal = Database.Animals.FirstOrDefault(a => a.ProjectId == Animal.ProjectId && a.AnimalId == Animal.AnimalId);
            if (Animal == null)
                throw new InvalidOperationException("Animal Details Form not provided a valid Animal.");

            var functions = new AnimalMovementFunctions();
            IsEditor = functions.IsProjectEditor(Animal.ProjectId, CurrentUser) ?? false;
            SetupHeader();
        }

        #region Form Controls

        private void SetupHeader()
        {
            ProjectTextBox.Text = Animal.Project.ProjectName;
            AnimalIdTextBox.Text = Animal.AnimalId;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AnimalTabControl.SelectedIndex = Properties.Settings.Default.AnimalDetailsFormActiveTab;
            if (AnimalTabControl.SelectedIndex == 0)
                //if new index is zero, index changed event will not fire, so fire it manually
                AnimalTabControl_SelectedIndexChanged(null, null);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Properties.Settings.Default.AnimalDetailsFormActiveTab = AnimalTabControl.SelectedIndex;
        }

        private void AnimalTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (AnimalTabControl.SelectedIndex)
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

        #endregion


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
            ConfigureMortalityDateTimePicker();
            EnableGeneralControls();
        }

        private void ConfigureMortalityDateTimePicker()
        {
            if (Animal.MortalityDate == null)
            {
                MortatlityDateTimePicker.Value = DateTime.Now.Date + TimeSpan.FromHours(12);
                MortatlityDateTimePicker.CustomFormat = " ";
            }
            else
            {
                MortatlityDateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm";
                MortatlityDateTimePicker.Value = Animal.MortalityDate.Value.ToLocalTime();
            }
            MortatlityDateTimePicker.Checked = (Animal.MortalityDate != null);
        }

        private void EnableGeneralControls()
        {
            EditSaveButton.Enabled = IsEditor;
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
            Animal.MortalityDate = MortatlityDateTimePicker.Checked
                                       ? (DateTime?) MortatlityDateTimePicker.Value.ToUniversalTime()
                                       : null;
        }

        private void EditSaveButton_Click(object sender, EventArgs e)
        {
            //This button is not enabled unless editing is permitted 
            if (EditSaveButton.Text == "Edit")
            {
                // The user wants to edit, Enable form
                EditSaveButton.Text = "Save";
                DoneCancelButton.Text = "Cancel";
                EnableGeneralControls();
            }
            else
            {
                //User is saving
                UpdateDataSource();
                if (SubmitChanges())
                {
                    OnDatabaseChanged();
                    EditSaveButton.Text = "Edit";
                    DoneCancelButton.Text = "Done";
                    EnableGeneralControls();
                }
            }
        }

        private void DoneCancelButton_Click(object sender, EventArgs e)
        {
            if (DoneCancelButton.Text == "Cancel")
            {
                DoneCancelButton.Text = "Done";
                EditSaveButton.Text = "Edit";
                EnableGeneralControls();
                //Reset state from database
                LoadDataContext();
                SetupGeneralTab();
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
            public CollarDeployment Deployment { get; set; }
            public Collar Collar { get; set; }
            public DateTime? DeploymentDate { get; set; }
            public DateTime? RetrievalDate { get; set; }
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
                    Deployment = d,
                    Collar = d.Collar,
                    DeploymentDate = d.DeploymentDate.ToLocalTime(),
                    RetrievalDate = d.RetrievalDate.HasValue ? d.RetrievalDate.Value.ToLocalTime() : (DateTime?)null,
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
            var form = new CollarDeploymentDetailsForm(item.Deployment, false, true);
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
            var form = new CollarDetailsForm(item.Collar);
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
