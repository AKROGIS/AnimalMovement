using System;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class AddCollarForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private bool IndependentContext { get; set; }
        private string CurrentUser { get; set; }
        private ProjectInvestigator ProjectInvestigator { get; set; }
        private bool IsProjectInvestigator { get; set; }
        internal event EventHandler DatabaseChanged;

        internal AddCollarForm(string user)
        {
            IndependentContext = true;
            CurrentUser = user;
            SetupForm();
        }

        internal AddCollarForm(AnimalMovementDataContext database, string user)
        {
            IndependentContext = false;
            Database = database;
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
                //gets repointed when we requery Database.ProjectInvestigators later on.
                //ProjectInvestigator = Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == CurrentUser);
            }
            if (Database == null || CurrentUser == null)
            {
                MessageBox.Show("Insufficent information to initialize form.", "Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            ManagerComboBox.DataSource = Database.ProjectInvestigators;
            ProjectInvestigator = Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == CurrentUser);
            IsProjectInvestigator = ProjectInvestigator != null;
            ManagerComboBox.DisplayMember = "Name";
            ManagerComboBox.SelectedItem = ProjectInvestigator;
            string manufacturer = Settings.GetDefaultCollarManufacturer();
            ManufacturerComboBox.DataSource = Database.LookupCollarManufacturers;
            ManufacturerComboBox.DisplayMember = "Name";
            SelectDefaultManufacturer(manufacturer);
            string model = Settings.GetDefaultCollarModel();
            ModelComboBox.DataSource = Database.LookupCollarModels;
            ModelComboBox.DisplayMember = "CollarModel";
            SelectDefaultModel(model);
        }

        private void SelectDefaultModel(string modelCode)
        {
            if (modelCode == null)
                return;
            var model = Database.LookupCollarModels.FirstOrDefault(m => m.CollarModel == modelCode);
            if (model != null)
                ModelComboBox.SelectedItem = model;
        }

        private void SelectDefaultManufacturer(string manufacturerId)
        {
            if (manufacturerId == null)
                return;
            var manufacturer = Database.LookupCollarManufacturers.FirstOrDefault(m => m.CollarManufacturer == manufacturerId);
            if (manufacturer != null)
                ManufacturerComboBox.SelectedItem = manufacturer;
        }

        private void EnableCreate()
        {
            CreateButton.Enabled = IsProjectInvestigator && ProjectInvestigator != null && !string.IsNullOrEmpty(CollarIdTextBox.Text);
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var mfgr = (LookupCollarManufacturer)ManufacturerComboBox.SelectedItem;
            string collarId = CollarIdTextBox.Text.NullifyIfEmpty();

            if (Database.Collars.Any(c => c.LookupCollarManufacturer == mfgr && c.CollarId == collarId))
            {
                MessageBox.Show("The collar Id is not unique.  Try again", "Database Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                CollarIdTextBox.Focus();
                CreateButton.Enabled = false;
                return;
            }
            var collar = new Collar
                {
                    AlternativeId = AlternativeIdTextBox.Text.NullifyIfEmpty(),
                    CollarId = CollarIdTextBox.Text.NullifyIfEmpty(),
                    DownloadInfo = DownloadInfoTextBox.Text.NullifyIfEmpty(),
                    Frequency = FrequencyTextBox.Text.DoubleOrNull(),
                    LookupCollarManufacturer = (LookupCollarManufacturer)ManufacturerComboBox.SelectedItem,
                    LookupCollarModel = (LookupCollarModel)ModelComboBox.SelectedItem,
                    Notes = NotesTextBox.Text.NullifyIfEmpty(),
                    Owner = OwnerTextBox.Text.NullifyIfEmpty(),
                    ProjectInvestigator = (ProjectInvestigator)ManagerComboBox.SelectedItem,
                    SerialNumber = SerialNumberTextBox.Text.NullifyIfEmpty(),
                };
            Database.Collars.InsertOnSubmit(collar);
            if (IndependentContext)
            {
                try
                {
                    Database.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Database.Collars.DeleteOnSubmit(collar);
                    MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CollarIdTextBox.Focus();
                    CreateButton.Enabled = false;
                    return;
                }
            }
            OnDatabaseChanged();
            DialogResult = DialogResult.OK;
        }

        private void CollarIdTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableCreate();
        }

        private void ManagerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProjectInvestigator = ManagerComboBox.SelectedItem as ProjectInvestigator;
            EnableCreate();
        }

        private void ManufacturerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mfgr = (LookupCollarManufacturer)ManufacturerComboBox.SelectedItem;
            if (mfgr != null)
                Settings.SetDefaultCollarManufacturer(mfgr.CollarManufacturer);
        }

        private void ModelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var model = (LookupCollarModel)ModelComboBox.SelectedItem;
            if (model != null)
                Settings.SetDefaultCollarModel(model.CollarModel);
        }

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this,EventArgs.Empty);
        }
    }
}
