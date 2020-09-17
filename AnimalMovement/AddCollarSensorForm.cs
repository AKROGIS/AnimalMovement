using DataModel;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace AnimalMovement
{
    internal partial class AddCollarSensorForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private AnimalMovementFunctions Functions { get; set; }
        private string CurrentUser { get; set; }
        private Collar Collar { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        internal AddCollarSensorForm(Collar collar)
        {
            InitializeComponent();
            RestoreWindow();
            Collar = collar;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpControls();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Collar is in a different DataContext, get one in this DataContext
            if (Collar != null)
            {
                Collar = Database.Collars.FirstOrDefault(c => c.CollarManufacturer == Collar.CollarManufacturer && c.CollarId == Collar.CollarId);
            }

            //If Collar is not provided, throw an error
            if (Collar == null)
            {
                throw new InvalidOperationException("Add Collar Sensor Form not provided a valid Collar.");
            }

            Functions = new AnimalMovementFunctions();
            IsEditor = Collar != null && (Functions.IsInvestigatorEditor(Collar.ProjectInvestigator.Login, CurrentUser) ?? false);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void SetUpControls()
        {
            SetUpSensorComboBox();
            CollarManufacturerTextBox.Text = Collar.CollarManufacturer;
            CollarIdTextBox.Text = Collar.CollarId;
            EnableControls();
        }

        private void SetUpSensorComboBox()
        {
            // Set to All Sensors in the Lookup table, except ones already assigne to this collar
            var collarSensorsCodes = Collar.CollarSensors.Select(s => s.SensorCode).ToList();
            var allSensors = Database.LookupCollarSensors.Where(s => !collarSensorsCodes.Contains(s.Code)).ToList();
            SensorComboBox.DataSource = allSensors[0];
            SensorComboBox.DisplayMember = "Name";
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
            DatabaseChanged?.Invoke(this, EventArgs.Empty);
        }

        private void EnableControls()
        {
            CreateButton.Enabled = IsEditor && SensorComboBox.SelectedItem != null;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var sensorCode = ((LookupCollarSensor)SensorComboBox.SelectedItem)?.Code;
            if (sensorCode == null)
            {
                return;
            }
            var sensor = new CollarSensor
            {
                Collar = Collar,
                SensorCode = sensorCode,
                IsActive = IsActiveCheckBox.Checked
            };
            Database.CollarSensors.InsertOnSubmit(sensor);
            if (!SubmitChanges())
            {
                CreateButton.Enabled = false;
                return;
            }
            OnDatabaseChanged();
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
