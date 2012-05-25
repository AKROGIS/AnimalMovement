using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataModel;

//TODO - Allow changing filename and collar

namespace AnimalMovement
{
    internal partial class FileDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private int FileId { get; set; }
        private CollarFile File { get; set; }
        private bool IsFileEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        internal FileDetailsForm(int fileId, string user)
        {
            InitializeComponent();
            RestoreWindow();
            FileId = fileId;
            CurrentUser = user;
            LoadDataContext();
        }

        private void LoadDataContext()
        {
 	        Database = new AnimalMovementDataContext();
            File = Database.CollarFiles.FirstOrDefault(f => f.FileId == FileId);
            if (File == null)
            {
                MessageBox.Show("File not found.", "Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            IsFileEditor = Database.IsEditor(File.Project, CurrentUser) ?? false;
            FileNameTextBox.Text = File.FileName;
            FileIdTextBox.Text = File.FileId.ToString(CultureInfo.CurrentCulture);
            FormatTextBox.Text = File.LookupCollarFileFormat.Name;
            CollarManufacturerTextBox.Text = File.LookupCollarManufacturer.Name.Trim();
            CollarIdTextBox.Text = File.CollarId;
            UserNameTextBox.Text = File.UserName;
            UploadDateTextBox.Text = File.UploadDate.ToString(CultureInfo.CurrentCulture);
            ProjectTextBox.Text = File.Project1.ProjectName;
            StatusTextBox.Text = File.LookupCollarFileStatus.Name;
            UpdateCollarFixes();
            EnableForm();
            Closebutton.Focus();
        }

        private void UpdateCollarFixes()
        {
            var db = new AnimalMovementViewsDataContext();
            var data = from fix in db.AnimalFixesByFiles
                       where fix.FileId == FileId
                       select fix;
            FixInfoDataGridView.DataSource = data;
        }

        private void ShowContentsButton_Click(object sender, EventArgs e)
        {
            var cursor = Cursor.Current;
            Cursor = Cursors.WaitCursor;
            string contents = Encoding.UTF8.GetString(File.Contents.ToArray());
            var form = new FileContentsForm(contents, File.FileName);
            Cursor = cursor;
            form.Show(this);
        }

        private void ChangeStatusbutton_Click(object sender, EventArgs e)
        {
            var cursor = Cursor.Current;
            Cursor = Cursors.WaitCursor;
            //When changing status, the database can only process about 80 records per second
            int defaultTimeout = Database.CommandTimeout;
            Database.CommandTimeout = 1250;  //In seconds, default is 30
            try
            {
                Database.CollarFile_UpdateStatus(File.FileId, File.Status == 'A' ? 'I' : 'A');
            }
            catch (Exception ex)
            {
                string msg = "Unable to change the status.\n" +
                             "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor = cursor;
                return;
            }
            Cursor = cursor;
            Database.CommandTimeout = defaultTimeout;
            OnDatabaseChanged();
            LoadDataContext();
        }

        private void EnableForm()
        {
            ChangeStatusbutton.Enabled = IsFileEditor;
        }

        private void Closebutton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }

    }
}
