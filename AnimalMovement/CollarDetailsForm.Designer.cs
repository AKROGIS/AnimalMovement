namespace AnimalMovement
{
    partial class CollarDetailsForm
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
            this.label10 = new System.Windows.Forms.Label();
            this.DownloadInfoTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.FrequencyTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SerialNumberTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ManagerComboBox = new System.Windows.Forms.ComboBox();
            this.ModelComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.NotesTextBox = new System.Windows.Forms.TextBox();
            this.OwnerTextBox = new System.Windows.Forms.TextBox();
            this.CollarIdTextBox = new System.Windows.Forms.TextBox();
            this.AlternativeIdTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DoneCancelButton = new System.Windows.Forms.Button();
            this.EditSaveButton = new System.Windows.Forms.Button();
            this.ManufacturerTextBox = new System.Windows.Forms.TextBox();
            this.DeploymentDataGridView = new System.Windows.Forms.DataGridView();
            this.CollarManufacturerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CollarIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AnimalColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeployDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RetrieveDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeploymentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeployRetrieveButton = new System.Windows.Forms.Button();
            this.DeleteDeploymentButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DeploymentDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 120);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 13);
            this.label10.TabIndex = 53;
            this.label10.Text = "Download Info:";
            // 
            // DownloadInfoTextBox
            // 
            this.DownloadInfoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DownloadInfoTextBox.Location = new System.Drawing.Point(94, 117);
            this.DownloadInfoTextBox.MaxLength = 8;
            this.DownloadInfoTextBox.Name = "DownloadInfoTextBox";
            this.DownloadInfoTextBox.Size = new System.Drawing.Size(375, 20);
            this.DownloadInfoTextBox.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(252, 94);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 51;
            this.label9.Text = "Frequency:";
            // 
            // FrequencyTextBox
            // 
            this.FrequencyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FrequencyTextBox.Location = new System.Drawing.Point(316, 91);
            this.FrequencyTextBox.MaxLength = 8;
            this.FrequencyTextBox.Name = "FrequencyTextBox";
            this.FrequencyTextBox.Size = new System.Drawing.Size(153, 20);
            this.FrequencyTextBox.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 13);
            this.label8.TabIndex = 49;
            this.label8.Text = "Serial Number:";
            // 
            // SerialNumberTextBox
            // 
            this.SerialNumberTextBox.Location = new System.Drawing.Point(94, 91);
            this.SerialNumberTextBox.MaxLength = 8;
            this.SerialNumberTextBox.Name = "SerialNumberTextBox";
            this.SerialNumberTextBox.Size = new System.Drawing.Size(151, 20);
            this.SerialNumberTextBox.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(271, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 47;
            this.label7.Text = "Owner:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 46;
            this.label6.Text = "Alternative Id:";
            // 
            // ManagerComboBox
            // 
            this.ManagerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ManagerComboBox.FormattingEnabled = true;
            this.ManagerComboBox.Location = new System.Drawing.Point(94, 38);
            this.ManagerComboBox.Name = "ManagerComboBox";
            this.ManagerComboBox.Size = new System.Drawing.Size(151, 21);
            this.ManagerComboBox.TabIndex = 3;
            // 
            // ModelComboBox
            // 
            this.ModelComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ModelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModelComboBox.FormattingEnabled = true;
            this.ModelComboBox.Location = new System.Drawing.Point(316, 38);
            this.ModelComboBox.Name = "ModelComboBox";
            this.ModelComboBox.Size = new System.Drawing.Size(153, 21);
            this.ModelComboBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "Manufacturer:";
            // 
            // NotesTextBox
            // 
            this.NotesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NotesTextBox.Location = new System.Drawing.Point(94, 143);
            this.NotesTextBox.MaxLength = 200;
            this.NotesTextBox.Multiline = true;
            this.NotesTextBox.Name = "NotesTextBox";
            this.NotesTextBox.Size = new System.Drawing.Size(375, 69);
            this.NotesTextBox.TabIndex = 10;
            // 
            // OwnerTextBox
            // 
            this.OwnerTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OwnerTextBox.Location = new System.Drawing.Point(316, 65);
            this.OwnerTextBox.MaxLength = 4;
            this.OwnerTextBox.Name = "OwnerTextBox";
            this.OwnerTextBox.Size = new System.Drawing.Size(153, 20);
            this.OwnerTextBox.TabIndex = 6;
            // 
            // CollarIdTextBox
            // 
            this.CollarIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarIdTextBox.Enabled = false;
            this.CollarIdTextBox.Location = new System.Drawing.Point(316, 12);
            this.CollarIdTextBox.MaxLength = 50;
            this.CollarIdTextBox.Name = "CollarIdTextBox";
            this.CollarIdTextBox.ReadOnly = true;
            this.CollarIdTextBox.Size = new System.Drawing.Size(153, 20);
            this.CollarIdTextBox.TabIndex = 2;
            // 
            // AlternativeIdTextBox
            // 
            this.AlternativeIdTextBox.Location = new System.Drawing.Point(94, 65);
            this.AlternativeIdTextBox.MaxLength = 8;
            this.AlternativeIdTextBox.Name = "AlternativeIdTextBox";
            this.AlternativeIdTextBox.Size = new System.Drawing.Size(151, 20);
            this.AlternativeIdTextBox.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(52, 146);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "Notes:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(273, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Model:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(264, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Collar Id:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Manager:";
            // 
            // DoneCancelButton
            // 
            this.DoneCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DoneCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneCancelButton.Location = new System.Drawing.Point(12, 326);
            this.DoneCancelButton.Name = "DoneCancelButton";
            this.DoneCancelButton.Size = new System.Drawing.Size(75, 23);
            this.DoneCancelButton.TabIndex = 20;
            this.DoneCancelButton.Text = "Done";
            this.DoneCancelButton.UseVisualStyleBackColor = true;
            this.DoneCancelButton.Click += new System.EventHandler(this.DoneCancelButton_Click);
            // 
            // EditSaveButton
            // 
            this.EditSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EditSaveButton.BackColor = System.Drawing.SystemColors.Control;
            this.EditSaveButton.Enabled = false;
            this.EditSaveButton.Location = new System.Drawing.Point(394, 326);
            this.EditSaveButton.Name = "EditSaveButton";
            this.EditSaveButton.Size = new System.Drawing.Size(75, 23);
            this.EditSaveButton.TabIndex = 26;
            this.EditSaveButton.Text = "Edit";
            this.EditSaveButton.UseVisualStyleBackColor = true;
            this.EditSaveButton.Click += new System.EventHandler(this.EditSaveButton_Click);
            // 
            // ManufacturerTextBox
            // 
            this.ManufacturerTextBox.Enabled = false;
            this.ManufacturerTextBox.Location = new System.Drawing.Point(94, 12);
            this.ManufacturerTextBox.MaxLength = 50;
            this.ManufacturerTextBox.Name = "ManufacturerTextBox";
            this.ManufacturerTextBox.ReadOnly = true;
            this.ManufacturerTextBox.Size = new System.Drawing.Size(151, 20);
            this.ManufacturerTextBox.TabIndex = 1;
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
            this.DeploymentDataGridView.Location = new System.Drawing.Point(12, 218);
            this.DeploymentDataGridView.MultiSelect = false;
            this.DeploymentDataGridView.Name = "DeploymentDataGridView";
            this.DeploymentDataGridView.RowHeadersVisible = false;
            this.DeploymentDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DeploymentDataGridView.Size = new System.Drawing.Size(457, 102);
            this.DeploymentDataGridView.TabIndex = 11;
            this.DeploymentDataGridView.DoubleClick += new System.EventHandler(this.DeploymentDataGridView_DoubleClick);
            // 
            // CollarManufacturerColumn
            // 
            this.CollarManufacturerColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.CollarManufacturerColumn.DataPropertyName = "Manufacturer";
            this.CollarManufacturerColumn.HeaderText = "Manufacturer";
            this.CollarManufacturerColumn.MinimumWidth = 60;
            this.CollarManufacturerColumn.Name = "CollarManufacturerColumn";
            this.CollarManufacturerColumn.Visible = false;
            // 
            // CollarIdColumn
            // 
            this.CollarIdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.CollarIdColumn.DataPropertyName = "CollarId";
            this.CollarIdColumn.HeaderText = "Collar";
            this.CollarIdColumn.MinimumWidth = 60;
            this.CollarIdColumn.Name = "CollarIdColumn";
            this.CollarIdColumn.Visible = false;
            // 
            // ProjectColumn
            // 
            this.ProjectColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ProjectColumn.DataPropertyName = "Project";
            this.ProjectColumn.HeaderText = "Project";
            this.ProjectColumn.MinimumWidth = 60;
            this.ProjectColumn.Name = "ProjectColumn";
            this.ProjectColumn.ReadOnly = true;
            this.ProjectColumn.Width = 65;
            // 
            // AnimalColumn
            // 
            this.AnimalColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.AnimalColumn.DataPropertyName = "AnimalId";
            this.AnimalColumn.HeaderText = "Animal";
            this.AnimalColumn.MinimumWidth = 60;
            this.AnimalColumn.Name = "AnimalColumn";
            this.AnimalColumn.ReadOnly = true;
            this.AnimalColumn.Width = 63;
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
            // DeployRetrieveButton
            // 
            this.DeployRetrieveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeployRetrieveButton.Location = new System.Drawing.Point(247, 326);
            this.DeployRetrieveButton.Name = "DeployRetrieveButton";
            this.DeployRetrieveButton.Size = new System.Drawing.Size(75, 23);
            this.DeployRetrieveButton.TabIndex = 24;
            this.DeployRetrieveButton.Text = "Deploy";
            this.DeployRetrieveButton.UseVisualStyleBackColor = true;
            this.DeployRetrieveButton.Click += new System.EventHandler(this.DeployRetrieveButton_Click);
            // 
            // DeleteDeploymentButton
            // 
            this.DeleteDeploymentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteDeploymentButton.Location = new System.Drawing.Point(166, 326);
            this.DeleteDeploymentButton.Name = "DeleteDeploymentButton";
            this.DeleteDeploymentButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteDeploymentButton.TabIndex = 22;
            this.DeleteDeploymentButton.Text = "Delete";
            this.DeleteDeploymentButton.UseVisualStyleBackColor = true;
            this.DeleteDeploymentButton.Click += new System.EventHandler(this.DeleteDeploymentButton_Click);
            // 
            // CollarDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 361);
            this.Controls.Add(this.DeleteDeploymentButton);
            this.Controls.Add(this.DeployRetrieveButton);
            this.Controls.Add(this.DeploymentDataGridView);
            this.Controls.Add(this.ManufacturerTextBox);
            this.Controls.Add(this.DoneCancelButton);
            this.Controls.Add(this.EditSaveButton);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.DownloadInfoTextBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.FrequencyTextBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.SerialNumberTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ManagerComboBox);
            this.Controls.Add(this.ModelComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.NotesTextBox);
            this.Controls.Add(this.OwnerTextBox);
            this.Controls.Add(this.CollarIdTextBox);
            this.Controls.Add(this.AlternativeIdTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(493, 395);
            this.Name = "CollarDetailsForm";
            this.Text = "Collar Details";
            ((System.ComponentModel.ISupportInitialize)(this.DeploymentDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox DownloadInfoTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox FrequencyTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox SerialNumberTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox ManagerComboBox;
        private System.Windows.Forms.ComboBox ModelComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox NotesTextBox;
        private System.Windows.Forms.TextBox OwnerTextBox;
        private System.Windows.Forms.TextBox CollarIdTextBox;
        private System.Windows.Forms.TextBox AlternativeIdTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DoneCancelButton;
        private System.Windows.Forms.Button EditSaveButton;
        private System.Windows.Forms.TextBox ManufacturerTextBox;
        private System.Windows.Forms.DataGridView DeploymentDataGridView;
        private System.Windows.Forms.Button DeployRetrieveButton;
        private System.Windows.Forms.Button DeleteDeploymentButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollarManufacturerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollarIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn AnimalColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeployDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RetrieveDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeploymentColumn;
    }
}