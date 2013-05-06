using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using DataModel;
using Telonics;

namespace AnimalMovement
{
    internal partial class AddCollarParameterFileForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private ProjectInvestigator Investigator { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;
        private Byte[] _fileContents;
        private Byte[] _fileHash;

        internal AddCollarParameterFileForm(ProjectInvestigator investigator = null)
        {
            InitializeComponent();
            RestoreWindow();
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            Investigator = investigator;
            LoadDataContext();
            SetUpForm();
        }

        private void SetUpForm()
        {
            SetupOwnerList();
            SetupFormatList();
            SetupStatusList();
            EnableForm();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Investigator is in a different DataContext, get one in this DataContext
            if (Investigator != null)
                Investigator = Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == Investigator.Login);

        }

        protected override void OnShown(EventArgs e)
        {
            if (Investigator != null)
            {
                var functions = new AnimalMovementFunctions();
                IsEditor = functions.IsInvestigatorEditor(Investigator.Login, CurrentUser) ?? false;
                if (!IsEditor)
                    MessageBox.Show("You do not have permission to add a file for this investigator", "Permission Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                IsEditor = OwnerComboBox.Items.Count > 0;
                if (!IsEditor)
                    MessageBox.Show("You do not have permission to add a file", "Permission Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupOwnerList()
        {
            //If given a Investigator, set that and lock it.
            //else, set list to all investigator I can edit, and select null per the constructor request
            if (Investigator != null)
                OwnerComboBox.Items.Add(Investigator);
            else
            {
                OwnerComboBox.DataSource =
                    Database.ProjectInvestigators.Where(pi => pi.Login == CurrentUser ||
                     pi.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser));
            }
            OwnerComboBox.Enabled = Investigator == null;
            OwnerComboBox.SelectedItem = Investigator;
            OwnerComboBox.DisplayMember = "Name";
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
            var query = Database.LookupFileStatus;
            var statuses = query.ToList();
            StatusComboBox.DataSource = statuses;
            StatusComboBox.DisplayMember = "Name";
            var status = statuses.FirstOrDefault(s => s.Code == statusCode.Value);
            if (status != null)
                StatusComboBox.SelectedItem = status;
        }

        private void EnableForm()
        {
            UploadButton.Enabled = OwnerComboBox.SelectedItem != null
                                   && !string.IsNullOrEmpty(FileNameTextBox.Text) 
                                   && FormatComboBox.SelectedItem != null;
            CreateCollarsCheckBox.Enabled = CreateParametersCheckBox.Enabled && CreateParametersCheckBox.Checked;
            IgnoreSuffixCheckBox.Enabled = CreateParametersCheckBox.Enabled && CreateParametersCheckBox.Checked;
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
                    if (UploadPpfFiles(FileNameTextBox.Text))
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
            //return true if any file is successfully uploaded
            return files.Split(';').Aggregate(false, (current, file) => current || UploadTpfFile(file));
        }

        private bool UploadTpfFile(string file)
        {
            var owner = (ProjectInvestigator)OwnerComboBox.SelectedItem;
            var format = (LookupCollarParameterFileFormat)FormatComboBox.SelectedItem;
            CollarParameterFile paramFile = UploadParameterFile(owner, format, file);
            if (paramFile == null)
                return false;
            if (CreateParametersCheckBox.Checked)
                CreateParameters(file, owner, paramFile);
            return true;
        }

        private bool UploadPpfFiles(string files)
        {
            //return true if any file is successfully uploaded
            return files.Split(';').Aggregate(false, (current, file) => current || UploadPpfFile(file));
        }

        private bool UploadPpfFile(string file)
        {
            var owner = (ProjectInvestigator)OwnerComboBox.SelectedItem;
            var format = (LookupCollarParameterFileFormat)FormatComboBox.SelectedItem;
            CollarParameterFile paramFile = UploadParameterFile(owner, format, file);
            if (paramFile == null)
                return false;
            return true;
        }

        private CollarParameterFile UploadParameterFile(ProjectInvestigator owner, LookupCollarParameterFileFormat format, string filename)
        {
            LoadAndHashFile(FileNameTextBox.Text);
            if (_fileContents == null)
                return null;
            if (AbortBecauseDuplicate())
                return null;

            var file = new CollarParameterFile
            {
                FileName = System.IO.Path.GetFileName(filename),
                LookupCollarParameterFileFormat = format,
                ProjectInvestigator = owner,
                LookupFileStatus = (LookupFileStatus)StatusComboBox.SelectedItem,
                Contents = _fileContents
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


        private void CreateParameters(string file, ProjectInvestigator owner, CollarParameterFile paramFile)
        {
            var tpfFile = new TpfFile(file);
            foreach (TpfCollar tpfCollar in tpfFile.GetCollars())
            {
                TpfCollar collar1 = tpfCollar;
                var collar =
                    Database.Collars.FirstOrDefault(c => c.CollarManufacturer == "Telonics" && c.CollarId == collar1.Ctn);
                if (collar == null)
                {
                    string msg = String.Format(
                        "The file: {3}\n describes a collar (CTN: {0}, Argos: {1}, Frequency: {2})\n" +
                        " which is not in the database.  Do you want to add this collar to the database?", collar1.Ctn,
                        collar1.ArgosId,
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
                if (collar.ArgosDeployments.All(d => d.PlatformId != collar1.ArgosId) || collar.Frequency != collar1.Frequency)
                {
                    string msg = String.Format(
                        "The database record for collar (CTN: {0})\n" +
                        "does not match the information in the TPF file.({1}\n" +
                        "Database Argos ID: {2}\n" + "TPF file Argos ID: {3}\n" +
                        "Database Frequency: {4}\n" + "TPF file Frequency: {5}\n" +
                        "This collar is being skipped.", collar1.Ctn, file,
                        String.Join(", ", collar.ArgosDeployments.Select(d => d.PlatformId)),
                        collar1.ArgosId, collar.Frequency, collar1.Frequency);
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
        }

        private Collar AddTpfCollar(TpfCollar tpfCollar, ProjectInvestigator owner)
        {
            //TODO - When loading multiple files, do not cancel remainder of files if you skip a file
            //FIXME - launch add collar form with defaults
            DisposeOfPreviousVersionOfCollar(tpfCollar);
            var collar = new Collar
            {
                CollarManufacturer = "Telonics",
                CollarId = tpfCollar.Ctn,
                CollarModel = "Gen4",
                //ArgosId = tpfCollar.ArgosId,
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
            var conflictingCollar = Database.Collars.FirstOrDefault(c => c.ArgosDeployments.Any(d => d.PlatformId == tpfCollar.ArgosId) && c.DisposalDate == null);
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
            var duplicate = Database.CollarParameterFiles.FirstOrDefault(f => f.Sha1Hash == _fileHash);
            if (duplicate == null)
                return false;
            var msg = "The contents of this file have already been loaded as" + Environment.NewLine +
                String.Format("file '{0}' for {1}.", duplicate.FileName, duplicate.ProjectInvestigator.Name) + Environment.NewLine +
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
                FileNameTextBox.Text = string.Join(";", openFileDialog.FileNames);
            }
        }

        private void FileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableForm();
        }

        private void OwnerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableForm();
        }

        private void FormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var format = FormatComboBox.SelectedItem as LookupCollarParameterFileFormat;
            if (format == null)
                return;
            switch (format.Code)
            {
                    // Keep this current with changes to LookupCollarParameterFileFormats table
                case 'A':
                    Settings.SetDefaultParameterFileFormat(format.Code);
                    SetupForTpfFile();
                    EnableForm();
                    break;
                case 'B':
                    Settings.SetDefaultParameterFileFormat(format.Code);
                    SetupForPpfFile();
                    EnableForm();
                    break;
                default:
                    MessageBox.Show("Un expected parameter file format encountered", "Program Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void SetupForTpfFile()
        {
            FileNameTextBox.Text = String.Empty;
            openFileDialog.DefaultExt = "tpf";
            openFileDialog.Filter = "TPF Files|*.tpf|All Files|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;
            CreateParametersCheckBox.Enabled = true;
        }

        private void SetupForPpfFile()
        {
            FileNameTextBox.Text = String.Empty;
            openFileDialog.DefaultExt = "*.ppf";
            openFileDialog.Filter = "PPF Files|*.ppf|All Files|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;
            CreateParametersCheckBox.Enabled = false;
        }

        private void CreateParametersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            EnableForm();
        }

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this,EventArgs.Empty);
        }

    }
}
