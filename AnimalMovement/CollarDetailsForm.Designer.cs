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
            this.components = new System.ComponentModel.Container();
            this.label9 = new System.Windows.Forms.Label();
            this.FrequencyTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SerialNumberTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ManagerComboBox = new System.Windows.Forms.ComboBox();
            this.ModelComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.NotesTextBox = new System.Windows.Forms.TextBox();
            this.OwnerTextBox = new System.Windows.Forms.TextBox();
            this.CollarIdTextBox = new System.Windows.Forms.TextBox();
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
            this.CollarTabs = new System.Windows.Forms.TabControl();
            this.GeneralTabPage = new System.Windows.Forms.TabPage();
            this.HasGpsCheckBox = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.DisposalDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.AnimalsTabPage = new System.Windows.Forms.TabPage();
            this.AnimalInfoButton = new System.Windows.Forms.Button();
            this.ArgosTabPage = new System.Windows.Forms.TabPage();
            this.ParametersTabPage = new System.Windows.Forms.TabPage();
            this.FilesTabPage = new System.Windows.Forms.TabPage();
            this.ChangeFileStatusButton = new System.Windows.Forms.Button();
            this.FileInfoButton = new System.Windows.Forms.Button();
            this.FilesDataGridView = new System.Windows.Forms.DataGridView();
            this.FixesTabPage = new System.Windows.Forms.TabPage();
            this.UnhideFixButton = new System.Windows.Forms.Button();
            this.FixConflictsDataGridView = new System.Windows.Forms.DataGridView();
            this.label11 = new System.Windows.Forms.Label();
            this.SummaryLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DeploymentDataGridView)).BeginInit();
            this.CollarTabs.SuspendLayout();
            this.GeneralTabPage.SuspendLayout();
            this.AnimalsTabPage.SuspendLayout();
            this.FilesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FilesDataGridView)).BeginInit();
            this.FixesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FixConflictsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(226, 62);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 51;
            this.label9.Text = "Frequency:";
            // 
            // FrequencyTextBox
            // 
            this.FrequencyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FrequencyTextBox.Location = new System.Drawing.Point(290, 59);
            this.FrequencyTextBox.MaxLength = 32;
            this.FrequencyTextBox.Name = "FrequencyTextBox";
            this.FrequencyTextBox.Size = new System.Drawing.Size(73, 20);
            this.FrequencyTextBox.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(76, 13);
            this.label8.TabIndex = 49;
            this.label8.Text = "Serial Number:";
            this.toolTip1.SetToolTip(this.label8, "For Telonics collars this is the same as the ID (optional)");
            // 
            // SerialNumberTextBox
            // 
            this.SerialNumberTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SerialNumberTextBox.Location = new System.Drawing.Point(90, 36);
            this.SerialNumberTextBox.MaxLength = 100;
            this.SerialNumberTextBox.Name = "SerialNumberTextBox";
            this.SerialNumberTextBox.Size = new System.Drawing.Size(129, 20);
            this.SerialNumberTextBox.TabIndex = 7;
            this.toolTip1.SetToolTip(this.SerialNumberTextBox, "For Telonics collars this is the same as the ID (optional)");
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(245, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 47;
            this.label7.Text = "Owner:";
            this.toolTip1.SetToolTip(this.label7, "This should be an organization (NPS, FWS, USGS, AKF&G,  etc)");
            // 
            // ManagerComboBox
            // 
            this.ManagerComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ManagerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ManagerComboBox.FormattingEnabled = true;
            this.ManagerComboBox.Location = new System.Drawing.Point(90, 6);
            this.ManagerComboBox.Name = "ManagerComboBox";
            this.ManagerComboBox.Size = new System.Drawing.Size(129, 21);
            this.ManagerComboBox.TabIndex = 3;
            this.toolTip1.SetToolTip(this.ManagerComboBox, "If you assign this collar to another PI you will not be able to edit it.");
            // 
            // ModelComboBox
            // 
            this.ModelComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ModelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModelComboBox.FormattingEnabled = true;
            this.ModelComboBox.Location = new System.Drawing.Point(290, 6);
            this.ModelComboBox.Name = "ModelComboBox";
            this.ModelComboBox.Size = new System.Drawing.Size(153, 21);
            this.ModelComboBox.TabIndex = 4;
            this.toolTip1.SetToolTip(this.ModelComboBox, "Required, but currently not used.");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "Manufacturer:";
            // 
            // NotesTextBox
            // 
            this.NotesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NotesTextBox.Location = new System.Drawing.Point(90, 88);
            this.NotesTextBox.Multiline = true;
            this.NotesTextBox.Name = "NotesTextBox";
            this.NotesTextBox.Size = new System.Drawing.Size(353, 211);
            this.NotesTextBox.TabIndex = 10;
            // 
            // OwnerTextBox
            // 
            this.OwnerTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OwnerTextBox.Location = new System.Drawing.Point(290, 33);
            this.OwnerTextBox.MaxLength = 100;
            this.OwnerTextBox.Name = "OwnerTextBox";
            this.OwnerTextBox.Size = new System.Drawing.Size(153, 20);
            this.OwnerTextBox.TabIndex = 6;
            this.toolTip1.SetToolTip(this.OwnerTextBox, "This should be an organization (NPS, FWS, USGS, AKF&G,  etc)");
            // 
            // CollarIdTextBox
            // 
            this.CollarIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarIdTextBox.Enabled = false;
            this.CollarIdTextBox.Location = new System.Drawing.Point(294, 7);
            this.CollarIdTextBox.MaxLength = 50;
            this.CollarIdTextBox.Name = "CollarIdTextBox";
            this.CollarIdTextBox.ReadOnly = true;
            this.CollarIdTextBox.Size = new System.Drawing.Size(153, 20);
            this.CollarIdTextBox.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(46, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "Notes:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(247, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Model:";
            this.toolTip1.SetToolTip(this.label4, "Required, but currently not used.");
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Collar Id:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Manager:";
            this.toolTip1.SetToolTip(this.label1, "If you assign this collar to another PI you will not be able to edit it.");
            // 
            // DoneCancelButton
            // 
            this.DoneCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DoneCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneCancelButton.Location = new System.Drawing.Point(90, 305);
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
            this.EditSaveButton.Location = new System.Drawing.Point(368, 305);
            this.EditSaveButton.Name = "EditSaveButton";
            this.EditSaveButton.Size = new System.Drawing.Size(75, 23);
            this.EditSaveButton.TabIndex = 26;
            this.EditSaveButton.Text = "Edit";
            this.EditSaveButton.UseVisualStyleBackColor = true;
            this.EditSaveButton.Click += new System.EventHandler(this.EditSaveButton_Click);
            // 
            // ManufacturerTextBox
            // 
            this.ManufacturerTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ManufacturerTextBox.Enabled = false;
            this.ManufacturerTextBox.Location = new System.Drawing.Point(94, 7);
            this.ManufacturerTextBox.MaxLength = 50;
            this.ManufacturerTextBox.Name = "ManufacturerTextBox";
            this.ManufacturerTextBox.ReadOnly = true;
            this.ManufacturerTextBox.Size = new System.Drawing.Size(129, 20);
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
            this.DeploymentDataGridView.Location = new System.Drawing.Point(6, 6);
            this.DeploymentDataGridView.Name = "DeploymentDataGridView";
            this.DeploymentDataGridView.ReadOnly = true;
            this.DeploymentDataGridView.RowHeadersVisible = false;
            this.DeploymentDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DeploymentDataGridView.Size = new System.Drawing.Size(437, 293);
            this.DeploymentDataGridView.TabIndex = 11;
            this.DeploymentDataGridView.DoubleClick += new System.EventHandler(this.AnimalInfoButton_Click);
            // 
            // CollarManufacturerColumn
            // 
            this.CollarManufacturerColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.CollarManufacturerColumn.DataPropertyName = "Manufacturer";
            this.CollarManufacturerColumn.HeaderText = "Manufacturer";
            this.CollarManufacturerColumn.MinimumWidth = 60;
            this.CollarManufacturerColumn.Name = "CollarManufacturerColumn";
            this.CollarManufacturerColumn.ReadOnly = true;
            this.CollarManufacturerColumn.Visible = false;
            // 
            // CollarIdColumn
            // 
            this.CollarIdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.CollarIdColumn.DataPropertyName = "CollarId";
            this.CollarIdColumn.HeaderText = "Collar";
            this.CollarIdColumn.MinimumWidth = 60;
            this.CollarIdColumn.Name = "CollarIdColumn";
            this.CollarIdColumn.ReadOnly = true;
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
            this.DeploymentColumn.ReadOnly = true;
            this.DeploymentColumn.Visible = false;
            // 
            // DeployRetrieveButton
            // 
            this.DeployRetrieveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeployRetrieveButton.Location = new System.Drawing.Point(368, 305);
            this.DeployRetrieveButton.Name = "DeployRetrieveButton";
            this.DeployRetrieveButton.Size = new System.Drawing.Size(75, 23);
            this.DeployRetrieveButton.TabIndex = 24;
            this.DeployRetrieveButton.Text = "Deploy";
            this.DeployRetrieveButton.UseVisualStyleBackColor = true;
            this.DeployRetrieveButton.Click += new System.EventHandler(this.DeployRetrieveButton_Click);
            // 
            // DeleteDeploymentButton
            // 
            this.DeleteDeploymentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteDeploymentButton.Location = new System.Drawing.Point(288, 305);
            this.DeleteDeploymentButton.Name = "DeleteDeploymentButton";
            this.DeleteDeploymentButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteDeploymentButton.TabIndex = 22;
            this.DeleteDeploymentButton.Text = "Delete";
            this.DeleteDeploymentButton.UseVisualStyleBackColor = true;
            this.DeleteDeploymentButton.Click += new System.EventHandler(this.DeleteDeploymentButton_Click);
            // 
            // CollarTabs
            // 
            this.CollarTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarTabs.Controls.Add(this.GeneralTabPage);
            this.CollarTabs.Controls.Add(this.AnimalsTabPage);
            this.CollarTabs.Controls.Add(this.ArgosTabPage);
            this.CollarTabs.Controls.Add(this.ParametersTabPage);
            this.CollarTabs.Controls.Add(this.FilesTabPage);
            this.CollarTabs.Controls.Add(this.FixesTabPage);
            this.CollarTabs.Location = new System.Drawing.Point(0, 33);
            this.CollarTabs.Name = "CollarTabs";
            this.CollarTabs.SelectedIndex = 0;
            this.CollarTabs.Size = new System.Drawing.Size(459, 362);
            this.CollarTabs.TabIndex = 54;
            this.CollarTabs.SelectedIndexChanged += new System.EventHandler(this.CollarTabs_SelectedIndexChanged);
            // 
            // GeneralTabPage
            // 
            this.GeneralTabPage.Controls.Add(this.HasGpsCheckBox);
            this.GeneralTabPage.Controls.Add(this.label12);
            this.GeneralTabPage.Controls.Add(this.DisposalDateTimePicker);
            this.GeneralTabPage.Controls.Add(this.NotesTextBox);
            this.GeneralTabPage.Controls.Add(this.label1);
            this.GeneralTabPage.Controls.Add(this.DoneCancelButton);
            this.GeneralTabPage.Controls.Add(this.EditSaveButton);
            this.GeneralTabPage.Controls.Add(this.label4);
            this.GeneralTabPage.Controls.Add(this.label5);
            this.GeneralTabPage.Controls.Add(this.label9);
            this.GeneralTabPage.Controls.Add(this.FrequencyTextBox);
            this.GeneralTabPage.Controls.Add(this.OwnerTextBox);
            this.GeneralTabPage.Controls.Add(this.label8);
            this.GeneralTabPage.Controls.Add(this.SerialNumberTextBox);
            this.GeneralTabPage.Controls.Add(this.ModelComboBox);
            this.GeneralTabPage.Controls.Add(this.label7);
            this.GeneralTabPage.Controls.Add(this.ManagerComboBox);
            this.GeneralTabPage.Location = new System.Drawing.Point(4, 22);
            this.GeneralTabPage.Name = "GeneralTabPage";
            this.GeneralTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.GeneralTabPage.Size = new System.Drawing.Size(451, 336);
            this.GeneralTabPage.TabIndex = 0;
            this.GeneralTabPage.Text = "General";
            this.GeneralTabPage.UseVisualStyleBackColor = true;
            // 
            // HasGpsCheckBox
            // 
            this.HasGpsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HasGpsCheckBox.AutoSize = true;
            this.HasGpsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.HasGpsCheckBox.Location = new System.Drawing.Point(376, 61);
            this.HasGpsCheckBox.Name = "HasGpsCheckBox";
            this.HasGpsCheckBox.Size = new System.Drawing.Size(67, 17);
            this.HasGpsCheckBox.TabIndex = 83;
            this.HasGpsCheckBox.Text = "Has Gps";
            this.toolTip1.SetToolTip(this.HasGpsCheckBox, "Check this box if the collar has an onboard GPS");
            this.HasGpsCheckBox.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 65);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 13);
            this.label12.TabIndex = 82;
            this.label12.Text = "Disposal Date:";
            this.toolTip1.SetToolTip(this.label12, "The date the collar was lost, destroyed, or retired");
            // 
            // DisposalDateTimePicker
            // 
            this.DisposalDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DisposalDateTimePicker.Checked = false;
            this.DisposalDateTimePicker.CustomFormat = " ";
            this.DisposalDateTimePicker.Enabled = false;
            this.DisposalDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DisposalDateTimePicker.Location = new System.Drawing.Point(90, 62);
            this.DisposalDateTimePicker.Name = "DisposalDateTimePicker";
            this.DisposalDateTimePicker.ShowCheckBox = true;
            this.DisposalDateTimePicker.Size = new System.Drawing.Size(129, 20);
            this.DisposalDateTimePicker.TabIndex = 81;
            this.toolTip1.SetToolTip(this.DisposalDateTimePicker, "The date the collar was lost, destroyed, or retired");
            this.DisposalDateTimePicker.ValueChanged += new System.EventHandler(this.DisposalDateTimePicker_ValueChanged);
            // 
            // AnimalsTabPage
            // 
            this.AnimalsTabPage.Controls.Add(this.AnimalInfoButton);
            this.AnimalsTabPage.Controls.Add(this.DeploymentDataGridView);
            this.AnimalsTabPage.Controls.Add(this.DeleteDeploymentButton);
            this.AnimalsTabPage.Controls.Add(this.DeployRetrieveButton);
            this.AnimalsTabPage.Location = new System.Drawing.Point(4, 22);
            this.AnimalsTabPage.Name = "AnimalsTabPage";
            this.AnimalsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.AnimalsTabPage.Size = new System.Drawing.Size(451, 336);
            this.AnimalsTabPage.TabIndex = 1;
            this.AnimalsTabPage.Text = "Animals";
            this.AnimalsTabPage.UseVisualStyleBackColor = true;
            // 
            // AnimalInfoButton
            // 
            this.AnimalInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimalInfoButton.Location = new System.Drawing.Point(207, 305);
            this.AnimalInfoButton.Name = "AnimalInfoButton";
            this.AnimalInfoButton.Size = new System.Drawing.Size(75, 23);
            this.AnimalInfoButton.TabIndex = 25;
            this.AnimalInfoButton.Text = "Info";
            this.AnimalInfoButton.UseVisualStyleBackColor = true;
            this.AnimalInfoButton.Click += new System.EventHandler(this.AnimalInfoButton_Click);
            // 
            // ArgosTabPage
            // 
            this.ArgosTabPage.Location = new System.Drawing.Point(4, 22);
            this.ArgosTabPage.Name = "ArgosTabPage";
            this.ArgosTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ArgosTabPage.Size = new System.Drawing.Size(451, 336);
            this.ArgosTabPage.TabIndex = 4;
            this.ArgosTabPage.Text = "Argos";
            this.ArgosTabPage.UseVisualStyleBackColor = true;
            // 
            // ParametersTabPage
            // 
            this.ParametersTabPage.Location = new System.Drawing.Point(4, 22);
            this.ParametersTabPage.Name = "ParametersTabPage";
            this.ParametersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ParametersTabPage.Size = new System.Drawing.Size(451, 336);
            this.ParametersTabPage.TabIndex = 5;
            this.ParametersTabPage.Text = "Parameters";
            this.ParametersTabPage.UseVisualStyleBackColor = true;
            // 
            // FilesTabPage
            // 
            this.FilesTabPage.Controls.Add(this.ChangeFileStatusButton);
            this.FilesTabPage.Controls.Add(this.FileInfoButton);
            this.FilesTabPage.Controls.Add(this.FilesDataGridView);
            this.FilesTabPage.Location = new System.Drawing.Point(4, 22);
            this.FilesTabPage.Name = "FilesTabPage";
            this.FilesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.FilesTabPage.Size = new System.Drawing.Size(451, 336);
            this.FilesTabPage.TabIndex = 3;
            this.FilesTabPage.Text = "Files";
            this.FilesTabPage.UseVisualStyleBackColor = true;
            // 
            // ChangeFileStatusButton
            // 
            this.ChangeFileStatusButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ChangeFileStatusButton.Location = new System.Drawing.Point(287, 305);
            this.ChangeFileStatusButton.Name = "ChangeFileStatusButton";
            this.ChangeFileStatusButton.Size = new System.Drawing.Size(75, 23);
            this.ChangeFileStatusButton.TabIndex = 27;
            this.ChangeFileStatusButton.Text = "Deactivate";
            this.ChangeFileStatusButton.UseVisualStyleBackColor = true;
            this.ChangeFileStatusButton.Click += new System.EventHandler(this.ChangeFileStatusButton_Click);
            // 
            // FileInfoButton
            // 
            this.FileInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FileInfoButton.Location = new System.Drawing.Point(368, 305);
            this.FileInfoButton.Name = "FileInfoButton";
            this.FileInfoButton.Size = new System.Drawing.Size(75, 23);
            this.FileInfoButton.TabIndex = 26;
            this.FileInfoButton.Text = "Info";
            this.FileInfoButton.UseVisualStyleBackColor = true;
            this.FileInfoButton.Click += new System.EventHandler(this.FileInfoButton_Click);
            // 
            // FilesDataGridView
            // 
            this.FilesDataGridView.AllowUserToAddRows = false;
            this.FilesDataGridView.AllowUserToDeleteRows = false;
            this.FilesDataGridView.AllowUserToOrderColumns = true;
            this.FilesDataGridView.AllowUserToResizeRows = false;
            this.FilesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.FilesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FilesDataGridView.Location = new System.Drawing.Point(6, 6);
            this.FilesDataGridView.Name = "FilesDataGridView";
            this.FilesDataGridView.ReadOnly = true;
            this.FilesDataGridView.RowHeadersVisible = false;
            this.FilesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.FilesDataGridView.Size = new System.Drawing.Size(437, 293);
            this.FilesDataGridView.TabIndex = 0;
            this.FilesDataGridView.SelectionChanged += new System.EventHandler(this.FilesDataGridView_SelectionChanged);
            this.FilesDataGridView.DoubleClick += new System.EventHandler(this.FileInfoButton_Click);
            // 
            // FixesTabPage
            // 
            this.FixesTabPage.Controls.Add(this.UnhideFixButton);
            this.FixesTabPage.Controls.Add(this.FixConflictsDataGridView);
            this.FixesTabPage.Controls.Add(this.label11);
            this.FixesTabPage.Controls.Add(this.SummaryLabel);
            this.FixesTabPage.Location = new System.Drawing.Point(4, 22);
            this.FixesTabPage.Name = "FixesTabPage";
            this.FixesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.FixesTabPage.Size = new System.Drawing.Size(451, 336);
            this.FixesTabPage.TabIndex = 2;
            this.FixesTabPage.Text = "Fixes";
            this.FixesTabPage.UseVisualStyleBackColor = true;
            // 
            // UnhideFixButton
            // 
            this.UnhideFixButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UnhideFixButton.Location = new System.Drawing.Point(368, 305);
            this.UnhideFixButton.Name = "UnhideFixButton";
            this.UnhideFixButton.Size = new System.Drawing.Size(75, 23);
            this.UnhideFixButton.TabIndex = 3;
            this.UnhideFixButton.Text = "Unhide Fix";
            this.UnhideFixButton.UseVisualStyleBackColor = true;
            this.UnhideFixButton.Click += new System.EventHandler(this.UnhideFixButton_Click);
            // 
            // FixConflictsDataGridView
            // 
            this.FixConflictsDataGridView.AllowUserToAddRows = false;
            this.FixConflictsDataGridView.AllowUserToDeleteRows = false;
            this.FixConflictsDataGridView.AllowUserToOrderColumns = true;
            this.FixConflictsDataGridView.AllowUserToResizeRows = false;
            this.FixConflictsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FixConflictsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.FixConflictsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FixConflictsDataGridView.Location = new System.Drawing.Point(6, 51);
            this.FixConflictsDataGridView.MultiSelect = false;
            this.FixConflictsDataGridView.Name = "FixConflictsDataGridView";
            this.FixConflictsDataGridView.ReadOnly = true;
            this.FixConflictsDataGridView.RowHeadersVisible = false;
            this.FixConflictsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.FixConflictsDataGridView.Size = new System.Drawing.Size(437, 248);
            this.FixConflictsDataGridView.TabIndex = 2;
            this.FixConflictsDataGridView.SelectionChanged += new System.EventHandler(this.FixConflictsDataGridView_SelectionChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Conflicts:";
            // 
            // SummaryLabel
            // 
            this.SummaryLabel.AutoSize = true;
            this.SummaryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SummaryLabel.Location = new System.Drawing.Point(3, 9);
            this.SummaryLabel.Name = "SummaryLabel";
            this.SummaryLabel.Size = new System.Drawing.Size(72, 18);
            this.SummaryLabel.TabIndex = 0;
            this.SummaryLabel.Text = "Summary";
            // 
            // CollarDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 395);
            this.Controls.Add(this.CollarTabs);
            this.Controls.Add(this.ManufacturerTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CollarIdTextBox);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(475, 275);
            this.Name = "CollarDetailsForm";
            this.Text = "Collar Details";
            ((System.ComponentModel.ISupportInitialize)(this.DeploymentDataGridView)).EndInit();
            this.CollarTabs.ResumeLayout(false);
            this.GeneralTabPage.ResumeLayout(false);
            this.GeneralTabPage.PerformLayout();
            this.AnimalsTabPage.ResumeLayout(false);
            this.FilesTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FilesDataGridView)).EndInit();
            this.FixesTabPage.ResumeLayout(false);
            this.FixesTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FixConflictsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox FrequencyTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox SerialNumberTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ManagerComboBox;
        private System.Windows.Forms.ComboBox ModelComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox NotesTextBox;
        private System.Windows.Forms.TextBox OwnerTextBox;
        private System.Windows.Forms.TextBox CollarIdTextBox;
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
        private System.Windows.Forms.TabControl CollarTabs;
        private System.Windows.Forms.TabPage GeneralTabPage;
        private System.Windows.Forms.TabPage AnimalsTabPage;
        private System.Windows.Forms.TabPage FixesTabPage;
        private System.Windows.Forms.DataGridView FixConflictsDataGridView;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label SummaryLabel;
        private System.Windows.Forms.Button AnimalInfoButton;
        private System.Windows.Forms.Button UnhideFixButton;
        private System.Windows.Forms.TabPage FilesTabPage;
        private System.Windows.Forms.DataGridView FilesDataGridView;
        private System.Windows.Forms.Button FileInfoButton;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker DisposalDateTimePicker;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox HasGpsCheckBox;
        private System.Windows.Forms.Button ChangeFileStatusButton;
        private System.Windows.Forms.TabPage ArgosTabPage;
        private System.Windows.Forms.TabPage ParametersTabPage;
    }
}