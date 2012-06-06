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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.MortatlityDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.CollarInfoButton = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.TopTextBox = new System.Windows.Forms.TextBox();
            this.BottomTextBox = new System.Windows.Forms.TextBox();
            this.RightTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.LeftTextBox = new System.Windows.Forms.TextBox();
            this.SummaryLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DeploymentDataGridView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // DeleteDeploymentButton
            // 
            this.DeleteDeploymentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteDeploymentButton.Location = new System.Drawing.Point(275, 164);
            this.DeleteDeploymentButton.Name = "DeleteDeploymentButton";
            this.DeleteDeploymentButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteDeploymentButton.TabIndex = 9;
            this.DeleteDeploymentButton.Text = "Delete";
            this.DeleteDeploymentButton.UseVisualStyleBackColor = true;
            this.DeleteDeploymentButton.Click += new System.EventHandler(this.DeleteDeploymentButton_Click);
            // 
            // DeployRetrieveButton
            // 
            this.DeployRetrieveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeployRetrieveButton.Location = new System.Drawing.Point(356, 164);
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
            this.DeploymentDataGridView.Location = new System.Drawing.Point(8, 6);
            this.DeploymentDataGridView.Name = "DeploymentDataGridView";
            this.DeploymentDataGridView.ReadOnly = true;
            this.DeploymentDataGridView.RowHeadersVisible = false;
            this.DeploymentDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DeploymentDataGridView.Size = new System.Drawing.Size(423, 152);
            this.DeploymentDataGridView.TabIndex = 7;
            this.DeploymentDataGridView.DoubleClick += new System.EventHandler(this.CollarInfoButton_Click);
            // 
            // CollarManufacturerColumn
            // 
            this.CollarManufacturerColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.CollarManufacturerColumn.DataPropertyName = "Manufacturer";
            this.CollarManufacturerColumn.HeaderText = "Manufacturer";
            this.CollarManufacturerColumn.MinimumWidth = 60;
            this.CollarManufacturerColumn.Name = "CollarManufacturerColumn";
            this.CollarManufacturerColumn.ReadOnly = true;
            this.CollarManufacturerColumn.Width = 95;
            // 
            // CollarIdColumn
            // 
            this.CollarIdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.CollarIdColumn.DataPropertyName = "CollarId";
            this.CollarIdColumn.HeaderText = "Collar";
            this.CollarIdColumn.MinimumWidth = 60;
            this.CollarIdColumn.Name = "CollarIdColumn";
            this.CollarIdColumn.ReadOnly = true;
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
            this.DeploymentColumn.ReadOnly = true;
            this.DeploymentColumn.Visible = false;
            // 
            // ProjectTextBox
            // 
            this.ProjectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectTextBox.Enabled = false;
            this.ProjectTextBox.Location = new System.Drawing.Point(87, 11);
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
            this.DoneCancelButton.Location = new System.Drawing.Point(87, 164);
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
            this.EditSaveButton.Location = new System.Drawing.Point(353, 164);
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
            this.label10.Location = new System.Drawing.Point(41, 93);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 13);
            this.label10.TabIndex = 78;
            this.label10.Text = "Group:";
            // 
            // GroupTextBox
            // 
            this.GroupTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupTextBox.Location = new System.Drawing.Point(87, 90);
            this.GroupTextBox.MaxLength = 500;
            this.GroupTextBox.Name = "GroupTextBox";
            this.GroupTextBox.Size = new System.Drawing.Size(341, 20);
            this.GroupTextBox.TabIndex = 5;
            // 
            // SpeciesComboBox
            // 
            this.SpeciesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpeciesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SpeciesComboBox.FormattingEnabled = true;
            this.SpeciesComboBox.Location = new System.Drawing.Point(87, 37);
            this.SpeciesComboBox.Name = "SpeciesComboBox";
            this.SpeciesComboBox.Size = new System.Drawing.Size(184, 21);
            this.SpeciesComboBox.TabIndex = 3;
            // 
            // GenderComboBox
            // 
            this.GenderComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GenderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GenderComboBox.FormattingEnabled = true;
            this.GenderComboBox.Location = new System.Drawing.Point(336, 37);
            this.GenderComboBox.Name = "GenderComboBox";
            this.GenderComboBox.Size = new System.Drawing.Size(92, 21);
            this.GenderComboBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 14);
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
            this.DescriptionTextBox.Location = new System.Drawing.Point(87, 116);
            this.DescriptionTextBox.MaxLength = 2000;
            this.DescriptionTextBox.Multiline = true;
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.Size = new System.Drawing.Size(341, 42);
            this.DescriptionTextBox.TabIndex = 6;
            // 
            // AnimalIdTextBox
            // 
            this.AnimalIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimalIdTextBox.Enabled = false;
            this.AnimalIdTextBox.Location = new System.Drawing.Point(336, 11);
            this.AnimalIdTextBox.MaxLength = 50;
            this.AnimalIdTextBox.Name = "AnimalIdTextBox";
            this.AnimalIdTextBox.ReadOnly = true;
            this.AnimalIdTextBox.Size = new System.Drawing.Size(92, 20);
            this.AnimalIdTextBox.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 63;
            this.label5.Text = "Description:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(285, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 62;
            this.label4.Text = "Gender:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(277, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 61;
            this.label2.Text = "Animal Id:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 60;
            this.label1.Text = "Species:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(449, 221);
            this.tabControl1.TabIndex = 79;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.MortatlityDateTimePicker);
            this.tabPage1.Controls.Add(this.DescriptionTextBox);
            this.tabPage1.Controls.Add(this.ProjectTextBox);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.DoneCancelButton);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.EditSaveButton);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.GroupTextBox);
            this.tabPage1.Controls.Add(this.AnimalIdTextBox);
            this.tabPage1.Controls.Add(this.SpeciesComboBox);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.GenderComboBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(441, 195);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 67);
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
            this.MortatlityDateTimePicker.Location = new System.Drawing.Point(87, 64);
            this.MortatlityDateTimePicker.Name = "MortatlityDateTimePicker";
            this.MortatlityDateTimePicker.ShowCheckBox = true;
            this.MortatlityDateTimePicker.Size = new System.Drawing.Size(341, 20);
            this.MortatlityDateTimePicker.TabIndex = 79;
            this.MortatlityDateTimePicker.ValueChanged += new System.EventHandler(this.MortatlityDateTimePicker_ValueChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.CollarInfoButton);
            this.tabPage2.Controls.Add(this.DeploymentDataGridView);
            this.tabPage2.Controls.Add(this.DeleteDeploymentButton);
            this.tabPage2.Controls.Add(this.DeployRetrieveButton);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(441, 195);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Collars";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // CollarInfoButton
            // 
            this.CollarInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarInfoButton.Location = new System.Drawing.Point(194, 164);
            this.CollarInfoButton.Name = "CollarInfoButton";
            this.CollarInfoButton.Size = new System.Drawing.Size(75, 23);
            this.CollarInfoButton.TabIndex = 11;
            this.CollarInfoButton.Text = "Info";
            this.CollarInfoButton.UseVisualStyleBackColor = true;
            this.CollarInfoButton.Click += new System.EventHandler(this.CollarInfoButton_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.TopTextBox);
            this.tabPage3.Controls.Add(this.BottomTextBox);
            this.tabPage3.Controls.Add(this.RightTextBox);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.LeftTextBox);
            this.tabPage3.Controls.Add(this.SummaryLabel);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(441, 195);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Locations";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 145);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 18);
            this.label7.TabIndex = 6;
            this.label7.Text = "More to come...";
            // 
            // TopTextBox
            // 
            this.TopTextBox.Location = new System.Drawing.Point(114, 53);
            this.TopTextBox.Name = "TopTextBox";
            this.TopTextBox.ReadOnly = true;
            this.TopTextBox.Size = new System.Drawing.Size(100, 20);
            this.TopTextBox.TabIndex = 5;
            // 
            // BottomTextBox
            // 
            this.BottomTextBox.Location = new System.Drawing.Point(114, 105);
            this.BottomTextBox.Name = "BottomTextBox";
            this.BottomTextBox.ReadOnly = true;
            this.BottomTextBox.Size = new System.Drawing.Size(100, 20);
            this.BottomTextBox.TabIndex = 4;
            // 
            // RightTextBox
            // 
            this.RightTextBox.Location = new System.Drawing.Point(165, 79);
            this.RightTextBox.Name = "RightTextBox";
            this.RightTextBox.ReadOnly = true;
            this.RightTextBox.Size = new System.Drawing.Size(100, 20);
            this.RightTextBox.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Extents:";
            // 
            // LeftTextBox
            // 
            this.LeftTextBox.Location = new System.Drawing.Point(59, 79);
            this.LeftTextBox.Name = "LeftTextBox";
            this.LeftTextBox.ReadOnly = true;
            this.LeftTextBox.Size = new System.Drawing.Size(100, 20);
            this.LeftTextBox.TabIndex = 1;
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
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(400, 250);
            this.Name = "AnimalDetailsForm";
            this.Text = "Animal Details";
            ((System.ComponentModel.ISupportInitialize)(this.DeploymentDataGridView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker MortatlityDateTimePicker;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TopTextBox;
        private System.Windows.Forms.TextBox BottomTextBox;
        private System.Windows.Forms.TextBox RightTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox LeftTextBox;
        private System.Windows.Forms.Label SummaryLabel;
        private System.Windows.Forms.Button CollarInfoButton;

    }
}