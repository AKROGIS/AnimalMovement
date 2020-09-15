using DataModel;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
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

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Investigator is in a different DataContext, get one in this DataContext
            if (Investigator != null)
                Investigator = Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == Investigator.Login);
        }

        #region SetUp Form

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            IgnoreSuffixCheckBox.Checked = Settings.GetIgnoreCtnSuffix();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Settings.SetIgnoreCtnSuffix(IgnoreSuffixCheckBox.Checked);
        }
        private void SetUpForm()
        {
            SetupOwnerList();
            SetupFormatList();
            SetupStatusList();
            EnableForm();
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

        #endregion

        private void EnableForm()
        {
            UploadButton.Enabled = OwnerComboBox.SelectedItem != null
                                   && !string.IsNullOrEmpty(FileNameTextBox.Text)
                                   && FormatComboBox.SelectedItem != null;
            CreateCollarsCheckBox.Enabled = CreateParametersCheckBox.Enabled && CreateParametersCheckBox.Checked;
            IgnoreSuffixCheckBox.Enabled = CreateParametersCheckBox.Enabled && CreateParametersCheckBox.Checked;
        }

        private bool SubmitChanges()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Database.SubmitChanges();
            }
            catch (SqlException ex)
            {
                string msg = "Unable to submit changes to the database.\n" +
                             "Error message:\n" + ex.Message;
                MessageBox.Show(msg, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            return true;
        }

        private void OnDatabaseChanged()
        {
            DatabaseChanged?.Invoke(this, EventArgs.Empty);
        }


        #region Form Control Events

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

        private void UploadButton_Click(object sender, EventArgs e)
        {
            UploadButton.Text = "Working...";
            UploadButton.Enabled = false;
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            try
            {
                var format = (LookupCollarParameterFileFormat)FormatComboBox.SelectedItem;
                switch (format.Code)
                {
                    // Keep this current with changes to LookupCollarParameterFileFormats table
                    case 'A':
                        if (UploadTpfFiles(FileNameTextBox.Text))
                        {
                            OnDatabaseChanged();
                            Close();
                        }
                        break;
                    case 'B':
                        if (UploadPpfFiles(FileNameTextBox.Text))
                        {
                            OnDatabaseChanged();
                            Close();
                        }
                        break;
                    default:
                        MessageBox.Show("Unexpected parameter file format encountered", "Program Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
            finally
            {
                UploadButton.Text = "Upload";
                UploadButton.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = Properties.Settings.Default.AddCollarParameterFileFolderPath;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileNameTextBox.Text = string.Join(";", openFileDialog.FileNames);
                if (openFileDialog.FileNames.Length > 0)
                {
                    var folder = Path.GetDirectoryName(openFileDialog.FileNames[0]);
                    Properties.Settings.Default.AddCollarParameterFileFolderPath = folder;
                }
            }
        }

        private void FileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableForm();
        }

        private void CreateParametersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            EnableForm();
        }

        private void OwnerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableForm();
        }

        private void FormatComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(FormatComboBox.SelectedItem is LookupCollarParameterFileFormat format))
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

        #endregion


        #region Upload Files

        private bool UploadTpfFiles(string files)
        {
            //return true if any file is successfully uploaded
            return files.Split(';').Aggregate(false, (current, file) => current || UploadTpfFile(file));
        }

        private bool UploadTpfFile(string filePath)
        {
            var owner = (ProjectInvestigator)OwnerComboBox.SelectedItem;
            var format = (LookupCollarParameterFileFormat)FormatComboBox.SelectedItem;
            CollarParameterFile paramFile = UploadParameterFile(owner, format, filePath);
            if (paramFile == null)
                return false;
            //paramfile has been successfully committed to the database.
            // CreateParameters is optional, so it should be a separate tranaction (and are not concerned with success or failure)
            if (CreateParametersCheckBox.Checked)
                CreateParameters(owner, paramFile);
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
            LoadAndHashFile(filename);
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
            if (SubmitChanges())
                return file;
            return null;
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
            _fileHash = new SHA1CryptoServiceProvider().ComputeHash(_fileContents);
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

        #endregion


        #region Create Parameters

        private void CreateParameters(ProjectInvestigator owner, CollarParameterFile paramFile)
        {
            var tpfFile = new TpfFile(_fileContents);
            foreach (TpfCollar tpfCollar in tpfFile.GetCollars())
            {
                if (IgnoreSuffixCheckBox.Checked && tpfCollar.Ctn.Length > 6)
                    tpfCollar.Ctn = tpfCollar.Ctn.Substring(0, 6);
                //Does this collar exist?
                var collar = Database.Collars.FirstOrDefault(c => c.CollarManufacturer == "Telonics" &&
                                                                  c.CollarId == tpfCollar.Ctn);
                if (collar == null && !CreateCollarsCheckBox.Checked)
                    continue;
                if (collar == null)
                {
                    collar = CreateTpfCollar(owner, tpfCollar.Ctn, tpfCollar.Frequency);
                    if (collar == null)
                        continue;
                }
                //Does this Argos platform exist?
                var fileHasArgos = !String.IsNullOrEmpty(tpfCollar.PlatformId) && tpfCollar.Platform == "Argos" &&
                                   !tpfCollar.PlatformId.Trim().Normalize().Equals("?", StringComparison.OrdinalIgnoreCase);
                if (fileHasArgos)
                {
                    var platform = Database.ArgosPlatforms.FirstOrDefault(p => p.PlatformId == tpfCollar.PlatformId && tpfCollar.Platform == "Argos");
                    if (platform == null && CreateCollarsCheckBox.Checked)
                        platform = CreatePlatform(tpfCollar.PlatformId);
                    if (platform != null)
                        //If not paired, then try to parit the collar and platform.  If it fails, or the the dates are wrong, they can correct that later
                        if (!Database.ArgosDeployments.Any(d => d.Collar == collar && d.ArgosPlatform == platform))
                            CreateDeployment(collar, platform, tpfCollar.TimeStamp);
                }
                CreateParameter(collar, paramFile, tpfCollar.TimeStamp);
            }
        }

        private Collar CreateTpfCollar(ProjectInvestigator owner, string collarId, double frequency)
        {
            var collarAdded = false;
            var form = new AddCollarForm(owner);
            form.DatabaseChanged += (o, x) => collarAdded = true;
            form.SetDefaultFrequency(frequency);
            form.SetDefaultModel("Telonics", "Gen4");
            form.SetDefaultId(collarId);
            form.ShowDialog(this); //Blocks until form closed
            if (!collarAdded)
                return null;
            return Database.Collars.FirstOrDefault(c => c.CollarManufacturer == "Telonics" &&
                                                                  c.CollarId == collarId);
        }

        private ArgosPlatform CreatePlatform(string argosId)
        {
            var platformAdded = false;
            var form = new AddArgosPlatformForm();
            form.DatabaseChanged += (o, x) => platformAdded = true;
            form.SetDefaultPlatform(argosId);
            form.ShowDialog(this);
            if (!platformAdded)
                return null;
            return Database.ArgosPlatforms.FirstOrDefault(p => p.PlatformId == argosId);
        }

        private void CreateDeployment(Collar collar, ArgosPlatform platform, DateTime startDateTime)
        {
            DateTime? endDateTime = null;
            if (platform.ArgosDeployments.Count > 0 || collar.ArgosDeployments.Count > 0)
                if (!FixOtherDeployments(collar, platform, startDateTime, ref endDateTime))
                    return;
            var deploy = new ArgosDeployment
            {
                ArgosPlatform = platform,
                Collar = collar,
                StartDate = startDateTime,
                EndDate = endDateTime
            };
            Database.ArgosDeployments.InsertOnSubmit(deploy);
            //Creating a deployment is optional, so we submit now, so a failure will not stop other transactions.
            if (!SubmitChanges())
                Database.ArgosDeployments.DeleteOnSubmit(deploy);
        }

        private void CreateParameter(Collar collar, CollarParameterFile file, DateTime startDateTime)
        {
            DateTime? endDateTime = null;
            if (collar.CollarParameters.Count > 0)
                if (!FixOtherParameters(collar, file, startDateTime, ref endDateTime))
                    return;
            var param = new CollarParameter
            {
                Collar = collar,
                CollarParameterFile = file,
                StartDate = startDateTime,
                EndDate = endDateTime
            };
            Database.CollarParameters.InsertOnSubmit(param);
            //Creating a parameter is optional, so we submit now, so a failure will not stop other transactions.
            if (!SubmitChanges())
                Database.CollarParameters.DeleteOnSubmit(param);
        }

        private bool FixOtherDeployments(Collar collar, ArgosPlatform platform, DateTime start, ref DateTime? endDate)
        {
            //I'm willing to move a existing null end dates (there can only be one for the platform and one for the collar) back to my start date.
            //Any existing non-null enddate must have been explicitly set by the user, so they should be dealt with explicitly
            //If this situation exists, and I correct it, I am guaranteed to be fine (the new one will exist in the space created)
            var deployment1 =
                collar.ArgosDeployments.SingleOrDefault(d =>
                                                        d.ArgosPlatform != platform && (d.StartDate == null || d.StartDate < start) && d.EndDate == null);
            var deployment2 =
                platform.ArgosDeployments.SingleOrDefault(d =>
                                                          d.Collar != collar && (d.StartDate == null || d.StartDate < start) && d.EndDate == null);
            if (deployment1 != null)
                deployment1.EndDate = start;
            if (deployment2 != null)
                deployment2.EndDate = start;
            if (deployment1 != null || deployment2 != null)
                if (!SubmitChanges())
                    return false;

            //If my enddate is null, I should set my end to the earliest start time of the others that are larger than my start but smaller than my end (infinity, so all).
            //I don't try to fix a non-null end date, because that was explicitly set by the user, and be should explicitly changed.
            if (endDate == null)
                endDate =
                    Database.ArgosDeployments.Where(d =>
                                                    ((d.Collar == collar && d.ArgosPlatform != platform) ||
                                                     (d.Collar != collar && d.ArgosPlatform == platform)) &&
                                                    start < d.StartDate)
                            .Select(d => d.StartDate).Min(); //If min gets an empty enumerable, it will return null, which in this case is no change.

            //Since my startdate is non-null, there is no situation where I would need to change an existing null start date.

            //now check if the new deployment is in conflict with any existing deployments
            DateTime? end = endDate;
            //Execute the query (.ToList()); otherwise LINQ will try to run the DatesOverlap predicate on the SQL Server
            var competition = Database.ArgosDeployments.Where(d => (d.Collar == collar && d.ArgosPlatform != platform) ||
                                                                (d.Collar != collar && d.ArgosPlatform == platform)).ToList();
            bool conflict = competition.Any(d => DatesOverlap(d.StartDate, d.EndDate, start, end));
            if (conflict)
            {
                MessageBox.Show(
                    "The other deployment(s) for this collar or platform will require manual adjustment before this platform can be used on this collar",
                    "Oh No!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool FixOtherParameters(Collar collar, CollarParameterFile file, DateTime start, ref DateTime? end)
        {
            //I'm willing to move a existing null end date (there can only be one) back to my start date.
            //Any existing non-null enddate must have been explicitly set by the user, so they should be dealt with explicitly
            //If this situation exists, and I correct it, I am guaranteed to be fine (the new one will exist in the space created), so I can exit
            var parameter = collar.CollarParameters.SingleOrDefault(p => p.CollarParameterFile != file && p.StartDate < start && p.EndDate == null);
            if (parameter != null)
            {
                parameter.EndDate = start;
                if (!SubmitChanges())
                    return false;
            }

            //If my enddate is null (when fixing existng, or adding new), I should set my end to the earliest start time of the others that are larger than my start but smaller than my end (infinity, so all).
            //This could only happen on a fix if there was an existing integrity violation.
            //I don't try to fix a non-null end date, because that was explicitly set by the user, and should explicitly changed.
            if (end == null)
                end = collar.CollarParameters.Where(p => p.CollarParameterFile != file && start < p.StartDate)
                            .Select(p => p.StartDate).Min(); //If there is no min gets an empty enumerable, it will return null, which in this case is no change.

            //There is no situation where I would need to change an existing null start date.

            //now check if the new parameter is in conflict with any existing parameters
            DateTime? endDate = end;
            var competition = collar.CollarParameters.Where(p => p.CollarParameterFile != file).ToList();
            bool conflict = competition.Any(p => DatesOverlap(p.StartDate, p.EndDate, start, endDate));
            if (conflict)
            {
                MessageBox.Show(
                    "The other parameter assignment(s) for this collar will require manual adjustment before this file can be used on this collar",
                    "Oh No!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private static bool DatesOverlap(DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2)
        {
            //touching is not considered overlapping.
            return (start2 ?? DateTime.MinValue) < (end1 ?? DateTime.MaxValue) && (start1 ?? DateTime.MinValue) < (end2 ?? DateTime.MaxValue);
        }

        #endregion

    }
}
