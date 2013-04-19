using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class CollarParametersDetailsForm : Form
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private int ParameterId { get; set; }
        private CollarParameter CollarParameter { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        public CollarParametersDetailsForm(int parameterId)
        {
            InitializeComponent();
            ParameterId = parameterId;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            LoadDefaultFormContents();  //Called before events are triggered
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Collar is in a different data context, get one in this Datacontext
            CollarParameter =
                    Database.CollarParameters.FirstOrDefault(p => p.ParameterId == ParameterId);
            if (CollarParameter == null)
                throw new InvalidOperationException("Collar Parameters Form not provided a valid Collar Parameter Id.");

            IsEditor = string.Equals(CollarParameter.Collar.Manager.Normalize(), CurrentUser.Normalize(),
                                     StringComparison.OrdinalIgnoreCase);
        }

        private void LoadDefaultFormContents()
        {
        }

        private void EnableFormControls()
        {
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

    }
}
