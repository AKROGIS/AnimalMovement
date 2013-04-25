using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class AddCollarDeploymentForm : Form
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private Collar Collar { get; set; }
        private Animal Animal { get; set; }
        private bool IsEditor { get; set; }
        public bool LockCollar { get; set; }
        public bool LockAnimal { get; set; }
        internal event EventHandler DatabaseChanged;

        public AddCollarDeploymentForm(Collar collar = null, Animal animal = null)
        {
            InitializeComponent();
            Collar = collar;
            Animal = animal;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            LoadDefaultFormContents();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Collar and Animal are in a different data context, get them in this DataContext
            if (Collar != null)
                Collar =
                    Database.Collars.FirstOrDefault(
                        c => c.CollarManufacturer == Collar.CollarManufacturer && c.CollarId == Collar.CollarId);
            if (Animal != null)
                Animal =
                    Database.Animals.FirstOrDefault(
                        a => a.ProjectId == Animal.ProjectId && a.AnimalId == Animal.AnimalId);
            if (Collar == null && Animal == null)
                throw new InvalidOperationException("Add Collar Deployment Form not provided a valid Collar or a valid Animal.");
            LockCollar = Collar != null;
            LockAnimal = Animal != null;

            var functions = new AnimalMovementFunctions();
            IsEditor = (Animal != null && (functions.IsProjectEditor(Animal.ProjectId, CurrentUser) ?? false)) ||
                       (Collar != null && (functions.IsInvestigatorEditor(Collar.Manager, CurrentUser) ?? false));
        }

        private void LoadDefaultFormContents()
        {
            //TODO limit lists to appropriate collars/animals
            CollarComboBox.DataSource = Database.Collars;
            CollarComboBox.SelectedItem = Collar;
            AnimalComboBox.DataSource = Database.Animals;
            AnimalComboBox.SelectedItem = Animal;
        }

        private void EnableFormControls()
        {
            AnimalComboBox.Enabled = IsEditor && !LockAnimal;
            CollarComboBox.Enabled = IsEditor && !LockCollar;
            CreateButton.Enabled = IsEditor;
            StartDateTimePicker.Enabled = IsEditor;
            EndDateTimePicker.Enabled = IsEditor;
            StartDateTimePicker.Value = DateTime.Now.Date - TimeSpan.FromDays(5) + TimeSpan.FromHours(12);
            EndDateTimePicker.Value = DateTime.Now.Date + TimeSpan.FromHours(12);
            EndDateTimePicker.Checked = false; //does not trigger the value changed event when set in code
            EndDateTimePicker.CustomFormat = " ";
            if (!IsEditor)
            {
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
                                             DatesOverlap(deployment.DeploymentDate,
                                                          deployment.RetrievalDate ?? DateTime.MaxValue, start, end)))
                return "This collar is deployed on another animal during your date range.";

            //An Animal cannot have two collars at the same time.
            if (animal.CollarDeployments.Any(deployment =>
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

        private bool AddDeployment()
        {
            var deployment = new CollarDeployment
            {
                Collar = CollarComboBox.SelectedItem as Collar,
                Animal = AnimalComboBox.SelectedItem as Animal,
                DeploymentDate = StartDateTimePicker.Value.ToUniversalTime(),
                RetrievalDate = EndDateTimePicker.Checked ? EndDateTimePicker.Value.ToUniversalTime() : (DateTime?)null
            };
            Database.CollarDeployments.InsertOnSubmit(deployment);
            if (SubmitChanges())
                return true;
            // The collar now thinks it has a deployment, deleteOnSubmit does not clear it, reloading the context will reset the form
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

        private void AddCollarDeploymentForm_Load(object sender, EventArgs e)
        {
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
