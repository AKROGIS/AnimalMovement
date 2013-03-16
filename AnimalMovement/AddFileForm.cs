using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DataModel;
using Telonics;
using FileLibrary;

//TODO - Provide better error messages when uploading files fails

/*
 * The collar list displays the following:
 * if all is selected,  all collars (regardless of owner) for the chosen manufacturer are shown.
 * otherwise, only those collars owned by the PI of the selected project and from the chosen manufacturer are shown
 */

namespace AnimalMovement
{
    internal partial class AddFileForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private string ProjectId { get; set; }
        private Project Project { get; set; }
        private List<Collar> AllCollars { get; set; } 
        internal event EventHandler DatabaseChanged;
        private Byte[] _fileContents;
        private Byte[] _fileHash;

        internal AddFileForm(string user)
        {
            CurrentUser = user;
            SetupForm();
        }

        internal AddFileForm(string projectId, string user)
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
            AllCollars = Database.Collars.ToList();
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
            //CollarComboBox.DataSource will be set to a filtered list once the manufacturer is selected;
            CollarComboBox.DisplayMember = "CollarId";
            CollarMfgrComboBox.DataSource = Database.LookupCollarManufacturers;
            CollarMfgrComboBox.DisplayMember = "Name";
            SetUpFormatList();
        }

        private void SetUpFormatList()
        {
            char? formatCode = Settings.GetDefaultFileFormat();
            var query = Database.LookupCollarFileFormats;
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
            UploadButton.Enabled = Project != null && !string.IsNullOrEmpty(FileNameTextBox.Text) && FormatComboBox.SelectedItem != null;
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            LoadAndHashFile(FileNameTextBox.Text);
            if (_fileContents == null)
                return;
            if (AbortBecauseDuplicate())
                return;

            UploadButton.Text = "Working...";
            UploadButton.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            try
            {
                var collar = CollarComboBox.Enabled ? (Collar)CollarComboBox.SelectedItem : null;
                (new FileProcessor()).LoadPath(FileNameTextBox.Text, Project, Collar);
            }
            /*
            var file = new CollarFile
                {
                    Project1 = Project,
                    FileName = System.IO.Path.GetFileName(FileNameTextBox.Text),
                    LookupCollarFileFormat = (LookupCollarFileFormat)FormatComboBox.SelectedItem,
                    LookupCollarManufacturer = (LookupCollarManufacturer)CollarMfgrComboBox.SelectedItem,
                    CollarId = CollarComboBox.Enabled ? ((Collar)CollarComboBox.SelectedItem).CollarId : null,
                    LookupCollarFileStatus = Database.LookupCollarFileStatus.FirstOrDefault(s => s.Code == (StatusActiveRadioButton.Checked ? 'A' : 'I')),
                    Contents = _fileContents,
                };
            Database.CollarFiles.InsertOnSubmit(file);

            try
            {
                Database.SubmitChanges();
            }
            */
            catch (SqlException ex)
            {
                Database.CollarFiles.DeleteOnSubmit(file);
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (FileException ex)
            {
                string msg = "The file cannot be read.\nSystem Message:\n  " + ex.Message;
                MessageBox.Show(msg, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileNameTextBox.Focus();
                return;
            }
            catch (FormatException ex)
            {
                string msg = "File format error:  " + ex.Message;
                MessageBox.Show(msg, "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileNameTextBox.Focus();
                return;
            }
            catch (DuplicateException ex)
            {
                var duplicate = Database.CollarFiles.FirstOrDefault(f => f.Sha1Hash == _fileHash);
                if (duplicate == null)
                    return false;
                var msg = "The contents of this file have already been loaded as" + Environment.NewLine +
                    String.Format("file '{0}' in project '{1}'.", duplicate.FileName, duplicate.Project1.ProjectName) + Environment.NewLine +
                        "Loading a file multiple times is not a problem for the database," + Environment.NewLine +
                        "but it is unnecessary, inefficient, and generally confusing." + Environment.NewLine +
                        "It is recommended that you do NOT load this file again." + Environment.NewLine + Environment.NewLine +
                        "Are you sure you want to proceed?";
                var result = MessageBox.Show(this, msg,
                                             "Duplicate file", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                return result != DialogResult.Yes;
            }
            finally
            {
                FileNameTextBox.Focus();
                UploadButton.Text = "Upload";
                Cursor.Current = Cursors.Default;
            }

            Cursor.Current = Cursors.Default;

            OnDatabaseChanged();
            UploadButton.Text = "Upload";
            FileNameTextBox.Text = String.Empty;
            DialogResult = DialogResult.OK;
        }

        private void LoadAndHashFile(string path)
        {
            try
            {
                _fileContents = System.IO.File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                string msg = "The file cannot be read.\nSystem Message:\n  " + ex.Message;
                MessageBox.Show(msg, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileNameTextBox.Focus();
                return;
            }
            _fileHash = (new SHA1CryptoServiceProvider()).ComputeHash(_fileContents);
        }

        private bool AbortBecauseDuplicate()
        {
            var duplicate = Database.CollarFiles.FirstOrDefault(f => f.Sha1Hash == _fileHash);
            if (duplicate == null)
                return false;
            var msg = "The contents of this file have already been loaded as" + Environment.NewLine +
                String.Format("file '{0}' in project '{1}'.", duplicate.FileName, duplicate.Project1.ProjectName) + Environment.NewLine +
                "Loading a file multiple times is not a problem for the database," + Environment.NewLine +
                "but it is unnecessary, inefficient, and generally confusing." + Environment.NewLine +
                "It is recommended that you do NOT load this file again." + Environment.NewLine + Environment.NewLine +
                "Are you sure you want to proceed?";
            var result = MessageBox.Show(this, msg,
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
                if (header.Regex != null)
                    if (new Regex(header.Regex).IsMatch(fileHeader))
                    {
                        code = header.FileFormat;
                        break;
                    }
            }
            if (code == '?' && (new ArgosEmailFile(path)).GetPrograms().Any())
                // We already checked for ArgosAwsFile with the header
                code = 'E';
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
            RefreshCollarComboBox();
        }

        private void FormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormatComboBox.SelectedItem != null)
            {
                var format = (LookupCollarFileFormat)FormatComboBox.SelectedItem;
                CollarComboBox.Enabled = format.HasCollarIdColumn == 'N';
                CollarMfgrComboBox.SelectedItem = format.LookupCollarManufacturer;
                Settings.SetDefaultFileFormat(format.Code);
            }
            EnableUpload();
        }

        private void CollarMfgrComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshCollarComboBox();
        }

        private void AllCollarsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            RefreshCollarComboBox();
        }

        private void RefreshCollarComboBox()
        {
            IEnumerable<Collar> data = AllCollars;
            if (CollarMfgrComboBox.SelectedItem != null)
                data = data.Where(
                c => c.CollarManufacturer == ((LookupCollarManufacturer)CollarMfgrComboBox.SelectedItem).CollarManufacturer);
            if (!AllCollarsCheckBox.Checked && Project != null)
                data = data.Where(c => c.Manager.Equals(Project.ProjectInvestigator, StringComparison.InvariantCultureIgnoreCase));
            CollarComboBox.DataSource = data.ToList();
        }

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this,EventArgs.Empty);
        }

        //FIXME - move these methods to a better place in UI
        private void SummerizeFileButton_Click(object sender, EventArgs e)
        {
            CollarFile file;
            (new FileProcessor()).SummerizeFile(file);
        }
        
        private void SummerizeAllButton_Click(object sender, EventArgs e)
        {
            (new FileProcessor()).SummerizeAll();
        }
        
        private void DownloadProgramButton_Click(object sender, EventArgs e)
        {
            ArgosProgram program;
            FileLibrary.ArgosDownloader.DownloadArgosProgram(program);
        }
        
        private void DownloadPlatformButton_Click(object sender, EventArgs e)
        {
            ArgosPlatform platform;
            FileLibrary.ArgosDownloader.DownloadArgosPlatform(platform);
        }
        
        private void DownloadAllButton_Click(object sender, EventArgs e)
        {
            ArgosProgram program;
            FileLibrary.ArgosDownloader.DownloadAll(CurrentUser);
        }
        

    }
}
