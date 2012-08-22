using System;
using DataModel;

namespace AnimalMovement
{
    internal partial class RetrieveCollarForm : BaseForm
    {
        private CollarDeployment Deployment { get; set; }

        internal RetrieveCollarForm(CollarDeployment deployment)
        {
            InitializeComponent();
            RestoreWindow();
            Deployment = deployment;
            RetrievalDateTimePicker.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0);
            EnableForm();
        }

        private void EnableForm()
        {
            CreateButton.Enabled = RetrievalDateTimePicker.Value.ToUniversalTime() > Deployment.DeploymentDate;
        }

        private void RetrievalDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            EnableForm();
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            Deployment.RetrievalDate = RetrievalDateTimePicker.Value.ToUniversalTime();
        }
    }
}
