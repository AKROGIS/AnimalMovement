using System;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class AddCollarParameterFileForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        internal event EventHandler DatabaseChanged;

        internal AddCollarParameterFileForm(string user)
        {
            CurrentUser = user;
            SetupForm();
        }

        private void SetupForm()
        {
            InitializeComponent();
            RestoreWindow();
            LoadDataContext();
            EnableUpload();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();

            if (Database == null || CurrentUser == null)
            {
                MessageBox.Show("Insufficent information to initialize form.", "Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            //Database.Log = Console.Out;
            SetUpFormatList();
        }

        private void SetUpFormatList()
        {
            char? formatCode = Settings.GetDefaultParameterFileFormat();
            var query = Database.LookupCollarParameterFileFormats;
            var formats = query.ToList();
            FormatComboBox.DataSource = formats;
            FormatComboBox.DisplayMember = "Name";
            if (!formatCode.HasValue)
                return;
            var format = formats.FirstOrDefault(f => f.Code == formatCode.Value);
            if (format != null)
                FormatComboBox.SelectedItem = format;
        }

        private void EnableUpload()
        {
            UploadButton.Enabled = !string.IsNullOrEmpty(FileNameTextBox.Text) && FormatComboBox.SelectedItem != null;
        }


        private void UploadButton_Click(object sender, EventArgs e)
        {
            if (AbortBecauseDuplicate())
                return;

            UploadButton.Text = "Working...";
            UploadButton.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            byte[] data;
            try
            {
                data = System.IO.File.ReadAllBytes(FileNameTextBox.Text);
            }
            catch (Exception ex)
            {
                string msg = "The file cannot be read.\nSystem Message:\n"+ex.Message;
                MessageBox.Show(msg, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileNameTextBox.Focus();
                return;
            }
            var file = new CollarParameterFile
                {
                    FileName = System.IO.Path.GetFileNameWithoutExtension(FileNameTextBox.Text),
                    LookupCollarParameterFileFormat = (LookupCollarParameterFileFormat)FormatComboBox.SelectedItem,
                    Owner = CurrentUser,
                    Contents = data
                };
            Database.CollarParameterFiles.InsertOnSubmit(file);

            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                Database.CollarParameterFiles.DeleteOnSubmit(file);
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileNameTextBox.Focus();
                UploadButton.Text = "Upload";
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;

            OnDatabaseChanged();
            UploadButton.Text = "Upload";
            FileNameTextBox.Text = String.Empty;
            DialogResult = DialogResult.OK;
        }

        private bool AbortBecauseDuplicate()
        {
            if (!Database.CollarParameterFiles.Any(f =>
                f.FileName == System.IO.Path.GetFileNameWithoutExtension(FileNameTextBox.Text) &&
                f.LookupCollarParameterFileFormat == (LookupCollarParameterFileFormat)FormatComboBox.SelectedItem
                ))
                return false;
            var result = MessageBox.Show(this, "It appears this file has already been uploaded.  Are you sure you want to proceed?",
                "Duplicate file", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            return result != DialogResult.Yes;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileNameTextBox.Text = openFileDialog.FileName;
                LookupCollarFileFormat format = GuessFileFormat(FileNameTextBox.Text);
                FormatComboBox.SelectedItem = format;
            }
        }

        private LookupCollarFileFormat GuessFileFormat(string path)
        {
            string fileHeader = System.IO.File.ReadLines(path).First().Trim();
            var db = new SettingsDataContext();
            char code = '?';
            foreach (var header in db.LookupCollarFileHeaders)
            {
                if (fileHeader.StartsWith(header.Header, StringComparison.OrdinalIgnoreCase))
                {
                    code = header.FileFormat;
                    break;
                }
            }
            return Database.LookupCollarFileFormats.FirstOrDefault(f => f.Code == code);
        }

        private void FileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableUpload();
        }

        private void FormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormatComboBox.SelectedItem != null)
            {
                var format = (LookupCollarParameterFileFormat)FormatComboBox.SelectedItem;
                Settings.SetDefaultFileFormat(format.Code);
            }
            EnableUpload();
        }

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this,EventArgs.Empty);
        }    }
}
