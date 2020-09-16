using DataModel;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace AnimalMovement
{
    internal partial class CollarDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private AnimalMovementViews DatabaseViews { get; set; }
        private AnimalMovementFunctions DatabaseFunctions { get; set; }
        private string CurrentUser { get; set; }
        private Collar Collar { get; set; }
        private bool IsEditor { get; set; }
        private bool IsEditMode { get; set; }
        private bool IsKeyEditMode { get; set; }
        internal event EventHandler DatabaseChanged;

        internal CollarDetailsForm(Collar collar)
        {
            InitializeComponent();
            RestoreWindow();
            Collar = collar;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpHeader();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Collar is in a different DataContext, get one in this DataContext
            if (Collar != null)
            {
                Collar = Database.Collars.FirstOrDefault(c => c.CollarManufacturer == Collar.CollarManufacturer && c.CollarId == Collar.CollarId);
            }

            if (Collar == null)
            {
                throw new InvalidOperationException("Collar Details Form not provided a valid Collar.");
            }

            DatabaseFunctions = new AnimalMovementFunctions();
            DatabaseViews = new AnimalMovementViews();
            IsEditor = DatabaseFunctions.IsInvestigatorEditor(Collar.Manager, CurrentUser) ?? false;
        }

        #region Form Controls

        private void SetUpHeader()
        {
            ManufacturerTextBox.Text = Collar.LookupCollarManufacturer.Name;
            CollarIdTextBox.Text = Collar.CollarId;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            IgnoreSuffixCheckBox.Checked = Settings.GetIgnoreCtnSuffix();
            CollarTabControl.SelectedIndex = Properties.Settings.Default.CollarDetailsFormActiveTab;
            if (CollarTabControl.SelectedIndex == 0)
            {
                //if new index is zero, index changed event will not fire, so fire it manually
                CollarTabControl_SelectedIndexChanged(null, null);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Properties.Settings.Default.CollarDetailsFormActiveTab = CollarTabControl.SelectedIndex;
            Settings.SetIgnoreCtnSuffix(IgnoreSuffixCheckBox.Checked);
        }

        private void CollarTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (CollarTabControl.SelectedIndex)
            {
                default:
                    SetupGeneralTab();
                    break;
                case 1:
                    SetupAnimalsTab();
                    break;
                case 2:
                    SetupArgosTab();
                    break;
                case 3:
                    SetupVectronicTab();
                    break;
                case 4:
                    SetupParametersTab();
                    break;
                case 5:
                    SetupFilesTab();
                    break;
                case 6:
                    SetupFixesTab();
                    break;
                case 7:
                    SetUpIssuesTab();
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
            DatabaseChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion


        #region General Tab

        private void SetupGeneralTab()
        {
            ManagerComboBox.DataSource = Database.ProjectInvestigators;
            ManagerComboBox.DisplayMember = "Name";
            ManagerComboBox.SelectedItem = Collar.ProjectInvestigator;
            ModelComboBox.DataSource = Database.LookupCollarModels.Where(m => m.CollarManufacturer == Collar.CollarManufacturer);
            ModelComboBox.DisplayMember = "CollarModel";
            ModelComboBox.SelectedItem = Collar.LookupCollarModel;
            HasGpsCheckBox.Checked = Collar.HasGps;
            OwnerTextBox.Text = Collar.Owner;
            SerialNumberTextBox.Text = Collar.SerialNumber;
            FrequencyTextBox.Text = Collar.Frequency.HasValue ? Collar.Frequency.Value.ToString(CultureInfo.InvariantCulture) : null;
            NotesTextBox.Text = Collar.Notes;
            EditSaveButton.Enabled = IsEditor;
            ConfigureDisposalDatePicker();
            EnableGeneralControls();
        }

        private void ConfigureDisposalDatePicker()
        {
            if (Collar.DisposalDate == null)
            {
                DisposalDateTimePicker.Value = DateTime.Now.Date + TimeSpan.FromHours(12);
                DisposalDateTimePicker.CustomFormat = " ";
            }
            else
            {
                DisposalDateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm";
                DisposalDateTimePicker.Value = Collar.DisposalDate.Value.ToLocalTime();
            }
            DisposalDateTimePicker.Checked = Collar.DisposalDate != null;
        }

        private void EnableGeneralControls()
        {
            EditSaveButton.Enabled = IsEditor;
            IsEditMode = EditSaveButton.Text == "Save";
            ManagerComboBox.Enabled = IsEditMode;
            ModelComboBox.Enabled = IsEditMode;
            HasGpsCheckBox.Enabled = IsEditMode;
            OwnerTextBox.Enabled = IsEditMode;
            SerialNumberTextBox.Enabled = IsEditMode;
            FrequencyTextBox.Enabled = IsEditMode;
            DisposalDateTimePicker.Enabled = IsEditMode;
            NotesTextBox.Enabled = IsEditMode;
        }

        private void ValidateFrequency(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool ok = String.IsNullOrEmpty(FrequencyTextBox.Text) ||
                      (Double.TryParse(FrequencyTextBox.Text, out double frequency) && frequency > 0);
            var msg = ok ? "" : "Frequency must be a null or a positive number";
            CollarErrorProvider.SetError(FrequencyTextBox, msg);
            e.Cancel = !ok;
        }

        private void UpdateDataSource()
        {
            Collar.ProjectInvestigator = (ProjectInvestigator)ManagerComboBox.SelectedItem;
            Collar.LookupCollarModel = (LookupCollarModel)ModelComboBox.SelectedItem;
            Collar.HasGps = HasGpsCheckBox.Checked;
            Collar.Owner = OwnerTextBox.Text;
            Collar.SerialNumber = SerialNumberTextBox.Text;
            Collar.Frequency = FrequencyTextBox.Text.DoubleOrNull();
            Collar.Notes = NotesTextBox.Text;
            Collar.DisposalDate = DisposalDateTimePicker.Checked
                                      ? (DateTime?)DisposalDateTimePicker.Value.ToUniversalTime()
                                      : null;
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
                UpdateDataSource();
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
                SetupGeneralTab();
            }
            else
            {
                Close();
            }
        }

        private void DisposalDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DisposalDateTimePicker.CustomFormat = DisposalDateTimePicker.Checked ? "yyyy-MM-dd HH:mm" : " ";
        }

        #endregion


        #region Animal Deployments Tab

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // public accessors are used by the control when these classes are accessed through the Datasource
        class DeploymentDataItem
        {
            public CollarDeployment Deployment { get; set; }
            public Animal Animal { get; set; }
            public DateTime? DeploymentDate { get; set; }
            public DateTime? RetrievalDate { get; set; }
        }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        private void SetupAnimalsTab()
        {
            DeploymentDataGridView.DataSource =
                from d in Database.CollarDeployments
                where d.Collar == Collar
                orderby d.DeploymentDate
                select new DeploymentDataItem
                {
                    Deployment = d,
                    Animal = d.Animal,
                    DeploymentDate = d.DeploymentDate.ToLocalTime(),
                    RetrievalDate = d.RetrievalDate.HasValue ? d.RetrievalDate.Value.ToLocalTime() : (DateTime?)null
                };
            EnableAnimalControls();
        }

        private void EnableAnimalControls()
        {
            AddDeploymentButton.Enabled = !IsEditMode && IsEditor;
            DeleteDeploymentButton.Enabled = !IsEditMode && IsEditor && DeploymentDataGridView.SelectedRows.Count > 0;
            EditDeploymentButton.Enabled = !IsEditMode && IsEditor && DeploymentDataGridView.SelectedRows.Count == 1;
            InfoAnimalButton.Enabled = !IsEditMode && DeploymentDataGridView.SelectedRows.Count == 1;
        }

        private void AnimalDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetupAnimalsTab();
        }

        private void AddDeploymentButton_Click(object sender, EventArgs e)
        {
            var form = new AddCollarDeploymentForm(Collar);
            form.DatabaseChanged += (o, x) => AnimalDataChanged();
            form.Show(this);
        }

        private void DeleteDeploymentButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DeploymentDataGridView.SelectedRows)
            {
                var deployment = (DeploymentDataItem)row.DataBoundItem;
                Database.CollarDeployments.DeleteOnSubmit(deployment.Deployment);
            }
            if (SubmitChanges())
            {
                AnimalDataChanged();
            }
        }

        private void EditDeploymentButton_Click(object sender, EventArgs e)
        {
            if (DeploymentDataGridView.CurrentRow == null)
            {
                return;
            }

            if (!(DeploymentDataGridView.CurrentRow.DataBoundItem is DeploymentDataItem item))
            {
                return;
            }

            var form = new CollarDeploymentDetailsForm(item.Deployment, true);
            form.DatabaseChanged += (o, x) => AnimalDataChanged();
            form.Show(this);
        }

        private void InfoAnimalButton_Click(object sender, EventArgs e)
        {
            if (DeploymentDataGridView.CurrentRow == null)
            {
                return;
            }

            if (!(DeploymentDataGridView.CurrentRow.DataBoundItem is DeploymentDataItem item))
            {
                return;
            }

            var form = new AnimalDetailsForm(item.Animal);
            form.DatabaseChanged += (o, x) => AnimalDataChanged();
            form.Show(this);
        }

        private void DeploymentDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && InfoAnimalButton.Enabled)
            {
                InfoAnimalButton_Click(sender, e);
            }
        }

        private void DeploymentDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            EnableAnimalControls();
        }

        #endregion


        #region Argos Tab

        private void SetupArgosTab()
        {
            ArgosDataGridView.DataSource =
                Collar.ArgosDeployments.Select(a => new
                {
                    a,
                    Argos_Id = a.PlatformId,
                    Start = a.StartDate == null ? "Long ago" : a.StartDate.Value.ToString("g"),
                    End = a.EndDate == null ? "Never" : a.EndDate.Value.ToString("g")
                }).ToList();
            ArgosDataGridView.Columns[0].Visible = false;
            EnableArgosControls();
        }

        private void EnableArgosControls()
        {
            AddArgosButton.Enabled = !IsEditMode && IsEditor;
            DeleteArgosButton.Enabled = !IsEditMode && IsEditor && ArgosDataGridView.SelectedRows.Count > 0;
            EditArgosButton.Enabled = !IsEditMode && ArgosDataGridView.SelectedRows.Count == 1;
            InfoArgosButton.Enabled = !IsEditMode && ArgosDataGridView.SelectedRows.Count == 1;
        }

        private void ArgosDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetupArgosTab();
        }

        private void AddArgosButton_Click(object sender, EventArgs e)
        {
            var form = new AddArgosDeploymentForm(Collar);
            form.DatabaseChanged += (o, x) => ArgosDataChanged();
            form.Show(this);
        }

        private void DeleteArgosButton_Click(object sender, EventArgs e)
        {
            var deployments =
                ArgosDataGridView.SelectedRows.Cast<DataGridViewRow>()
                                 .Select(row => (ArgosDeployment)row.Cells[0].Value)
                                 .ToList();
            if (deployments.Any(p => p.CollarFiles.Any()))
            {
                var message = String.Format("Deleting {0} will delete derived collar files and fixes.",
                                            deployments.Count == 1 ? "this deployment" : "these deployments")
                              + Environment.NewLine + "Are you sure you want to continue?";
                var response = MessageBox.Show(message, "Deleting derived data", MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Question);
                if (response != DialogResult.Yes)
                {
                    return;
                }
            }
            foreach (var deployment in deployments)
            {
                foreach (var collarFile in deployment.CollarFiles)
                {
                    Database.CollarFiles.DeleteOnSubmit(collarFile);
                }

                Database.ArgosDeployments.DeleteOnSubmit(deployment);
            }
            if (SubmitChanges())
            {
                ArgosDataChanged();
            }
        }

        private void EditArgosButton_Click(object sender, EventArgs e)
        {
            var deployment = (ArgosDeployment)ArgosDataGridView.SelectedRows[0].Cells[0].Value;
            var form = new ArgosDeploymentDetailsForm(deployment.DeploymentId, true);
            form.DatabaseChanged += (o, x) => ArgosDataChanged();
            form.Show(this);
        }

        private void InfoArgosButton_Click(object sender, EventArgs e)
        {
            var deployment = (ArgosDeployment)ArgosDataGridView.SelectedRows[0].Cells[0].Value;
            var form = new ArgosPlatformDetailsForm(deployment.ArgosPlatform);
            form.DatabaseChanged += (o, x) => ArgosDataChanged();
            form.Show(this);
        }

        private void ArgosDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && InfoArgosButton.Enabled)
            {
                InfoArgosButton_Click(sender, e);
            }
        }

        private void ArgosDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            EnableArgosControls();
        }

        #endregion


        #region Parameters Tab

        private void SetupParametersTab()
        {
            SetUpTpfTab();
            switch (Collar.CollarModel)
            {
                case "Gen3":
                    ParametersDataGridView.DataSource =
                        Collar.CollarParameters.Select(
                            p =>
                            new
                            {
                                Parameter = p,
                                Period = p.Gen3Period == null
                                                 ? null
                                                 : (p.Gen3Period % 60 == 0
                                                        ? (p.Gen3Period / 60) + " hrs"
                                                        : p.Gen3Period + " min"),
                                File = p.CollarParameterFile?.FileName,
                                Start = p.StartDate == null ? "Long ago" : p.StartDate.Value.ToLocalTime().ToString("g"),
                                End = p.EndDate == null ? "Never" : p.EndDate.Value.ToLocalTime().ToString("g"),
                                Data = p.CollarFiles.Any()
                            }).ToList();
                    ParametersDataGridView.Columns[5].HeaderText = "Has Derived Data";
                    break;
                case "Gen4":
                    ParametersDataGridView.DataSource =
                        Collar.CollarParameters.Select(p => new
                        {
                            Parameter = p,
                            File = p.CollarParameterFile?.FileName,
                            Start = p.StartDate == null ? "Long ago" : p.StartDate.Value.ToLocalTime().ToString("g"),
                            End = p.EndDate == null ? "Never" : p.EndDate.Value.ToLocalTime().ToString("g"),
                            Data = p.CollarFiles.Any()
                        })
                              .ToList();
                    ParametersDataGridView.Columns[4].HeaderText = "Has Derived Data";
                    break;
                default:
                    break;
            }
            ParametersDataGridView.Columns[0].Visible = false;
            EnableParametersControls();
        }

        private void EnableParametersControls()
        {
            AddParameterButton.Enabled = !IsEditMode && IsEditor;
            DeleteParameterButton.Enabled = !IsEditMode && IsEditor && ParametersDataGridView.SelectedRows.Count > 0;
            EditParameterButton.Enabled = !IsEditMode && IsEditor && ParametersDataGridView.SelectedRows.Count == 1;
            InfoParameterButton.Enabled = !IsEditMode && ParametersDataGridView.SelectedRows.Count == 1 && //has a file
                                          ((CollarParameter)ParametersDataGridView.SelectedRows[0].Cells[0].Value)
                                              .CollarParameterFile != null;
        }

        private void ParametersDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetupParametersTab();
        }

        private void IgnoreSuffixCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            TpfDataGridView.DataSource = null;
            SetUpTpfTab();
        }

        private void AddParameterButton_Click(object sender, EventArgs e)
        {
            var form = new AddCollarParametersForm(Collar);
            form.DatabaseChanged += (o, x) => ParametersDataChanged();
            form.Show(this);
        }

        private void DeleteParameterButton_Click(object sender, EventArgs e)
        {
            var parameters =
                ParametersDataGridView.SelectedRows.Cast<DataGridViewRow>()
                                      .Select(row => (CollarParameter)row.Cells[0].Value)
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
            {
                return;
            }

            foreach (var parameter in parameters)
            {
                foreach (var collarFile in parameter.CollarFiles)
                {
                    Database.CollarFiles.DeleteOnSubmit(collarFile);
                }

                Database.CollarParameters.DeleteOnSubmit(parameter);
            }
            if (SubmitChanges())
            {
                ParametersDataChanged();
            }
        }

        private void EditParameterButton_Click(object sender, EventArgs e)
        {
            var parameter = (CollarParameter)ParametersDataGridView.SelectedRows[0].Cells[0].Value;
            var form = new CollarParametersDetailsForm(parameter, true);
            form.DatabaseChanged += (o, x) => ParametersDataChanged();
            form.Show(this);
        }

        private void InfoParameterButton_Click(object sender, EventArgs e)
        {
            var parameter = (CollarParameter)ParametersDataGridView.SelectedRows[0].Cells[0].Value;
            if (parameter.CollarParameterFile == null)
            {
                return;
            }

            var form = new CollarParameterFileDetailsForm(parameter.CollarParameterFile);
            form.DatabaseChanged += (o, x) => ParametersDataChanged();
            form.Show(this);
        }

        private void ParametersDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && InfoParameterButton.Enabled)
            {
                InfoParameterButton_Click(sender, e);
            }
        }

        private void ParametersDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            EnableParametersControls();
        }

        #region Tpf Data Grid

        private void SetUpTpfTab()
        {
            if (TpfDataGridView.DataSource != null)
            {
                return;
            }

            if (Collar.CollarManufacturer != "Telonics" || Collar.CollarModel != "Gen4")
            {
                return;
            }

            var views = new AnimalMovementViews();
            var ignoreSuffix = IgnoreSuffixCheckBox.Checked;
            var tpfList = views.AllTpfFileDatas.ToList();
            tpfList = ignoreSuffix
                          ? tpfList.Where(t => t.CTN.Length >= 6 && t.CTN.Substring(0, 6) == Collar.CollarId).ToList()
                          : tpfList.Where(t => t.CTN == Collar.CollarId).ToList();
            TpfDataGridView.DataSource = tpfList.Select(t => new
            {
                t.FileId,
                t.FileName,
                t.Status,
                t.CTN,
                t.Frequency,
                StartDate = t.TimeStamp,
            }).ToList();
            TpfDataGridView.Columns[5].HeaderText = "Start Date (UTC)";
        }

        private void ShowTpfFileDetails()
        {
            var fileId = (int)TpfDataGridView.SelectedRows[0].Cells[0].Value;
            var file = Database.CollarParameterFiles.FirstOrDefault(f => f.FileId == fileId);
            if (file == null)
            {
                return;
            }

            var form = new CollarParameterFileDetailsForm(file);
            form.DatabaseChanged += (o, x) => ParametersDataChanged();
            form.Show(this);
        }

        private void TpfDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && !IsEditMode)
            {
                ShowTpfFileDetails();
            }
        }

        #endregion

        #endregion


        #region Vectronic Tab

        private void SetupVectronicTab()
        {
            //TODO: Get the List of sensors and the Vectronic Key
            //VectronicSensorDataGridView.DataSource = VectronicSensors
            VectronicSensorDataGridView.Columns[0].Visible = false;
            //VectronicKeyTextBox.Text = Vectonic.Key;
            EnableVectronicControls();
        }

        private void EnableVectronicControls()
        {
            AddVectronicSensorButton.Enabled = !IsEditMode && IsEditor;
            DeleteVectronicSensorButton.Enabled = !IsEditMode && IsEditor && VectronicSensorDataGridView.SelectedRows.Count > 0;
            EditVectronicSensorButton.Enabled = !IsEditMode && VectronicSensorDataGridView.SelectedRows.Count == 1;
            VectronicKeyEditSaveButton.Enabled = IsEditor;
            IsKeyEditMode = EditSaveButton.Text == "Save";
            VectronicKeyTextBox.Enabled = IsKeyEditMode;
            VectronicKeyEditCancelButton.Visible = IsKeyEditMode;
        }

        private void VectronicDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetupVectronicTab();
        }

        private void AddVectronicSensorButton_Click(object sender, EventArgs e)
        {
            //TODO: Create Add Form
            //var form = new AddVectronicSensorForm(Collar);
            var form = new AddArgosDeploymentForm(Collar);
            form.DatabaseChanged += (o, x) => VectronicDataChanged();
            form.Show(this);
        }

        private void DeleteVectronicSensorButton_Click(object sender, EventArgs e)
        {

        }

        private void EditVectronicSensorButton_Click(object sender, EventArgs e)
        {
            //TODO: Get Sensor Record Type and VectronicSensor Edit Form
            //var sensor = (VectronicSensor)VectronicSensorDataGridView.SelectedRows[0].Cells[0].Value;
            //var form = new VectronicSensorDetailsForm(sensor.SensorCode, true);
            //form.DatabaseChanged += (o, x) => VectronicDataChanged();
            //form.Show(this);
        }

        private void VectronicKeyEditSaveButton_Click(object sender, EventArgs e)
        {
            //This button is not enabled unless editing is permitted 
            if (VectronicKeyEditSaveButton.Text == "Edit")
            {
                // The user wants to edit, Enable form
                VectronicKeyEditSaveButton.Text = "Save";
                EnableVectronicControls();
            }
            else
            {
                //User is saving
                UpdateDataSource();
                if (SubmitChanges())
                {
                    OnDatabaseChanged();
                    EditSaveButton.Text = "Edit";
                    EnableVectronicControls();
                }
            }
        }

        private void VectronicKeyEditCancelButton_Click(object sender, EventArgs e)
        {
            VectronicKeyEditSaveButton.Text = "Edit";
            EnableVectronicControls();
            //Reset state from database
            LoadDataContext();
            SetupVectronicTab();
        }

        private void VectronicSensorDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && EditVectronicSensorButton.Enabled)
            {
                EditVectronicSensorButton_Click(sender, e);
            }
        }

        private void VectronicSensorDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            EnableVectronicControls();
        }

        #endregion


        #region Files Tab

        private void SetupFilesTab()
        {
            if (Collar == null)
            {
                return;
            }

            FilesDataGridView.DataSource = DatabaseViews.CollarFixesByFile(Collar.CollarManufacturer, Collar.CollarId);
            EnableFileControls();
        }

        private void EnableFileControls()
        {
            FileInfoButton.Enabled = FilesDataGridView.CurrentRow != null && !IsEditMode && FilesDataGridView.SelectedRows.Count == 1;
            ChangeFileStatusButton.Enabled = FilesDataGridView.CurrentRow != null && !IsEditMode && IsEditor;
            if (FilesDataGridView.SelectedRows.Count > 0)
            {
                var firstRowStatus = (string)FilesDataGridView.SelectedRows[0].Cells["Status"].Value;
                ChangeFileStatusButton.Text = firstRowStatus != "Active" ? "Activate" : "Deactivate";
                if (FilesDataGridView.SelectedRows.Cast<DataGridViewRow>()
                                     .Any(row => (string)row.Cells["Status"].Value != firstRowStatus))
                {
                    ChangeFileStatusButton.Enabled = false;
                }
            }
        }

        private void FileDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetupFilesTab();
        }

        private void ChangeFileStatusButton_Click(object sender, EventArgs e)
        {
            //If the button is labeled/enabled correctly, then the user wants to change the status of all the selected files
            var newStatus = Database.LookupFileStatus.First(s => s.Code == 'A');
            if (ChangeFileStatusButton.Text == "Deactivate")
            {
                newStatus = Database.LookupFileStatus.First(s => s.Code == 'I');
            }

            var fileIds = FilesDataGridView.SelectedRows.Cast<DataGridViewRow>()
                                           .Select(row => (int)row.Cells["FileId"].Value).ToList();
            var files = Database.CollarFiles.Where(f => fileIds.Contains(f.FileId));
            foreach (var collarFile in files)
            {
                collarFile.LookupFileStatus = newStatus;
            }

            if (SubmitChanges())
            {
                FileDataChanged();
            }
        }

        private void FileInfoButton_Click(object sender, EventArgs e)
        {
            if (FilesDataGridView.CurrentRow == null)
            {
                return;
            }

            if (!(FilesDataGridView.CurrentRow.DataBoundItem is CollarFixesByFileResult item))
            {
                return;
            }

            var file = Database.CollarFiles.FirstOrDefault(f => f.FileId == item.FileId);
            if (file == null)
            {
                return;
            }

            var form = new FileDetailsForm(file);
            form.DatabaseChanged += (o, x) => FileDataChanged();
            form.Show(this);
        }

        private void FilesDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && FileInfoButton.Enabled)
            {
                FileInfoButton_Click(sender, e);
            }
        }

        private void FilesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            EnableFileControls();
        }

        #endregion


        #region Fixes Tab

        private void SetupFixesTab()
        {
            if (Collar == null)
            {
                return;
            }

            FixConflictsDataGridView.DataSource = DatabaseViews.ConflictingFixes(Collar.CollarManufacturer, Collar.CollarId, 36500); //last 100 years
            var summary = DatabaseViews.CollarFixSummary(Collar.CollarManufacturer, Collar.CollarId).FirstOrDefault();
            SummaryLabel.Text = summary == null
                              ? "There are NO fixes."
                              : (summary.Count == summary.Unique
                                 ? String.Format("{0} fixes between {1:d} and {2:d}.", summary.Count, summary.First, summary.Last)
                                 : String.Format("{3}/{0} unique/total fixes between {1:d} and {2:d}.", summary.Count, summary.First, summary.Last, summary.Unique));
            var query = from fix in Database.CollarFixes
                        where fix.Collar == Collar
                        select new { fix.FixDate, fix.Lat, fix.Lon, fix.CollarFile.FileName, fix.LineNumber };
            FixesGridView.DataSource = query;
            FixConflictsDataGridView_SelectionChanged(null, null);
        }

        private void EnableFixControls()
        {
            UnhideFixButton.Enabled = IsUnhideFixButtonEnabled();
        }

        private bool IsUnhideFixButtonEnabled()
        {
            if (FixConflictsDataGridView.CurrentRow == null)
            {
                return false;
            }

            if (!(FixConflictsDataGridView.CurrentRow.DataBoundItem is ConflictingFixesResult selectedFix))
            {
                return false;
            }

            bool isEditor = DatabaseFunctions.IsFixEditor(selectedFix.FixId, CurrentUser) ?? false;
            bool isHidden = selectedFix.HiddenBy != null;
            return isEditor && isHidden;
        }

        private void FixDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetupFixesTab();
        }

        private void UnhideFixButton_Click(object sender, EventArgs e)
        {
            if (FixConflictsDataGridView.CurrentRow == null)
            {
                return;
            }

            if (!(FixConflictsDataGridView.CurrentRow.DataBoundItem is ConflictingFixesResult selectedFix))
            {
                return;
            }

            if (ExecuteUnhideFix(selectedFix.FixId))
            {
                FixDataChanged();
            }
        }

        private bool ExecuteUnhideFix(int fixId)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Database.CollarFixes_UpdateUnhideFix(fixId);
            }
            catch (SqlException ex)
            {
                string msg = "Unable to update the fix.\n" +
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

        private void FixConflictsDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            EnableFixControls();
        }

        #endregion


        #region Issues Tab

        private void SetUpIssuesTab()
        {
            ProcessingIssuesDataGridView.DataSource = Collar.ArgosFileProcessingIssues.Select(i => new
            {
                i.CollarFile,
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
            if (collar == null)
            {
                return;
            }

            var form = new CollarDetailsForm(collar);
            form.DatabaseChanged += (o, x) => IssueDataChanged();
            form.Show(this);
        }

        private void ProcessingIssuesDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && !IsEditMode)
            {
                if (e.ColumnIndex == 1)
                {
                    IssueCollarDetails();
                }
                else
                {
                    IssueFileDetails();
                }
            }
        }

        #endregion

    }
}
