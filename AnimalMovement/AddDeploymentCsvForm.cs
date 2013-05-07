using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class AddDeploymentCsvForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private ProjectInvestigator Investigator { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        public AddDeploymentCsvForm(ProjectInvestigator investigator = null)
        {
            InitializeComponent();
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
            SetUpInvestigatorComboBox();
            EnableControls();
        }

        private void SetUpInvestigatorComboBox()
        {
            //If given a Investigator, set that and lock it.
            //else, set list to all projects I can edit, and select null per the constructor request
            if (Investigator != null)
                InvestigatorComboBox.Items.Add(Investigator);
            else
            {
                InvestigatorComboBox.DataSource =
                    Database.ProjectInvestigators.Where(pi => pi.Login == CurrentUser ||
                     pi.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser));
            }
            InvestigatorComboBox.Enabled = Investigator == null;
            InvestigatorComboBox.SelectedItem = Investigator;
            InvestigatorComboBox.DisplayMember = "Name";
        }

        private void EnableControls()
        {
            ProcessButton.Enabled = IsEditor && InvestigatorComboBox.SelectedItem != null &&
                                   !string.IsNullOrEmpty(FileNameTextBox.Text);
        }

        private void ValidateEditor()
        {
            //If Investigator is provided, it will be locked, so you must be the pi or an assistant
            if (Investigator != null && Investigator.Login != CurrentUser &&
                Investigator.ProjectInvestigatorAssistants.All(a => a.Assistant != CurrentUser))
            {
                MessageBox.Show(
                    "You do not have permission to load deployments for this investigator.", "No Permission",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsEditor = false;
                return;
            }
            //If Investigator is not provided, make sure there is a investigator (we can edit) to pick from
            if (Investigator == null && InvestigatorComboBox.Items.Count == 0)
            {
                MessageBox.Show(
                    "You can't load deployments unless you are a PI or a PI's assistant.", "No Permission",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsEditor = false;
                return;
            }
            IsEditor = true; //Hope for the best
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

        #region Read Excel File

        private DataTable GetDataTableFromExcel(string path, string sheetName = "Sheet1")
        {
            var dataTable = new DataTable();
            var ext = Path.GetExtension(path);
            var extProps = new Dictionary<string, string> { { ".xls", "Excel 8.0" }, { ".xlsx", "Excel 12.0 Xml" }, { ".xlsm", "Excel 12.0 Macro" }, { ".xlsb", "Excel 12.0" } };
            if (ext == null || !extProps.ContainsKey(ext))
            {
                MessageBox.Show("Unrecognized file type");
                return null;
            }
            var constr = "Provider=Microsoft.ACE.OLEDB.12.0" +
                         ";Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)}" +
                         ";DBQ=" + path +
                         ";Extended Properties='" + extProps[ext] + ";IMEX=1;HDR=Yes;'";
            using (var connection = new OleDbConnection(constr))
            {
                connection.Open();
                var command = new OleDbDataAdapter(" SELECT * FROM [" + sheetName + "$]", connection);
                command.Fill(dataTable);
            }
            return dataTable;
        }

        #endregion


        #region Read CSV File

        public static DataTable ConvertTabFiles(string path, char delimeter = ',')
        {
            var dataTable = new DataTable();
            bool gotHeader = false;
            foreach (var line in File.ReadLines(path))
            {
                if (!gotHeader)
                {
                    foreach (string column in line.Split(delimeter))
                        dataTable.Columns.Add(column);
                    gotHeader = true;
                    continue;
                }
                // ReSharper disable CoVariantArrayConversion
                dataTable.Rows.Add(line.Split(delimeter));
                // ReSharper restore CoVariantArrayConversion
            }
            return dataTable;
        }

        #endregion


        #region Form Control Events

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            ValidateEditor();
            EnableControls();
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            DataTable dataTable = null;
            try
            {
                dataTable = ConvertTabFiles(FileNameTextBox.Text);
            }
            catch (Exception ex1)
            {
                MessageBox.Show("Unable to parse the CSV file." + Environment.NewLine + ex1.Message);
            }
            if (dataTable == null)
                return;
            ExcelDataGridView.DataSource = dataTable;
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            if (OpenFileDialog.ShowDialog() != DialogResult.Cancel)
                FileNameTextBox.Text = OpenFileDialog.FileName;
        }

        private void InvestigatorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void FileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        #endregion
    }
}
