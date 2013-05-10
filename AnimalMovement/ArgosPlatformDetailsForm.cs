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
                    SetUpIssuesTab();
                    break;
                case 5:
                    SetUpTransmissionsTab();
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
                CancelButton.Visible = true;
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
                    CancelButton.Visible = false;
                    EnableGeneralControls();
                }
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            CancelButton.Visible = false;
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
            CollarsDataGridView.DataSource = Platform.ArgosDeployments;
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
                                                        t.CTN,
                                                        t.Frequency,
                                                        StartDate = t.TimeStamp,
                                                        t.FileName,
                                                        t.FileId,
                                                        t.Status,
                                                    }).ToList();
        }

        #endregion


        #region Downloads Tab

        private void SetUpDownloadsTab()
        {
            DownloadsDataGridView.DataSource = Database.ArgosDownloads.Where(d => d.PlatformId == Platform.PlatformId);
        }

        private void ProgramDownloadsButton_Click(object sender, EventArgs e)
        {
            var form = new ArgosProgramDetailsForm(Platform.ArgosProgram);
            form.DatabaseChanged += (o, x) =>
                {
                    OnDatabaseChanged();
                    LoadDataContext();
                    SetUpDownloadsTab();
                };
            //TODO - create the download tab on the Argos form, and select it.
            form.Show();
        }

        #endregion


        #region Issues Tab

        private void SetUpIssuesTab()
        {
            ProcessingIssuesDataGridView.DataSource = Platform.ArgosFileProcessingIssues;
        }

        #endregion


        #region Transmissions Tab

        private void SetUpTransmissionsTab()
        {
            TransmissionsDataGridView.DataSource =
                Database.ArgosFilePlatformDates.Where(d => d.PlatformId == Platform.PlatformId);
        }

        #endregion


        #region Derived Data Tab

        private void SetUpDerivedDataTab()
        {
            DerivedDataGridView.DataSource = Database.CollarFiles.Where(f => f.ArgosDeployment.ArgosPlatform == Platform);
        }

        #endregion


    }
}
