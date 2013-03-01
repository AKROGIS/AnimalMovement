using System;
using System.Linq;
using System.Windows.Forms;
using DataModel;
using Telonics;

//FIXME - Loading a TPF file should add ALL collars (even conflicting as inactive with a warning) - check PK in database
//TODO - When loading multiple files, do not cancel remainder of files if you skip a file
//TODO - Reload project Investigator form after add parameter file adds a collar
//TODO - Reload project Investigator form after add parameter file loads multiple files, but one has a problem

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
            SetupStatusList();
        }

        private void SetupOwnerList()
        {
            //currentPi = Settings.GetDefaultParameterFileFormat();
            var query = Database.ProjectInvestigators;
            var owners = query.ToList();
            OwnerComboBox.DataSource = owners;
            OwnerComboBox.DisplayMember = "Name";
            var owner = query.FirstOrDefault(o => o.Login == CurrentUser);
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

        private void SetupStatusList()
        {
            char? statusCode = 'A';
            var query = Database.LookupCollarFileStatus;
            var statuses = query.ToList();
            StatusComboBox.DataSource = statuses;
            StatusComboBox.DisplayMember = "Name";
            var status = statuses.FirstOrDefault(s => s.Code == statusCode.Value);
            if (status != null)
                StatusComboBox.SelectedItem = status;
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
            var query = Database.Collars.Where(c => c.ProjectInvestigator == owner && c.CollarModel == "Gen3"
                && c.ArgosId != null);
            var collars = query.ToList();
            CollarComboBox.DataSource = collars;
            CollarComboBox.DisplayMember = "ArgosId";
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
            return files.Split(';').All(UploadTpfFile);
        }

        private bool UploadTpfFile(string file)
        {
            var owner = (ProjectInvestigator)OwnerComboBox.SelectedItem;
            var format = (LookupCollarParameterFileFormat)FormatComboBox.SelectedItem;
            CollarParameterFile paramFile = UploadParameterFile(owner, format, file);
            if (paramFile == null)
                return false;
            var tpfFile = new TpfFile(file);
            foreach (TpfCollar tpfCollar in tpfFile.GetCollars())
            {
                TpfCollar collar1 = tpfCollar;
                var collar = Database.Collars.FirstOrDefault(c => c.CollarManufacturer == "Telonics" && c.CollarId == collar1.Ctn);
                if (collar == null)
                {
                    string msg = String.Format(
                        "The file: {3}\n describes a collar (CTN: {0}, Argos: {1}, Frequency: {2})\n" +
                        " which is not in the database.  Do you want to add this collar to the database?", collar1.Ctn, collar1.ArgosId,
                        collar1.Frequency, file);
                    DialogResult answer = MessageBox.Show(msg, "Add Collar?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (answer == DialogResult.Yes)
                    {
                        collar = AddTpfCollar(collar1, owner);
                        if (collar == null)
                            continue;
                    }
                    else
                        continue;
                }
                if (collar.ArgosId != collar1.ArgosId || collar.Frequency != collar1.Frequency)
                {
                    string msg = String.Format(
                        "The database record for collar (CTN: {0})\n" +
                        "does not match the information in the TPF file.({1}\n" +
                        "Database Argos ID: {2}\n" + "TPF file Argos ID: {3}\n" +
                        "Database Frequency: {4}\n" + "TPF file Frequency: {5}\n" +
                        "This collar is being skipped.", collar1.Ctn, file,
                        collar.ArgosId, collar1.ArgosId, collar.Frequency ,collar1.Frequency);
                    MessageBox.Show(msg, "Consistancy Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }
                if (!AddCollarParameters(collar, paramFile, collar1.TimeStamp))
                {
                    string msg = String.Format(
                        "The collar: {0} could not be associated with the file: {1}\n" +
                        "You will need to edit the collar parameters to fix this.", collar1.Ctn, file);
                    MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return true;
        }

        private bool UploadPpfFile(string file)
        {
            var owner = (ProjectInvestigator)OwnerComboBox.SelectedItem;
            var collar = (Collar)CollarComboBox.SelectedItem;
            var format = (LookupCollarParameterFileFormat)FormatComboBox.SelectedItem;
            CollarParameterFile paramFile = UploadParameterFile(owner, format, file);
            if (paramFile == null)
                return false;
            if (!AddCollarParameters(collar, paramFile, null))
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
                data = System.IO.File.ReadAllBytes(filename);
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
                LookupCollarFileStatus = (LookupCollarFileStatus)StatusComboBox.SelectedItem,
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


        private Collar AddTpfCollar(TpfCollar tpfCollar, ProjectInvestigator owner)
        {
            //FIXME - launch add collar form with defaults
            DisposeOfPreviousVersionOfCollar(tpfCollar);
            var collar = new Collar
            {
                CollarManufacturer = "Telonics",
                CollarId = tpfCollar.Ctn,
                CollarModel = "Gen4",
                ArgosId = tpfCollar.ArgosId,
                HasGps = true, //guess
                //DisposalDate = ???,
                //Owner = ???,
                //Notes = ???,
                SerialNumber = tpfCollar.Ctn.Substring(0,6),
                Frequency = tpfCollar.Frequency,
                Manager = owner.Login
            };
            Database.Collars.InsertOnSubmit(collar);

            try
            {
                Database.SubmitChanges();
            }
            catch (Exception ex)
            {
                Database.Collars.DeleteOnSubmit(collar);
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return collar;
        }

        private void DisposeOfPreviousVersionOfCollar(TpfCollar tpfCollar)
        {
            var conflictingCollar = Database.Collars.FirstOrDefault(c => c.ArgosId == tpfCollar.ArgosId && c.DisposalDate == null);
            if (conflictingCollar == null)
                return;

            string msg = String.Format(
                "The Argos Id ({0}) for the new collar (Telonics/{1})\n" +
                "is being used by an existing collar ({2})\n" +
                "You cannot have two active collars with the same Argos Id.\n" +
                "Adding the new collar will fail unless the existing collar is deactivated.\n\n"+
                "Do you want to deactivate the existing collar?", tpfCollar.ArgosId, tpfCollar.Ctn, conflictingCollar);
            DialogResult answer = MessageBox.Show(msg, "Deactivate Existing Collar?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.Yes)
            {
                conflictingCollar.DisposalDate = tpfCollar.TimeStamp;
                try
                {
                    Database.SubmitChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private bool AddCollarParameters(Collar collar, CollarParameterFile paramFile, DateTime? timeStamp)
        {
            var collarParameters = new CollarParameter
            {
                Collar = collar,
                CollarParameterFile = paramFile,
                StartDate = timeStamp
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
