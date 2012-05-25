using System.Windows.Forms;
using AnimalMovementLibrary;

namespace TestUI
{
    public partial class InvestigatorDetailsForm : Form
    {
        public InvestigatorDetailsForm(string investigatorId)
        {
            InitializeComponent();
            var investigator = Investigator.FromName(investigatorId); 
            LoginTextBox.Text = investigator.Login;
            NameTextBox.Text = investigator.Name;
            EmailTextBox.Text = investigator.Email;
            PhoneTextBox.Text = investigator.Phone;
        }
    }
}
