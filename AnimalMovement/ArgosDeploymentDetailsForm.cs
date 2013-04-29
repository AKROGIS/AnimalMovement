using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class ArgosDeploymentDetailsForm : Form
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private int DeploymentId { get; set; }
        private ArgosDeployment ArgosDeployment { get; set; }
        private bool IsEditor { get; set; }
        private bool IsEditMode { get; set; }
        private bool LockCollar { get; set; }
        private bool LockArgos { get; set; }
        internal event EventHandler DatabaseChanged;

        public ArgosDeploymentDetailsForm(int deploymentId, bool lockCollar = false, bool lockArgos = false)
        {
            InitializeComponent();
            DeploymentId = deploymentId;
            LockCollar = lockCollar;
            LockArgos = lockArgos;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            LoadDefaultFormContents();  //Called before events are triggered
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Get an ArgosDeployment in this data context
            ArgosDeployment =
                    Database.ArgosDeployments.FirstOrDefault(d => d.DeploymentId == DeploymentId);
            if (ArgosDeployment == null)
                throw new InvalidOperationException("Argos Deployments Form not provided a valid Argos Deployment Id.");

            var functions = new AnimalMovementFunctions();
            IsEditor = (functions.IsInvestigatorEditor(ArgosDeployment.Collar.Manager, CurrentUser) ?? false) ||
                       (functions.IsInvestigatorEditor(ArgosDeployment.ArgosPlatform.ArgosProgram.Manager, CurrentUser) ?? false);
        }

        private void LoadDefaultFormContents()
        {
            var argosQuery = from platform in Database.ArgosPlatforms
                             where platform.ArgosProgram.Manager == ArgosDeployment.Collar.Manager
                             select platform.PlatformId;
            ArgosComboBox.DataSource = argosQuery.ToList();
            ArgosComboBox.SelectedItem = ArgosDeployment.PlatformId;
            var collarQuery = from collar in Database.Collars
                             where collar.Manager == ArgosDeployment.Collar.Manager
                             select collar;
            var collars = collarQuery.ToList();
            CollarComboBox.DataSource = collars;
            CollarComboBox.SelectedItem =
                collars.First(c =>
                              c.CollarManufacturer == ArgosDeployment.Collar.CollarManufacturer &&
                              c.CollarId == ArgosDeployment.Collar.CollarId);
            StartDateTimePicker.Value = ArgosDeployment.StartDate == null ? DateTime.Now.Date : ArgosDeployment.StartDate.Value.ToLocalTime();
            StartDateTimePicker.Checked = ArgosDeployment.StartDate != null;
            StartDateTimePicker.CustomFormat = ArgosDeployment.StartDate != null ? "MMM-d-yyyy" : " ";
            EndDateTimePicker.Value = ArgosDeployment.EndDate == null ? DateTime.Now.Date : ArgosDeployment.EndDate.Value.ToLocalTime();
            EndDateTimePicker.Checked = ArgosDeployment.EndDate != null;
            EndDateTimePicker.CustomFormat = ArgosDeployment.EndDate != null ? "MMM-d-yyyy" : " ";
        }

        private void EnableFormControls()
        {
            EditSaveButton.Enabled = IsEditor;
            IsEditMode = EditSaveButton.Text == "Save";
            EditSaveButton.Enabled = IsEditMode;
            CollarComboBox.Enabled = IsEditMode && !LockCollar;
            ArgosComboBox.Enabled = IsEditMode && !LockArgos;
            StartDateTimePicker.Enabled = IsEditMode;
            EndDateTimePicker.Enabled = IsEditMode;
            ValidateForm();
        }

        private void ValidateForm()
        {
            var error = ValidateError();
            if (error != null)
                ValidationTextBox.Text = error;
            ValidationTextBox.Visible = error != null;
            EditSaveButton.Enabled = (IsEditMode && error == null) || (!IsEditMode && IsEditor);
            FixItButton.Visible = error != null;
        }

        private string ValidateError()
        {
            //We must have a collar
            var collar = CollarComboBox.SelectedItem as Collar;
            if (collar == null)
                return "No collar selected.";

            //We must have a platform
            var platform = ArgosComboBox.SelectedItem as string;
            if (platform == null)
                return "No Argos Id selected.";

            var start = StartDateTimePicker.Checked ? StartDateTimePicker.Value.ToUniversalTime() : DateTime.MinValue;
            var end   =   EndDateTimePicker.Checked ?   EndDateTimePicker.Value.ToUniversalTime() : DateTime.MaxValue;
            if (end < start)
                return "The end date must be after the start date";

            //A collar cannot have multiple Argos Platforms at the same time
            if (collar.ArgosDeployments.Any(deployment =>
                                            deployment.DeploymentId != ArgosDeployment.DeploymentId &&
                                            DatesOverlap(deployment.StartDate ?? DateTime.MinValue,
                                                         deployment.EndDate ?? DateTime.MaxValue, start, end)))
                return "This collar has another Argos Id during your date range.";
            
            //An Argos Platform cannot be on two collars at the same time.
            //I must create a list, because LinqToSql cannot translate the second lambda to SQL
            var deployments =
                Database.ArgosDeployments.Where(
                    d => d.PlatformId == platform && d.DeploymentId != ArgosDeployment.DeploymentId).ToList();
            if (deployments.Any(deployment =>
                                DatesOverlap(deployment.StartDate ?? DateTime.MinValue,
                                             deployment.EndDate ?? DateTime.MaxValue, start, end)))
                return "Another collar is using this Argos Id during your date range.";
            return null;
        }

        private static bool DatesOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            //touching is not considered overlapping.
            return (start2 < end1 && start1 < end2);
        }

        private bool UpdateDeployment()
        {
            //LinqToSql denys direct changes to FK, we must assign an entity.  I only do this if the keys have changed.
            var newCollar = (Collar)CollarComboBox.SelectedItem;
            if (ArgosDeployment.CollarManufacturer != newCollar.CollarManufacturer || ArgosDeployment.CollarId != newCollar.CollarId)
                ArgosDeployment.Collar = newCollar;
            var newPlatform = Database.ArgosPlatforms.FirstOrDefault(d => d.PlatformId == (string) ArgosComboBox.SelectedItem);
            if (newPlatform != null && ArgosDeployment.PlatformId != newPlatform.PlatformId)
                ArgosDeployment.ArgosPlatform = newPlatform;
            ArgosDeployment.StartDate = StartDateTimePicker.Checked ? StartDateTimePicker.Value.ToUniversalTime() : (DateTime?) null;
            ArgosDeployment.EndDate = EndDateTimePicker.Checked ? EndDateTimePicker.Value.ToUniversalTime() : (DateTime?) null;

            return SubmitChanges();
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
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }

        #region Form Control Events

        private void ArgosDeploymentDetailsForm_Load(object sender, EventArgs e)
        {
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

        private void EditSaveButton_Click(object sender, EventArgs e)
        {
            //This button is not enabled unless editing is permitted 
            if (EditSaveButton.Text == "Edit")
            {
                // The user wants to edit, Enable form
                EditSaveButton.Text = "Save";
                DoneCancelButton.Text = "Cancel";
                EnableFormControls();
            }
            else
            {
                if (UpdateDeployment())
                {
                    EditSaveButton.Text = "Edit";
                    DoneCancelButton.Text = "Done";
                    EnableFormControls();
                }
            }
        }

        private void DoneCancelButton_Click(object sender, EventArgs e)
        {
            if (DoneCancelButton.Text == "Cancel")
            {
                DoneCancelButton.Text = "Done";
                EditSaveButton.Text = "Edit";
                EnableFormControls();
                //Reset state from database
                LoadDefaultFormContents();
            }
            else
            {
                Close();
            }
        }

        #endregion
    }
}
