using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class CollarDeploymentDetailsForm : Form
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private CollarDeployment CollarDeployment { get; set; }
        private bool IsEditor { get; set; }
        private bool LockCollar { get; set; }
        private bool LockAnimal { get; set; }
        internal event EventHandler DatabaseChanged;

        public CollarDeploymentDetailsForm(CollarDeployment collarDeployment, bool lockCollar = false, bool lockAnimal = false)
        {
            InitializeComponent();
            CollarDeployment = collarDeployment;
            //TODO - remove locks when (if) CollarDeployments allows updates to collars or animals
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            LockCollar = lockCollar || true;
            LockAnimal = lockAnimal || true;
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            LoadDefaultFormContents();  //Called before events are triggered
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Get a CollarDeployment in this DataContext
            if (CollarDeployment != null)
            CollarDeployment =
                    Database.CollarDeployments.FirstOrDefault(d => d.DeploymentId == CollarDeployment.DeploymentId);
            if (CollarDeployment == null)
                throw new InvalidOperationException("Collar Deployments Form not provided a valid Deployment.");

            var functions = new AnimalMovementFunctions();
            IsEditor = (functions.IsProjectEditor(CollarDeployment.Animal.ProjectId, CurrentUser) ?? false) ||
                       (functions.IsInvestigatorEditor(CollarDeployment.Collar.Manager, CurrentUser) ?? false);
        }

        private void LoadDefaultFormContents()
        {
            if (LockAnimal)
                AnimalComboBox.DataSource = new [] {CollarDeployment.Animal};
            else
            {
                AnimalComboBox.DataSource = from animal in Database.Animals
                                            where animal == CollarDeployment.Animal ||
                                                  animal.Project.ProjectInvestigator == CurrentUser ||
                                                  animal.Project.ProjectEditors.Any(
                                                      e => e.Editor == CurrentUser)
                                            select animal;
            }
            if (LockCollar)
                CollarComboBox.DataSource = new [] {CollarDeployment.Collar};
            else
            {
                CollarComboBox.DataSource = from collar in Database.Collars
                                            where
                                                collar == CollarDeployment.Collar ||
                                                collar.Manager == CurrentUser ||
                                                collar.ProjectInvestigator.ProjectInvestigatorAssistants.Any(
                                                    a => a.Assistant == CurrentUser)
                                            select collar;
            }
            AnimalComboBox.SelectedItem = CollarDeployment.Animal;
            CollarComboBox.SelectedItem = CollarDeployment.Collar;
        }

        private void SetDatePickers()
        {
            //Setting DatePickerValues must be done after the form is loaded.
            StartDateTimePicker.Value = CollarDeployment.DeploymentDate.ToLocalTime();
            EndDateTimePicker.Value = CollarDeployment.RetrievalDate == null
                                          ? DateTime.Now.Date + TimeSpan.FromHours(12)
                                          : CollarDeployment.RetrievalDate.Value.ToLocalTime();
            EndDateTimePicker.Checked = CollarDeployment.RetrievalDate != null;
            EndDateTimePicker.CustomFormat = CollarDeployment.RetrievalDate != null ? " MMM-d-yyyy h:mm tt" : " ";
        }

        private void EnableFormControls()
        {
            SaveButton.Enabled = IsEditor;
            CollarComboBox.Enabled = IsEditor && !LockCollar;
            AnimalComboBox.Enabled = IsEditor && !LockAnimal;
            StartDateTimePicker.Enabled = IsEditor;
            EndDateTimePicker.Enabled = IsEditor;
            ValidateForm();
        }

        private void ValidateForm()
        {
            var error = ValidateError();
            if (error != null)
                ValidationTextBox.Text = error;
            ValidationTextBox.Visible = error != null;
            FixItButton.Visible = error != null;
            SaveButton.Enabled = IsEditor && error == null && DeploymentChanged();
        }

        private bool DeploymentChanged()
        {
            return CollarComboBox.SelectedItem as Collar != CollarDeployment.Collar ||
                   AnimalComboBox.SelectedItem as Animal != CollarDeployment.Animal ||
                   StartDateTimePicker.Value != CollarDeployment.DeploymentDate.ToLocalTime() ||
                   (CollarDeployment.RetrievalDate == null && EndDateTimePicker.Checked) ||
                   (CollarDeployment.RetrievalDate != null && (!EndDateTimePicker.Checked ||
                    EndDateTimePicker.Value != CollarDeployment.RetrievalDate.Value.ToLocalTime()));
        }

        private string ValidateError()
        {
            //We must have a collar
            if (!(CollarComboBox.SelectedItem is Collar collar))
                return "You must select a collar";

            //We must have a platform
            if (!(AnimalComboBox.SelectedItem is Animal animal))
                return "You must select an animal";

            //Check dates
            var start = StartDateTimePicker.Value.ToUniversalTime();
            var end = EndDateTimePicker.Checked ? EndDateTimePicker.Value.ToUniversalTime() : DateTime.MaxValue;
            if (end < start)
                return "The end date must be after the start date";

            //A collar cannot be deployed on multiple animals at the same time
            if (collar.CollarDeployments.Any(deployment =>
                                             deployment.DeploymentId != CollarDeployment.DeploymentId &&
                                             DatesOverlap(deployment.DeploymentDate,
                                                          deployment.RetrievalDate ?? DateTime.MaxValue, start, end)))
                return "This collar is deployed on another animal during your date range.";

            //An Animal cannot have two collars at the same time.
            if (animal.CollarDeployments.Any(deployment =>
                                             deployment.DeploymentId != CollarDeployment.DeploymentId &&
                                             DatesOverlap(deployment.DeploymentDate,
                                                          deployment.RetrievalDate ?? DateTime.MaxValue, start, end)))
                return "This animal has another collar during your date range.";
            
            return null;
        }

        private static bool DatesOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            //touching is not considered overlapping.
            return start2 < end1 && start1 < end2;
        }

        private bool UpdateDeployment()
        {
            CollarDeployment.Collar = (Collar)CollarComboBox.SelectedItem;
            CollarDeployment.Animal = (Animal) AnimalComboBox.SelectedItem;
            CollarDeployment.DeploymentDate = StartDateTimePicker.Value.ToUniversalTime();
            CollarDeployment.RetrievalDate = EndDateTimePicker.Checked ? EndDateTimePicker.Value.ToUniversalTime() : (DateTime?)null;

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
            DatabaseChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Form Control Events

        private void CollarDeploymentDetailsForm_Load(object sender, EventArgs e)
        {
            SetDatePickers();
            EnableFormControls();
        }

        private void CollarComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void AnimalComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void StartDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            ValidateForm();
        }

        private void EndDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            EndDateTimePicker.CustomFormat = EndDateTimePicker.Checked ? "MMM-d-yyyy h:mm tt" : " ";
            ValidateForm();
        }

        private void FixItButton_Click(object sender, EventArgs e)
        {
            //TODO - implement FixIt code
            MessageBox.Show("You must fix it manually", "Not Implemented");
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (UpdateDeployment())
                Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
