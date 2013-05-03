using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class AddArgosPlatformForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private ArgosProgram Program { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        internal AddArgosPlatformForm(ArgosProgram program = null)
        {
            InitializeComponent();
            RestoreWindow();
            Program = program;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpControls();
        }

        internal void SetDefaultPlatform(string platformId)
        {
            ArgosIdTextBox.Text = platformId;
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Program is in a different DataContext, get one in this DataContext
            if (Program != null)
                Program = Database.ArgosPrograms.FirstOrDefault(p => p.ProgramId == Program.ProgramId);

            //Validate Program and Editor on load, so we can show a messagebox.
        }

        private void SetUpControls()
        {
            SetUpProgramComboBox();
            EnableControls();
        }

        private void SetUpProgramComboBox()
        {
            //If given a Project, set that and lock it.
            //else, set list to all projects I can edit, and select null per the constructor request
            if (Program != null)
                ArgosProgramComboBox.Items.Add(Program);
            else
            {
                ArgosProgramComboBox.DataSource =
                    Database.ArgosPrograms.Where(p => p.Manager == CurrentUser ||
                     p.ProjectInvestigator.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser));
            }
            ArgosProgramComboBox.Enabled = Program == null;
            ArgosProgramComboBox.SelectedItem = Program;
        }

        private void EnableControls()
        {
            CreateButton.Enabled = IsEditor && ArgosProgramComboBox.SelectedItem != null &&
                                   !string.IsNullOrEmpty(ArgosIdTextBox.Text);
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

        #region Form controls

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            IsEditor = true; //Hope for the best
            //If Program provided, it will be locked, so you must be an editor on that program
            if (Program != null && Program.Manager != CurrentUser &&
                Program.ProjectInvestigator.ProjectInvestigatorAssistants.All(a => a.Assistant != CurrentUser))
            {
                MessageBox.Show(
                    "You do not have permission to add platforms to this program.", "No Permission",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsEditor = false;
            }
            //If Program is not provided, make sure there is a program (we can edit) to pick from
            if (Program == null && ArgosProgramComboBox.Items.Count == 0)
            {
                MessageBox.Show(
                    "You do not have permissions on any Argos Programs as a PI or assistant.", "No Permission", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsEditor = false;
            }

            DisposalDateTimePicker.Value = DateTime.Now.Date + TimeSpan.FromHours(12);
            DisposalDateTimePicker.Checked = false;
            DisposalDateTimePicker.CustomFormat = " ";
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var argosId = ArgosIdTextBox.Text.NullifyIfEmpty();
            var program = (ArgosProgram) ArgosProgramComboBox.SelectedItem;

            if (Database.ArgosPlatforms.Any(p => p.ArgosProgram == program && p.PlatformId == argosId))
            {
                MessageBox.Show("The Argos Id is not unique.  Try again", "Database Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                ArgosIdTextBox.Focus();
                CreateButton.Enabled = false;
                return;
            }
            var argos = new ArgosPlatform
            {
                ArgosProgram = program,
                PlatformId = argosId,
                DisposalDate = DisposalDateTimePicker.Checked ? DisposalDateTimePicker.Value.ToUniversalTime() : (DateTime?)null,
                Active = ActiveCheckBox.Checked,
                Notes = NotesTextBox.Text,
            };
            Database.ArgosPlatforms.InsertOnSubmit(argos);
            if (!SubmitChanges())
            {
                ArgosIdTextBox.Focus();
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

        private void ArgosProgramComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void ArgosIdTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void DisposalDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DisposalDateTimePicker.CustomFormat = DisposalDateTimePicker.Checked ? "yyyy-MM-dd" : " ";
        }

        #endregion

    }
}
