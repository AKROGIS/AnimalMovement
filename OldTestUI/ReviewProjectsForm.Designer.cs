namespace TestUI
{
    partial class ReviewProjectsForm
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
            this.InfoProjectButton = new System.Windows.Forms.Button();
            this.DeleteProjectButton = new System.Windows.Forms.Button();
            this.AddProjectButton = new System.Windows.Forms.Button();
            this.ShowHideButton = new System.Windows.Forms.Button();
            this.ProjectsGridView = new System.Windows.Forms.DataGridView();
            this.allProjectsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.animal_MovementDataSet = new TestUI.Animal_MovementDataSet();
            this.myProjectsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ProjectsTableAdapter = new TestUI.Animal_MovementDataSetTableAdapters.AllProjectsTableAdapter();
            this.tableAdapterManager = new TestUI.Animal_MovementDataSetTableAdapters.TableAdapterManager();
            this.myProjectsTableAdapter = new TestUI.Animal_MovementDataSetTableAdapters.MyProjectsTableAdapter();
            this.dataGridViewColumnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewColumnProjectLead = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewColumnUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewColumnDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ProjectsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.allProjectsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.animal_MovementDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.myProjectsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // InfoProjectButton
            // 
            this.InfoProjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InfoProjectButton.FlatAppearance.BorderSize = 0;
            this.InfoProjectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoProjectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoProjectButton.Image = global::TestUI.Properties.Resources.GenericInformation_B_16;
            this.InfoProjectButton.Location = new System.Drawing.Point(64, 226);
            this.InfoProjectButton.Name = "InfoProjectButton";
            this.InfoProjectButton.Size = new System.Drawing.Size(24, 24);
            this.InfoProjectButton.TabIndex = 25;
            this.InfoProjectButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.InfoProjectButton.UseVisualStyleBackColor = true;
            this.InfoProjectButton.Click += new System.EventHandler(this.InfoProjectButton_Click);
            // 
            // DeleteProjectButton
            // 
            this.DeleteProjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteProjectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteProjectButton.Image = global::TestUI.Properties.Resources.GenericDeleteRed16;
            this.DeleteProjectButton.Location = new System.Drawing.Point(39, 226);
            this.DeleteProjectButton.Name = "DeleteProjectButton";
            this.DeleteProjectButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteProjectButton.TabIndex = 24;
            this.DeleteProjectButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.DeleteProjectButton.UseVisualStyleBackColor = true;
            this.DeleteProjectButton.Click += new System.EventHandler(this.DeleteProjectButton_Click);
            // 
            // AddProjectButton
            // 
            this.AddProjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddProjectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddProjectButton.Image = global::TestUI.Properties.Resources.GenericAddGreen16;
            this.AddProjectButton.Location = new System.Drawing.Point(12, 226);
            this.AddProjectButton.Margin = new System.Windows.Forms.Padding(0);
            this.AddProjectButton.Name = "AddProjectButton";
            this.AddProjectButton.Size = new System.Drawing.Size(24, 24);
            this.AddProjectButton.TabIndex = 23;
            this.AddProjectButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.AddProjectButton.UseVisualStyleBackColor = true;
            this.AddProjectButton.Click += new System.EventHandler(this.AddProjectButton_Click);
            // 
            // ShowHideButton
            // 
            this.ShowHideButton.Location = new System.Drawing.Point(12, 13);
            this.ShowHideButton.Name = "ShowHideButton";
            this.ShowHideButton.Size = new System.Drawing.Size(142, 23);
            this.ShowHideButton.TabIndex = 26;
            this.ShowHideButton.Text = "Show Only My Projects";
            this.ShowHideButton.UseVisualStyleBackColor = true;
            this.ShowHideButton.Click += new System.EventHandler(this.ShowHideButton_Click);
            // 
            // ProjectsGridView
            // 
            this.ProjectsGridView.AllowUserToAddRows = false;
            this.ProjectsGridView.AllowUserToDeleteRows = false;
            this.ProjectsGridView.AllowUserToOrderColumns = true;
            this.ProjectsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectsGridView.AutoGenerateColumns = false;
            this.ProjectsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProjectsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewColumnCode,
            this.dataGridViewColumnName,
            this.dataGridViewColumnProjectLead,
            this.dataGridViewColumnUnit,
            this.dataGridViewColumnDescription});
            this.ProjectsGridView.DataSource = this.allProjectsBindingSource;
            this.ProjectsGridView.Location = new System.Drawing.Point(12, 42);
            this.ProjectsGridView.Name = "ProjectsGridView";
            this.ProjectsGridView.ReadOnly = true;
            this.ProjectsGridView.Size = new System.Drawing.Size(598, 178);
            this.ProjectsGridView.TabIndex = 27;
            // 
            // allProjectsBindingSource
            // 
            this.allProjectsBindingSource.DataMember = "AllProjects";
            this.allProjectsBindingSource.DataSource = this.animal_MovementDataSet;
            // 
            // animal_MovementDataSet
            // 
            this.animal_MovementDataSet.DataSetName = "Animal_MovementDataSet";
            this.animal_MovementDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // myProjectsBindingSource
            // 
            this.myProjectsBindingSource.DataMember = "MyProjects";
            this.myProjectsBindingSource.DataSource = this.animal_MovementDataSet;
            // 
            // ProjectsTableAdapter
            // 
            this.ProjectsTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AnimalsTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CollarDeploymentsTableAdapter = null;
            this.tableAdapterManager.CollarManufacturersTableAdapter = null;
            this.tableAdapterManager.CollarsTableAdapter = null;
            this.tableAdapterManager.Connection = null;
            this.tableAdapterManager.ProjectEditorsTableAdapter = null;
            this.tableAdapterManager.ProjectInvestigatorsTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = TestUI.Animal_MovementDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // myProjectsTableAdapter
            // 
            this.myProjectsTableAdapter.ClearBeforeFill = true;
            // 
            // dataGridViewColumnCode
            // 
            this.dataGridViewColumnCode.DataPropertyName = "Id";
            this.dataGridViewColumnCode.HeaderText = "Code";
            this.dataGridViewColumnCode.Name = "dataGridViewColumnCode";
            this.dataGridViewColumnCode.ReadOnly = true;
            this.dataGridViewColumnCode.Width = 67;
            // 
            // dataGridViewColumnName
            // 
            this.dataGridViewColumnName.DataPropertyName = "Name";
            this.dataGridViewColumnName.HeaderText = "Name";
            this.dataGridViewColumnName.Name = "dataGridViewColumnName";
            this.dataGridViewColumnName.ReadOnly = true;
            this.dataGridViewColumnName.Width = 150;
            // 
            // dataGridViewColumnProjectLead
            // 
            this.dataGridViewColumnProjectLead.DataPropertyName = "Investigator";
            this.dataGridViewColumnProjectLead.HeaderText = "Project Lead";
            this.dataGridViewColumnProjectLead.Name = "dataGridViewColumnProjectLead";
            this.dataGridViewColumnProjectLead.ReadOnly = true;
            this.dataGridViewColumnProjectLead.Width = 120;
            // 
            // dataGridViewColumnUnit
            // 
            this.dataGridViewColumnUnit.DataPropertyName = "Unit";
            this.dataGridViewColumnUnit.HeaderText = "Unit";
            this.dataGridViewColumnUnit.Name = "dataGridViewColumnUnit";
            this.dataGridViewColumnUnit.ReadOnly = true;
            this.dataGridViewColumnUnit.Width = 60;
            // 
            // dataGridViewColumnDescription
            // 
            this.dataGridViewColumnDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewColumnDescription.DataPropertyName = "Description";
            this.dataGridViewColumnDescription.HeaderText = "Description";
            this.dataGridViewColumnDescription.Name = "dataGridViewColumnDescription";
            this.dataGridViewColumnDescription.ReadOnly = true;
            // 
            // ReviewProjectsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 262);
            this.Controls.Add(this.ProjectsGridView);
            this.Controls.Add(this.ShowHideButton);
            this.Controls.Add(this.InfoProjectButton);
            this.Controls.Add(this.DeleteProjectButton);
            this.Controls.Add(this.AddProjectButton);
            this.MinimumSize = new System.Drawing.Size(295, 193);
            this.Name = "ReviewProjectsForm";
            this.Text = "Animal Movement Projects";
            this.Load += new System.EventHandler(this.ReviewProjectsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ProjectsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.allProjectsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.animal_MovementDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.myProjectsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button InfoProjectButton;
        private System.Windows.Forms.Button DeleteProjectButton;
        private System.Windows.Forms.Button AddProjectButton;
        private System.Windows.Forms.Button ShowHideButton;
        private System.Windows.Forms.DataGridView ProjectsGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn principalInvestigatorDataGridViewTextBoxColumn;
        private Animal_MovementDataSetTableAdapters.AllProjectsTableAdapter ProjectsTableAdapter;
        private Animal_MovementDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingSource allProjectsBindingSource;
        private Animal_MovementDataSet animal_MovementDataSet;
        private System.Windows.Forms.BindingSource myProjectsBindingSource;
        private Animal_MovementDataSetTableAdapters.MyProjectsTableAdapter myProjectsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewColumnCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewColumnProjectLead;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewColumnUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewColumnDescription;
    }
}