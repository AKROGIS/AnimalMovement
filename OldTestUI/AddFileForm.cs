using System;
using System.Windows.Forms;
using AnimalMovementLibrary;

namespace TestUI
{
    public partial class AddFileForm : Form
    {
        public AddFileForm(string user)
        {
            InitializeComponent();
            CurrentUser = user;
            LoadLists();
        }

        public string CurrentUser { get; private set; }
        public string Project { get; set; }

        private void LoadLists()
        {
            try
            {
                AML.Connect();
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to connect.\n" + e.Message, "Aborting", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            LoadProjectList(CurrentUser);
            LoadCollarMfgrList();
            LoadFormatList();
            LoadCollarList((string)CollarMfgrComboBox.SelectedValue);
            GuessDefaultProject();
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            string message;
            if (ValidateForm(out message))
            {
                try
                {
                    AML.UploadFile(FileNameTextBox.Text, GetFormatCode(),
                                   GetCollarMfgrCode(), GetCollar(),
                                   (string)ProjectComboBox.SelectedValue, GetStatusCode());
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to upload file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                MessageBox.Show(message, "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
            FileNameTextBox.Text = openFileDialog.FileName;
            ProcessFileName();
        }

        private void LoadFormatList()
        {
            FormatComboBox.DisplayMember = "Name";
            FormatComboBox.ValueMember = "Code";
            FormatComboBox.DataSource = AML.Formats;
        }

        private void LoadCollarMfgrList()
        {
            CollarMfgrComboBox.DisplayMember = "Name";
            CollarMfgrComboBox.ValueMember = "CollarManufacturer";
            CollarMfgrComboBox.DataSource = AML.CollarMfgrs;
        }

        private void LoadProjectList(string user)
        {
            ProjectComboBox.DisplayMember = "ProjectName";
            ProjectComboBox.ValueMember = "ProjectId";
            ProjectComboBox.DataSource = AML.Projects(user);
        }

        private void LoadCollarList(string mfgr)
        {
            CollarComboBox.DisplayMember = "CollarId";
            CollarComboBox.ValueMember = "CollarId";
            CollarComboBox.DataSource = AML.Collars(mfgr);
        }

        private string GetFormatCode()
        {
            return (string)FormatComboBox.SelectedValue;
        }

        private string GetCollarMfgrCode()
        {
            return (string)CollarMfgrComboBox.SelectedValue;
        }

        private string GetStatusCode()
        {
            return (StatusActiveRadioButton.Checked) ? "A" : "I";
        }

        private string GetCollar()
        {
            return (CollarComboBox.Enabled) ? (string)CollarComboBox.SelectedValue : null;
        }

        private bool ValidateForm(out string message)
        {
            message = AML.ValidateInput(FileNameTextBox.Text, GetFormatCode(),
                    GetCollarMfgrCode(), GetCollar(),
                    (string)ProjectComboBox.SelectedValue, GetStatusCode());
            return string.IsNullOrEmpty(message);
        }

        private void ProcessFileName()
        {
            string code = AML.GuessFileFormat(FileNameTextBox.Text);
            FormatComboBox.SelectedValue = code;
        }

        private void FormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCollarMfgrComboBox();
            CollarComboBox.Enabled = !AML.DoesFormatHaveCollarId((string)FormatComboBox.SelectedValue);
        }

        private void UpdateCollarMfgrComboBox()
        {
            string collarMfgr = AML.CollarManufacturer((string)FormatComboBox.SelectedValue);
            if ((string)CollarMfgrComboBox.SelectedValue != collarMfgr)
            {
                CollarMfgrComboBox.SelectedValue = collarMfgr;
            }
        }

        private void GuessDefaultProject()
        {
            var project = AML.GuessDefaultProject(CurrentUser);
            if (!string.IsNullOrEmpty(project))
                ProjectComboBox.SelectedValue = project;
        }

        private void CollarMfgrComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCollarList((string)CollarMfgrComboBox.SelectedValue);
        }

        private void ProjectComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            AML.UpdateSetting("project", (string)ProjectComboBox.SelectedValue);
        }

    }
}
