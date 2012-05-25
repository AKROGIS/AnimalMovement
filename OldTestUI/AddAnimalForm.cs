using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AnimalMovementLibrary;

namespace TestUI
{
    public partial class AddAnimalForm : Form
    {
        public AddAnimalForm(string project)
        {
            InitializeComponent();
            Project = project;
            ProjectCodeTextBox.Text = project;
            AnimalIdTextBox.Text = GetDefaultId(project);
            GenderComboBox.SelectedIndex = 0;
            SpeciesComboBox.SelectedIndex = 0;
        }

        public string Project { get; set; }

        private static string GetDefaultId(string project)
        {
            return AML.GetNextAnimalId(project);
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            try
            {
                AML.AddAnimal(ProjectCodeTextBox.Text, AnimalIdTextBox.Text, SpeciesComboBox.Text,
                               GenderComboBox.Text, GroupTextBox.Text, DescriptionTextBox.Text);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable To Create Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }



    }
}
