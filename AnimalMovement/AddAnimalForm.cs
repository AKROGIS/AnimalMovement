﻿using System;
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
            Project = project;
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            SetupForm();
        }

        private void SetupForm()
        {
            InitializeComponent();
            RestoreWindow();
            LoadDataContext();
            EnableControls();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
            //Project is in a different DataContext, get one in this DataContext
            if (Project != null)
                Project = Database.Projects.FirstOrDefault(p => p.ProjectId == Project.ProjectId);   
            if (Project == null)
                throw new InvalidOperationException("Add Animal Form not provided a valid Project.");

            Functions = new AnimalMovementFunctions();
            IsEditor = Functions.IsProjectEditor(Project.ProjectId, CurrentUser) ?? false;
            SetUpControls();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var now = DateTime.Now;
            MortatlityDateTimePicker.Value = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0);
            MortatlityDateTimePicker.Checked = false;
            MortatlityDateTimePicker.CustomFormat = " ";
        }

        private void SetUpControls()
        {
            ProjectCodeTextBox.Text = Project.ProjectId;
            AnimalIdTextBox.Text = Functions.NextAnimalId(Project.ProjectId);
            GenderComboBox.DataSource = Database.LookupGenders;
            GenderComboBox.DisplayMember = "Sex";
            GenderComboBox.SelectedIndex = 0;
            SpeciesComboBox.DataSource = Database.LookupSpecies;
            SpeciesComboBox.DisplayMember = "Species";
            SelectDefaultSpecies(Settings.GetDefaultSpecies());
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
            DialogResult = DialogResult.OK; //Closes form
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

    }
}
