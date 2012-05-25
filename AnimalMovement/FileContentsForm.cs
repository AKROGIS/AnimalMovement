using System;
using System.IO;
using System.Windows.Forms;

namespace AnimalMovement
{
    internal sealed partial class FileContentsForm : BaseForm
    {
        internal FileContentsForm(string text, string name)
        {
            InitializeComponent();
            RestoreWindow();
            ContentRichTextBox.Text = text;
            Text = name;
        }

        private void CopyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(ContentRichTextBox.Text);
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog.FileName = Text;
            SaveFileDialog.ShowDialog(this);
            var file = File.CreateText(SaveFileDialog.FileName);
            file.Write(ContentRichTextBox.Text);
            file.Close();
        }
    }
}
