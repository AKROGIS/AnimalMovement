using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class AddArgosProgramForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private ProjectInvestigator Investigator { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        public AddArgosProgramForm(ProjectInvestigator investigator = null)
        {
            InitializeComponent();
            RestoreWindow();
            Investigator = investigator;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpControls();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Investigator is in a different DataContext, get one in this DataContext
            if (Investigator != null)
                Investigator = Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == Investigator.Login);

            //Validate Program and Editor on load, so we can show a messagebox.
        }

        private void SetUpControls()
        {
            SetUpOwnerComboBox();
            EnableControls();
        }

        private void SetUpOwnerComboBox()
        {
            //If given a Investigator, set that and lock it.
            //else, set list to all projects I can edit, and select null per the constructor request
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

        private void EnableControls()
        {
            CreateButton.Enabled = IsEditor && OwnerComboBox.SelectedItem != null &&
                                   !string.IsNullOrEmpty(ProgramIdTextBox.Text) &&
                                   !string.IsNullOrEmpty(UserNameTextBox.Text) &&
                                   !string.IsNullOrEmpty(PasswordTextBox.Text);
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

        private void ValidateEditor()
        {
            IsEditor = true; //Hope for the best
            //If Investigator is provided, it will be locked, so you must be the pi or an assistant
            if (Investigator != null && Investigator.Login != CurrentUser &&
                Investigator.ProjectInvestigatorAssistants.All(a => a.Assistant != CurrentUser))
            {
                MessageBox.Show(
                    "You do not have permission to add programs for this investigator.", "No Permission",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsEditor = false;
            }
            //If Investigator is not provided, make sure there is a investigator (we can edit) to pick from
            if (Investigator == null && OwnerComboBox.Items.Count == 0)
            {
                MessageBox.Show(
                    "You can't create a program unless you are a PI or a PI's assistant.", "No Permission",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsEditor = false;
            }
        }

        private void SetUpDatePickers()
        {
            StartDateTimePicker.Value = DateTime.Now.Date + TimeSpan.FromHours(12);
            StartDateTimePicker.Checked = false;
            StartDateTimePicker.CustomFormat = " ";
            EndDateTimePicker.Value = DateTime.Now.Date + TimeSpan.FromHours(12);
            EndDateTimePicker.Checked = false;
            EndDateTimePicker.CustomFormat = " ";
        }

        #region Form controls

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ValidateEditor();
            SetUpDatePickers();
        }

        private void OwnerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void ProgramIdTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void StartDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            StartDateTimePicker.CustomFormat = StartDateTimePicker.Checked ? "yyyy-MM-dd" : " ";
        }

        private void EndDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            EndDateTimePicker.CustomFormat = EndDateTimePicker.Checked ? "yyyy-MM-dd" : " ";
        }

        private void UserNameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var programId = ProgramIdTextBox.Text;
            if (Database.ArgosPrograms.Any(p => p.ProgramId == programId))
            {
                MessageBox.Show("The Program Id already exists in the database.  Try again", "Database Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                ProgramIdTextBox.Focus();
                CreateButton.Enabled = false;
                return;
            }
            var program = new ArgosProgram
            {
                ProgramId = programId,
                ProjectInvestigator = (ProjectInvestigator)OwnerComboBox.SelectedItem,
                ProgramName = ProgramNameTextBox.Text.NullifyIfEmpty(),
                UserName = UserNameTextBox.Text,
                Password = PasswordTextBox.Text,
                StartDate = StartDateTimePicker.Checked ? StartDateTimePicker.Value.ToUniversalTime() : (DateTime?)null,
                EndDate = EndDateTimePicker.Checked ? EndDateTimePicker.Value.ToUniversalTime() : (DateTime?)null,
                Active = ActiveCheckBox.CheckState == CheckState.Indeterminate ? (bool?)null : ActiveCheckBox.Checked,
                Notes = NotesTextBox.Text.NullifyIfEmpty(),
            };
            Database.ArgosPrograms.InsertOnSubmit(program);
            if (!SubmitChanges())
            {
                ProgramIdTextBox.Focus();
                CreateButton.Enabled = false;
                return;
            }
            OnDatabaseChanged();
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
