using System;
using System.Linq;
using System.Windows.Forms;
using DataModel;


/*
 * The collar list displays the following:
 * if all is selected,  all collars (regardless of owner) for the chosen manufacturer are shown.
 * otherwise, only those collars owned by the PI of the selected project and from the chosen manufacturer are shown
 */

namespace AnimalMovement
{
    internal partial class AddFolderForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private string ProjectId { get; set; }
        private Project Project { get; set; }
        private LookupCollarFileFormat Format;
        internal event EventHandler DatabaseChanged;

        internal AddFolderForm(string user)
        {
            CurrentUser = user;
            SetupForm();
        }

        internal AddFolderForm(string projectId, string user)
        {
            ProjectId = projectId;
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
            if (ProjectId == null)
                ProjectId = Settings.GetDefaultProject();

            Database = new AnimalMovementDataContext();
            //Weirdness: Project points into our datacontext, which gets changed
            //when we requery the projects table, so setting it here doen't work
            //Database.Projects.FirstOrDefault(p => p.ProjectId == ProjectId);

            if (Database == null || CurrentUser == null)
            {
                MessageBox.Show("Insufficent information to initialize form.", "Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            //Database.Log = Console.Out;
            var query = from p in Database.Projects
                        where p.ProjectInvestigator == CurrentUser ||
                              p.ProjectEditors.Any(u => u.Editor == CurrentUser)
                        select p;
            var myProjects = query.ToList();
            ProjectComboBox.DataSource = myProjects;
            Project = ProjectId == null
                    ? myProjects.FirstOrDefault()
                    : Project = myProjects.FirstOrDefault(p => p.ProjectId == ProjectId);
            ProjectComboBox.DisplayMember = "ProjectName";
            ProjectComboBox.SelectedItem = Project;
        }

        private void EnableUpload()
        {
            UploadButton.Enabled = Project != null && !string.IsNullOrEmpty(FileNameTextBox.Text) && Format != null;
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            UploadButton.Text = "Working...";
            UploadButton.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            foreach (var fileName in System.IO.Directory.EnumerateFiles(folderBrowserDialog.SelectedPath))
            {
                UploadFile(fileName);
            }

            Cursor.Current = Cursors.Default;
            UploadButton.Text = "Upload";
            FileNameTextBox.Text = String.Empty;
            DialogResult = DialogResult.OK;
        }

        private void UploadFile(string filePath)
        {
            if (AbortBecauseDuplicate(filePath))
                return;

            byte[] data;
            try
            {
                data = System.IO.File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                string msg = "The file cannot be read.\nSystem Message:\n"+ex.Message;
                MessageBox.Show(msg, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileNameTextBox.Focus();
                return;
            }
            var file = new CollarFile
                {
                    Project = Project,
                    FileName = System.IO.Path.GetFileName(filePath),
                    LookupCollarFileFormat = Format,
                    CollarManufacturer = Format.LookupCollarManufacturer.CollarManufacturer,
                    CollarId = GetCollar(filePath),
                    LookupFileStatus = Database.LookupFileStatus.FirstOrDefault(s => s.Code == (StatusActiveRadioButton.Checked ? 'A' : 'I')),
                    Contents = data
                };
            Database.CollarFiles.InsertOnSubmit(file);

            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                Database.CollarFiles.DeleteOnSubmit(file);
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            OnDatabaseChanged();
        }

        private static string GetCollar(string filePath)
        {
            var fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            if (fileName == null)
                return null;
            var collar = fileName.Split('_')[0];
            return collar;
        }

        private bool AbortBecauseDuplicate(string filePath)
        {
            if (!Database.CollarFiles.Any(f =>
                f.Project == Project &&
                f.FileName == System.IO.Path.GetFileName(filePath) &&
                f.LookupCollarFileFormat == Format
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
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                FileNameTextBox.Text = folderBrowserDialog.SelectedPath;
                var file1 = System.IO.Directory.EnumerateFiles(folderBrowserDialog.SelectedPath).FirstOrDefault();
                Format = GuessFileFormat(file1);
            }
        }

        private LookupCollarFileFormat GuessFileFormat(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return null;
            }
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

        private void ProjectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Project = ProjectComboBox.SelectedItem as Project;
            if (Project != null)
                Settings.SetDefaultProject(Project.ProjectId);
            EnableUpload();
        }

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this,EventArgs.Empty);
        }

    }
}
