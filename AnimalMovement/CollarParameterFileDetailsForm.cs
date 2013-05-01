using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DataModel;
using Telonics;

//TODO - Provide an interface to see which collars in the TPF file are not in the db, and an option to add them.
//TODO - Enable Add/Edit/Delete for parameter assignments and date ranges
//TODO - add warning about PPF file types not being used and they are binary

namespace AnimalMovement
{
    internal partial class CollarParameterFileDetailsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private CollarParameterFile File { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        internal CollarParameterFileDetailsForm(CollarParameterFile file)
        {
            InitializeComponent();
            RestoreWindow();
            File = file;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpControls();
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
        }

        private void SetUpControls()
        {
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
            CollarsDataGridView.DataSource =
                Database.CollarParameters.Where(cp => cp.CollarParameterFile == File)
                        .Select(cp => new {cp.Collar, cp.StartDate, cp.EndDate});
            ShowTpfData();
            EnableForm();
            DoneCancelButton.Focus();
        }

        private void ShowTpfData()
        {
            if (File.Format != 'A')
            {
                FileTabControl.TabPages.Remove(TpfDetailsTabPage);
                return;
            }
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

        private void UpdateDataSource()
        {
            File.FileName = FileNameTextBox.Text.NullifyIfEmpty();
            File.ProjectInvestigator = (ProjectInvestigator)OwnerComboBox.SelectedItem;
            File.LookupFileStatus = (LookupFileStatus)StatusComboBox.SelectedItem;
        }

        private void ShowContentsButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var form = new FileContentsForm(File.Contents.ToArray(), File.FileName);
            Cursor.Current = Cursors.Default;
            form.Show(this);
        }

        private void EnableForm()
        {
            EditSaveButton.Enabled = IsEditor;
        }

        private void EditSaveButton_Click(object sender, EventArgs e)
        {
            //This button is not enabled unless editing is permitted 
            if (EditSaveButton.Text == "Edit")
            {
                // The user wants to edit, Enable form
                EditSaveButton.Text = "Save";
                DoneCancelButton.Text = "Cancel";
                SetEditingControls();
            }
            else
            {
                //User is saving
                try
                {
                    UpdateDataSource();
                    Database.SubmitChanges();
                    OnDatabaseChanged();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to save changes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                EditSaveButton.Text = "Edit";
                DoneCancelButton.Text = "Done";
                SetEditingControls();
            }
        }


        private void DoneCancelButton_Click(object sender, EventArgs e)
        {
            if (DoneCancelButton.Text == "Cancel")
            {
                DoneCancelButton.Text = "Done";
                EditSaveButton.Text = "Edit";
                SetEditingControls();
                //Reset state from database
                LoadDataContext();
            }
            else
            {
                Close();
            }
        }


        private void SetEditingControls()
        {
            bool editModeEnabled = EditSaveButton.Text == "Save";
            FileNameTextBox.Enabled = editModeEnabled;
            OwnerComboBox.Enabled = editModeEnabled;
            StatusComboBox.Enabled = editModeEnabled;
        }


        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }
    }
}
