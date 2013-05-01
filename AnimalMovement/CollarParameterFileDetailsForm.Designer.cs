namespace AnimalMovement
{
    partial class CollarParameterFileDetailsForm
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
            this.DoneCancelButton = new System.Windows.Forms.Button();
            this.EditSaveButton = new System.Windows.Forms.Button();
            this.ParametersDataGridView = new System.Windows.Forms.DataGridView();
            this.CollarColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CollarIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CollarManufacturerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CollarParameterFileColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShowContentsButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FileIdTextBox = new System.Windows.Forms.TextBox();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.UploadDateTextBox = new System.Windows.Forms.TextBox();
            this.FormatTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.OwnerComboBox = new System.Windows.Forms.ComboBox();
            this.StatusComboBox = new System.Windows.Forms.ComboBox();
            this.FileTabControl = new System.Windows.Forms.TabControl();
            this.ParametersTabPage = new System.Windows.Forms.TabPage();
            this.TpfDetailsTabPage = new System.Windows.Forms.TabPage();
            this.TpfDataGridView = new System.Windows.Forms.DataGridView();
            this.ValidationTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.AddParameterButton = new System.Windows.Forms.Button();
            this.DeleteParameterButton = new System.Windows.Forms.Button();
            this.InfoParameterButton = new System.Windows.Forms.Button();
            this.EditParameterButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ParametersDataGridView)).BeginInit();
            this.FileTabControl.SuspendLayout();
            this.ParametersTabPage.SuspendLayout();
            this.TpfDetailsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TpfDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // DoneCancelButton
            // 
            this.DoneCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DoneCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneCancelButton.Location = new System.Drawing.Point(16, 344);
            this.DoneCancelButton.Name = "DoneCancelButton";
            this.DoneCancelButton.Size = new System.Drawing.Size(75, 23);
            this.DoneCancelButton.TabIndex = 70;
            this.DoneCancelButton.Text = "Done";
            this.DoneCancelButton.UseVisualStyleBackColor = true;
            this.DoneCancelButton.Click += new System.EventHandler(this.DoneCancelButton_Click);
            // 
            // EditSaveButton
            // 
            this.EditSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EditSaveButton.BackColor = System.Drawing.SystemColors.Control;
            this.EditSaveButton.Location = new System.Drawing.Point(394, 344);
            this.EditSaveButton.Name = "EditSaveButton";
            this.EditSaveButton.Size = new System.Drawing.Size(75, 23);
            this.EditSaveButton.TabIndex = 71;
            this.EditSaveButton.Text = "Edit";
            this.EditSaveButton.UseVisualStyleBackColor = true;
            this.EditSaveButton.Click += new System.EventHandler(this.EditSaveButton_Click);
            // 
            // ParametersDataGridView
            // 
            this.ParametersDataGridView.AllowUserToAddRows = false;
            this.ParametersDataGridView.AllowUserToDeleteRows = false;
            this.ParametersDataGridView.AllowUserToOrderColumns = true;
            this.ParametersDataGridView.AllowUserToResizeRows = false;
            this.ParametersDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ParametersDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ParametersDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CollarColumn,
            this.CollarIdColumn,
            this.CollarManufacturerColumn,
            this.FileIdColumn,
            this.CollarParameterFileColumn,
            this.StartDateColumn,
            this.EndDateColumn});
            this.ParametersDataGridView.Location = new System.Drawing.Point(3, 3);
            this.ParametersDataGridView.MultiSelect = false;
            this.ParametersDataGridView.Name = "ParametersDataGridView";
            this.ParametersDataGridView.ReadOnly = true;
            this.ParametersDataGridView.RowHeadersVisible = false;
            this.ParametersDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ParametersDataGridView.Size = new System.Drawing.Size(439, 119);
            this.ParametersDataGridView.TabIndex = 60;
            this.ParametersDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.CollarsDataGridView_CellMouseDoubleClick);
            // 
            // CollarColumn
            // 
            this.CollarColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CollarColumn.DataPropertyName = "Collar";
            this.CollarColumn.HeaderText = "Collar";
            this.CollarColumn.Name = "CollarColumn";
            this.CollarColumn.ReadOnly = true;
            this.CollarColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // CollarIdColumn
            // 
            this.CollarIdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CollarIdColumn.DataPropertyName = "CollarId";
            this.CollarIdColumn.HeaderText = "CollarId";
            this.CollarIdColumn.MinimumWidth = 70;
            this.CollarIdColumn.Name = "CollarIdColumn";
            this.CollarIdColumn.ReadOnly = true;
            this.CollarIdColumn.Visible = false;
            // 
            // CollarManufacturerColumn
            // 
            this.CollarManufacturerColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CollarManufacturerColumn.DataPropertyName = "CollarManufacturer";
            this.CollarManufacturerColumn.HeaderText = "Manufacturer";
            this.CollarManufacturerColumn.MinimumWidth = 80;
            this.CollarManufacturerColumn.Name = "CollarManufacturerColumn";
            this.CollarManufacturerColumn.ReadOnly = true;
            this.CollarManufacturerColumn.Visible = false;
            // 
            // FileIdColumn
            // 
            this.FileIdColumn.DataPropertyName = "FileId";
            this.FileIdColumn.HeaderText = "File Id";
            this.FileIdColumn.Name = "FileIdColumn";
            this.FileIdColumn.ReadOnly = true;
            this.FileIdColumn.Visible = false;
            // 
            // CollarParameterFileColumn
            // 
            this.CollarParameterFileColumn.DataPropertyName = "CollarParameterFile";
            this.CollarParameterFileColumn.HeaderText = "File";
            this.CollarParameterFileColumn.Name = "CollarParameterFileColumn";
            this.CollarParameterFileColumn.ReadOnly = true;
            this.CollarParameterFileColumn.Visible = false;
            // 
            // StartDateColumn
            // 
            this.StartDateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.StartDateColumn.DataPropertyName = "StartDate";
            this.StartDateColumn.HeaderText = "Start Date";
            this.StartDateColumn.MinimumWidth = 110;
            this.StartDateColumn.Name = "StartDateColumn";
            this.StartDateColumn.ReadOnly = true;
            this.StartDateColumn.Width = 110;
            // 
            // EndDateColumn
            // 
            this.EndDateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.EndDateColumn.DataPropertyName = "EndDate";
            this.EndDateColumn.HeaderText = "End Date";
            this.EndDateColumn.MinimumWidth = 110;
            this.EndDateColumn.Name = "EndDateColumn";
            this.EndDateColumn.ReadOnly = true;
            this.EndDateColumn.Width = 110;
            // 
            // ShowContentsButton
            // 
            this.ShowContentsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowContentsButton.Location = new System.Drawing.Point(307, 116);
            this.ShowContentsButton.Name = "ShowContentsButton";
            this.ShowContentsButton.Size = new System.Drawing.Size(162, 23);
            this.ShowContentsButton.TabIndex = 59;
            this.ShowContentsButton.Text = "Show Contents";
            this.ShowContentsButton.UseVisualStyleBackColor = true;
            this.ShowContentsButton.Click += new System.EventHandler(this.ShowContentsButton_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(230, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 13);
            this.label9.TabIndex = 69;
            this.label9.Text = "Uploaded By:";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(231, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 13);
            this.label8.TabIndex = 68;
            this.label8.Text = "Upload Date:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(263, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 66;
            this.label3.Text = "File Id:";
            // 
            // FileIdTextBox
            // 
            this.FileIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FileIdTextBox.Enabled = false;
            this.FileIdTextBox.Location = new System.Drawing.Point(307, 12);
            this.FileIdTextBox.Name = "FileIdTextBox";
            this.FileIdTextBox.Size = new System.Drawing.Size(162, 20);
            this.FileIdTextBox.TabIndex = 49;
            // 
            // UserNameTextBox
            // 
            this.UserNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UserNameTextBox.Enabled = false;
            this.UserNameTextBox.Location = new System.Drawing.Point(307, 90);
            this.UserNameTextBox.Name = "UserNameTextBox";
            this.UserNameTextBox.Size = new System.Drawing.Size(162, 20);
            this.UserNameTextBox.TabIndex = 56;
            // 
            // UploadDateTextBox
            // 
            this.UploadDateTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UploadDateTextBox.Enabled = false;
            this.UploadDateTextBox.Location = new System.Drawing.Point(307, 64);
            this.UploadDateTextBox.Name = "UploadDateTextBox";
            this.UploadDateTextBox.Size = new System.Drawing.Size(162, 20);
            this.UploadDateTextBox.TabIndex = 54;
            // 
            // FormatTextBox
            // 
            this.FormatTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FormatTextBox.Enabled = false;
            this.FormatTextBox.Location = new System.Drawing.Point(307, 38);
            this.FormatTextBox.Name = "FormatTextBox";
            this.FormatTextBox.Size = new System.Drawing.Size(162, 20);
            this.FormatTextBox.TabIndex = 53;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 65;
            this.label6.Text = "Owner:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(259, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "Format:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 61;
            this.label1.Text = "Name:";
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileNameTextBox.Enabled = false;
            this.FileNameTextBox.Location = new System.Drawing.Point(62, 12);
            this.FileNameTextBox.MaxLength = 255;
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(151, 20);
            this.FileNameTextBox.TabIndex = 51;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 73;
            this.label4.Text = "Status:";
            // 
            // OwnerComboBox
            // 
            this.OwnerComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OwnerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OwnerComboBox.Enabled = false;
            this.OwnerComboBox.FormattingEnabled = true;
            this.OwnerComboBox.Location = new System.Drawing.Point(62, 37);
            this.OwnerComboBox.Name = "OwnerComboBox";
            this.OwnerComboBox.Size = new System.Drawing.Size(151, 21);
            this.OwnerComboBox.TabIndex = 74;
            // 
            // StatusComboBox
            // 
            this.StatusComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StatusComboBox.Enabled = false;
            this.StatusComboBox.FormattingEnabled = true;
            this.StatusComboBox.Location = new System.Drawing.Point(62, 64);
            this.StatusComboBox.Name = "StatusComboBox";
            this.StatusComboBox.Size = new System.Drawing.Size(151, 21);
            this.StatusComboBox.TabIndex = 77;
            // 
            // FileTabControl
            // 
            this.FileTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileTabControl.Controls.Add(this.ParametersTabPage);
            this.FileTabControl.Controls.Add(this.TpfDetailsTabPage);
            this.FileTabControl.Location = new System.Drawing.Point(16, 137);
            this.FileTabControl.Name = "FileTabControl";
            this.FileTabControl.SelectedIndex = 0;
            this.FileTabControl.Size = new System.Drawing.Size(453, 181);
            this.FileTabControl.TabIndex = 79;
            this.FileTabControl.Tag = "";
            // 
            // ParametersTabPage
            // 
            this.ParametersTabPage.Controls.Add(this.EditParameterButton);
            this.ParametersTabPage.Controls.Add(this.AddParameterButton);
            this.ParametersTabPage.Controls.Add(this.DeleteParameterButton);
            this.ParametersTabPage.Controls.Add(this.InfoParameterButton);
            this.ParametersTabPage.Controls.Add(this.ParametersDataGridView);
            this.ParametersTabPage.Location = new System.Drawing.Point(4, 22);
            this.ParametersTabPage.Name = "ParametersTabPage";
            this.ParametersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ParametersTabPage.Size = new System.Drawing.Size(445, 155);
            this.ParametersTabPage.TabIndex = 3;
            this.ParametersTabPage.Tag = "Argos";
            this.ParametersTabPage.Text = "Collars using file";
            this.ParametersTabPage.UseVisualStyleBackColor = true;
            // 
            // TpfDetailsTabPage
            // 
            this.TpfDetailsTabPage.Controls.Add(this.TpfDataGridView);
            this.TpfDetailsTabPage.Location = new System.Drawing.Point(4, 22);
            this.TpfDetailsTabPage.Name = "TpfDetailsTabPage";
            this.TpfDetailsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.TpfDetailsTabPage.Size = new System.Drawing.Size(445, 155);
            this.TpfDetailsTabPage.TabIndex = 1;
            this.TpfDetailsTabPage.Tag = "DerivedFiles";
            this.TpfDetailsTabPage.Text = "TPF Details";
            this.TpfDetailsTabPage.UseVisualStyleBackColor = true;
            // 
            // TpfDataGridView
            // 
            this.TpfDataGridView.AllowUserToAddRows = false;
            this.TpfDataGridView.AllowUserToDeleteRows = false;
            this.TpfDataGridView.AllowUserToOrderColumns = true;
            this.TpfDataGridView.AllowUserToResizeRows = false;
            this.TpfDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TpfDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.TpfDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TpfDataGridView.Location = new System.Drawing.Point(2, 3);
            this.TpfDataGridView.MultiSelect = false;
            this.TpfDataGridView.Name = "TpfDataGridView";
            this.TpfDataGridView.ReadOnly = true;
            this.TpfDataGridView.RowHeadersVisible = false;
            this.TpfDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.TpfDataGridView.Size = new System.Drawing.Size(440, 149);
            this.TpfDataGridView.TabIndex = 51;
            // 
            // ValidationTextBox
            // 
            this.ValidationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ValidationTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.ValidationTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ValidationTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValidationTextBox.ForeColor = System.Drawing.Color.Red;
            this.ValidationTextBox.Location = new System.Drawing.Point(16, 324);
            this.ValidationTextBox.Name = "ValidationTextBox";
            this.ValidationTextBox.ReadOnly = true;
            this.ValidationTextBox.Size = new System.Drawing.Size(453, 14);
            this.ValidationTextBox.TabIndex = 78;
            this.ValidationTextBox.Tag = "";
            this.ValidationTextBox.Text = "Validation Error";
            this.ValidationTextBox.Visible = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(249, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 80;
            this.label5.Text = "Contents:";
            // 
            // AddParameterButton
            // 
            this.AddParameterButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddParameterButton.Enabled = false;
            this.AddParameterButton.Image = global::AnimalMovement.Properties.Resources.GenericAddGreen16;
            this.AddParameterButton.Location = new System.Drawing.Point(3, 128);
            this.AddParameterButton.Name = "AddParameterButton";
            this.AddParameterButton.Size = new System.Drawing.Size(24, 24);
            this.AddParameterButton.TabIndex = 61;
            this.AddParameterButton.UseVisualStyleBackColor = true;
            this.AddParameterButton.Click += new System.EventHandler(this.AddParameterButton_Click);
            // 
            // DeleteParameterButton
            // 
            this.DeleteParameterButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteParameterButton.Enabled = false;
            this.DeleteParameterButton.Image = global::AnimalMovement.Properties.Resources.GenericDeleteRed16;
            this.DeleteParameterButton.Location = new System.Drawing.Point(30, 128);
            this.DeleteParameterButton.Name = "DeleteParameterButton";
            this.DeleteParameterButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteParameterButton.TabIndex = 62;
            this.DeleteParameterButton.UseVisualStyleBackColor = true;
            this.DeleteParameterButton.Click += new System.EventHandler(this.DeleteParameterButton_Click);
            // 
            // InfoParameterButton
            // 
            this.InfoParameterButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InfoParameterButton.FlatAppearance.BorderSize = 0;
            this.InfoParameterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoParameterButton.Image = global::AnimalMovement.Properties.Resources.GenericInformation_B_16;
            this.InfoParameterButton.Location = new System.Drawing.Point(82, 128);
            this.InfoParameterButton.Name = "InfoParameterButton";
            this.InfoParameterButton.Size = new System.Drawing.Size(24, 24);
            this.InfoParameterButton.TabIndex = 63;
            this.InfoParameterButton.UseVisualStyleBackColor = true;
            this.InfoParameterButton.Click += new System.EventHandler(this.InfoParameterButton_Click);
            // 
            // EditParameterButton
            // 
            this.EditParameterButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.EditParameterButton.FlatAppearance.BorderSize = 0;
            this.EditParameterButton.Image = global::AnimalMovement.Properties.Resources.GenericPencil16;
            this.EditParameterButton.Location = new System.Drawing.Point(57, 128);
            this.EditParameterButton.Name = "EditParameterButton";
            this.EditParameterButton.Size = new System.Drawing.Size(24, 24);
            this.EditParameterButton.TabIndex = 64;
            this.EditParameterButton.UseVisualStyleBackColor = true;
            this.EditParameterButton.Click += new System.EventHandler(this.EditParameterButton_Click);
            // 
            // CollarParameterFileDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 375);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ValidationTextBox);
            this.Controls.Add(this.StatusComboBox);
            this.Controls.Add(this.OwnerComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DoneCancelButton);
            this.Controls.Add(this.EditSaveButton);
            this.Controls.Add(this.ShowContentsButton);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.FileIdTextBox);
            this.Controls.Add(this.UserNameTextBox);
            this.Controls.Add(this.UploadDateTextBox);
            this.Controls.Add(this.FormatTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileNameTextBox);
            this.Controls.Add(this.FileTabControl);
            this.MinimumSize = new System.Drawing.Size(490, 360);
            this.Name = "CollarParameterFileDetailsForm";
            this.Text = "Collar Parameter File Details";
            ((System.ComponentModel.ISupportInitialize)(this.ParametersDataGridView)).EndInit();
            this.FileTabControl.ResumeLayout(false);
            this.ParametersTabPage.ResumeLayout(false);
            this.TpfDetailsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TpfDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DoneCancelButton;
        private System.Windows.Forms.Button EditSaveButton;
        private System.Windows.Forms.DataGridView ParametersDataGridView;
        private System.Windows.Forms.Button ShowContentsButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FileIdTextBox;
        private System.Windows.Forms.TextBox UserNameTextBox;
        private System.Windows.Forms.TextBox UploadDateTextBox;
        private System.Windows.Forms.TextBox FormatTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollarColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollarIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollarManufacturerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollarParameterFileColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDateColumn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox OwnerComboBox;
        private System.Windows.Forms.ComboBox StatusComboBox;
        private System.Windows.Forms.TabControl FileTabControl;
        private System.Windows.Forms.TabPage ParametersTabPage;
        private System.Windows.Forms.TabPage TpfDetailsTabPage;
        private System.Windows.Forms.DataGridView TpfDataGridView;
        private System.Windows.Forms.TextBox ValidationTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button AddParameterButton;
        private System.Windows.Forms.Button DeleteParameterButton;
        private System.Windows.Forms.Button InfoParameterButton;
        private System.Windows.Forms.Button EditParameterButton;

    }
}