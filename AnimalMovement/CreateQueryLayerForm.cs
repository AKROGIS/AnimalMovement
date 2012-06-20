using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using DataModel;

//TODO - Add Region Filter
//TODO - Cleanup and Simplify the form
//TODO - Put everything in one group layer (include no movement, hidden locations); remove location/movements checkbox

namespace AnimalMovement
{
    internal partial class CreateQueryLayerForm : BaseForm
    {
        private const string ArcMapConnectionTemplate = @"dbclient=sqlserver;serverinstance={0};database=Animal_Movement;authentication_mode=OSA";
        private const string SqlConnectionTemplate = @"Data Source={0};Initial Catalog=Animal_Movement;Integrated Security=True";
        private const string QueryLayerBuilderExe = "QueryLayerBuilder.exe";

        private AnimalMovementDataContext Database { get; set; }
        private List<Project> Projects { get; set; }
        private List<Animal> Animals { get; set; }
        private List<LookupSpecies> Species { get; set; }


        public CreateQueryLayerForm()
        {
            InitializeComponent();
            RestoreWindow();
            splitContainer1.SplitterDistance = Properties.Settings.Default.CreateQueryLayerFormSplitter1Distance;
            splitContainer2.SplitterDistance = Properties.Settings.Default.CreateQueryLayerFormSplitter2Distance;
            LoadDataContext();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Properties.Settings.Default.CreateQueryLayerFormSplitter1Distance = splitContainer1.SplitterDistance;
            Properties.Settings.Default.CreateQueryLayerFormSplitter2Distance = splitContainer2.SplitterDistance;
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            Projects = Database.Projects.ToList();
            Animals = Database.Animals.ToList();
            Species = Database.LookupSpecies.ToList();
            LoadDatabaseList();
            LoadProjectsList(); 
            LoadSpeciesList();
            LoadAnimalsList();
            LoadDefaultDates();
        }

        private void LoadDatabaseList()
        {
            var db = new SettingsDataContext();
            DatabaseComboBox.DataSource = db.LookupQueryLayerServers;
            DatabaseComboBox.DisplayMember = "Location";
            DatabaseComboBox.ValueMember = "Connection";
        }

        private void LoadProjectsList()
        {
            ProjectsCheckedListBox.DataSource = Projects;
            ProjectsCheckedListBox.DisplayMember = "ProjectName";
        }

        private void LoadSpeciesList()
        {
            SpeciesCheckedListBox.DataSource = Species;
            SpeciesCheckedListBox.DisplayMember = "Species";
        }

        private void LoadAnimalsList()
        {
            AnimalsCheckedListBox.DataSource = Animals;
        }

        private void LoadDefaultDates()
        {
            StartDateTimePicker.Value = DateTime.Now.Date - TimeSpan.FromDays(365);
            EndDateTimePicker.Value = DateTime.Now.Date;
        }

        private void AllSpeciesButton_Click(object sender, EventArgs e)
        {
            SetAllCheckedListItems(SpeciesCheckedListBox, CheckState.Checked);
        }

        private void ClearSpeciesButton_Click(object sender, EventArgs e)
        {
            SetAllCheckedListItems(SpeciesCheckedListBox, CheckState.Unchecked);
        }

        private void AllProjectsButton_Click(object sender, EventArgs e)
        {
            SetAllCheckedListItems(AnimalsCheckedListBox, CheckState.Checked);
        }

        private void ClearProjectsButton_Click(object sender, EventArgs e)
        {
            SetAllCheckedListItems(AnimalsCheckedListBox, CheckState.Unchecked);
        }

        private void AllAnimalsButton_Click(object sender, EventArgs e)
        {
            SetAllCheckedListItems(ProjectsCheckedListBox, CheckState.Checked);
        }

        private void ClearAnimalsButton_Click(object sender, EventArgs e)
        {
            SetAllCheckedListItems(ProjectsCheckedListBox, CheckState.Unchecked);
        }

        private static void SetAllCheckedListItems(CheckedListBox listBox, CheckState state)
        {
            for (int index = 0; index < listBox.Items.Count; index++)
                listBox.SetItemCheckState(index, state);
        }

        private void FilterByProjectCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!FilterByProjectsCheckBox.Checked)
                ClearProjectsButton.PerformClick();
            ProjectsCheckedListBox.Enabled = FilterByProjectsCheckBox.Checked;
            AllProjectsButton.Enabled = FilterByProjectsCheckBox.Checked;
            ClearProjectsButton.Enabled = FilterByProjectsCheckBox.Checked;
        }

        private void FilterBySpeciesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!FilterBySpeciesCheckBox.Checked)
                ClearSpeciesButton.PerformClick();
            SpeciesCheckedListBox.Enabled = FilterBySpeciesCheckBox.Checked;
            AllSpeciesButton.Enabled = FilterBySpeciesCheckBox.Checked;
            ClearSpeciesButton.Enabled = FilterBySpeciesCheckBox.Checked;
        }

        private void FilterByAnimalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!FilterByAnimalsCheckBox.Checked)
                ClearAnimalsButton.PerformClick();
            AnimalsCheckedListBox.Enabled = FilterByAnimalsCheckBox.Checked;
            AllAnimalsButton.Enabled = FilterByAnimalsCheckBox.Checked;
            ClearAnimalsButton.Enabled = FilterByAnimalsCheckBox.Checked;
        }

        private void FilterByDatesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            StartDateTimePicker.Enabled = FilterByDatesCheckBox.Checked && !UseEarliestDateCheckBox.Checked;
            EndDateTimePicker.Enabled = FilterByDatesCheckBox.Checked && !UseLatestDateCheckBox.Checked;
            UseEarliestDateCheckBox.Enabled = FilterByDatesCheckBox.Checked;
            UseLatestDateCheckBox.Enabled = FilterByDatesCheckBox.Checked;
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            string arcMapConnectionString = String.Format(ArcMapConnectionTemplate, DatabaseComboBox.SelectedValue);
            string predicate = BuildPredicate();
            if (!String.IsNullOrEmpty(predicate))
            {
                var sql = "SELECT TOP 1 1 FROM [dbo].[ValidLocations] WHERE " + predicate.Replace("EndLocalDateTime", "LocalDateTime");

                string connectionString = String.Format(SqlConnectionTemplate, DatabaseComboBox.SelectedValue);
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var command = new SqlCommand(sql, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        MessageBox.Show(this, "There are no locations in the database that meet your criteria.  Please try again.",
                                        "No Results", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            if (CreateLocationsCheckBox.Checked)
                BuildQueryLayer("Locations", arcMapConnectionString, predicate);
            if (CreateMovementsCheckBox.Checked)
                BuildQueryLayer("Movements", arcMapConnectionString, predicate);
        }

        private void BuildQueryLayer(string table, string connection, string predicate)
        {
            saveFileDialog1.Title = table + " Query Layer";
            saveFileDialog1.DefaultExt = ".lyr";
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                SaveQueryLayer(table, saveFileDialog1.FileName, connection, BuildSql(table, predicate));
        }

        private static void SaveQueryLayer(string name, string path, string connection, string query)
        {
            string exeDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (exeDir == null)
            {
                MessageBox.Show("Path to Query Layer Builder not found", "Layer File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string exe = System.IO.Path.Combine(exeDir, QueryLayerBuilderExe);

            string arguments = "\"" + name + "\" \"" + path + "\" \"" + connection + "\" \"" + query + "\"";
            var process = new Process();
            var processStartInfo = new ProcessStartInfo(exe, arguments) { UseShellExecute = false, RedirectStandardError = true };
            process.StartInfo = processStartInfo;
            process.Start();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            if (process.ExitCode == 0)
                return;
            var message = "Name:{0}\nFile:{1}\nConnection:{2}\nQuery:{3}\nError:{4}";
            message = String.Format(message, name, path, connection, query, error);
            MessageBox.Show(message, "Layer File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static string BuildSql(string table, string predicate)
        {
            string sql;
            switch (table)
            {
                case "Locations":
                    sql = "SELECT * FROM [ValidLocations]";
                    return String.IsNullOrEmpty(predicate) ? sql : sql + " WHERE " + predicate.Replace("EndLocalDateTime", "LocalDateTime");
                case "Movements":
                    sql = "SELECT * FROM [VelocityVectors]";
                    return String.IsNullOrEmpty(predicate) ? sql : sql + " WHERE " + predicate;
                default:
                    throw new ArgumentOutOfRangeException("table", table, "Table name is not recognized");
            }
        }

        private string BuildPredicate()
        {

            var animal = String.Format(BuildAnimalPredicate() ?? "", "[ProjectId]", "[AnimalId]");
            var project = String.Format(BuildProjectPredicate() ?? "", "[ProjectId]");
            var species = String.Format(BuildSpeciesPredicate() ?? "", "[Species]");
            string date = String.Format(BuildDatePredicate() ?? "", "[LocalDateTime]", "[EndLocalDateTime]");
            string sqlEnd = String.Empty;
            var notList = new[] { "" /*animals*/, "" /*project*/ , "" /*species*/, "" /*date*/};
            var opList = new[] { /*animals*/ " AND ", /*project*/ " AND ", /*species*/ " AND ", /*date*/ " AND " /*sqlEnd*/ };
            var predList = new[] { animal, project, species, date };
            for (int i = predList.Length - 1; i >= 0; i--)
            {
                if (string.IsNullOrEmpty(predList[i]))
                    continue;
                var p = notList[i] + predList[i];
                sqlEnd = (sqlEnd == String.Empty) ? p : p + opList[i] + sqlEnd;
            }
            return sqlEnd;
        }

        private string BuildAnimalPredicate()
        {
            if (!FilterByAnimalsCheckBox.Checked)
                return null;
            if (AnimalsCheckedListBox.CheckedItems.Count == 0 ||
                AnimalsCheckedListBox.CheckedItems.Count == AnimalsCheckedListBox.Items.Count)
                return null;
            var animalSelectors = AnimalsCheckedListBox.CheckedItems.Cast<Animal>().Select(a => "({0} = '" + a.ProjectId + "' AND {1} = '" + a.AnimalId + "')");
            var sqlList = "(" + String.Join(" OR ", animalSelectors) + ")";
            return sqlList == "()" ? null : sqlList;
        }

        private string BuildProjectPredicate()
        {
            if (!FilterByProjectsCheckBox.Checked)
                return null;
            if (ProjectsCheckedListBox.CheckedItems.Count == 0 ||
                ProjectsCheckedListBox.CheckedItems.Count == ProjectsCheckedListBox.Items.Count)
                return null;
            var sqlList = ProjectsCheckedListBox.CheckedItems.Cast<Project>().Select(p => p.ProjectId).BuildSqlList();
            if (sqlList == null)
                return null;
            return "{0} IN " + sqlList;
        }

        private string BuildSpeciesPredicate()
        {
            if (!FilterBySpeciesCheckBox.Checked)
                return null;
            if (SpeciesCheckedListBox.CheckedItems.Count == 0 ||
                SpeciesCheckedListBox.CheckedItems.Count == SpeciesCheckedListBox.Items.Count)
                return null;
            var sqlList = SpeciesCheckedListBox.CheckedItems.Cast<LookupSpecies>().Select(s => s.Species).BuildSqlList();
            if (sqlList == null)
                return null;
            return "{0} IN " + sqlList;
        }

        private string BuildDatePredicate()
        {
            if (!FilterByDatesCheckBox.Checked)
                return null;
            if (UseEarliestDateCheckBox.Checked && UseLatestDateCheckBox.Checked)
                return null;
            if (UseLatestDateCheckBox.Checked)
                return "'" + StartDateTimePicker.Value.ToString("yyyy-MM-dd HH:mm") + "' <= {0}";
            if (UseEarliestDateCheckBox.Checked)
                return "{1} <= '" + EndDateTimePicker.Value.ToString("yyyy-MM-dd HH:mm") + "'";
            return "'" + StartDateTimePicker.Value.ToString("yyyy-MM-dd HH:mm") + "' <= {0} AND {1} <= '" + EndDateTimePicker.Value.ToString("yyyy-MM-dd HH:mm") + "'";
        }
    }
}
