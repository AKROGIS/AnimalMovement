namespace TestUI
{
    partial class DeploymentsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.projectsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.animalMovementDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.animal_MovementDataSet = new TestUI.Animal_MovementDataSet();
            this.projectsTableAdapter = new TestUI.Animal_MovementDataSetTableAdapters.ProjectsTableAdapter();
            this.collarDeploymentsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.collarDeploymentsTableAdapter = new TestUI.Animal_MovementDataSetTableAdapters.CollarDeploymentsTableAdapter();
            this.collarManufacturersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.collarManufacturersTableAdapter = new TestUI.Animal_MovementDataSetTableAdapters.CollarManufacturersTableAdapter();
            this.projectsBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.animalsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.animalsTableAdapter = new TestUI.Animal_MovementDataSetTableAdapters.AnimalsTableAdapter();
            this.collarsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.collarsTableAdapter = new TestUI.Animal_MovementDataSetTableAdapters.CollarsTableAdapter();
            this.projectIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.AnimalId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.CollarManufacturer = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.CollarId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.DeploymentDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RetrievalDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.animalMovementDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.animal_MovementDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collarDeploymentsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collarManufacturersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectsBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.animalsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collarsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.projectIdDataGridViewTextBoxColumn,
            this.AnimalId,
            this.CollarManufacturer,
            this.CollarId,
            this.DeploymentDate,
            this.RetrievalDate});
            this.dataGridView1.DataSource = this.collarDeploymentsBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(12, 42);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(678, 330);
            this.dataGridView1.TabIndex = 0;
            // 
            // projectsBindingSource
            // 
            this.projectsBindingSource.DataMember = "Projects";
            this.projectsBindingSource.DataSource = this.animalMovementDataSetBindingSource;
            // 
            // animalMovementDataSetBindingSource
            // 
            this.animalMovementDataSetBindingSource.DataSource = this.animal_MovementDataSet;
            this.animalMovementDataSetBindingSource.Position = 0;
            // 
            // animal_MovementDataSet
            // 
            this.animal_MovementDataSet.DataSetName = "Animal_MovementDataSet";
            this.animal_MovementDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // projectsTableAdapter
            // 
            this.projectsTableAdapter.ClearBeforeFill = true;
            // 
            // collarDeploymentsBindingSource
            // 
            this.collarDeploymentsBindingSource.DataMember = "CollarDeployments";
            this.collarDeploymentsBindingSource.DataSource = this.animalMovementDataSetBindingSource;
            // 
            // collarDeploymentsTableAdapter
            // 
            this.collarDeploymentsTableAdapter.ClearBeforeFill = true;
            // 
            // collarManufacturersBindingSource
            // 
            this.collarManufacturersBindingSource.DataMember = "CollarManufacturers";
            this.collarManufacturersBindingSource.DataSource = this.animalMovementDataSetBindingSource;
            // 
            // collarManufacturersTableAdapter
            // 
            this.collarManufacturersTableAdapter.ClearBeforeFill = true;
            // 
            // projectsBindingSource1
            // 
            this.projectsBindingSource1.DataMember = "Projects";
            this.projectsBindingSource1.DataSource = this.animalMovementDataSetBindingSource;
            // 
            // animalsBindingSource
            // 
            this.animalsBindingSource.DataMember = "Animals";
            this.animalsBindingSource.DataSource = this.animalMovementDataSetBindingSource;
            // 
            // animalsTableAdapter
            // 
            this.animalsTableAdapter.ClearBeforeFill = true;
            // 
            // collarsBindingSource
            // 
            this.collarsBindingSource.DataMember = "Collars";
            this.collarsBindingSource.DataSource = this.animalMovementDataSetBindingSource;
            // 
            // collarsTableAdapter
            // 
            this.collarsTableAdapter.ClearBeforeFill = true;
            // 
            // projectIdDataGridViewTextBoxColumn
            // 
            this.projectIdDataGridViewTextBoxColumn.DataPropertyName = "ProjectId";
            this.projectIdDataGridViewTextBoxColumn.DataSource = this.projectsBindingSource1;
            this.projectIdDataGridViewTextBoxColumn.DisplayMember = "ProjectName";
            this.projectIdDataGridViewTextBoxColumn.HeaderText = "ProjectId";
            this.projectIdDataGridViewTextBoxColumn.Name = "projectIdDataGridViewTextBoxColumn";
            this.projectIdDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.projectIdDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.projectIdDataGridViewTextBoxColumn.ValueMember = "ProjectId";
            // 
            // AnimalId
            // 
            this.AnimalId.DataPropertyName = "AnimalId";
            this.AnimalId.DataSource = this.animalsBindingSource;
            this.AnimalId.DisplayMember = "AnimalId";
            this.AnimalId.HeaderText = "AnimalId";
            this.AnimalId.Name = "AnimalId";
            this.AnimalId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.AnimalId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.AnimalId.ValueMember = "AnimalId";
            // 
            // CollarManufacturer
            // 
            this.CollarManufacturer.DataPropertyName = "CollarManufacturer";
            this.CollarManufacturer.DataSource = this.collarManufacturersBindingSource;
            this.CollarManufacturer.DisplayMember = "Name";
            this.CollarManufacturer.HeaderText = "CollarManufacturer";
            this.CollarManufacturer.Name = "CollarManufacturer";
            this.CollarManufacturer.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CollarManufacturer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CollarManufacturer.ValueMember = "CollarManufacturer";
            // 
            // CollarId
            // 
            this.CollarId.DataPropertyName = "CollarId";
            this.CollarId.DataSource = this.collarsBindingSource;
            this.CollarId.DisplayMember = "CollarId";
            this.CollarId.HeaderText = "CollarId";
            this.CollarId.Name = "CollarId";
            this.CollarId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CollarId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CollarId.ValueMember = "CollarId";
            // 
            // DeploymentDate
            // 
            this.DeploymentDate.DataPropertyName = "DeploymentDate";
            this.DeploymentDate.HeaderText = "DeploymentDate";
            this.DeploymentDate.Name = "DeploymentDate";
            // 
            // RetrievalDate
            // 
            this.RetrievalDate.DataPropertyName = "RetrievalDate";
            this.RetrievalDate.HeaderText = "RetrievalDate";
            this.RetrievalDate.Name = "RetrievalDate";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Show All",
            "My Projects"});
            this.comboBox1.Location = new System.Drawing.Point(65, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Filter By:";
            // 
            // DeploymentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 384);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "DeploymentsForm";
            this.Text = "DeploymentsForm";
            this.Load += new System.EventHandler(this.DeploymentsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.animalMovementDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.animal_MovementDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collarDeploymentsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collarManufacturersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectsBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.animalsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collarsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource animalMovementDataSetBindingSource;
        private Animal_MovementDataSet animal_MovementDataSet;
        private System.Windows.Forms.BindingSource projectsBindingSource;
        private Animal_MovementDataSetTableAdapters.ProjectsTableAdapter projectsTableAdapter;
        private System.Windows.Forms.BindingSource collarDeploymentsBindingSource;
        private Animal_MovementDataSetTableAdapters.CollarDeploymentsTableAdapter collarDeploymentsTableAdapter;
        private System.Windows.Forms.BindingSource collarManufacturersBindingSource;
        private Animal_MovementDataSetTableAdapters.CollarManufacturersTableAdapter collarManufacturersTableAdapter;
        private System.Windows.Forms.BindingSource projectsBindingSource1;
        private System.Windows.Forms.BindingSource animalsBindingSource;
        private Animal_MovementDataSetTableAdapters.AnimalsTableAdapter animalsTableAdapter;
        private System.Windows.Forms.BindingSource collarsBindingSource;
        private Animal_MovementDataSetTableAdapters.CollarsTableAdapter collarsTableAdapter;
        private System.Windows.Forms.DataGridViewComboBoxColumn projectIdDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn AnimalId;
        private System.Windows.Forms.DataGridViewComboBoxColumn CollarManufacturer;
        private System.Windows.Forms.DataGridViewComboBoxColumn CollarId;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeploymentDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn RetrievalDate;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
    }
}