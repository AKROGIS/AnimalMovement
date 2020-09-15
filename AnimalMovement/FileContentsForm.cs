using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AnimalMovement
{
    internal sealed partial class FileContentsForm : BaseForm
    {
        private readonly Byte[] _contents;

        internal FileContentsForm(Byte[] contents, string name)
        {
            InitializeComponent();
            RestoreWindow();
            _contents = contents;
            ContentRichTextBox.Text = Encoding.ASCII.GetString(contents);
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
            var file = File.Create(SaveFileDialog.FileName);
            file.Write(_contents, 0, _contents.Length);
            file.Close();
        }
    }
}
