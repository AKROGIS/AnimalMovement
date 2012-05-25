namespace TestUI
{
    partial class ReviewProjectsForm2
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
            this.columnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLead = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.InfoProjectButton.Location = new System.Drawing.Point(64, 215);
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
            this.DeleteProjectButton.Location = new System.Drawing.Point(39, 215);
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
            this.AddProjectButton.Location = new System.Drawing.Point(12, 215);
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
            this.ProjectsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProjectsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnCode,
            this.columnName,
            this.columnLead,
            this.columnUnit,
            this.columnDescription,
            this.columnProject});
            this.ProjectsGridView.Location = new System.Drawing.Point(12, 42);
            this.ProjectsGridView.Name = "ProjectsGridView";
            this.ProjectsGridView.ReadOnly = true;
            this.ProjectsGridView.Size = new System.Drawing.Size(578, 167);
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
            // columnCode
            // 
            this.columnCode.DataPropertyName = "Code";
            this.columnCode.HeaderText = "Code";
            this.columnCode.Name = "columnCode";
            this.columnCode.ReadOnly = true;
            this.columnCode.Width = 60;
            // 
            // columnName
            // 
            this.columnName.DataPropertyName = "Name";
            this.columnName.HeaderText = "Name";
            this.columnName.Name = "columnName";
            this.columnName.ReadOnly = true;
            this.columnName.Width = 150;
            // 
            // columnLead
            // 
            this.columnLead.DataPropertyName = "Lead";
            this.columnLead.HeaderText = "Lead";
            this.columnLead.Name = "columnLead";
            this.columnLead.ReadOnly = true;
            this.columnLead.Width = 120;
            // 
            // columnUnit
            // 
            this.columnUnit.DataPropertyName = "Unit";
            this.columnUnit.HeaderText = "Unit";
            this.columnUnit.Name = "columnUnit";
            this.columnUnit.ReadOnly = true;
            this.columnUnit.Width = 40;
            // 
            // columnDescription
            // 
            this.columnDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnDescription.DataPropertyName = "Description";
            this.columnDescription.HeaderText = "Description";
            this.columnDescription.Name = "columnDescription";
            this.columnDescription.ReadOnly = true;
            // 
            // columnProject
            // 
            this.columnProject.DataPropertyName = "Project";
            this.columnProject.HeaderText = "Project";
            this.columnProject.Name = "columnProject";
            this.columnProject.ReadOnly = true;
            this.columnProject.Visible = false;
            // 
            // ReviewProjectsForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 251);
            this.Controls.Add(this.ProjectsGridView);
            this.Controls.Add(this.ShowHideButton);
            this.Controls.Add(this.InfoProjectButton);
            this.Controls.Add(this.DeleteProjectButton);
            this.Controls.Add(this.AddProjectButton);
            this.MinimumSize = new System.Drawing.Size(295, 193);
            this.Name = "ReviewProjectsForm2";
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
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewColumnProjectLead;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewColumnUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLead;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnUnit;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnProject;
    }
}