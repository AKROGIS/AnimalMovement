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
    public partial class FilesForm : Form
    {
        public FilesForm()
        {
            InitializeComponent();
        }

        public string CurrentUser { get; set; }
        public string Project { get; set; }

    }
}
