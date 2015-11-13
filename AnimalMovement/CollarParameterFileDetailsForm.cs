using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DataModel;
using Telonics;

namespace AnimalMovement
{
    internal partial class CollarParameterFileDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private CollarParameterFile File { get; set; }
        private bool IsEditMode { get; set; }
        private bool IsEditor { get; set; }
        private bool HasParameters { get; set; }
        internal event EventHandler DatabaseChanged;

        internal CollarParameterFileDetailsForm(CollarParameterFile file)
        {
            InitializeComponent();
            RestoreWindow();
            File = file;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpGeneral();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //File is in a different DataContext, get one in this DataContext
            if (File != null)
                File = Database.CollarParameterFiles.FirstOrDefault(f => f.FileId == File.FileId);
            if (File == null)
                throw new InvalidOperationException("Collar Parameter File Details Form not provided a valid Parameter File.");

            var functions = new AnimalMovementFunctions();
            IsEditor = functions.IsInvestigatorEditor(File.Owner, CurrentUser) ?? false;
            HasParameters = File.CollarParameters.Any();
            ParametersDataGridView.DataSource = null;
        }

        #region Form Control

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            IgnoreSuffixCheckBox.Checked = Settings.GetIgnoreCtnSuffix();
            FileTabControl.SelectedIndex = Properties.Settings.Default.CollarParameterFileDetailsFormActiveTab;
            if (FileTabControl.SelectedIndex == 0)
                //if new index is zero, index changed event will not fire, so fire it manually
                FileTabControl_SelectedIndexChanged(null, null);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Properties.Settings.Default.CollarParameterFileDetailsFormActiveTab = FileTabControl.SelectedIndex;
            Settings.SetIgnoreCtnSuffix(IgnoreSuffixCheckBox.Checked);
        }

        private void FileTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (FileTabControl.SelectedIndex)
            {
                default:
                    SetUpParametersTab();
                    break;
                case 1:
                    SetUpTpfDetailsTab();
                    break;
            }
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
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }

        #endregion


        #region General

        private void SetUpGeneral()
        {
            HideTpfDetailsTab();
            FileNameTextBox.Text = File.FileName;
            FileIdTextBox.Text = File.FileId.ToString(CultureInfo.CurrentCulture);
            FormatTextBox.Text = File.LookupCollarParameterFileFormat.Name;
            UserNameTextBox.Text = File.UploadUser;
            UploadDateTextBox.Text = File.UploadDate.ToString(CultureInfo.CurrentCulture);
            OwnerComboBox.DataSource =
                Database.ProjectInvestigators.Where(
                    pi =>
                    pi.Login == CurrentUser || pi.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser));
            OwnerComboBox.DisplayMember = "Name";
            OwnerComboBox.SelectedItem = File.ProjectInvestigator;
            StatusComboBox.DataSource = Database.LookupFileStatus;
            StatusComboBox.DisplayMember = "Name";
            StatusComboBox.SelectedItem = File.LookupFileStatus;
            EnableGeneralControls();
            DoneCancelButton.Focus();
        }

        private void EnableGeneralControls()
        {
            EditSaveButton.Enabled = IsEditor;
            IsEditMode = EditSaveButton.Text == "Save";
            FileNameTextBox.Enabled = IsEditMode;
            OwnerComboBox.Enabled = IsEditMode;
            StatusComboBox.Enabled = IsEditMode && !HasParameters;
        }

        private void FileNameTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool ok = !String.IsNullOrEmpty(FileNameTextBox.Text);
            var msg = ok ? "" : "File Name cannot be empty";
            ErrorProvider.SetError(FileNameTextBox, msg);
            e.Cancel = !ok;
        }

        private void ShowContentsButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var form = new FileContentsForm(File.Contents.ToArray(), File.FileName);
            Cursor.Current = Cursors.Default;
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
                EnableGeneralControls();
            }
            else
            {
                //User is saving
                UpdateFile();
                if (SubmitChanges())
                {
                    OnDatabaseChanged();
                    EditSaveButton.Text = "Edit";
                    DoneCancelButton.Text = "Done";
                    EnableGeneralControls();
                }
            }
        }

        private void DoneCancelButton_Click(object sender, EventArgs e)
        {
            if (DoneCancelButton.Text == "Cancel")
            {
                DoneCancelButton.Text = "Done";
                EditSaveButton.Text = "Edit";
                EnableGeneralControls();
                //Reset state from database
                LoadDataContext();
                SetUpGeneral();
            }
            else
            {
                Close();
            }
        }

        private void UpdateFile()
        {
            File.FileName = FileNameTextBox.Text;
            File.ProjectInvestigator = (ProjectInvestigator)OwnerComboBox.SelectedItem;
            File.LookupFileStatus = (LookupFileStatus)StatusComboBox.SelectedItem;
        }

        #endregion


        #region Collar Parameters Tab

        private void SetUpParametersTab()
        {
            if (ParametersDataGridView.DataSource == null)
                ParametersDataGridView.DataSource =
                    Database.CollarParameters.Where(cp => cp.CollarParameterFile == File)
                            .Select(cp => new
                                {
                                    cp,
                                    cp.Collar,
                                    cp.StartDate,
                                    cp.EndDate,
                                    Data = cp.CollarFiles.Any()
                                });
            ParametersDataGridView.Columns[0].Visible = false;
            ParametersDataGridView.Columns[4].HeaderText = "Has Derived Data";
            EnableCollarFilesControls();
        }

        private void EnableCollarFilesControls()
        {
            AddParameterButton.Enabled = !IsEditMode && IsEditor;
            DeleteParameterButton.Enabled = !IsEditMode && IsEditor &&
                                            ParametersDataGridView.SelectedRows.Count > 0;
            EditParameterButton.Enabled = !IsEditMode && IsEditor && ParametersDataGridView.SelectedRows.Count == 1;
            InfoParameterButton.Enabled = !IsEditMode && ParametersDataGridView.SelectedRows.Count == 1;
        }

        private void ParametersDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpParametersTab();
        }

        private void AddParameterButton_Click(object sender, EventArgs e)
        {
            var form = new AddCollarParametersForm(null, File);
            form.DatabaseChanged += (o, x) => ParametersDataChanged();
            form.Show(this);
        }

        private void DeleteParameterButton_Click(object sender, EventArgs e)
        {
            var parameters =
                ParametersDataGridView.SelectedRows.Cast<DataGridViewRow>()
                                      .Select(row => (CollarParameter) row.Cells[0].Value)
                                      .ToList();
            bool abort = false;
            if (parameters.Any(p => p.CollarFiles.Any()))
            {
                var message = String.Format("Deleting {0} will delete derived collar files and fixes.",
                                            parameters.Count == 1 ? "this parameter" : "these parameters")
                              + Environment.NewLine + "Are you sure you want to continue?";
                abort =
                    MessageBox.Show(message, "Deleting derived data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                    DialogResult.Yes;
            }
            if (abort)
                return;
            foreach (var parameter in parameters)
            {
                foreach (var collarFile in parameter.CollarFiles)
                    Database.CollarFiles.DeleteOnSubmit(collarFile);
                Database.CollarParameters.DeleteOnSubmit(parameter);
            }
            if (SubmitChanges())
                ParametersDataChanged();
        }

        private void EditParameterButton_Click(object sender, EventArgs e)
        {
            var parameter = (CollarParameter)ParametersDataGridView.SelectedRows[0].Cells[0].Value;
            var form = new CollarParametersDetailsForm(parameter, false, true);
            form.DatabaseChanged += (o, x) => ParametersDataChanged();
            form.Show(this);
        }

        private void InfoParameterButton_Click(object sender, EventArgs e)
        {
            var parameter = (CollarParameter)ParametersDataGridView.SelectedRows[0].Cells[0].Value;
            var form = new CollarDetailsForm(parameter.Collar);
            form.DatabaseChanged += (o, x) => ParametersDataChanged();
            form.Show(this);
        }

        private void ParametersDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && InfoParameterButton.Enabled)
                InfoParameterButton_Click(sender, e);
        }

        private void ParametersDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            EnableCollarFilesControls();
        }

        #endregion


        #region Telonics Parameter File Tab

        private void HideTpfDetailsTab()
        {
            if (File.Format != 'A' && FileTabControl.TabPages.Contains(TpfDetailsTabPage))
                FileTabControl.TabPages.Remove(TpfDetailsTabPage);
        }

        private void SetUpTpfDetailsTab()
        {
            if (TpfDataGridView.DataSource != null)
                return;
            TpfDataGridView.DataSource =
                new TpfFile(File.Contents.ToArray()).GetCollars()
                                                    .Select(t => new
                                                    {
                                                        t.Owner,
                                                        CTN = t.Ctn,
                                                        t.ArgosId,
                                                        t.Frequency,
                                                        StartDate = t.TimeStamp,
                                                    }).ToList();
            EnableTpfDetailsControls();
        }

        private void IgnoreSuffixCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_collars == null)
                return;
            _collars = null;
            TpfDataGridView.DataSource = null;
            SetUpTpfDetailsTab();
            EnableTpfDetailsControls();
        }

        private void TpfDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            EnableTpfDetailsControls();
        }

        private void EnableTpfDetailsControls()
        {
            CheckButton.Visible = _collars == null;
            AddFixCollarButton.Visible = _collars != null;
            AddArgosButton.Visible = _collars != null;
            AddFixParameterButton.Visible = _collars != null;
            if (_collars == null || TpfDataGridView.SelectedRows.Count != 1)
                return;
            var index = TpfDataGridView.SelectedRows[0].Index;
            var collar = _collars[index];
            if (collar == null)
            {
                var ctn = (string) TpfDataGridView.Rows[index].Cells[1].Value;
                AddFixCollarButton.Text = "Add " +
                                       (IgnoreSuffixCheckBox.Checked && ctn.Length > 6 ? ctn.Substring(0, 6) : ctn);
                AddFixCollarButton.Enabled = true;
                AddArgosButton.Enabled = false;
                AddFixParameterButton.Enabled = false;
                return;
            }
            var argosColor = TpfDataGridView.Rows[index].Cells[2].Style.ForeColor;
            var frequencyColor = TpfDataGridView.Rows[index].Cells[3].Style.ForeColor;
            var paramColor = TpfDataGridView.Rows[index].Cells[4].Style.ForeColor;
            AddArgosButton.Enabled = argosColor == Color.Red;
            AddFixCollarButton.Enabled = frequencyColor == Color.Red;
            AddFixCollarButton.Text = frequencyColor == Color.Red ? "Fix Frequency" : AddFixCollarButton.Text;
            AddFixParameterButton.Enabled = paramColor == Color.Red || paramColor == Color.Fuchsia;
            AddFixParameterButton.Text = paramColor == Color.Fuchsia ? "Fix Parameter" : "Add Parameter";
        }

        private void CheckButton_Click(object sender, EventArgs e)
        {
            CheckTpfData();
            EnableTpfDetailsControls();
        }

        private void AddCollarButton_Click(object sender, EventArgs e)
        {
            //TODO support multi-select
            var index = TpfDataGridView.SelectedRows[0].Index;
            var collar = _collars[index];
            if (collar == null)
            {
                var ctn = (string)TpfDataGridView.Rows[index].Cells[1].Value;
                ctn = IgnoreSuffixCheckBox.Checked && ctn.Length > 6 ? ctn.Substring(0, 6) : ctn;
                var form = new AddCollarForm(File.ProjectInvestigator);
                form.DatabaseChanged += (o, x) => CollarAdded(ctn);
                form.SetDefaultFrequency((double)TpfDataGridView.SelectedRows[0].Cells[3].Value);
                form.SetDefaultModel("Telonics","Gen4");
                form.SetDefaultId(ctn);
                form.Show(this);
            }
            else
            {
                //Fix the frequency
                collar.Frequency = (double)TpfDataGridView.SelectedRows[0].Cells[3].Value;
                if (SubmitChanges())
                    TpfDataChanged();
            }
        }

        private void CollarAdded(string id)
        {
            var collar = Database.Collars.FirstOrDefault(c => c.CollarManufacturer == "Telonics" && c.CollarId == id);
            if (collar == null)
                return;
            //Add Parameter
            var start = (DateTime) TpfDataGridView.SelectedRows[0].Cells[4].Value;
            //Since this is a new collar, I don't need to worry about any parameter conflicts.
            var param = new CollarParameter
                {
                    Collar = collar,
                    CollarParameterFile = File,
                    StartDate = start
                };
            Database.CollarParameters.InsertOnSubmit(param);
            if (SubmitChanges())
                TpfDataChanged();

            //Add ArgosDeployment
            var argosId = (string) TpfDataGridView.SelectedRows[0].Cells[2].Value;
            var platform = Database.ArgosPlatforms.FirstOrDefault(a => a.PlatformId == argosId);
            if (platform == null)
            {
                var form = new AddArgosPlatformForm();
                form.DatabaseChanged += (o, x) => ArgosAdded(argosId, collar, start);
                form.SetDefaultPlatform(argosId);
                form.Show(this);
            }
            else
            {
                AddArgosDeployment(argosId, collar, start);
            }
        }

        private void ArgosAdded(string argosId, Collar collar, DateTime start)
        {
            var platform = Database.ArgosPlatforms.FirstOrDefault(a => a.PlatformId == argosId);
            if (platform == null)
                return;
            var deploy = new ArgosDeployment
                {
                    ArgosPlatform = platform,
                    Collar = collar,
                    StartDate = start
                };
            Database.ArgosDeployments.InsertOnSubmit(deploy);
            if (SubmitChanges())
                TpfDataChanged();
        }

        private void AddArgosButton_Click(object sender, EventArgs e)
        {
            var index = TpfDataGridView.SelectedRows[0].Index;
            var collar = _collars[index];
            var argosId = (string)TpfDataGridView.SelectedRows[0].Cells[2].Value;
            var start = (DateTime)TpfDataGridView.SelectedRows[0].Cells[4].Value;
            if (Database.ArgosPlatforms.Any(a => a.PlatformId == argosId))
                AddArgosDeployment(argosId, collar, start);
            else
            {
                //First add the Argosplatform
                var form = new AddArgosPlatformForm();
                form.SetDefaultPlatform(argosId);
                form.DatabaseChanged += (o, x) => AddArgosDeployment(argosId, collar, start);
                form.Show(this);
            }
        }

        private void AddArgosDeployment(string argosId, Collar collar, DateTime start)
        {
            var platform = Database.ArgosPlatforms.FirstOrDefault(a => a.PlatformId == argosId);
            if (platform == null)
                return;
            DateTime? endDate = null;
            if (platform.ArgosDeployments.Count > 0 || collar.ArgosDeployments.Count > 0)
                if (!FixOtherDeployments(collar, platform, start, ref endDate))
                    return;
            var deploy = new ArgosDeployment
            {
                ArgosPlatform = platform,
                Collar = collar,
                StartDate = start,
                EndDate = endDate
            };
            Database.ArgosDeployments.InsertOnSubmit(deploy);
            if (SubmitChanges())
                TpfDataChanged();
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

        private void AddFixParameterButton_Click(object sender, EventArgs e)
        {
            var index = TpfDataGridView.SelectedRows[0].Index;
            var start = (DateTime)TpfDataGridView.SelectedRows[0].Cells[4].Value;
            var collar = _collars[index];
            DateTime? endDate = null;
            if (AddFixParameterButton.Text == "Fix Parameter")
            {
                // If we are here, there is one (possibly though unlikely more) parameter(s) with this collar/file but not this start date.
                // If there is only one parameter on this collar (this one), then no problems, just fix the date.  Otherwise, check/fix the other one(s) first. 
                if (collar.CollarParameters.Count > 1)
                {
                    if (collar.CollarParameters.Count(p => p.CollarParameterFile == File) > 1)
                    {
                        MessageBox.Show(
                            "This file is assigned to this collar more than once.  This ambiguity prevents automatic correction.",
                            "Oh No!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    endDate = collar.CollarParameters.First(p => p.CollarParameterFile == File).EndDate;
                    if (!FixOtherParameters(collar, start, ref endDate))
                        return;
                }
                var param = collar.CollarParameters.First(p => p.CollarParameterFile == File);
                param.StartDate = start;
                param.EndDate = endDate;
            }
            else // Add
            {
                // If we are here, then this file is not on this collar, but the collar may have other files.
                // If there are no other parameters, then no problems, otherwise, try to fix the other one(s) first
                if (collar.CollarParameters.Count > 0)
                    if (!FixOtherParameters(collar, start, ref endDate))
                        return;
                var param = new CollarParameter
                {
                    Collar = collar,
                    CollarParameterFile = File,
                    StartDate = start,
                    EndDate = endDate
                };
                Database.CollarParameters.InsertOnSubmit(param);
            }
            if (SubmitChanges())
                TpfDataChanged();
        }

        private void TpfDataChanged()
        {
            OnDatabaseChanged();
            ParametersDataGridView.DataSource = null; //cause the sister grid to refresh when it becomes active
            _collars = null; //force a refresh of our collar cache (requery the database since we may have added a collar)
            CheckTpfData(); //Redraw the datagridview
            EnableTpfDetailsControls();
        }

        private bool FixOtherParameters(Collar collar, DateTime start, ref DateTime? end)
        {
            //I'm willing to move a existing null end date (there can only be one) back to my start date.
            //Any existing non-null enddate must have been explicitly set by the user, so they should be dealt with explicitly
            //If this situation exists, and I correct it, I am guaranteed to be fine (the new one will exist in the space created), so I can exit
            var parameter = collar.CollarParameters.SingleOrDefault(p => p.CollarParameterFile != File && p.StartDate < start && p.EndDate == null);
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
                end = collar.CollarParameters.Where(p => p.CollarParameterFile != File && start < p.StartDate)
                            .Select(p => p.StartDate).Min(); //If there is no min gets an empty enumerable, it will return null, which in this case is no change.

            //There is no situation where I would need to change an existing null start date.

            //now check if the new parameter is in conflict with any existing parameters
            DateTime? endDate = end;
            var competition = collar.CollarParameters.Where(p => p.CollarParameterFile != File).ToList();
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

        //Array of collars that match a CTN, Index of the enclosing array is the row number of the datagridview
        private Collar[] _collars;

        private void CheckTpfData()
        {
            if (_collars == null)
                _collars = GetCollars();
            foreach (DataGridViewRow row in TpfDataGridView.Rows)
            {
                var collar = _collars[row.Index];
                var argosId = (string)row.Cells[2].Value;
                var frequency = (double)row.Cells[3].Value;
                var paramaterStart = (DateTime)row.Cells[4].Value;
                //reset
                row.DefaultCellStyle.ForeColor = Color.Empty;
                row.Cells[2].Style.ForeColor = Color.Empty;
                row.Cells[3].Style.ForeColor = Color.Empty;
                row.Cells[4].Style.ForeColor = Color.Empty;

                if (collar == null) //No collar highlight all
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    continue;
                }
                if (MissingArgosDeployment(collar, argosId))
                    row.Cells[2].Style.ForeColor = Color.Red;
                if (FrequencyMismatch(collar, frequency))
                    row.Cells[3].Style.ForeColor = Color.Red;
                if (FileNotOnCollar(collar))
                    row.Cells[4].Style.ForeColor = Color.Red;
                else
                {
                    //File is on collar (possibly more than once)
                    if (!ParameterExistsWithDate(collar, paramaterStart))
                        //There is no parameter on this collar with this date (figure out which one to fix later)
                        row.Cells[4].Style.ForeColor = Color.Fuchsia;
                }
            }
        }

        private Collar[] GetCollars()
        {
            var collarIds = TpfDataGridView.Rows.Cast<DataGridViewRow>().Select(r => (string) r.Cells[1].Value).ToArray();
            if (IgnoreSuffixCheckBox.Checked)
                collarIds = collarIds.Select(c => c.Length > 6 ? c.Substring(0, 6) : c).ToArray();

            var unsortedCollars =
                Database.Collars.Where(
                    c =>
                    c.CollarManufacturer == "Telonics" && collarIds.Contains(c.CollarId)).ToList();

            var sortedCollars = new Collar[TpfDataGridView.Rows.Count];
            for (int i = 0; i < TpfDataGridView.Rows.Count; i++)
            {
                sortedCollars[i] = unsortedCollars.SingleOrDefault(c => c.CollarId == collarIds[i]);
            }
            return sortedCollars;
        }

        private static bool MissingArgosDeployment(Collar collar, string argosId)
        {
            return collar.ArgosDeployments.All(a => a.PlatformId != argosId);
        }

        private static bool FrequencyMismatch(Collar collar, double frequency)
        {
            return collar.Frequency != frequency;
        }

        private bool FileNotOnCollar(Collar collar)
        {
            //true if this file is not assigned to this collar
            return collar.CollarParameters.All(p => p.CollarParameterFile != File);
        }

        private bool ParameterExistsWithDate(Collar collar, DateTime paramaterStart)
        {
            //possible, though not probable, or even logical, for collar to have more than one parameter set (different date ranges) with File
            //If any of the parameters match, assume we are goodone of them matches
            return collar.CollarParameters.Any(p => p.CollarParameterFile == File && p.StartDate == paramaterStart);
        }

        #endregion

    }
}
