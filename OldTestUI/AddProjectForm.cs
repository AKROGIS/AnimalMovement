using System;
using System.Windows.Forms;
using AnimalMovementLibrary;

namespace TestUI
{
    public partial class AddProjectForm : Form
    {
        public AddProjectForm()
        {
            InitializeComponent();
            EnableCreate();
        }

        public string CurrentUser { get; private set; }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            try
            {
                //investigator is not needed, the investigator will default to the caller
                AML.AddProject(CodeTextBox.Text, NameTextBox.Text, null,
                               UnitTextBox.Text, DescriptionTextBox.Text);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable To Create Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnableCreate()
        {
            if (string.IsNullOrEmpty(CodeTextBox.Text))
            {
                CreateButton.Enabled = false;
                return;
            }
            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                CreateButton.Enabled = false;
                return;
            }
            CreateButton.Enabled = true;
        }

        private void CodeTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableCreate();
        }

        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableCreate();
        }
    }
}
