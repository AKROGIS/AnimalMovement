using System;
using DataModel;
using System.Linq;

namespace AnimalMovement
{
    internal partial class MainForm : BaseForm
    {
        private readonly string _currentUser;

        internal MainForm()
        {
            InitializeComponent();
            RestoreWindow();
            _currentUser = GetUserName();
            EnableMyProfile();
        }

        private void EnableMyProfile()
        {
            var db = new AnimalMovementDataContext();
            MyProfileButton.Visible = db.ProjectInvestigators.Any(pi => pi.Login == _currentUser);
        }

        private static string GetUserName()
        {
            return Environment.UserDomainName + "\\" + Environment.UserName;
        }

        private void MyProfileButton_Click(object sender, EventArgs e)
        {
            var form = new InvestigatorForm(_currentUser, _currentUser);
            form.Show(this);
        }

        private void ProjectsButton_Click(object sender, EventArgs e)
        {
            var form = new ProjectsForm(_currentUser);
            form.Show(this);
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            var form = new AddFileForm(_currentUser);
            form.Show(this);
        }

        private void GenerateMapButton_Click(object sender, EventArgs e)
        {
            var form = new CreateQueryLayerForm();
            form.Show(this);
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
