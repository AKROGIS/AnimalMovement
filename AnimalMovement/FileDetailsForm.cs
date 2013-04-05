using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class FileDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private AnimalMovementViews DatabaseViews { get; set; }
        private AnimalMovementFunctions DatabaseFunctions { get; set; }
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
            DatabaseViews = new AnimalMovementViews();
            DatabaseFunctions = new AnimalMovementFunctions();
            File = Database.CollarFiles.FirstOrDefault(f => f.FileId == FileId);
            if (File == null)
            {
                MessageBox.Show("File not found.", "Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            IsFileEditor = DatabaseFunctions.IsEditor(File.ProjectId, CurrentUser) ?? false;
            FileNameTextBox.Text = File.FileName;
            FileIdTextBox.Text = File.FileId.ToString(CultureInfo.CurrentCulture);
            FormatTextBox.Text = File.LookupCollarFileFormat.Name;
            CollarManufacturerTextBox.Text = File.Collar == null ? "" : File.Collar.LookupCollarManufacturer.Name.Trim();
            CollarIdTextBox.Text = File.CollarId;
            UserNameTextBox.Text = File.UserName;
            UploadDateTextBox.Text = File.UploadDate.ToString(CultureInfo.CurrentCulture);
            ProjectTextBox.Text = File.Project.ProjectName;
            StatusTextBox.Text = File.LookupFileStatus.Name;
            UpdateCollarFixes();
            EnableForm();
            DoneCancelButton.Focus();
            SourceFileButton.Visible = File.ParentFileId != null;
        }

        private void UpdateDataSource()
        {
            File.CollarId = CollarIdTextBox.Text.NullifyIfEmpty();
            File.FileName = FileNameTextBox.Text.NullifyIfEmpty();
        }

        private void UpdateCollarFixes()
        {
            var data = from fix in DatabaseViews.AnimalFixesByFiles
                       where fix.FileId == FileId
                       select fix;
            FixInfoDataGridView.DataSource = data;
        }

        private void ShowContentsButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var form = new FileContentsForm(File.Contents.ToArray(), File.FileName);
            Cursor.Current = Cursors.Default;
            form.Show(this);
        }

        private void ChangeStatusbutton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                File.Status = (File.Status == 'A' ? 'I' : 'A');
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                string msg = "Unable to change the status.\n" +
                             "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cursor.Current = Cursors.Default;
                return;
            }
            Cursor.Current = Cursors.Default;
            OnDatabaseChanged();
            LoadDataContext();
        }

        private void EnableForm()
        {
            EditSaveButton.Enabled = IsFileEditor && File.ParentFileId == null;
            ChangeStatusButton.Enabled = IsFileEditor && File.ParentFileId == null;
        }


        private void SourceFileButton_Click(object sender, EventArgs e)
        {
            //If the user make changes in the info dialog, they happen in a different context, so we need to reload this context if changes were made
            if (File.ParentFileId == null)
                return;
            var form = new FileDetailsForm(File.ParentFileId.Value, CurrentUser);
            form.DatabaseChanged += (o, args) => { OnDatabaseChanged(); LoadDataContext(); };
            form.Show(this);
        }

        private void EditSaveButton_Click(object sender, EventArgs e)
        {
            //This button is not enabled unless editing is permitted 
            if (EditSaveButton.Text == "Edit")
            {
                // The user wants to edit, Enable form
                EditSaveButton.Text = "Save";
                DoneCancelButton.Text = "Cancel";
                SetEditingControls();
            }
            else
            {
                //User is saving
                try
                {
                    UpdateDataSource();
                    Database.SubmitChanges();
                    OnDatabaseChanged();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to save changes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                EditSaveButton.Text = "Edit";
                DoneCancelButton.Text = "Done";
                SetEditingControls();
            }
        }


        private void DoneCancelButton_Click(object sender, EventArgs e)
        {
            if (DoneCancelButton.Text == "Cancel")
            {
                DoneCancelButton.Text = "Done";
                EditSaveButton.Text = "Edit";
                SetEditingControls();
                //Reset state from database
                LoadDataContext();
            }
            else
            {
                Close();
            }
        }


        private void SetEditingControls()
        {
            bool editModeEnabled = EditSaveButton.Text == "Save";
            FileNameTextBox.Enabled = editModeEnabled;
            CollarIdTextBox.Enabled = editModeEnabled && File.Status == 'I';
            ChangeStatusButton.Enabled = !editModeEnabled && IsFileEditor;
        }


        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }

    }
}
