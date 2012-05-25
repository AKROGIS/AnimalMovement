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
    public partial class AnimalDetailsForm : Form
    {
        public AnimalDetailsForm(string project, string animal)
        {
            InitializeComponent();
            Project = project;
            Animal = animal;
        }

        public string Project { get; set; }
        public string Animal { get; set; }


    }
}
