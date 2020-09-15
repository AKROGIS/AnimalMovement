using DataModel;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AnimalMovement
{
    internal partial class InvestigatorsForm : BaseForm
    {
        private AnimalMovementDataContext Database { get; set; }
        private string CurrentUser { get; set; }
        internal event EventHandler DatabaseChanged;

        internal InvestigatorsForm()
        {
            InitializeComponent();
            RestoreWindow();
            CurrentUser = Environment.UserDomainName + @"\" + Environment.UserName;
            LoadDataContext();
            SetUpForm();
        }

        private void LoadDataContext()
        {
            Database = new AnimalMovementDataContext();
            //Database.Log = Console.Out;
        }

        private void SetUpForm()
        {
            InvestigatorsGridView.DataSource = Database.ProjectInvestigators;
            InvestigatorsGridView.Columns[0].Visible = false;
            InvestigatorsGridView.Sort(InvestigatorsGridView.Columns[1], ListSortDirection.Ascending);
            SelectInvestigatorRow(CurrentUser);
            EnableControls();
        }

        private void EnableControls()
        {
            InfoInvestigatorButton.Enabled = InvestigatorsGridView.SelectedRows.Count == 1;
        }

        private void OnDatabaseChanged()
        {
            DatabaseChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ProjectInvestigatorDataChanged()
        {
            OnDatabaseChanged();
            LoadDataContext();
            SetUpForm();
        }

        private void InfoInvestigatorButton_Click(object sender, EventArgs e)
        {
            if (InvestigatorsGridView.CurrentRow == null)
                return; //This buttton is only enabled when Current Row is not not null
            var pi = (ProjectInvestigator)InvestigatorsGridView.CurrentRow.DataBoundItem;
            var form = new InvestigatorDetailsForm(pi);
            form.DatabaseChanged += (o, args) =>
                {
                    ProjectInvestigatorDataChanged();
                    SelectInvestigatorRow(pi.Login);
                };
            form.Show(this);
        }

        private void SelectInvestigatorRow(string login)
        {
            foreach (DataGridViewRow row in InvestigatorsGridView.Rows)
                if (((ProjectInvestigator)row.DataBoundItem).Login == login)
                    InvestigatorsGridView.CurrentCell = row.Cells[1];
        }

        private void InvestigatorsGridView_SelectionChanged(object sender, EventArgs e)
        {
            InfoInvestigatorButton.Enabled = InvestigatorsGridView.SelectedRows.Count == 1;
        }

        private void InvestigatorsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && InfoInvestigatorButton.Enabled)
                InfoInvestigatorButton_Click(sender, e);
        }
    }
}
