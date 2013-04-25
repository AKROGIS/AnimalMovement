﻿using System;
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
        private int DeploymentId { get; set; }
        private CollarDeployment CollarDeployment { get; set; }
        private bool IsEditor { get; set; }
        private bool LockCollar { get; set; }
        private bool LockAnimal { get; set; }
        internal event EventHandler DatabaseChanged;

        public CollarDeploymentDetailsForm(int deploymentId, bool lockCollar = false, bool lockAnimal = false)
        {
            InitializeComponent();
            DeploymentId = deploymentId;
            //FIXME - remove locks when CollarDeployments_Update SPROC accepts collars and animals
            LockCollar = lockCollar || true;
            LockAnimal = lockAnimal || true;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            LoadDefaultFormContents();  //Called before events are triggered
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Get an CollarDeployment in this data context
            CollarDeployment =
                    Database.CollarDeployments.FirstOrDefault(d => d.DeploymentId == DeploymentId);
            if (CollarDeployment == null)
                throw new InvalidOperationException("Collar Deployments Form not provided a valid Deployment Id.");

            //Todo - put check in database function to get assistants as well
            var functions = new AnimalMovementFunctions();
            IsEditor = (functions.IsProjectEditor(CollarDeployment.Animal.ProjectId, CurrentUser) ?? false) ||
                       (string.Equals(CollarDeployment.Collar.Manager.Normalize(), CurrentUser.Normalize(), StringComparison.OrdinalIgnoreCase));
        }

        private void LoadDefaultFormContents()
        {
            //TODO - filter list of animals and collars (by assistant as well)
            AnimalComboBox.DataSource = Database.Animals;
            AnimalComboBox.SelectedItem = CollarDeployment.Animal;
            var collarQuery = from collar in Database.Collars
                              where collar.Manager == CollarDeployment.Collar.Manager
                              select collar;
            var collars = collarQuery.ToList();
            CollarComboBox.DataSource = collars;
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
                   (CollarDeployment.RetrievalDate != null &&
                    EndDateTimePicker.Value != CollarDeployment.RetrievalDate.Value.ToLocalTime());
        }

        private string ValidateError()
        {
            //We must have a collar
            var collar = CollarComboBox.SelectedItem as Collar;
            if (collar == null)
                return "You must select a collar";

            //We must have a platform
            var animal = AnimalComboBox.SelectedItem as Animal;
            if (animal == null)
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
            return (start2 < end1 && start1 < end2);
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
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
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
