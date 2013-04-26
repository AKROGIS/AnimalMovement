using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class AddAnimalForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private AnimalMovementFunctions Functions { get; set; }
        private string CurrentUser { get; set; }
        private Project Project { get; set; }
        private bool IsEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        internal AddAnimalForm(Project project)
        {
            InitializeComponent();
            RestoreWindow();
            Project = project;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpControls();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Project is in a different DataContext, get one in this DataContext
            if (Project != null)
                Project = Database.Projects.FirstOrDefault(p => p.ProjectId == Project.ProjectId);

            //If Project is not provided, Current user must be a PI or an project editor
            if (Project == null)
                if (!Database.Projects.Any(p => p.ProjectInvestigator == CurrentUser) &&
                    !Database.ProjectEditors.Any(e => e.Editor == CurrentUser))
                    throw new InvalidOperationException("Add Animal Form not provided a valid Project or you are not a PI or editor on any projects.");

            Functions = new AnimalMovementFunctions();
            IsEditor = Project != null && (Functions.IsProjectEditor(Project.ProjectId, CurrentUser) ?? false);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            MortatlityDateTimePicker.Value = DateTime.Now.Date + TimeSpan.FromHours(12);
            MortatlityDateTimePicker.Checked = false;
            MortatlityDateTimePicker.CustomFormat = " ";
        }

        private void SetUpControls()
        {
            SetUpProjectComboBox();
            AnimalIdTextBox.Text = Functions.NextAnimalId(Project.ProjectId);
            GenderComboBox.DataSource = Database.LookupGenders;
            GenderComboBox.DisplayMember = "Sex";
            GenderComboBox.SelectedIndex = 0;
            SpeciesComboBox.DataSource = Database.LookupSpecies;
            SpeciesComboBox.DisplayMember = "Species";
            SelectDefaultSpecies(Settings.GetDefaultSpecies());
            EnableControls();
        }

        private void SetUpProjectComboBox()
        {
            //If given a Project, set that and lock it.
            //else, set list to all projects I can edit, and select null per the constructor request
            if (Project != null)
                ProjectComboBox.Items.Add(Project);
            else
            {
                ProjectComboBox.DataSource =
                    Database.Projects.Where(p => p.ProjectInvestigator == CurrentUser).Concat(
                        Database.ProjectEditors.Where(e => e.Editor == CurrentUser)
                                .Select(e => e.Project));
            }
            ProjectComboBox.DisplayMember = "ProjectName";
            ProjectComboBox.Enabled = Project == null;
            ProjectComboBox.SelectedItem = Project;
        }

        private void SelectDefaultSpecies(string speciesCode)
        {
            if (speciesCode != null)
            {
                var species = Database.LookupSpecies.FirstOrDefault(s => s.Species == speciesCode);
                if (species != null)
                    SpeciesComboBox.SelectedItem = species;
                return;
            }
            SpeciesComboBox.SelectedIndex = 0;
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

        private void EnableControls()
        {
            CreateButton.Enabled = IsEditor && !string.IsNullOrEmpty(AnimalIdTextBox.Text);
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            string animalId = AnimalIdTextBox.Text.NullifyIfEmpty();

            if (Database.Animals.Any(a => a.Project == Project && a.AnimalId == animalId))
            {
                MessageBox.Show("The animal Id is not unique.  Try again", "Database Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                AnimalIdTextBox.Focus();
                CreateButton.Enabled = false;
                return;
            }
            var animal = new Animal
            {
                Project = Project,
                AnimalId = AnimalIdTextBox.Text,
                LookupGender = (LookupGender)GenderComboBox.SelectedItem,
                LookupSpecies = (LookupSpecies)SpeciesComboBox.SelectedItem,
                MortalityDate = MortatlityDateTimePicker.Checked ? MortatlityDateTimePicker.Value.ToUniversalTime() : (DateTime?)null,
                GroupName = GroupTextBox.Text,
                Description = DescriptionTextBox.Text
            };
            Database.Animals.InsertOnSubmit(animal);
            if (!SubmitChanges())
            {
                AnimalIdTextBox.Focus();
                CreateButton.Enabled = false;
                return;
            }
            OnDatabaseChanged();
            Close();
        }

        private void AnimalIdTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableControls();
        }

        private void SpeciesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var species = (LookupSpecies)SpeciesComboBox.SelectedItem;
            if (species != null)
                Settings.SetDefaultSpecies(species.Species);
        }

        private void MortatlityDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            MortatlityDateTimePicker.CustomFormat = MortatlityDateTimePicker.Checked ? "yyyy-MM-dd HH:mm" : " ";
        }

        private void ProjectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var project = ProjectComboBox.SelectedItem as Project;
            IsEditor = project != null &&
                       (Functions.IsProjectEditor(project.ProjectId, CurrentUser) ?? false);
            EnableControls();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
