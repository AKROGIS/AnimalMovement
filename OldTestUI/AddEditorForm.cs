using System;
using System.Windows.Forms;
using AnimalMovementLibrary;

namespace TestUI
{
    public partial class AddEditorForm : Form
    {
        public AddEditorForm(string user)
        {
            InitializeComponent();
            CurrentUser = user;
        }

        public string CurrentUser { get; private set; }
        public string Project { get; set; }

        private void AddEditorButton_Click(object sender, EventArgs e)
        {
            try
            {
                AML.AddEditor(CurrentUser, Project, EditorTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to Add Editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
