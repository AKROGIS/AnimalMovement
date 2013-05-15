using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DataModel;
using Telonics;

namespace AnimalMovement
{
    internal partial class ArgosProgramDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private ArgosProgram Program { get; set; }
        private bool IsEditor { get; set; }
        private bool IsEditMode { get; set; }
        internal event EventHandler DatabaseChanged;

        public ArgosProgramDetailsForm(ArgosProgram program)
        {
            InitializeComponent();
            RestoreWindow();
            Program = program;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpForm();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Platform is in a different DataContext, get one in this DataContext
            if (Program != null)
                Program = Database.ArgosPrograms.FirstOrDefault(p => p.ProgramId == Program.ProgramId);
            if (Program == null)
                throw new InvalidOperationException("Argos Platform Details Form not provided a valid Platform.");

            var functions = new AnimalMovementFunctions();
            IsEditor = functions.IsInvestigatorEditor(Program.Manager, CurrentUser) ?? false;
        }


        #region Form Controls

        private void SetUpForm()
        {
            ProgramIdTextBox.Text = Program.ProgramId;
            ProgramIdTextBox.Enabled = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ArgosProgramTabControl.SelectedIndex = Properties.Settings.Default.ArgosProgramDetailsFormActiveTab;
            if (ArgosProgramTabControl.SelectedIndex == 0)
                ArgosTabControl_SelectedIndexChanged(ArgosProgramTabControl, null);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Properties.Settings.Default.ArgosProgramDetailsFormActiveTab = ArgosProgramTabControl.SelectedIndex;
        }

        private void ArgosTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (((TabControl)sender).SelectedIndex)
            {
                default:
                    SetUpGeneralTab();
                    break;
                case 1:
                    SetUpPlatformsTab();
                    break;
                case 2:
                    SetUpDownloadsTab();
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
            SetUpOwnerComboBox();
            ProgramNameTextBox.Text = Program.ProgramName;
            UserNameTextBox.Text = Program.UserName;
            PasswordMaskedTextBox.Text = Program.Password;
            PasswordMaskedTextBox.UseSystemPasswordChar = !IsEditor;
            ConfigureDateTimePicker(StartDateTimePicker, Program.StartDate);
            ConfigureDateTimePicker(EndDateTimePicker, Program.EndDate);
            if (Program.Active.HasValue)
                ActiveCheckBox.Checked = Program.Active.Value;
            else
                ActiveCheckBox.CheckState = CheckState.Indeterminate;
            NotesTextBox.Text = Program.Notes;
            EnableGeneralControls();
        }

        private void SetUpOwnerComboBox()
        {
            //If I am not an editor, then set the current program (we will lock it later).
            //else, set list to all projects I can edit (which must include this one)
            if (!IsEditor)
                OwnerComboBox.Items.Add(Program.ProjectInvestigator);
            else
            {
                OwnerComboBox.DataSource =
                    Database.ProjectInvestigators.Where(pi => pi.Login == CurrentUser ||
                                                              pi.ProjectInvestigatorAssistants.Any(
                                                                  a => a.Assistant == CurrentUser));
            }
            OwnerComboBox.SelectedItem = Program.ProjectInvestigator;
            OwnerComboBox.DisplayMember = "Name";
        }

        private void EnableGeneralControls()
        {
            EnableEditSaveButton();
            IsEditMode = EditSaveButton.Text == "Save";
            OwnerComboBox.Enabled = IsEditMode;
            ProgramNameTextBox.Enabled = IsEditMode;
            UserNameTextBox.Enabled = IsEditMode;
            PasswordMaskedTextBox.Enabled = IsEditMode;
            StartDateTimePicker.Enabled = IsEditMode;
            EndDateTimePicker.Enabled = IsEditMode;
            ActiveCheckBox.Enabled = IsEditMode;
            NotesTextBox.Enabled = IsEditMode;
            EnablePlatformControls();
        }

        private void EnableEditSaveButton()
        {
            EditSaveButton.Enabled = IsEditor && (!IsEditMode ||
                                                  (OwnerComboBox.SelectedItem != null &&
                                                   !string.IsNullOrEmpty(ProgramIdTextBox.Text) &&
                                                   !string.IsNullOrEmpty(UserNameTextBox.Text) &&
                                                   !string.IsNullOrEmpty(PasswordMaskedTextBox.Text)));
        }

        private static void ConfigureDateTimePicker(DateTimePicker dateTimePicker, DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                dateTimePicker.CustomFormat = "yyyy-MM-dd";
                dateTimePicker.Value = dateTime.Value.ToLocalTime();
            }
            else
            {
                dateTimePicker.Value = DateTime.Now.Date + TimeSpan.FromHours(12);
                dateTimePicker.CustomFormat = " ";
            }
            dateTimePicker.Checked = dateTime.HasValue;
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
            Program.ProgramId = ProgramIdTextBox.Text;
            Program.ProjectInvestigator = (ProjectInvestigator)OwnerComboBox.SelectedItem;
            Program.ProgramName = ProgramNameTextBox.Text.NullifyIfEmpty();
            Program.UserName = UserNameTextBox.Text;
            Program.Password = PasswordMaskedTextBox.Text;
            Program.StartDate = StartDateTimePicker.Checked
                                        ? StartDateTimePicker.Value.ToUniversalTime()
                                        : (DateTime?)null;
            Program.EndDate = EndDateTimePicker.Checked
                                  ? EndDateTimePicker.Value.ToUniversalTime()
                                  : (DateTime?) null;
            Program.Active = ActiveCheckBox.CheckState == CheckState.Indeterminate ? (bool?)null : ActiveCheckBox.Checked;
            Program.Notes = NotesTextBox.Text.NullifyIfEmpty();
        }

        private void OwnerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableGeneralControls();
        }

        private void StartDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            StartDateTimePicker.CustomFormat = StartDateTimePicker.Checked ? "yyyy-MM-dd" : " ";
        }

        private void EndDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            EndDateTimePicker.CustomFormat = EndDateTimePicker.Checked ? "yyyy-MM-dd" : " ";
        }

        private void PasswordMaskedTextBox_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            EnableGeneralControls();
        }

        private void UserNameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableGeneralControls();
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


        #endregion


        #region Platforms Tab

        private void SetUpPlatformsTab()
        {
            SetUpPlatformsDataGridView();
            EnablePlatformControls();
        }

        private void SetUpPlatformsDataGridView()
        {
            PlatformsGridView.DataSource = Program.ArgosPlatforms;
            if (PlatformsGridView.Columns["ProgramId"] != null)
                PlatformsGridView.Columns["ProgramId"].Visible = false;
            if (PlatformsGridView.Columns["ArgosProgram"] != null)
                PlatformsGridView.Columns["ArgosProgram"].Visible = false;
        }

        private void EnablePlatformControls()
        {
            AddPlatformButton.Enabled = IsEditor && !IsEditMode;
            DeletePlatformButton.Enabled = IsEditor && !IsEditMode &&
                                           PlatformsGridView.SelectedRows.Cast<DataGridViewRow>()
                                                            .Any(row =>
                                                                 !((ArgosPlatform) row.DataBoundItem).ArgosDeployments
                                                                                                     .Any());
            InfoPlatformButton.Enabled = !IsEditMode && PlatformsGridView.SelectedRows.Count == 1;
            AddMissingPlatformsButton.Enabled = IsEditor && !IsEditMode;
        }

        private void PlatformDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpPlatformsTab();
        }

        private void AddPlatformButton_Click(object sender, EventArgs e)
        {
            var form = new AddArgosPlatformForm(Program);
            form.DatabaseChanged += (o, x) => PlatformDataChanged();
            form.Show(this);
        }

        private void DeletePlatformButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in PlatformsGridView.SelectedRows)
            {
                var platform = (ArgosPlatform)row.DataBoundItem;
                if (!platform.ArgosDeployments.Any())
                    Database.ArgosPlatforms.DeleteOnSubmit(platform);
            }
            if (SubmitChanges())
                PlatformDataChanged();
        }

        private void InfoPlatformButton_Click(object sender, EventArgs e)
        {
            var platform = (ArgosPlatform)PlatformsGridView.SelectedRows[0].DataBoundItem;
            var form = new ArgosPlatformDetailsForm(platform);
            form.DatabaseChanged += (o, x) => PlatformDataChanged();
            form.Show(this);
        }

        private void PlatformsGridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnablePlatformControls();
        }

        private void PlatformsGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && InfoPlatformButton.Enabled)
                InfoPlatformButton_Click(sender, e);
        }

        private void AddMissingPlatformsButton_Click(object sender, EventArgs e)
        {
            AddMissingPlatformsButton.Enabled = false;
            AddMissingPlatformsButton.Text = "Working...";
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            try
            {
                string error;
                var programPlatforms = ArgosWebSite.GetPlatformList(Program.UserName, Program.Password, out error);
                var AddedNewPlatform = false;
                if (error != null)
                {
                    MessageBox.Show("Argos Web Server returned an error" + Environment.NewLine + error, "Server Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                foreach (var item in programPlatforms)
                {
                    if (item.Item1 == Program.ProgramId)
                    {
                        if (Program.ArgosPlatforms.All(p => p.PlatformId != item.Item2))
                        {
                            var platform = new ArgosPlatform
                            {
                                ArgosProgram = Program,
                                PlatformId = item.Item2,
                                Active = true
                            };
                            Database.ArgosPlatforms.InsertOnSubmit(platform);
                            AddedNewPlatform = true;
                        }
                    }
                }
                if (AddedNewPlatform && SubmitChanges())
                    PlatformDataChanged();
            }
            finally
            {
                AddMissingPlatformsButton.Enabled = true;
                AddMissingPlatformsButton.Text = "Add Missing Platforms";
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion


        #region Downloads Tab

        private void SetUpDownloadsTab()
        {
            DownloadsDataGridView.DataSource = Program.ArgosDownloads.Select(d => new
            {
                d.TimeStamp,
                d.Days,
                d.CollarFile,
                d.ErrorMessage
            }).OrderByDescending(x => x.TimeStamp).ToList();
        }

        private void DownloadsChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpDownloadsTab();
        }

        private void DownloadsDetails()
        {
            var file = (CollarFile)DownloadsDataGridView.SelectedRows[0].Cells[2].Value;
            if (file == null)
                return;
            var form = new FileDetailsForm(file);
            form.DatabaseChanged += (o, x) => DownloadsChanged();
            form.Show(this);
        }

        private void DownloadsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && !IsEditMode)
                DownloadsDetails();
        }

        #endregion

    }
}
