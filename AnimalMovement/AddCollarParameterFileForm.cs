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
            SetupOwnerList();
            SetupFormatList();
            SetupCollarList();
        }

        private void SetupOwnerList()
        {
            //currentPi = Settings.GetDefaultParameterFileFormat();
            var query = Database.ProjectInvestigators;
            var owners = query.ToList();
            OwnerComboBox.DataSource = owners;
            OwnerComboBox.DisplayMember = "Name";
            var owner = owners.FirstOrDefault(o => o.Login == CurrentUser);
            if (owner != null)
                OwnerComboBox.SelectedItem = owner;
        }

        private void SetupFormatList()
        {
            char? formatCode = Settings.GetDefaultParameterFileFormat();
            var query = Database.LookupCollarParameterFileFormats;
            var formats = query.ToList();
            FormatComboBox.DataSource = formats;
            FormatComboBox.DisplayMember = "Description";
            if (!formatCode.HasValue)
                return;
            var format = formats.FirstOrDefault(f => f.Code == formatCode.Value);
            if (format != null)
                FormatComboBox.SelectedItem = format;
        }

        private void SetupCollarList()
        {
            if (OwnerComboBox.SelectedItem == null)
            {
                CollarComboBox.DataSource = null;
                return;
            }
            var owner = (ProjectInvestigator) OwnerComboBox.SelectedItem;
            //A collar is only required with a ppf file, which means a telonics gen 3 collar.
            //The AlternaitveID (i.e. Argos ID) is used as the identifier since the ppf files are usually named with the argos id.
            var query = Database.Collars.Where(c => c.ProjectInvestigator == owner && c.CollarModel == "TelonicsGen3"
                && c.AlternativeId != null);
            var collars = query.ToList();
            CollarComboBox.DataSource = collars;
            CollarComboBox.DisplayMember = "AlternativeId";
        }
 
        private void EnableUpload()
        {
            UploadButton.Enabled = OwnerComboBox.SelectedItem != null
                                   && !string.IsNullOrEmpty(FileNameTextBox.Text) 
                                   && FormatComboBox.SelectedItem != null
                                   && (((LookupCollarParameterFileFormat)FormatComboBox.SelectedItem).Code != 'B'
                                       || CollarComboBox.SelectedItem != null);
        }


        private void UploadButton_Click(object sender, EventArgs e)
        {
            UploadButton.Text = "Working...";
            UploadButton.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();

            var format = (LookupCollarParameterFileFormat)FormatComboBox.SelectedItem;
            switch (format.Code)
            {
                // Keep this current with changes to LookupCollarParameterFileFormats table
                case 'A':
                    if (UploadTpfFiles(FileNameTextBox.Text))
                    {
                        OnDatabaseChanged();
                        Cursor.Current = Cursors.Default;
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        UploadButton.Text = "Upload";
                    }
                    break;
                case 'B':
                    if (UploadPpfFile(FileNameTextBox.Text))
                    {
                        OnDatabaseChanged();
                        Cursor.Current = Cursors.Default;
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        UploadButton.Text = "Upload";
                    }
                    break;
                default:
                    MessageBox.Show("Unexpected parameter file format encountered", "Program Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UploadButton.Text = "Upload";
                    break;
            }
        }

        private bool UploadTpfFiles(string files)
        {
            var filelist = files.Split(';');
            return false;
        }

        private bool UploadPpfFile(string file)
        {
            var owner = (ProjectInvestigator) OwnerComboBox.SelectedItem;
            var collar = (Collar) CollarComboBox.SelectedItem;
            var format = (LookupCollarParameterFileFormat) FormatComboBox.SelectedItem; 
            CollarParameterFile paramFile = UploadParameterFile(owner, format, file);
            if (paramFile == null)
                return false;
            if (!AddCollarParameters(collar, paramFile))
            {
                string msg =
                    "The parameter file was added to the database, but could not be associated with the collar.\n";
                msg = msg + "You will need to edit the parameter file details to fix this.";
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return true;
        }

        private CollarParameterFile UploadParameterFile(ProjectInvestigator owner, LookupCollarParameterFileFormat format, string filename)
        {
            if (AbortBecauseDuplicate(owner, format, filename))
                return null;

            byte[] data;
            try
            {
                data = System.IO.File.ReadAllBytes(FileNameTextBox.Text);
            }
            catch (Exception ex)
            {
                string msg = "The file cannot be read.\nSystem Message:\n" + ex.Message;
                MessageBox.Show(msg, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileNameTextBox.Focus();
                return null;
            }
            var file = new CollarParameterFile
            {
                FileName = System.IO.Path.GetFileName(filename),
                LookupCollarParameterFileFormat = format,
                ProjectInvestigator = owner,
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
                return null;
            }
            return file;
        }


        private bool AddCollarParameters(Collar collar, CollarParameterFile paramFile)
        {
            var collarParameters = new CollarParameter
                {
                    Collar = collar,
                    CollarParameterFile = paramFile
                };
            Database.CollarParameters.InsertOnSubmit(collarParameters);

            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                Database.CollarParameters.DeleteOnSubmit(collarParameters);
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }


        private bool AbortBecauseDuplicate(ProjectInvestigator owner, LookupCollarParameterFileFormat format, string fileName)
        {
            if (!Database.CollarParameterFiles.Any(f =>
                f.FileName == System.IO.Path.GetFileName(fileName) &&
                f.LookupCollarParameterFileFormat == format &&
                f.ProjectInvestigator == owner
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
                FileNameTextBox.Text = string.Join(";", openFileDialog.FileNames);
            }
        }

        private void FileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableUpload();
        }

        private void OwnerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupCollarList();
            EnableUpload();
        }

        private void FormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormatComboBox.SelectedItem != null)
            {
                var format = (LookupCollarParameterFileFormat)FormatComboBox.SelectedItem;
                switch (format.Code)
                {
                    // Keep this current with changes to LookupCollarParameterFileFormats table
                    case 'A':
                        Settings.SetDefaultParameterFileFormat(format.Code);
                        SetupForTpfFile();
                        EnableUpload();
                        break;
                    case 'B':
                        Settings.SetDefaultParameterFileFormat(format.Code);
                        SetupForPpfFile();
                        EnableUpload();
                        break;
                    default:
                        MessageBox.Show("Un expected parameter file format encountered", "Program Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
        }

        private void SetupForTpfFile()
        {
            FileNameTextBox.Text = String.Empty;
            CollarComboBox.Visible = false;
            CollarLabel.Visible = false;
            openFileDialog.DefaultExt = "tpf";
            openFileDialog.Filter = "TPF Files|*.tpf|All Files|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;
        }

        private void SetupForPpfFile()
        {
            FileNameTextBox.Text = String.Empty;
            CollarComboBox.Visible = true;
            CollarLabel.Visible = true;
            openFileDialog.DefaultExt = "*.ppf";
            openFileDialog.Filter = "PPF Files|*.ppf|All Files|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
        }

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this,EventArgs.Empty);
        }

    }
}
