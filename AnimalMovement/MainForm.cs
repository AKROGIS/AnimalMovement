using System;
using DataModel;
using System.Linq;
using System.Windows.Forms;

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
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Application.DoEvents(); //Trick to make sure all the contained controls are drawn before potentially hanging
            EnableForm();
        }

        private void EnableForm()
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents(); // Make sure the wait cursor is shown;
            var db = new AnimalMovementDataContext();
            try
            {
                MyProfileButton.Visible = db.ProjectInvestigators.Any(pi => pi.Login == _currentUser);                
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MyProfileButton.Text = "Connection Failed";
                MyProfileButton.Enabled = false;
                MessageBox.Show(Environment.NewLine + ex.Message + Environment.NewLine +
                    "Connection String:" + Environment.NewLine + db.Connection.ConnectionString,
                    "Unable to connect to the database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Cursor.Current = Cursors.Default;
            MyProfileButton.Text = "Project Investigator Details";
            ProjectsButton.Enabled = true;
            UploadButton.Enabled = true;
            GenerateMapButton.Enabled = true;
        }

        private static string GetUserName()
        {
            return Environment.UserDomainName + "\\" + Environment.UserName;
        }

        private void MyProfileButton_Click(object sender, EventArgs e)
        {
            var form = new InvestigatorForm(_currentUser);
            form.Show(this);
        }

        private void ProjectsButton_Click(object sender, EventArgs e)
        {
            var form = new ProjectsForm(_currentUser);
            form.Show(this);
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            var form = new UploadFilesForm();
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
