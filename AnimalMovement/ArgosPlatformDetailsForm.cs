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
            SetUpArgosProgramComboBox();
            //defer DisposalDateTimePicker until loaded, otherwise the default value get messed up
            ActiveCheckBox.Checked = Platform.Active;
            NotesTextBox.Text = Platform.Notes;
            EnableControls();
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

        private void EnableControls()
        {
            ArgosIdTextBox.Enabled = false;
            EditSaveButton.Enabled = IsEditor;
            IsEditMode = EditSaveButton.Text == "Save";
            ArgosProgramComboBox.Enabled = IsEditMode;
            DisposalDateTimePicker.Enabled = IsEditMode;
            ActiveCheckBox.Enabled = IsEditMode;
            NotesTextBox.Enabled = IsEditMode;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ConfigureDispsoalDateTimePicker();
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
            Platform.Notes = NotesTextBox.Text;
        }

        private void EditSaveButton_Click(object sender, EventArgs e)
        {
            //This button is not enabled unless editing is permitted 
            if (EditSaveButton.Text == "Edit")
            {
                // The user wants to edit, Enable form
                EditSaveButton.Text = "Save";
                DoneCancelButton.Text = "Cancel";
                EnableControls();
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
                    EnableControls();
                }
            }
        }

        private void DoneCancelButton_Click(object sender, EventArgs e)
        {
            if (DoneCancelButton.Text == "Cancel")
            {
                DoneCancelButton.Text = "Done";
                EditSaveButton.Text = "Edit";
                //Reset state from database
                LoadDataContext();
                SetUpForm();
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

    }
}
