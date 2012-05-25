using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestUI
{
    public partial class DeploymentsForm : Form
    {
        public DeploymentsForm()
        {
            InitializeComponent();
        }

        public string CurrentUser { get; set; }
        public string Project { get; set; }

        private void DeploymentsForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'animal_MovementDataSet.Collars' table. You can move, or remove it, as needed.
            this.collarsTableAdapter.Fill(this.animal_MovementDataSet.Collars);
            // TODO: This line of code loads data into the 'animal_MovementDataSet.Animals' table. You can move, or remove it, as needed.
            this.animalsTableAdapter.Fill(this.animal_MovementDataSet.Animals);
            // TODO: This line of code loads data into the 'animal_MovementDataSet.CollarManufacturers' table. You can move, or remove it, as needed.
            this.collarManufacturersTableAdapter.Fill(this.animal_MovementDataSet.CollarManufacturers);
            // TODO: This line of code loads data into the 'animal_MovementDataSet.CollarDeployments' table. You can move, or remove it, as needed.
            this.collarDeploymentsTableAdapter.Fill(this.animal_MovementDataSet.CollarDeployments);
            // TODO: This line of code loads data into the 'animal_MovementDataSet.Projects' table. You can move, or remove it, as needed.
            this.projectsTableAdapter.Fill(this.animal_MovementDataSet.Projects);

        }
    }
}
