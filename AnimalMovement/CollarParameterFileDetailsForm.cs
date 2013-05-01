using System;
using System.Data.SqlClient;
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
            ParametersDataGridView.DataSource = null;
        }

        #region Form Control

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            FileTabControl_SelectedIndexChanged(null, null);
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
            StatusComboBox.Enabled = IsEditMode;
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
            File.FileName = FileNameTextBox.Text.NullifyIfEmpty();
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
                                    cp.Collar,
                                    cp.StartDate,
                                    cp.EndDate,
                                    cp,
                                    CanDelete = !Database.CollarFiles.Any(f => f.CollarParameter == cp)
                                }).ToList();
            ParametersDataGridView.Columns[3].Visible = false;
            ParametersDataGridView.Columns[4].Visible = false;
            EnableCollarFilesControls();
        }

        private void EnableCollarFilesControls()
        {
            AddParameterButton.Enabled = !IsEditMode && IsEditor;
            DeleteParameterButton.Enabled = !IsEditMode && IsEditor &&
                                            ParametersDataGridView.SelectedRows.Cast<DataGridViewRow>()
                                                                  .Any(row => (bool) row.Cells["CanDelete"].Value);
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
            foreach (DataGridViewRow row in ParametersDataGridView.SelectedRows)
                if ((bool)row.Cells["CanDelete"].Value)
                    Database.CollarParameters.DeleteOnSubmit((CollarParameter) row.Cells[3].Value);
            if (SubmitChanges())
                ParametersDataChanged();
        }

        private void EditParameterButton_Click(object sender, EventArgs e)
        {
            var parameter = (CollarParameter)ParametersDataGridView.SelectedRows[0].Cells[3].Value;
            var form = new CollarParametersDetailsForm(parameter, false, true);
            form.DatabaseChanged += (o, x) => ParametersDataChanged();
            form.Show(this);
        }

        private void InfoParameterButton_Click(object sender, EventArgs e)
        {
            var parameter = (CollarParameter)ParametersDataGridView.SelectedRows[0].Cells[3].Value;
            var form = new CollarDetailsForm(parameter.Collar);
            form.DatabaseChanged += (o, x) => ParametersDataChanged();
            form.Show(this);
        }

        private void ParametersDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
                InfoParameterButton_Click(sender, e);
        }

        private void ParametersDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            EnableCollarFilesControls();
        }

        #endregion


        #region Telonics Parameter File Tab

        //TODO - Provide an interface to see which collars in the TPF file are not in the db, and an option to add them.

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
                                                        StartDate = t.TimeStamp
                                                    }).ToList();
        }

        #endregion

    }
}
