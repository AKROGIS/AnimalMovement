using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestUI
{
    public partial class MainForm : Form
    {
        private string _user;

        public MainForm()
        {
            InitializeComponent();
            _user = GetUserName();
        }

        private void ReviewProjectsButton_Click(object sender, EventArgs e)
        {
            var form = new ReviewProjectsForm2 { CurrentUser = _user };
            form.Show(this);
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            var form = new AddFileForm(_user);
            form.Show(this);
        }

        private void GenerateMapButton_Click(object sender, EventArgs e)
        {

        }

        private void FilesButton_Click(object sender, EventArgs e)
        {
            var form = new FilesForm { CurrentUser = _user };
            form.Show(this);
        }

        private void DeployButton_Click(object sender, EventArgs e)
        {
            var form = new DeploymentsForm {CurrentUser = _user};
            form.Show(this);
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static string GetUserName()
        {
            return Environment.UserDomainName + "\\" + Environment.UserName;
        }

    }
}
