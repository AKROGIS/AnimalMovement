using System;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class DeployCollarForm : BaseForm
    {
        private Collar Collar { get; set; }
        private AnimalMovementDataContext Database { get; set; }
        private Project Project { get; set; }
        private string CurrentUser { get; set; }

        internal DeployCollarForm(AnimalMovementDataContext database, Collar collar, string user)
        {
            InitializeComponent();
            RestoreWindow();
            Database = database;
            Collar = collar;
            CurrentUser = user;
            LoadForm();
        }

        private void LoadForm()
        {
            //read the defaults before setting the combobox, because changing the selection saves the default
            string projectId = Settings.GetDefaultProject();
            ProjectComboBox.DataSource = from p in Database.Projects
                                         where p.ProjectInvestigator == CurrentUser ||
                                               p.ProjectEditors.Any(u => u.Editor == CurrentUser)
                                         select p;
            ProjectComboBox.DisplayMember = "ProjectName";
            SelectDefaultProject(projectId);
            AnimalComboBox.DisplayMember = "AnimalId";
        }

        private void SelectDefaultProject(string projectId)
        {
            if (projectId != null)
                Project = Database.Projects.FirstOrDefault(p => p.ProjectId == projectId);
            if (Project == null)
                Project = (Project)ProjectComboBox.SelectedItem;
            else
                ProjectComboBox.SelectedItem = Project;
        }

        private void ProjectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Project = (Project)ProjectComboBox.SelectedItem;
            if (Project != null)
                Settings.SetDefaultProject(Project.ProjectId);
            LoadAnimalList();
        }

        private void LoadAnimalList()
        {
            var animalsInProject = (from animal in Project.Animals select animal);
            var animalsWithCollars = from deploy in Database.CollarDeployments
                                     where deploy.RetrievalDate == null
                                     select deploy.Animal;
            var animalsInProjectWithNoCollar = animalsInProject.ToList();
            foreach (var animal in animalsWithCollars)
                animalsInProjectWithNoCollar.Remove(animal);
            AnimalComboBox.DataSource = animalsInProjectWithNoCollar;
            EnableForm();
        }

        private void DeployDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            EnableForm();
        }

        private void AnimalComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableForm();
        }

        private void EnableForm()
        {
            //Can only pick from animals that the CurrentUser can edit
            DeployButton.Enabled = ProjectComboBox.SelectedItem != null && AnimalComboBox.SelectedItem != null && CollarAndAnimalFreeOnDate(DeployDateTimePicker.Value);
        }

        private bool CollarAndAnimalFreeOnDate(DateTime dateTime)
        {
            var animal = (Animal)AnimalComboBox.SelectedItem;
            return !Database.CollarDeployments.Any(d => d.DeploymentDate < dateTime && dateTime < d.RetrievalDate && (d.Animal == animal || d.Collar == Collar));
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var animal = (Animal)AnimalComboBox.SelectedItem;
            var deployment = new CollarDeployment
                                    {
                                        //Using the objects in this cases causes a primary key problem in the dbml code
                                        AnimalId = animal.AnimalId,
                                        ProjectId = animal.ProjectId,
                                        CollarId = Collar.CollarId,
                                        CollarManufacturer = Collar.CollarManufacturer,
                                        DeploymentDate = DeployDateTimePicker.Value.Date
                                    };
            Database.CollarDeployments.InsertOnSubmit(deployment);
        }
    }
}
