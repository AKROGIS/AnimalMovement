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
        private string CurrentUser { get; set; }
        private CollarFile File { get; set; }
        private bool IsFileEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        internal FileDetailsForm(CollarFile file)
        {
            InitializeComponent();
            RestoreWindow();
            File = file;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpControls();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //File is in a different DataContext, get one in this DataContext
            if (File != null)
                File = Database.CollarFiles.FirstOrDefault(f => f.FileId == File.FileId);
            if (File == null)
                throw new InvalidOperationException("File Details Form not provided a valid File.");

            DatabaseViews = new AnimalMovementViews();
            var functions = new AnimalMovementFunctions();
            IsFileEditor = functions.IsProjectEditor(File.ProjectId, CurrentUser) ?? false;
        }

        private void SetUpControls()
        {
            FileNameTextBox.Text = File.FileName;
            FileIdTextBox.Text = File.FileId.ToString(CultureInfo.CurrentCulture);
            FormatTextBox.Text = File.LookupCollarFileFormat.Name;
            CollarManufacturerTextBox.Text = File.Collar == null ? "" : File.Collar.LookupCollarManufacturer.Name;
            CollarIdTextBox.Text = File.CollarId;
            UserNameTextBox.Text = File.UserName;
            UploadDateTextBox.Text = File.UploadDate.ToString(CultureInfo.CurrentCulture);
            ProjectTextBox.Text = File.Project == null ? "" : File.Project.ProjectName;
            OwnerTextBox.Text = File.ProjectInvestigator == null ? "" : File.ProjectInvestigator.Name;
            StatusTextBox.Text = File.LookupFileStatus.Name;
            EnableForm();
            SetParentChildFiles();
            DoneCancelButton.Focus();
        }

        private void SetParentChildFiles()
        {
            var fileHasChildren = Database.CollarFiles.Any(f => f.ParentFileId == File.FileId);
            var fileHasParent = File.ParentFileId != null;
            SourceFileButton.Visible = fileHasParent;
            GridViewLabel.Text = fileHasChildren ? "Files derived from this file" : "Summary of fixes in file";
            ChildFilesDataGridView.Visible = fileHasChildren;
            FixInfoDataGridView.Visible = !fileHasChildren;
            if (fileHasChildren)
            {
                var data = from file in Database.CollarFiles
                           where file.ParentFileId == File.FileId
                           select new
                               {
                                   file,
                                   file.FileId,
                                   file.FileName,
                                   Format = file.LookupCollarFileFormat.Name,
                                   Status = file.LookupFileStatus.Name,
                                   file.Collar
                               };
                ChildFilesDataGridView.DataSource = data;
                ChildFilesDataGridView.Columns[0].Visible = false;
            }
            else
            {
                var data = from fix in DatabaseViews.AnimalFixesByFiles
                           where fix.FileId == File.FileId
                           select fix;
                FixInfoDataGridView.DataSource = data;
            }
        }

        private void UpdateDataSource()
        {
            File.CollarId = CollarIdTextBox.Text.NullifyIfEmpty();
            File.FileName = FileNameTextBox.Text.NullifyIfEmpty();
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
                File.LookupFileStatus = (File.Status == 'A'
                                             ? Database.LookupFileStatus.First(s => s.Code == 'I')
                                             : Database.LookupFileStatus.First(s => s.Code == 'A'));
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
            if (File.ParentFileId == null)
                return;
            var form = new FileDetailsForm(File.ParentFile);
            form.DatabaseChanged += (o, args) => { OnDatabaseChanged(); LoadDataContext(); };
            form.Show(this);
        }

        private void ChildFilesDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //I can't use the DataSource here, because it is an anoymous type.
            var file = (CollarFile)ChildFilesDataGridView.SelectedRows[0].Cells[0].Value;
            var form = new FileDetailsForm(file);
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
