using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class ArgosPlatformDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private ArgosPlatform Platform { get; set; }
        private bool IsEditor { get; set; }
        private bool IsEditMode { get; set; }
        internal event EventHandler DatabaseChanged;

        public ArgosPlatformDetailsForm(ArgosPlatform platform)
        {
            InitializeComponent();
            RestoreWindow();
            Platform = platform;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpForm();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Platform is in a different DataContext, get one in this DataContext
            if (Platform != null)
                Platform = Database.ArgosPlatforms.FirstOrDefault(p => p.PlatformId == Platform.PlatformId);
            if (Platform == null)
                throw new InvalidOperationException("Argos Platform Details Form not provided a valid Platform.");

            var functions = new AnimalMovementFunctions();
            IsEditor = functions.IsInvestigatorEditor(Platform.ArgosProgram.Manager, CurrentUser) ?? false;
        }

        #region Form Controls

        private void SetUpForm()
        {
            ArgosIdTextBox.Text = Platform.PlatformId;
            ArgosIdTextBox.Enabled = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ArgosTabControl.SelectedIndex = Properties.Settings.Default.ArgosDetailsFormActiveTab;
            if (ArgosTabControl.SelectedIndex == 0)
                ArgosTabControl_SelectedIndexChanged(ArgosTabControl, null);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Properties.Settings.Default.ArgosDetailsFormActiveTab = ArgosTabControl.SelectedIndex;
        }

        private void ArgosTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (((TabControl)sender).SelectedIndex)
            {
                default:
                    SetUpGeneralTab();
                    break;
                case 1:
                    SetUpCollarsTab();
                    break;
                case 2:
                    SetUpParameterFilesTab();
                    break;
                case 3:
                    SetUpDownloadsTab();
                    break;
                case 4:
                    SetUpTransmissionsTab();
                    break;
                case 5:
                    SetUpIssuesTab();
                    break;
                case 6:
                    SetUpDerivedDataTab();
                    break;
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion


        #region General Tab

        private void SetUpGeneralTab()
        {
            SetUpArgosProgramComboBox();
            ConfigureDispsoalDateTimePicker();
            ActiveCheckBox.Checked = Platform.Active;
            NotesTextBox.Text = Platform.Notes;
            EnableGeneralControls();
        }

        private void SetUpArgosProgramComboBox()
        {
            //If I am not an editor, then set the current program (we will lock it later).
            //else, set list to all projects I can edit (which must include this one)
            if (!IsEditor)
                ArgosProgramComboBox.Items.Add(Platform.ArgosProgram);
            else
            {
                ArgosProgramComboBox.DataSource =
                    Database.ArgosPrograms.Where(p => p.Manager == CurrentUser ||
                     p.ProjectInvestigator.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser));
            }
            ArgosProgramComboBox.SelectedItem = Platform.ArgosProgram;
        }

        private void EnableGeneralControls()
        {
            EnableEditSaveButton();
            IsEditMode = EditSaveButton.Text == "Save";
            ArgosProgramComboBox.Enabled = IsEditMode;
            DisposalDateTimePicker.Enabled = IsEditMode;
            ActiveCheckBox.Enabled = IsEditMode && Platform.ArgosProgram.Active == null;
            NotesTextBox.Enabled = IsEditMode;
        }

        private void EnableEditSaveButton()
        {
            EditSaveButton.Enabled = IsEditor && (!IsEditMode ||
                                                  (ArgosProgramComboBox.SelectedItem != null &&
                                                   !string.IsNullOrEmpty(ArgosIdTextBox.Text)));
        }

        private void ConfigureDispsoalDateTimePicker()
        {
            if (Platform.DisposalDate.HasValue)
            {
                DisposalDateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm";
                DisposalDateTimePicker.Value = Platform.DisposalDate.Value.ToLocalTime();
            }
            else
            {
                DisposalDateTimePicker.Value = DateTime.Now.Date + TimeSpan.FromHours(12);
                DisposalDateTimePicker.CustomFormat = " ";
            }
            DisposalDateTimePicker.Checked = Platform.DisposalDate.HasValue;
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

        private void UpdateDataSource()
        {
            Platform.ArgosProgram = (ArgosProgram) ArgosProgramComboBox.SelectedItem;
            Platform.DisposalDate = DisposalDateTimePicker.Checked
                                        ? (DateTime?) DisposalDateTimePicker.Value.ToUniversalTime()
                                        : null;
            Platform.Active = ActiveCheckBox.Checked;
            Platform.Notes = NotesTextBox.Text.NullifyIfEmpty();
        }

        private void EditSaveButton_Click(object sender, EventArgs e)
        {
            //This button is not enabled unless editing is permitted 
            if (EditSaveButton.Text == "Edit")
            {
                // The user wants to edit, Enable form
                EditSaveButton.Text = "Save";
                cancelButton.Visible = true;
                EnableGeneralControls();
            }
            else
            {
                //User is saving
                UpdateDataSource();
                if (SubmitChanges())
                {
                    OnDatabaseChanged();
                    EditSaveButton.Text = "Edit";
                    cancelButton.Visible = false;
                    EnableGeneralControls();
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            cancelButton.Visible = false;
            EditSaveButton.Text = "Edit";
            //Reset state from database
            LoadDataContext();
            SetUpGeneralTab();
        }

        private void DisposalDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DisposalDateTimePicker.CustomFormat = DisposalDateTimePicker.Checked ? "yyyy-MM-dd HH:mm" : " ";
        }

        #endregion


        #region Collars Tab

        private void SetUpCollarsTab()
        {
            CollarsDataGridView.DataSource = Platform.ArgosDeployments.Select(d => new
            {
                d,
                d.Collar,
                Start = d.StartDate == null ? "Long ago" : d.StartDate.Value.ToString("g"),
                End = d.EndDate == null ? "Never" : d.EndDate.Value.ToString("g")
            }).ToList();
            CollarsDataGridView.Columns[0].Visible = false;
            EnableCollarControls();
        }

        private void EnableCollarControls()
        {
            AddCollarButton.Enabled = !IsEditMode && IsEditor;
            DeleteCollarButton.Enabled = !IsEditMode && IsEditor && CollarsDataGridView.SelectedRows.Count > 0;
            EditCollarButton.Enabled = !IsEditMode && CollarsDataGridView.SelectedRows.Count == 1;
            InfoCollarButton.Enabled = !IsEditMode && CollarsDataGridView.SelectedRows.Count == 1;
        }

        private void CollarDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpCollarsTab();
        }

        private void AddCollarButton_Click(object sender, EventArgs e)
        {
            var form = new AddArgosDeploymentForm(null,Platform);
            form.DatabaseChanged += (o, x) => CollarDataChanged();
            form.Show(this);
        }

        private void DeleteCollarButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in CollarsDataGridView.SelectedRows)
            {
                var deployment = (CollarDeployment)row.Cells[0].Value;
                Database.CollarDeployments.DeleteOnSubmit(deployment);
            }
            if (SubmitChanges())
                CollarDataChanged();
        }

        private void EditCollarButton_Click(object sender, EventArgs e)
        {
            var deployment = (ArgosDeployment)CollarsDataGridView.SelectedRows[0].Cells[0].Value;
            var form = new ArgosDeploymentDetailsForm(deployment.DeploymentId, true);
            form.DatabaseChanged += (o, x) => CollarDataChanged();
            form.Show(this);
        }

        private void InfoCollarButton_Click(object sender, EventArgs e)
        {
            var deployment = (ArgosDeployment)CollarsDataGridView.SelectedRows[0].Cells[0].Value;
            var form = new CollarDetailsForm(deployment.Collar);
            form.DatabaseChanged += (o, x) => CollarDataChanged();
            form.Show(this);
        }

        private void CollarDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
                InfoCollarButton_Click(sender, e);
        }

        private void CollarDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            EnableCollarControls();
        }

        #endregion


        #region Parameter Files Tab

        private void SetUpParameterFilesTab()
        {
            if (ParametersDataGridView.DataSource != null)
                return;
            var views = new AnimalMovementViews();

            ParametersDataGridView.DataSource = views.AllTpfFileDatas.Where(t => t.Platform == Platform.PlatformId).Select(t => new
                                                    {
                                                        t.FileId,
                                                        t.FileName,
                                                        t.Status,
                                                        t.CTN,
                                                        t.Frequency,
                                                        StartDate = t.TimeStamp,
                                                    }).ToList();
        }

        #endregion


        #region Downloads Tab

        private void SetUpDownloadsTab()
        {
            DownloadsDataGridView.DataSource = Platform.ArgosDownloads.Select(d => new
                {
                    d.TimeStamp,
                    d.Days,
                    d.CollarFile,
                    d.ErrorMessage
                }).ToList();
        }

        private void DownloadsChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpDownloadsTab();
        }

        private void DownloadsDetails()
        {
            var file = (CollarFile)DownloadsDataGridView.SelectedRows[0].Cells[0].Value;
            if (file == null)
                return;
            var form = new FileDetailsForm(file);
            form.DatabaseChanged += (o, x) => DownloadsChanged();
            form.Show(this);
        }

        private void DownloadsDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
                DownloadsDetails();
        }

        private void ProgramDownloadsButton_Click(object sender, EventArgs e)
        {
            var form = new ArgosProgramDetailsForm(Platform.ArgosProgram);
            form.DatabaseChanged += (o, x) => DownloadsChanged();
            //TODO - create the download tab on the Argos form, and select it.
            form.Show();
        }

        #endregion


        #region Issues Tab

        private void SetUpIssuesTab()
        {
            //TODO - creating anonymous (or nested type) is empty unless ToList() is used,  lists do not support sorting.
            //ProcessingIssuesDataGridView.DataSource = Platform.ArgosFileProcessingIssues; //works but too many fields in wrong order
            ProcessingIssuesDataGridView.DataSource = Platform.ArgosFileProcessingIssues.Select(i => new
                {
                    i.CollarFile,
                    i.Collar,
                    i.Issue
                }).ToList();
        }

        private void IssueDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpIssuesTab();
        }

        private void IssueFileDetails()
        {
            var file = (CollarFile)ProcessingIssuesDataGridView.SelectedRows[0].Cells[0].Value;
            var form = new FileDetailsForm(file);
            form.DatabaseChanged += (o, x) => IssueDataChanged();
            form.Show(this);
        }

        private void IssueCollarDetails()
        {
            var collar = (Collar)ProcessingIssuesDataGridView.SelectedRows[0].Cells[1].Value;
            var form = new CollarDetailsForm(collar);
            form.DatabaseChanged += (o, x) => IssueDataChanged();
            form.Show(this);
        }

        private void ProcessingIssuesDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
                if (e.ColumnIndex == 1)
                    IssueCollarDetails();
                else
                    IssueFileDetails();
        }


        #endregion


        #region Transmissions Tab

        private void SetUpTransmissionsTab()
        {
            TransmissionsDataGridView.DataSource = Platform.ArgosFilePlatformDates.Select(t => new
                {
                    t.CollarFile,
                    t.FirstTransmission,
                    t.LastTransmission
                }).ToList();
            TransmissionsDataGridView.Columns[0].HeaderText = "Collar File";
            TransmissionsDataGridView.Columns[1].HeaderText = "First Transmission";
            TransmissionsDataGridView.Columns[2].HeaderText = "Last Transmission";
        }

        private void TransmissionDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpTransmissionsTab();
        }

        private void TransmissionDetails()
        {
            var file = (CollarFile)TransmissionsDataGridView.SelectedRows[0].Cells[0].Value;
            var form = new FileDetailsForm(file);
            form.DatabaseChanged += (o, x) => TransmissionDataChanged();
            form.Show(this);
        }

        private void TransmissionsDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
                TransmissionDetails();
        }

        #endregion


        #region Derived Data Tab

        private void SetUpDerivedDataTab()
        {
            DerivedDataGridView.DataSource = Platform.ArgosDeployments.SelectMany(d => d.CollarFiles).Select(f => new
                {
                    f,
                    f.LookupCollarFileFormat.Name,
                    f.Status,
                    f.Collar,
                    Parent = f.ParentFile
                }).ToList();
            DerivedDataGridView.Columns[0].HeaderText = "Derived File";
            DerivedDataGridView.Columns[1].HeaderText = "Format";
            DerivedDataGridView.Columns[2].HeaderText = "Status";
            DerivedDataGridView.Columns[4].HeaderText = "Source File";
        }

        private void DerivedDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpDerivedDataTab();
        }

        private void DerivedDataDetails()
        {
            var file = (CollarFile)DerivedDataGridView.SelectedRows[0].Cells[0].Value;
            var form = new FileDetailsForm(file);
            form.DatabaseChanged += (o, x) => DerivedDataChanged();
            form.Show(this);
        }

        private void DerivedDataGridView_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
                DerivedDataDetails();
        }

        #endregion


    }
}
