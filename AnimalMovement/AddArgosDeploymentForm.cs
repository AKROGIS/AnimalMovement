using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DataModel;

//TODO - Add button to add new Argos Platform
//TODO - Add button to fix validation errors

namespace AnimalMovement
{
    internal partial class AddArgosDeploymentForm : Form
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private Collar Collar { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        public AddArgosDeploymentForm(Collar collar)
        {
            InitializeComponent();
            Collar = collar;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            LoadDefaultFormContents();  //Called before events are triggered
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Collar is in a different data context, get one in this Datacontext
            if (Collar != null)
                Collar =
                    Database.Collars.FirstOrDefault(
                        c => c.CollarManufacturer == Collar.CollarManufacturer && c.CollarId == Collar.CollarId);
            if (Collar == null)
                throw new InvalidOperationException("Add Argos Deployment Form not provided a valid Collar.");

            IsEditor = string.Equals(Collar.Manager.Normalize(), CurrentUser.Normalize(),
                                     StringComparison.OrdinalIgnoreCase);
        }

        private void LoadDefaultFormContents()
        {
            CollarTextBox.Text = Collar.ToString();
            var argosQuery = from platform in Database.ArgosPlatforms
                             where platform.ArgosProgram.Manager == Collar.Manager
                             select platform.PlatformId;
            ArgosComboBox.DataSource = argosQuery.ToList();
        }

        private void EnableFormControls()
        {
            if (!IsEditor)
            {
                CreateButton.Enabled = false;
                ArgosComboBox.Enabled = false;
                StartDateTimePicker.Enabled = false;
                EndDateTimePicker.Enabled = false;
                ValidationTextBox.Text = "You do not have permission to edit this collar.";
                return;
            }
            ValidateForm();
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
            var start = StartDateTimePicker.Checked ? StartDateTimePicker.Value : DateTime.MinValue;
            var end   =   EndDateTimePicker.Checked ?   EndDateTimePicker.Value : DateTime.MaxValue;
            
            //A collar cannot have multiple Argos Platforms at the same time
            if (Collar.ArgosDeployments.Any(deployment =>
                                            DatesOverlap(deployment.StartDate ?? DateTime.MinValue,
                                                         deployment.EndDate ?? DateTime.MaxValue, start, end)))
                return "This collar has another Argos Id during your  date range.";
            
            //An Argos Platform cannot be on two collars at the same time.
            //I must create a list, because it cannot translate the second lambda to SQL
            var deployments = Database.ArgosDeployments.Where(d => d.PlatformId == (string)ArgosComboBox.SelectedItem).ToList();
            if (deployments.Any(deployment =>
                                DatesOverlap(deployment.StartDate ?? DateTime.MinValue,
                                             deployment.EndDate ?? DateTime.MaxValue, start, end)))
                return "Another collar is using this Argos Id during your date range.";
            return null;
        }

        private bool DatesOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            //touching is not considere overlapping.
            return (start2 < end1 && start1 < end2);
        }

        private bool AddDeployment()
        {
            var deployment = new ArgosDeployment
                {
                    Collar = Collar,
                    PlatformId = (string)ArgosComboBox.SelectedItem,
                    StartDate = StartDateTimePicker.Checked ? StartDateTimePicker.Value.Date : (DateTime?) null,
                    EndDate = EndDateTimePicker.Checked ? EndDateTimePicker.Value.Date : (DateTime?) null
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
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }

        #region Form Control Events

        private void AddArgosDeploymentForm_Load(object sender, EventArgs e)
        {
            EnableFormControls();
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
            MessageBox.Show("Not Implemented Yet.");
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
