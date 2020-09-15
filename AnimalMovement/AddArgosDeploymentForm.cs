using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DataModel;

//TODO - Add button to add new Argos Platform

namespace AnimalMovement
{
    internal partial class AddArgosDeploymentForm : Form
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private Collar Collar { get; set; }
        private ArgosPlatform Platform { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        public AddArgosDeploymentForm(Collar collar = null, ArgosPlatform platform = null)
        {
            InitializeComponent();
            Collar = collar;
            Platform = platform;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpControls();  //Called before events are triggered
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Collar and Platform are in a different DataContext, get them in this DataContext
            if (Collar != null)
                Collar =
                    Database.Collars.FirstOrDefault(
                        c => c.CollarManufacturer == Collar.CollarManufacturer && c.CollarId == Collar.CollarId);
            if (Platform != null)
                Platform =
                    Database.ArgosPlatforms.FirstOrDefault(p => p.PlatformId == Platform.PlatformId);
        }

        private void SetUpControls()
        {
            LoadCollarComboBox();
            LoadPlatformComboBox();
            //Defer setup of DateTimePickers until onload (setting them in the form ctor doesn't work well)
            //Defer Enabling controls until after load so error message boxes have better context
        }

        private void LoadCollarComboBox()
        {
            //If given a Collar, set that and lock it.
            //else, set list to all collars I can edit, and select null per the constructor request
            if (Collar != null)
                CollarComboBox.Items.Add(Collar);
            else
            {
                CollarComboBox.DataSource =
                    Database.Collars.Where(c => c.Manager == CurrentUser ||
                     c.ProjectInvestigator.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser));
            }
            CollarComboBox.SelectedItem = Collar;
        }

        private void LoadPlatformComboBox()
        {
            //If given a Collar, set that and lock it.
            //else, set list to all collars I can edit, and select null per the constructor request
            if (Platform != null)
                ArgosComboBox.Items.Add(Platform);
            else
            {
                ArgosComboBox.DataSource =
                    Database.ArgosPlatforms.Where(p => p.ArgosProgram.Manager == CurrentUser ||
                     p.ArgosProgram.ProjectInvestigator.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser));
            }
            ArgosComboBox.SelectedItem = Platform;
            ArgosComboBox.DisplayMember = "PlatformId";
        }

        private void SetUpDatePickers()
        {
            StartDateTimePicker.Value = DateTime.Now.Date + TimeSpan.FromHours(12);
            StartDateTimePicker.Checked = false;
            StartDateTimePicker.CustomFormat = " ";
            EndDateTimePicker.Value = DateTime.Now.Date + TimeSpan.FromHours(12);
            EndDateTimePicker.Checked = false;
            EndDateTimePicker.CustomFormat = " ";
        }

        private void EnableFormControls()
        {
            ValidateEditor();
            if (!IsEditor)
            {
                CreateButton.Enabled = false;
                CollarComboBox.Enabled = false;
                ArgosComboBox.Enabled = false;
                StartDateTimePicker.Enabled = false;
                EndDateTimePicker.Enabled = false;
                ValidationTextBox.Text = "You do not have permission to edit this collar.";
                return;
            }
            CollarComboBox.Enabled = Collar == null;
            ArgosComboBox.Enabled = Platform == null;
            StartDateTimePicker.Enabled = true;
            EndDateTimePicker.Enabled = true;
            ValidateForm();
        }

        private void ValidateEditor()
        {
            //If Collar is provided, it will be locked, so you must be the pi or an assistant
            if (Collar != null &&
                !Collar.Manager.Normalize().Equals(CurrentUser.Normalize(), StringComparison.OrdinalIgnoreCase) &&
                Collar.ProjectInvestigator.ProjectInvestigatorAssistants.All(
                    a => !a.Assistant.Normalize().Equals(CurrentUser.Normalize(), StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show(
                    "You do not have permission to edit this collar.", "No Permission",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsEditor = false;
                return;
            }
            //If Collar is not provided, make sure there is a collar to pick from (we can edit everything in the list)
            if (Collar == null && CollarComboBox.Items.Count < 1)
            {
                MessageBox.Show(
                    "You can't create a deployment unless you are a PI or an assitant to a PI with collars.", "No Permission",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsEditor = false;
                return;
            }
            //If Platform is provided, it will be locked, so you must be the pi or an assistant
            if (Platform != null &&
                !Platform.ArgosProgram.Manager.Normalize()
                         .Equals(CurrentUser.Normalize(), StringComparison.OrdinalIgnoreCase) &&
                Platform.ArgosProgram.ProjectInvestigator.ProjectInvestigatorAssistants.All(
                    a => !a.Assistant.Normalize().Equals(CurrentUser.Normalize(), StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show(
                    "You do not have permission to edit this Argos Platform.", "No Permission",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsEditor = false;
                return;
            }
            //If Platform is not provided, make sure there is a platform to pick from (we can edit everything in the list)
            if (Platform == null && ArgosComboBox.Items.Count < 1)
            {
                MessageBox.Show(
                    "You can't create a deployment unless you are a PI or an assitant to a PI with Argos platforms.", "No Permission",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsEditor = false;
                return;
            }
            IsEditor = true;
        }

        private void ValidateForm()
        {
            var error = ValidateError();
            if (error != null)
                ValidationTextBox.Text = error;
            ValidationTextBox.Visible = error != null;
            CreateButton.Enabled = error == null;
            FixItButton.Visible = error != null;
        }

        private string ValidateError()
        {
            if (!(CollarComboBox.SelectedItem is Collar collar))
                return "You must select a collar";

            if (!(ArgosComboBox.SelectedItem is ArgosPlatform platform))
                return "You must select an Argos Id";

            var start = StartDateTimePicker.Checked ? StartDateTimePicker.Value.Date.ToUniversalTime() : DateTime.MinValue;
            var end   =   EndDateTimePicker.Checked ?   EndDateTimePicker.Value.Date.ToUniversalTime() : DateTime.MaxValue;
            if (end < start)
                return "The end date must be after the start date";

            //A collar cannot have multiple Argos Platforms at the same time
            if (collar.ArgosDeployments.Any(deployment =>
                                            DatesOverlap(deployment.StartDate ?? DateTime.MinValue,
                                                         deployment.EndDate ?? DateTime.MaxValue, start, end)))
                return "This collar has another Argos Id during your date range.";
            
            //A platform cannot be on two collars at the same time.
            if (platform.ArgosDeployments.Any(deployment =>
                                              DatesOverlap(deployment.StartDate ?? DateTime.MinValue,
                                                           deployment.EndDate ?? DateTime.MaxValue, start, end)))
                return "Another collar is using this Argos Id during your date range.";
            return null;
        }

        private static bool DatesOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            //touching is not considered overlapping.
            return start2 < end1 && start1 < end2;
        }

        private bool AddDeployment()
        {
            var deployment = new ArgosDeployment
                {
                    Collar = (Collar)CollarComboBox.SelectedItem,
                    ArgosPlatform = (ArgosPlatform)ArgosComboBox.SelectedItem,
                    StartDate = StartDateTimePicker.Checked ? StartDateTimePicker.Value.Date.ToUniversalTime() : (DateTime?)null,
                    EndDate = EndDateTimePicker.Checked ? EndDateTimePicker.Value.Date.ToUniversalTime() : (DateTime?)null
                };
            Database.ArgosDeployments.InsertOnSubmit(deployment);
            if (SubmitChanges())
                return true;
            // The collar now thinks it has a deployment, deleteOnSubmit does not clear it
            LoadDataContext();
            return false;
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
            OnDatabaseChanged();
            return true;
        }

        private void OnDatabaseChanged()
        {
            DatabaseChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Form Control Events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetUpDatePickers();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            EnableFormControls();
        }

        private void CollarComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void ArgosComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void StartDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            StartDateTimePicker.CustomFormat = StartDateTimePicker.Checked ? "MMM-d-yyyy" : " ";
            ValidateForm();
        }

        private void EndDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            EndDateTimePicker.CustomFormat = EndDateTimePicker.Checked ? "MMM-d-yyyy" : " ";
            ValidateForm();
        }

        private void FixItButton_Click(object sender, EventArgs e)
        {
            //TODO - implement FixIt code
            MessageBox.Show("You must fix it manually", "Not Implemented");
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            if (AddDeployment())
                Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

   }
}
