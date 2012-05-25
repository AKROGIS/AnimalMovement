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
    public partial class FileDetailsForm : Form
    {
        public FileDetailsForm(int fileId)
        {
            InitializeComponent();
            FileId = fileId;
        }

        public int FileId { get; set; }
    }
}
