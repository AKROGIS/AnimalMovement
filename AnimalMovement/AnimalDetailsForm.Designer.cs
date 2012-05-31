namespace AnimalMovement
{
    partial class AnimalDetailsForm
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
            this.DeleteDeploymentButton = new System.Windows.Forms.Button();
            this.DeployRetrieveButton = new System.Windows.Forms.Button();
            this.DeploymentDataGridView = new System.Windows.Forms.DataGridView();
            this.CollarManufacturerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CollarIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AnimalColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeployDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RetrieveDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeploymentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectTextBox = new System.Windows.Forms.TextBox();
            this.DoneCancelButton = new System.Windows.Forms.Button();
            this.EditSaveButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.GroupTextBox = new System.Windows.Forms.TextBox();
            this.SpeciesComboBox = new System.Windows.Forms.ComboBox();
            this.GenderComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.AnimalIdTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DeploymentDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // DeleteDeploymentButton
            // 
            this.DeleteDeploymentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteDeploymentButton.Location = new System.Drawing.Point(167, 279);
            this.DeleteDeploymentButton.Name = "DeleteDeploymentButton";
            this.DeleteDeploymentButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteDeploymentButton.TabIndex = 9;
            this.DeleteDeploymentButton.Text = "Delete";
            this.DeleteDeploymentButton.UseVisualStyleBackColor = true;
            this.DeleteDeploymentButton.Click += new System.EventHandler(this.DeleteDeploymentButton_Click);
            // 
            // DeployRetrieveButton
            // 
            this.DeployRetrieveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeployRetrieveButton.Location = new System.Drawing.Point(248, 279);
            this.DeployRetrieveButton.Name = "DeployRetrieveButton";
            this.DeployRetrieveButton.Size = new System.Drawing.Size(75, 23);
            this.DeployRetrieveButton.TabIndex = 10;
            this.DeployRetrieveButton.Text = "Deploy";
            this.DeployRetrieveButton.UseVisualStyleBackColor = true;
            this.DeployRetrieveButton.Click += new System.EventHandler(this.DeployRetrieveButton_Click);
            // 
            // DeploymentDataGridView
            // 
            this.DeploymentDataGridView.AllowUserToAddRows = false;
            this.DeploymentDataGridView.AllowUserToDeleteRows = false;
            this.DeploymentDataGridView.AllowUserToOrderColumns = true;
            this.DeploymentDataGridView.AllowUserToResizeRows = false;
            this.DeploymentDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DeploymentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DeploymentDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CollarManufacturerColumn,
            this.CollarIdColumn,
            this.ProjectColumn,
            this.AnimalColumn,
            this.DeployDateColumn,
            this.RetrieveDateColumn,
            this.DeploymentColumn});
            this.DeploymentDataGridView.Location = new System.Drawing.Point(13, 166);
            this.DeploymentDataGridView.MultiSelect = false;
            this.DeploymentDataGridView.Name = "DeploymentDataGridView";
            this.DeploymentDataGridView.RowHeadersVisible = false;
            this.DeploymentDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DeploymentDataGridView.Size = new System.Drawing.Size(463, 107);
            this.DeploymentDataGridView.TabIndex = 7;
            this.DeploymentDataGridView.DoubleClick += new System.EventHandler(this.DeploymentDataGridView_DoubleClick);
            // 
            // CollarManufacturerColumn
            // 
            this.CollarManufacturerColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.CollarManufacturerColumn.DataPropertyName = "Manufacturer";
            this.CollarManufacturerColumn.HeaderText = "Manufacturer";
            this.CollarManufacturerColumn.MinimumWidth = 60;
            this.CollarManufacturerColumn.Name = "CollarManufacturerColumn";
            this.CollarManufacturerColumn.Width = 95;
            // 
            // CollarIdColumn
            // 
            this.CollarIdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.CollarIdColumn.DataPropertyName = "CollarId";
            this.CollarIdColumn.HeaderText = "Collar";
            this.CollarIdColumn.MinimumWidth = 60;
            this.CollarIdColumn.Name = "CollarIdColumn";
            this.CollarIdColumn.Width = 60;
            // 
            // ProjectColumn
            // 
            this.ProjectColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ProjectColumn.DataPropertyName = "Project";
            this.ProjectColumn.HeaderText = "Project";
            this.ProjectColumn.MinimumWidth = 60;
            this.ProjectColumn.Name = "ProjectColumn";
            this.ProjectColumn.ReadOnly = true;
            this.ProjectColumn.Visible = false;
            // 
            // AnimalColumn
            // 
            this.AnimalColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.AnimalColumn.DataPropertyName = "AnimalId";
            this.AnimalColumn.HeaderText = "Animal";
            this.AnimalColumn.MinimumWidth = 60;
            this.AnimalColumn.Name = "AnimalColumn";
            this.AnimalColumn.ReadOnly = true;
            this.AnimalColumn.Visible = false;
            // 
            // DeployDateColumn
            // 
            this.DeployDateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DeployDateColumn.DataPropertyName = "DeploymentDate";
            this.DeployDateColumn.HeaderText = "Deployed";
            this.DeployDateColumn.MinimumWidth = 80;
            this.DeployDateColumn.Name = "DeployDateColumn";
            this.DeployDateColumn.ReadOnly = true;
            // 
            // RetrieveDateColumn
            // 
            this.RetrieveDateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RetrieveDateColumn.DataPropertyName = "RetrievalDate";
            this.RetrieveDateColumn.HeaderText = "Retrieved";
            this.RetrieveDateColumn.MinimumWidth = 80;
            this.RetrieveDateColumn.Name = "RetrieveDateColumn";
            this.RetrieveDateColumn.ReadOnly = true;
            // 
            // DeploymentColumn
            // 
            this.DeploymentColumn.DataPropertyName = "Deployment";
            this.DeploymentColumn.HeaderText = "Deployment";
            this.DeploymentColumn.Name = "DeploymentColumn";
            this.DeploymentColumn.Visible = false;
            // 
            // ProjectTextBox
            // 
            this.ProjectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectTextBox.Enabled = false;
            this.ProjectTextBox.Location = new System.Drawing.Point(95, 12);
            this.ProjectTextBox.MaxLength = 50;
            this.ProjectTextBox.Name = "ProjectTextBox";
            this.ProjectTextBox.ReadOnly = true;
            this.ProjectTextBox.Size = new System.Drawing.Size(224, 20);
            this.ProjectTextBox.TabIndex = 1;
            // 
            // DoneCancelButton
            // 
            this.DoneCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DoneCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneCancelButton.Location = new System.Drawing.Point(13, 279);
            this.DoneCancelButton.Name = "DoneCancelButton";
            this.DoneCancelButton.Size = new System.Drawing.Size(75, 23);
            this.DoneCancelButton.TabIndex = 8;
            this.DoneCancelButton.Text = "Done";
            this.DoneCancelButton.UseVisualStyleBackColor = true;
            this.DoneCancelButton.Click += new System.EventHandler(this.DoneCancelButton_Click);
            // 
            // EditSaveButton
            // 
            this.EditSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EditSaveButton.BackColor = System.Drawing.SystemColors.Control;
            this.EditSaveButton.Enabled = false;
            this.EditSaveButton.Location = new System.Drawing.Point(401, 279);
            this.EditSaveButton.Name = "EditSaveButton";
            this.EditSaveButton.Size = new System.Drawing.Size(75, 23);
            this.EditSaveButton.TabIndex = 11;
            this.EditSaveButton.Text = "Edit";
            this.EditSaveButton.UseVisualStyleBackColor = true;
            this.EditSaveButton.Click += new System.EventHandler(this.EditSaveButton_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(51, 68);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 13);
            this.label10.TabIndex = 78;
            this.label10.Text = "Group:";
            // 
            // GroupTextBox
            // 
            this.GroupTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupTextBox.Location = new System.Drawing.Point(96, 65);
            this.GroupTextBox.MaxLength = 8;
            this.GroupTextBox.Name = "GroupTextBox";
            this.GroupTextBox.Size = new System.Drawing.Size(381, 20);
            this.GroupTextBox.TabIndex = 5;
            // 
            // SpeciesComboBox
            // 
            this.SpeciesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpeciesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SpeciesComboBox.FormattingEnabled = true;
            this.SpeciesComboBox.Location = new System.Drawing.Point(95, 38);
            this.SpeciesComboBox.Name = "SpeciesComboBox";
            this.SpeciesComboBox.Size = new System.Drawing.Size(224, 21);
            this.SpeciesComboBox.TabIndex = 3;
            // 
            // GenderComboBox
            // 
            this.GenderComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GenderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GenderComboBox.FormattingEnabled = true;
            this.GenderComboBox.Location = new System.Drawing.Point(384, 38);
            this.GenderComboBox.Name = "GenderComboBox";
            this.GenderComboBox.Size = new System.Drawing.Size(92, 21);
            this.GenderComboBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 68;
            this.label3.Text = "Project:";
            // 
            // DescriptionTextBox
            // 
            this.DescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionTextBox.Location = new System.Drawing.Point(96, 91);
            this.DescriptionTextBox.MaxLength = 200;
            this.DescriptionTextBox.Multiline = true;
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.Size = new System.Drawing.Size(381, 69);
            this.DescriptionTextBox.TabIndex = 6;
            // 
            // AnimalIdTextBox
            // 
            this.AnimalIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimalIdTextBox.Enabled = false;
            this.AnimalIdTextBox.Location = new System.Drawing.Point(384, 12);
            this.AnimalIdTextBox.MaxLength = 50;
            this.AnimalIdTextBox.Name = "AnimalIdTextBox";
            this.AnimalIdTextBox.ReadOnly = true;
            this.AnimalIdTextBox.Size = new System.Drawing.Size(92, 20);
            this.AnimalIdTextBox.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 63;
            this.label5.Text = "Description:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(333, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 62;
            this.label4.Text = "Gender:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(325, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 61;
            this.label2.Text = "Animal Id:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "Species:";
            // 
            // AnimalDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 314);
            this.Controls.Add(this.DeleteDeploymentButton);
            this.Controls.Add(this.DeployRetrieveButton);
            this.Controls.Add(this.DeploymentDataGridView);
            this.Controls.Add(this.ProjectTextBox);
            this.Controls.Add(this.DoneCancelButton);
            this.Controls.Add(this.EditSaveButton);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.GroupTextBox);
            this.Controls.Add(this.SpeciesComboBox);
            this.Controls.Add(this.GenderComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DescriptionTextBox);
            this.Controls.Add(this.AnimalIdTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(437, 303);
            this.Name = "AnimalDetailsForm";
            this.Text = "Animal Details";
            ((System.ComponentModel.ISupportInitialize)(this.DeploymentDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DeleteDeploymentButton;
        private System.Windows.Forms.Button DeployRetrieveButton;
        private System.Windows.Forms.DataGridView DeploymentDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollarManufacturerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollarIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AnimalColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeployDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RetrieveDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeploymentColumn;
        private System.Windows.Forms.TextBox ProjectTextBox;
        private System.Windows.Forms.Button DoneCancelButton;
        private System.Windows.Forms.Button EditSaveButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox GroupTextBox;
        private System.Windows.Forms.ComboBox SpeciesComboBox;
        private System.Windows.Forms.ComboBox GenderComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox DescriptionTextBox;
        private System.Windows.Forms.TextBox AnimalIdTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;

    }
}