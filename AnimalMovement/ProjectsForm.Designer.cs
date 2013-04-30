namespace AnimalMovement
{
    partial class ProjectsForm
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lead = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CanDelete = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Project = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ProjectsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // InfoProjectButton
            // 
            this.InfoProjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InfoProjectButton.FlatAppearance.BorderSize = 0;
            this.InfoProjectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoProjectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoProjectButton.Image = global::AnimalMovement.Properties.Resources.GenericInformation_B_16;
            this.InfoProjectButton.Location = new System.Drawing.Point(64, 215);
            this.InfoProjectButton.Name = "InfoProjectButton";
            this.InfoProjectButton.Size = new System.Drawing.Size(24, 24);
            this.InfoProjectButton.TabIndex = 5;
            this.InfoProjectButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.InfoProjectButton.UseVisualStyleBackColor = true;
            this.InfoProjectButton.Click += new System.EventHandler(this.InfoProjectButton_Click);
            // 
            // DeleteProjectButton
            // 
            this.DeleteProjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteProjectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteProjectButton.Image = global::AnimalMovement.Properties.Resources.GenericDeleteRed16;
            this.DeleteProjectButton.Location = new System.Drawing.Point(39, 215);
            this.DeleteProjectButton.Name = "DeleteProjectButton";
            this.DeleteProjectButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteProjectButton.TabIndex = 4;
            this.DeleteProjectButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.DeleteProjectButton, "To delete a project, it must have no animals or files,\r\nand you must be the proje" +
        "ct investigator.");
            this.DeleteProjectButton.UseVisualStyleBackColor = true;
            this.DeleteProjectButton.Click += new System.EventHandler(this.DeleteProjectButton_Click);
            // 
            // AddProjectButton
            // 
            this.AddProjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddProjectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddProjectButton.Image = global::AnimalMovement.Properties.Resources.GenericAddGreen16;
            this.AddProjectButton.Location = new System.Drawing.Point(12, 215);
            this.AddProjectButton.Margin = new System.Windows.Forms.Padding(0);
            this.AddProjectButton.Name = "AddProjectButton";
            this.AddProjectButton.Size = new System.Drawing.Size(24, 24);
            this.AddProjectButton.TabIndex = 3;
            this.AddProjectButton.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.toolTip1.SetToolTip(this.AddProjectButton, "To create a project you must be in the list of\r\nproject investigators.  Contact t" +
        "he database\r\nadministrator to get added to that list.");
            this.AddProjectButton.UseVisualStyleBackColor = true;
            this.AddProjectButton.Click += new System.EventHandler(this.AddProjectButton_Click);
            // 
            // ShowHideButton
            // 
            this.ShowHideButton.Location = new System.Drawing.Point(12, 13);
            this.ShowHideButton.Name = "ShowHideButton";
            this.ShowHideButton.Size = new System.Drawing.Size(142, 23);
            this.ShowHideButton.TabIndex = 1;
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
            this.ProjectName,
            this.Lead,
            this.UnitCode,
            this.Description,
            this.ProjectId,
            this.CanDelete,
            this.Project});
            this.ProjectsGridView.Location = new System.Drawing.Point(12, 42);
            this.ProjectsGridView.Name = "ProjectsGridView";
            this.ProjectsGridView.ReadOnly = true;
            this.ProjectsGridView.RowHeadersWidth = 25;
            this.ProjectsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ProjectsGridView.Size = new System.Drawing.Size(578, 167);
            this.ProjectsGridView.TabIndex = 2;
            this.ProjectsGridView.SelectionChanged += new System.EventHandler(this.ProjectsGridView_SelectionChanged);
            this.ProjectsGridView.DoubleClick += new System.EventHandler(this.InfoProjectButton_Click);
            // 
            // ProjectName
            // 
            this.ProjectName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ProjectName.DataPropertyName = "ProjectName";
            this.ProjectName.FillWeight = 30F;
            this.ProjectName.HeaderText = "Name";
            this.ProjectName.MinimumWidth = 50;
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.ReadOnly = true;
            // 
            // Lead
            // 
            this.Lead.DataPropertyName = "Lead";
            this.Lead.HeaderText = "Investigator";
            this.Lead.MinimumWidth = 50;
            this.Lead.Name = "Lead";
            this.Lead.ReadOnly = true;
            this.Lead.Width = 120;
            // 
            // UnitCode
            // 
            this.UnitCode.DataPropertyName = "UnitCode";
            this.UnitCode.HeaderText = "Unit";
            this.UnitCode.MinimumWidth = 40;
            this.UnitCode.Name = "UnitCode";
            this.UnitCode.ReadOnly = true;
            this.UnitCode.Width = 40;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.DataPropertyName = "Description";
            this.Description.FillWeight = 70F;
            this.Description.HeaderText = "Description";
            this.Description.MinimumWidth = 100;
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // ProjectId
            // 
            this.ProjectId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ProjectId.DataPropertyName = "ProjectId";
            this.ProjectId.HeaderText = "Code";
            this.ProjectId.MinimumWidth = 40;
            this.ProjectId.Name = "ProjectId";
            this.ProjectId.ReadOnly = true;
            this.ProjectId.Width = 57;
            // 
            // CanDelete
            // 
            this.CanDelete.DataPropertyName = "CanDelete";
            this.CanDelete.HeaderText = "CanDelete";
            this.CanDelete.Name = "CanDelete";
            this.CanDelete.ReadOnly = true;
            this.CanDelete.Visible = false;
            // 
            // Project
            // 
            this.Project.DataPropertyName = "Project";
            this.Project.HeaderText = "Project";
            this.Project.Name = "Project";
            this.Project.ReadOnly = true;
            this.Project.Visible = false;
            // 
            // ProjectsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 251);
            this.Controls.Add(this.ProjectsGridView);
            this.Controls.Add(this.ShowHideButton);
            this.Controls.Add(this.InfoProjectButton);
            this.Controls.Add(this.DeleteProjectButton);
            this.Controls.Add(this.AddProjectButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(295, 193);
            this.Name = "ProjectsForm";
            this.Text = "Project List";
            ((System.ComponentModel.ISupportInitialize)(this.ProjectsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button InfoProjectButton;
        private System.Windows.Forms.Button DeleteProjectButton;
        private System.Windows.Forms.Button AddProjectButton;
        private System.Windows.Forms.Button ShowHideButton;
        private System.Windows.Forms.DataGridView ProjectsGridView;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lead;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectId;
        private System.Windows.Forms.DataGridViewTextBoxColumn CanDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn Project;
    }
}