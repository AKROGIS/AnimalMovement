using DataModel;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace AnimalMovement
{
    internal partial class CollarSensorDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private AnimalMovementFunctions Functions { get; set; }
        private string CurrentUser { get; set; }

        private CollarSensor CollarSensor { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        internal CollarSensorDetailsForm(CollarSensor sensor)
        {
            InitializeComponent();
            RestoreWindow();
            CollarSensor = sensor;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpControls();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //CollarSensor is in a different DataContext, get one in this DataContext
            if (CollarSensor != null)
            {
                CollarSensor = Database.CollarSensors.FirstOrDefault(s => s.CollarManufacturer == CollarSensor.CollarManufacturer && s.CollarId == CollarSensor.CollarId && s.SensorCode == CollarSensor.SensorCode);
            }

            //If CollarSensor is not provided, throw an error
            if (CollarSensor == null)
            {
                throw new InvalidOperationException("Collar Sensor Details Form not provided a valid Collar Sensor.");
            }

            Functions = new AnimalMovementFunctions();
            IsEditor = CollarSensor != null && (Functions.IsInvestigatorEditor(CollarSensor.Collar.ProjectInvestigator.Login, CurrentUser) ?? false);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void SetUpControls()
        {
            CollarManufacturerTextBox.Text = CollarSensor.CollarManufacturer;
            CollarIdTextBox.Text = CollarSensor.CollarId;
            CollarSensorTextBox.Text = CollarSensor.LookupCollarSensor.Name;
            IsActiveCheckBox.Checked = CollarSensor.IsActive;
            EnableControls();
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
            CreateButton.Enabled = IsEditor;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            if (CollarSensor.IsActive == IsActiveCheckBox.Checked)
            {
                Close();
                return;
            }
            CollarSensor.IsActive = IsActiveCheckBox.Checked;
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
