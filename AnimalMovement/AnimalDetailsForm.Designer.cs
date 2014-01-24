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
            this.DeploymentDataGridView = new System.Windows.Forms.DataGridView();
            this.DeploymentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CollarIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeployDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RetrieveDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.AnimalTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.MortatlityDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.EditDeploymentButton = new System.Windows.Forms.Button();
            this.AddDeploymentButton = new System.Windows.Forms.Button();
            this.DeleteDeploymentButton = new System.Windows.Forms.Button();
            this.InfoCollarButton = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.LocationsGridView = new System.Windows.Forms.DataGridView();
            this.SummaryLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DeploymentDataGridView)).BeginInit();
            this.AnimalTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LocationsGridView)).BeginInit();
            this.SuspendLayout();
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
            this.DeploymentColumn,
            this.CollarIdColumn,
            this.DeployDateColumn,
            this.RetrieveDateColumn});
            this.DeploymentDataGridView.Location = new System.Drawing.Point(8, 6);
            this.DeploymentDataGridView.Name = "DeploymentDataGridView";
            this.DeploymentDataGridView.ReadOnly = true;
            this.DeploymentDataGridView.RowHeadersVisible = false;
            this.DeploymentDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DeploymentDataGridView.Size = new System.Drawing.Size(423, 119);
            this.DeploymentDataGridView.TabIndex = 0;
            this.DeploymentDataGridView.SelectionChanged += new System.EventHandler(this.DeploymentDataGridView_SelectionChanged);
            this.DeploymentDataGridView.DoubleClick += new System.EventHandler(this.InfoCollarButton_Click);
            // 
            // DeploymentColumn
            // 
            this.DeploymentColumn.DataPropertyName = "Deployment";
            this.DeploymentColumn.HeaderText = "Deployment";
            this.DeploymentColumn.Name = "DeploymentColumn";
            this.DeploymentColumn.ReadOnly = true;
            this.DeploymentColumn.Visible = false;
            // 
            // CollarIdColumn
            // 
            this.CollarIdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.CollarIdColumn.DataPropertyName = "Collar";
            this.CollarIdColumn.HeaderText = "Collar";
            this.CollarIdColumn.MinimumWidth = 160;
            this.CollarIdColumn.Name = "CollarIdColumn";
            this.CollarIdColumn.ReadOnly = true;
            this.CollarIdColumn.Width = 160;
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
            // ProjectTextBox
            // 
            this.ProjectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectTextBox.Enabled = false;
            this.ProjectTextBox.Location = new System.Drawing.Point(94, 7);
            this.ProjectTextBox.MaxLength = 50;
            this.ProjectTextBox.Name = "ProjectTextBox";
            this.ProjectTextBox.ReadOnly = true;
            this.ProjectTextBox.Size = new System.Drawing.Size(184, 20);
            this.ProjectTextBox.TabIndex = 1;
            // 
            // DoneCancelButton
            // 
            this.DoneCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DoneCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneCancelButton.Location = new System.Drawing.Point(87, 131);
            this.DoneCancelButton.Name = "DoneCancelButton";
            this.DoneCancelButton.Size = new System.Drawing.Size(75, 23);
            this.DoneCancelButton.TabIndex = 5;
            this.DoneCancelButton.Text = "Done";
            this.DoneCancelButton.UseVisualStyleBackColor = true;
            this.DoneCancelButton.Click += new System.EventHandler(this.DoneCancelButton_Click);
            // 
            // EditSaveButton
            // 
            this.EditSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EditSaveButton.BackColor = System.Drawing.SystemColors.Control;
            this.EditSaveButton.Enabled = false;
            this.EditSaveButton.Location = new System.Drawing.Point(356, 131);
            this.EditSaveButton.Name = "EditSaveButton";
            this.EditSaveButton.Size = new System.Drawing.Size(75, 23);
            this.EditSaveButton.TabIndex = 6;
            this.EditSaveButton.Text = "Edit";
            this.EditSaveButton.UseVisualStyleBackColor = true;
            this.EditSaveButton.Click += new System.EventHandler(this.EditSaveButton_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(41, 62);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 13);
            this.label10.TabIndex = 78;
            this.label10.Text = "Group:";
            // 
            // GroupTextBox
            // 
            this.GroupTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupTextBox.Location = new System.Drawing.Point(87, 59);
            this.GroupTextBox.MaxLength = 500;
            this.GroupTextBox.Name = "GroupTextBox";
            this.GroupTextBox.Size = new System.Drawing.Size(344, 20);
            this.GroupTextBox.TabIndex = 3;
            // 
            // SpeciesComboBox
            // 
            this.SpeciesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpeciesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SpeciesComboBox.FormattingEnabled = true;
            this.SpeciesComboBox.Location = new System.Drawing.Point(87, 6);
            this.SpeciesComboBox.Name = "SpeciesComboBox";
            this.SpeciesComboBox.Size = new System.Drawing.Size(184, 21);
            this.SpeciesComboBox.TabIndex = 0;
            // 
            // GenderComboBox
            // 
            this.GenderComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GenderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GenderComboBox.FormattingEnabled = true;
            this.GenderComboBox.Location = new System.Drawing.Point(336, 6);
            this.GenderComboBox.Name = "GenderComboBox";
            this.GenderComboBox.Size = new System.Drawing.Size(95, 21);
            this.GenderComboBox.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 68;
            this.label3.Text = "Project:";
            // 
            // DescriptionTextBox
            // 
            this.DescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionTextBox.Location = new System.Drawing.Point(87, 85);
            this.DescriptionTextBox.MaxLength = 2000;
            this.DescriptionTextBox.Multiline = true;
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.Size = new System.Drawing.Size(344, 40);
            this.DescriptionTextBox.TabIndex = 4;
            // 
            // AnimalIdTextBox
            // 
            this.AnimalIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimalIdTextBox.Enabled = false;
            this.AnimalIdTextBox.Location = new System.Drawing.Point(343, 7);
            this.AnimalIdTextBox.MaxLength = 50;
            this.AnimalIdTextBox.Name = "AnimalIdTextBox";
            this.AnimalIdTextBox.ReadOnly = true;
            this.AnimalIdTextBox.Size = new System.Drawing.Size(92, 20);
            this.AnimalIdTextBox.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 63;
            this.label5.Text = "Description:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(285, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 62;
            this.label4.Text = "Gender:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(284, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 61;
            this.label2.Text = "Animal Id:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "Species:";
            // 
            // AnimalTabControl
            // 
            this.AnimalTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimalTabControl.Controls.Add(this.tabPage1);
            this.AnimalTabControl.Controls.Add(this.tabPage2);
            this.AnimalTabControl.Controls.Add(this.tabPage3);
            this.AnimalTabControl.Location = new System.Drawing.Point(0, 33);
            this.AnimalTabControl.Name = "AnimalTabControl";
            this.AnimalTabControl.SelectedIndex = 0;
            this.AnimalTabControl.Size = new System.Drawing.Size(449, 188);
            this.AnimalTabControl.TabIndex = 1;
            this.AnimalTabControl.SelectedIndexChanged += new System.EventHandler(this.AnimalTabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.MortatlityDateTimePicker);
            this.tabPage1.Controls.Add(this.DescriptionTextBox);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.DoneCancelButton);
            this.tabPage1.Controls.Add(this.EditSaveButton);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.GroupTextBox);
            this.tabPage1.Controls.Add(this.SpeciesComboBox);
            this.tabPage1.Controls.Add(this.GenderComboBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(441, 162);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 80;
            this.label6.Text = "Date of death:";
            // 
            // MortatlityDateTimePicker
            // 
            this.MortatlityDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MortatlityDateTimePicker.Checked = false;
            this.MortatlityDateTimePicker.CustomFormat = " ";
            this.MortatlityDateTimePicker.Enabled = false;
            this.MortatlityDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.MortatlityDateTimePicker.Location = new System.Drawing.Point(87, 33);
            this.MortatlityDateTimePicker.Name = "MortatlityDateTimePicker";
            this.MortatlityDateTimePicker.ShowCheckBox = true;
            this.MortatlityDateTimePicker.Size = new System.Drawing.Size(344, 20);
            this.MortatlityDateTimePicker.TabIndex = 2;
            this.MortatlityDateTimePicker.ValueChanged += new System.EventHandler(this.MortatlityDateTimePicker_ValueChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.EditDeploymentButton);
            this.tabPage2.Controls.Add(this.AddDeploymentButton);
            this.tabPage2.Controls.Add(this.DeleteDeploymentButton);
            this.tabPage2.Controls.Add(this.InfoCollarButton);
            this.tabPage2.Controls.Add(this.DeploymentDataGridView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(441, 162);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Collars";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // EditDeploymentButton
            // 
            this.EditDeploymentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.EditDeploymentButton.FlatAppearance.BorderSize = 0;
            this.EditDeploymentButton.Image = global::AnimalMovement.Properties.Resources.GenericPencil16;
            this.EditDeploymentButton.Location = new System.Drawing.Point(62, 132);
            this.EditDeploymentButton.Name = "EditDeploymentButton";
            this.EditDeploymentButton.Size = new System.Drawing.Size(24, 24);
            this.EditDeploymentButton.TabIndex = 4;
            this.EditDeploymentButton.UseVisualStyleBackColor = true;
            this.EditDeploymentButton.Click += new System.EventHandler(this.EditDeploymentButton_Click);
            // 
            // AddDeploymentButton
            // 
            this.AddDeploymentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddDeploymentButton.Enabled = false;
            this.AddDeploymentButton.Image = global::AnimalMovement.Properties.Resources.GenericAddGreen16;
            this.AddDeploymentButton.Location = new System.Drawing.Point(8, 132);
            this.AddDeploymentButton.Name = "AddDeploymentButton";
            this.AddDeploymentButton.Size = new System.Drawing.Size(24, 24);
            this.AddDeploymentButton.TabIndex = 2;
            this.AddDeploymentButton.UseVisualStyleBackColor = true;
            this.AddDeploymentButton.Click += new System.EventHandler(this.AddDeploymentButton_Click);
            // 
            // DeleteDeploymentButton
            // 
            this.DeleteDeploymentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteDeploymentButton.Enabled = false;
            this.DeleteDeploymentButton.Image = global::AnimalMovement.Properties.Resources.GenericDeleteRed16;
            this.DeleteDeploymentButton.Location = new System.Drawing.Point(35, 132);
            this.DeleteDeploymentButton.Name = "DeleteDeploymentButton";
            this.DeleteDeploymentButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteDeploymentButton.TabIndex = 3;
            this.DeleteDeploymentButton.UseVisualStyleBackColor = true;
            this.DeleteDeploymentButton.Click += new System.EventHandler(this.DeleteDeploymentButton_Click);
            // 
            // InfoCollarButton
            // 
            this.InfoCollarButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InfoCollarButton.FlatAppearance.BorderSize = 0;
            this.InfoCollarButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoCollarButton.Image = global::AnimalMovement.Properties.Resources.GenericInformation_B_16;
            this.InfoCollarButton.Location = new System.Drawing.Point(86, 132);
            this.InfoCollarButton.Name = "InfoCollarButton";
            this.InfoCollarButton.Size = new System.Drawing.Size(24, 24);
            this.InfoCollarButton.TabIndex = 0;
            this.InfoCollarButton.UseVisualStyleBackColor = true;
            this.InfoCollarButton.Click += new System.EventHandler(this.InfoCollarButton_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.LocationsGridView);
            this.tabPage3.Controls.Add(this.SummaryLabel);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(441, 162);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Locations";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // LocationsGridView
            // 
            this.LocationsGridView.AllowUserToAddRows = false;
            this.LocationsGridView.AllowUserToDeleteRows = false;
            this.LocationsGridView.AllowUserToResizeRows = false;
            this.LocationsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LocationsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.LocationsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LocationsGridView.Location = new System.Drawing.Point(6, 33);
            this.LocationsGridView.Name = "LocationsGridView";
            this.LocationsGridView.ReadOnly = true;
            this.LocationsGridView.Size = new System.Drawing.Size(429, 121);
            this.LocationsGridView.TabIndex = 6;
            // 
            // SummaryLabel
            // 
            this.SummaryLabel.AutoSize = true;
            this.SummaryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SummaryLabel.Location = new System.Drawing.Point(8, 12);
            this.SummaryLabel.Name = "SummaryLabel";
            this.SummaryLabel.Size = new System.Drawing.Size(72, 18);
            this.SummaryLabel.TabIndex = 0;
            this.SummaryLabel.Text = "Summary";
            // 
            // AnimalDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 221);
            this.Controls.Add(this.AnimalTabControl);
            this.Controls.Add(this.ProjectTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AnimalIdTextBox);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(400, 250);
            this.Name = "AnimalDetailsForm";
            this.Text = "Animal Details";
            ((System.ComponentModel.ISupportInitialize)(this.DeploymentDataGridView)).EndInit();
            this.AnimalTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LocationsGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DeploymentDataGridView;
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
        private System.Windows.Forms.TabControl AnimalTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker MortatlityDateTimePicker;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label SummaryLabel;
        private System.Windows.Forms.Button EditDeploymentButton;
        private System.Windows.Forms.Button AddDeploymentButton;
        private System.Windows.Forms.Button DeleteDeploymentButton;
        private System.Windows.Forms.Button InfoCollarButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeploymentColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollarIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeployDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RetrieveDateColumn;
        private System.Windows.Forms.DataGridView LocationsGridView;

    }
}