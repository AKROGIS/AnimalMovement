using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataModel;

//TODO - Enable Add/Edit/Delete for parameter assignments and date ranges
//TODO - Enable edit of parameter file type
//TODO - Implement Psuedo PPF files (i.e. Buck's interval data w/o ppf file)
//FIXME - Contents of PPF files is binary, Save as does not work
//FIXME - Save As when viewing file contents should not add the csv extension

namespace AnimalMovement
{
    internal partial class CollarParameterFileDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private int FileId { get; set; }
        private CollarParameterFile File { get; set; }
        private bool IsFileEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        internal CollarParameterFileDetailsForm(int fileId, string user)
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
            File = Database.CollarParameterFiles.FirstOrDefault(f => f.FileId == FileId);
            if (File == null)
            {
                MessageBox.Show("File not found.", "Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            IsFileEditor = String.Equals(File.Owner, CurrentUser, StringComparison.OrdinalIgnoreCase);
            FileNameTextBox.Text = File.FileName;
            FileIdTextBox.Text = File.FileId.ToString(CultureInfo.CurrentCulture);
            FormatTextBox.Text = File.LookupCollarParameterFileFormat.Name;
            UserNameTextBox.Text = File.UploadUser;
            UploadDateTextBox.Text = File.UploadDate.ToString(CultureInfo.CurrentCulture);
            OwnerTextBox.Text = File.Owner;
            UpdateCollars();
            EnableForm();
            DoneCancelButton.Focus();
        }

        private void UpdateDataSource()
        {
            File.FileName = FileNameTextBox.Text.NullifyIfEmpty();
        }

        private void UpdateCollars()
        {
            //var db = new AnimalMovementViewsDataContext();
            var data = from collar in Database.CollarParameters
                       where collar.FileId == FileId
                       select collar;
            CollarsDataGridView.DataSource = data;
        }

        private void ShowContentsButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string contents = Encoding.UTF8.GetString(File.Contents.ToArray());
            var form = new FileContentsForm(contents, File.FileName);
            Cursor.Current = Cursors.Default;
            form.Show(this);
        }

        private void EnableForm()
        {
            //ChangeStatusbutton.Enabled = IsFileEditor;
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
            FileNameTextBox.Enabled = editModeEnabled; //&& IsFileEditor;
            OwnerTextBox.Enabled = editModeEnabled && IsFileEditor;
        }


        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }
    }
}
