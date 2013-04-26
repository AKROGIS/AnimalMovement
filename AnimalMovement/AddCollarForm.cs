using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class AddCollarForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private AnimalMovementFunctions Functions { get; set; }
        private string CurrentUser { get; set; }
        private ProjectInvestigator ProjectInvestigator { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        internal AddCollarForm(ProjectInvestigator investigator)
        {
            InitializeComponent();
            RestoreWindow();
            ProjectInvestigator = investigator;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpControls();
        }

        private void LoadDataContext()
        {

            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //ProjectInvestigator is in a different DataContext, get one in this DataContext
            if (ProjectInvestigator != null)
                ProjectInvestigator = Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == ProjectInvestigator.Login);
            //If Project Investigator is not provided, Current user must be a PI or an assistant
            if (ProjectInvestigator == null)
                if (!Database.ProjectInvestigators.Any(pi => pi.Login == CurrentUser) &&
                    !Database.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser))
                    throw new InvalidOperationException("Add Collar Form not provided a valid Project Investigator or you are not a PI or an assistant.");

            Functions = new AnimalMovementFunctions();
            IsEditor = ProjectInvestigator != null && (Functions.IsInvestigatorEditor(ProjectInvestigator.Login, CurrentUser) ?? false);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DisposalDateTimePicker.Value = DateTime.Now.Date + TimeSpan.FromHours(12);
            DisposalDateTimePicker.Checked = false;
            DisposalDateTimePicker.CustomFormat = " ";
        }

        private void SetUpControls()
        {
            SetUpManagerComboBox();
                //  first get the default, then set the datasource, then set the selection to the default.
            //    this order is required because setting the datasource sets the selected index to 0 triggering the selectedindex_changed event
            //    the selectedindex_changed event saves the user's selection, which when setting the datasource, overwrites the user's previous
            //    default with the item at index 0, so initialization deletes the user's preference.
            string manufacturerId = Settings.GetDefaultCollarManufacturer();
            SetUpModelList(manufacturerId);
            ManufacturerComboBox.DataSource = Database.LookupCollarManufacturers;
            ManufacturerComboBox.DisplayMember = "Name";
            SelectDefaultManufacturer(manufacturerId);
            EnableControls();
        }

        private void SetUpManagerComboBox()
        {
            //If given a PI, set that and lock it.
            //else, set list to me, if I am a PI, plus all PI's I can assist, and select null per the constructor request
            if (ProjectInvestigator != null)
                ManagerComboBox.Items.Add(ProjectInvestigator);
            else
            {
                ManagerComboBox.DataSource =
                    Database.ProjectInvestigators.Where(pi => pi.Login == CurrentUser).Concat(
                        Database.ProjectInvestigatorAssistants.Where(a => a.Assistant == CurrentUser)
                                .Select(a => a.ProjectInvestigator1));
            }
            ManagerComboBox.DisplayMember = "Name";
            ManagerComboBox.Enabled = ProjectInvestigator == null;
            ManagerComboBox.SelectedItem = ProjectInvestigator;
        }

        private void SetUpModelList(string mfgr)
        {
            string model = Settings.GetDefaultCollarModel(mfgr);
            ModelComboBox.DataSource = Database.LookupCollarModels.Where(m => m.CollarManufacturer == mfgr);
            ModelComboBox.DisplayMember = "CollarModel";
            SelectDefaultModel(mfgr, model);
        }

        private void SelectDefaultModel(string mfgr, string modelCode)
        {
            if ( mfgr == null || modelCode == null)
                return;
            var model = Database.LookupCollarModels.FirstOrDefault(m => m.CollarManufacturer == mfgr && m.CollarModel == modelCode);
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


        private void EnableControls()
        {
            CreateButton.Enabled = IsEditor && !string.IsNullOrEmpty(CollarIdTextBox.Text);
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
                    CollarId = CollarIdTextBox.Text.NullifyIfEmpty(),
                    Frequency = FrequencyTextBox.Text.DoubleOrNull(),
                    HasGps = HasGpsCheckBox.Checked,
                    DisposalDate = DisposalDateTimePicker.Checked ? DisposalDateTimePicker.Value : (DateTime?)null,
                    LookupCollarManufacturer = (LookupCollarManufacturer)ManufacturerComboBox.SelectedItem,
                    LookupCollarModel = (LookupCollarModel)ModelComboBox.SelectedItem,
                    Notes = NotesTextBox.Text.NullifyIfEmpty(),
                    Owner = OwnerTextBox.Text.NullifyIfEmpty(),
                    ProjectInvestigator = (ProjectInvestigator)ManagerComboBox.SelectedItem,
                    SerialNumber = SerialNumberTextBox.Text.NullifyIfEmpty(),
                };
            Database.Collars.InsertOnSubmit(collar);
            if (!SubmitChanges())
            {
                CollarIdTextBox.Focus();
                CreateButton.Enabled = false;
                return;
            }
            OnDatabaseChanged();
            Close();
        }

        private void CollarIdTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void ManagerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var investigator = ManagerComboBox.SelectedItem as ProjectInvestigator;
            IsEditor = investigator != null &&
                       (Functions.IsInvestigatorEditor(investigator.Login, CurrentUser) ?? false);
            EnableControls();
        }

        private void ManufacturerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mfgr = (LookupCollarManufacturer)ManufacturerComboBox.SelectedItem;
            if (mfgr != null)
            {
                Settings.SetDefaultCollarManufacturer(mfgr.CollarManufacturer);
                SetUpModelList(mfgr.CollarManufacturer);
            }
        }

        private void ModelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var model = (LookupCollarModel)ModelComboBox.SelectedItem;
            if (model != null)
                Settings.SetDefaultCollarModel(model.CollarManufacturer, model.CollarModel);
        }

        private void DisposalDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DisposalDateTimePicker.CustomFormat = DisposalDateTimePicker.Checked ? "yyyy-MM-dd HH:mm" : " ";
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
