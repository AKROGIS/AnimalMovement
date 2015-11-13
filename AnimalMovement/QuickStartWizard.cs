using System;
using System.Linq;
using System.Windows.Forms;
using DataModel;

namespace AnimalMovement
{
    internal partial class QuickStartWizard : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        private int PageNumber { get; set; }
        private ProjectInvestigator Investigator { get; set; }

        public QuickStartWizard()
        {
            InitializeComponent();
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            PageNumber = 0;
            SetUpPage();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            Investigator = Database.ProjectInvestigators.FirstOrDefault(pi => pi.Login == CurrentUser);
        }

        private void SetUpPage()
        {
            switch (PageNumber)
            {
                case 0:
                    SetUpInvestigatorPage();
                    break;
                case 1: //Argos Program Page
                    if (Investigator == null)
                    {
                        MessageBox.Show("You must select a Project Investigator before proceeding");
                        PageNumber--;
                        break;
                    }
                    CloseInvestigatorPage();
                    SetUpDeploymentsPage();
                    //SetUpArgosProgramPage();
                    break;
                case 2:  //Load Argos Platforms
                    SetUpArgosPlatformPage();
                    break;
                case 3:  //Load TPF Files
                    SetUpTpfFilesPage();
                    break;
                case 4:  //Create A Project
                    SetUpProjectPage();
                    break;
                case 5:  //Create Deployments (Animals and Collars)
                    SetUpDeploymentsPage();
                    break;
                case 6:  //Load Collar Files
                    SetUpLoadFilesPage();
                    break;
                case 7:  //Finished
                    SetUpDonePage();
                    break;
            }
        }

        private void SetUpInvestigatorPage()
        {
            InvestigatorComboBox.DataSource =
                Database.ProjectInvestigators.Where(pi =>
                                                    pi.Login == CurrentUser ||
                                                    pi.ProjectInvestigatorAssistants.Any(a => a.Assistant == CurrentUser));
            InvestigatorComboBox.DisplayMember = "Name";
            if (Investigator == null && InvestigatorComboBox.Items.Count == 1)
                Investigator = (ProjectInvestigator)InvestigatorComboBox.Items[0];
            InvestigatorComboBox.SelectedItem = Investigator;

            if (Investigator == null && InvestigatorComboBox.Items.Count == 0)
            {
                NextButton.Text = "Finish";
                var dbaContact = Settings.GetSystemDbaContact();
                InstructionsRichTextBox.Text =
                    "You must be a Project Investigator (PI), or an assistant for a PI to use this wizard.  " +
                    "A PI manages collars and projects that track animal movements.  " +
                    "A PI can give other people permission to act on their behalf as an assistant.  " +
                    "If you want to be an assistant, then contact the PI.  " +
                    "If you want to be a PI, then you must contact " +
                    (String.IsNullOrEmpty(dbaContact) ? "the database administrator" : dbaContact) + ".";
            }

            const string text = "This wizard will help you create animals, collars, and the link between them." +
                                "You can also setup the automatic download of Argos data, as well as load existing collar data. ";

            if (Investigator == null && InvestigatorComboBox.Items.Count > 1)
            {
                InstructionsRichTextBox.Text = text +
                    "You must first select the Project Investigator (PI) you are working for today " +
                    "then click the Next button to proceed.";
            }
            if (Investigator != null && InvestigatorComboBox.Items.Count > 1)
            {
                InstructionsRichTextBox.Text =
                    "You can do this for yourself, or one of the other Project Investigators you can assist." +
                    "First select the correct Project Investigator then click the Next button to proceed.";
            }
            if (InvestigatorComboBox.Items.Count == 1)
            {
                InstructionsRichTextBox.Text = text +
                    "Click the Next button to proceed.";
            }
        }

        private void CloseInvestigatorPage()
        {
            InvestigatorComboBox.Enabled = false;
            InstructionsRichTextBox.Text = null;
        }

        //TODO: Implement Create a Argos Program Wizard
        // ReSharper disable once UnusedMember.Local
        private void SetUpArgosProgramPage()
        {
            InstructionsRichTextBox.Text = "Create a Argos Program";
            NextButton.Click -= NextButton_Click;
            NextButton.Click += AddProgram_Click;
       }

        private void AddProgram_Click(object sender, EventArgs e)
        {
            var form = new AddArgosProgramForm(Investigator);
            NextButton.Enabled = false;
            form.DatabaseChanged += (o, a) =>
            {
                NextButton.Click -= AddProgram_Click;
                NextButton.Click += NextButton_Click;
            };
            form.ShowDialog();
            NextButton.Enabled = true;
        }

        private void SetUpArgosPlatformPage()
        {
            InstructionsRichTextBox.Text = "Get the Platforms (Argos Ids) belonging to the Argos Program";
            NextButton.Click -= NextButton_Click;
            NextButton.Click += AddPlatform_Click;
        }

        private void AddPlatform_Click(object sender, EventArgs e)
        {
            //FIXME - how do I get the new program??
            var form = new ArgosProgramDetailsForm(Investigator.ArgosPrograms.FirstOrDefault());
            NextButton.Enabled = false;
            form.DatabaseChanged += (o, a) =>
            {
                NextButton.Click -= AddPlatform_Click;
                NextButton.Click += NextButton_Click;
            };
            form.ShowDialog();
            NextButton.Enabled = true;
        }

        private void SetUpTpfFilesPage()
        {
            InstructionsRichTextBox.Text = "Load the TPF files for all you Gen4 Collars." +
                "This will created the collars, match them to the Argos Platforms, " +
                "And link the parameter file to the collars. " +
                "If you have Gen 3 collars, this wizard can't help you.";
            NextButton.Click -= NextButton_Click;
            NextButton.Click += AddTpfFiles_Click;
        }

        private void AddTpfFiles_Click(object sender, EventArgs e)
        {
            var form = new AddCollarParameterFileForm(Investigator);
            NextButton.Enabled = false;
            form.DatabaseChanged += (o, a) =>
            {
                NextButton.Click -= AddTpfFiles_Click;
                NextButton.Click += NextButton_Click;
            };
            form.ShowDialog();
            NextButton.Enabled = true;
        }


        private void SetUpProjectPage()
        {
            InstructionsRichTextBox.Text = "Create a Project";
            NextButton.Click -= NextButton_Click;
            NextButton.Click += AddProject_Click;
        }

        private void AddProject_Click(object sender, EventArgs e)
        {
            //TODO Fix AddProjectForm to take a PI
            var form = new AddProjectForm(); //Investigator);
            NextButton.Enabled = false;
            form.DatabaseChanged += (o, a) =>
            {
                NextButton.Click -= AddProject_Click;
                NextButton.Click += NextButton_Click;
            };
            form.ShowDialog();
            NextButton.Enabled = true;
        }

        private void SetUpDeploymentsPage()
        {
            InstructionsRichTextBox.Text = "Load a deployments table as CSV file (exported from excel spreadsheet)" +
                "See the help for the format of this spreadsheet.";
            NextButton.Click -= NextButton_Click;
            NextButton.Click += AddDeployments_Click;
        }

        private void AddDeployments_Click(object sender, EventArgs e)
        {
            var form = new AddDeploymentCsvForm(Investigator);
            NextButton.Enabled = false;
            form.DatabaseChanged += (o, a) =>
            {
                NextButton.Click -= AddDeployments_Click;
                NextButton.Click += NextButton_Click;
            };
            form.ShowDialog();
            NextButton.Enabled = true;
        }

        private void SetUpLoadFilesPage()
        {
            InstructionsRichTextBox.Text = "Load files to the project. Describe the files...";
            NextButton.Click -= NextButton_Click;
            NextButton.Click += AddCollarFiles_Click;
        }

        private void AddCollarFiles_Click(object sender, EventArgs e)
        {
            //Fixme - get the new project
            var form = new UploadFilesForm(); //Project);
            NextButton.Enabled = false;
            form.DatabaseChanged += (o, a) =>
            {
                NextButton.Click -= AddCollarFiles_Click;
                NextButton.Click += NextButton_Click;
            };
            form.ShowDialog();
            NextButton.Enabled = true;
        }


        private void SetUpDonePage()
        {
            InstructionsRichTextBox.Text = "Congratulations...";
            NextButton.Text = "Finish";
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (NextButton.Text == "Finish")
                Close();
            else
            {
                PageNumber++;
                SetUpPage();
            }
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void InvestigatorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Visible)
                Investigator = InvestigatorComboBox.SelectedItem as ProjectInvestigator;
            NextButton.Enabled = Investigator != null;
        }
    }
}
