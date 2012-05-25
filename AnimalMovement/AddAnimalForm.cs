using System;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class AddAnimalForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private bool IndependentContext { get; set; }
        private string CurrentUser { get; set; }
        private string ProjectId { get; set; }
        private Project Project { get; set; }
        private bool IsProjectEditor { get; set; }
        internal event EventHandler DatabaseChanged;

        internal AddAnimalForm(string projectId, string user)
        {
            IndependentContext = true;
            ProjectId = projectId;
            CurrentUser = user;
            SetupForm();
        }

        internal AddAnimalForm(AnimalMovementDataContext database, Project project, string user)
        {
            IndependentContext = false;
            Database = database;
            Project = project ;
            CurrentUser = user;
            SetupForm();
        }

        private void SetupForm()
        {
            InitializeComponent();
            RestoreWindow();
            LoadDataContext();
            EnableCreate();
        }

        private void LoadDataContext()
        {
            if (IndependentContext)
            {
                Database = new AnimalMovementDataContext();
                Project = Database.Projects.FirstOrDefault(p => p.ProjectId == ProjectId);   
            }
            if (Database == null || Project == null || CurrentUser == null)
            {
                MessageBox.Show("Insufficent information to initialize form.", "Form Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            IsProjectEditor = Database.IsEditor(Project.ProjectId, CurrentUser) ?? false;
            ProjectCodeTextBox.Text = Project.ProjectId;
            AnimalIdTextBox.Text = Database.NextAnimalId(Project.ProjectId);
            GenderComboBox.DataSource = Database.LookupGenders;
            GenderComboBox.DisplayMember = "Sex";
            GenderComboBox.SelectedIndex = 0;
            string species = Settings.GetDefaultSpecies();
            SpeciesComboBox.DataSource = Database.LookupSpecies;
            SpeciesComboBox.DisplayMember = "Species";
            SelectDefaultSpecies(species);
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

        private void EnableCreate()
        {
            CreateButton.Enabled = IsProjectEditor && !string.IsNullOrEmpty(AnimalIdTextBox.Text);
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
                GroupName = GroupTextBox.Text,
                Description = DescriptionTextBox.Text
            };
            Database.Animals.InsertOnSubmit(animal);
            if (IndependentContext)
            {
                try
                {
                    Database.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Database.Animals.DeleteOnSubmit(animal);
                    MessageBox.Show(ex.Message, "Unable to create new animal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AnimalIdTextBox.Focus();
                    CreateButton.Enabled = false;
                    return;
                }
            }
            OnDatabaseChanged();
            DialogResult = DialogResult.OK;
        }

        private void AnimalIdTextBox_TextChanged(object sender, EventArgs e)
        {
            EnableCreate();
        }

        private void SpeciesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var species = (LookupSpecies)SpeciesComboBox.SelectedItem;
            if (species != null)
                Settings.SetDefaultSpecies(species.Species);
        }

        private void OnDatabaseChanged()
        {
            EventHandler handle = DatabaseChanged;
            if (handle != null)
                handle(this, EventArgs.Empty);
        }

    }
}
