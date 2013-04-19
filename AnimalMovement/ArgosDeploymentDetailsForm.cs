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
    internal partial class ArgosDeploymentDetailsForm : Form
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private int DeploymentId { get; set; }
        private ArgosDeployment ArgosDeployment { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        public ArgosDeploymentDetailsForm(int deploymentId)
        {
            InitializeComponent();
            DeploymentId = deploymentId;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            LoadDefaultFormContents();  //Called before events are triggered
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Collar is in a different data context, get one in this Datacontext
            ArgosDeployment =
                    Database.ArgosDeployments.FirstOrDefault(d => d.DeploymentId == DeploymentId);
            if (ArgosDeployment == null)
                throw new InvalidOperationException("Argos Deployments Form not provided a valid Argos Deployment Id.");

            IsEditor = string.Equals(ArgosDeployment.Collar.Manager.Normalize(), CurrentUser.Normalize(),
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
