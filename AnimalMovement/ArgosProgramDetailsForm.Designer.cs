namespace AnimalMovement
{
    partial class ArgosProgramDetailsForm
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
            this.label8 = new System.Windows.Forms.Label();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ProgramNameTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.EditSaveButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.NotesTextBox = new System.Windows.Forms.TextBox();
            this.ActiveCheckBox = new System.Windows.Forms.CheckBox();
            this.OwnerComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ProgramIdTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.PasswordMaskedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.AddMissingPlatformsButton = new System.Windows.Forms.Button();
            this.PlatformsGridView = new System.Windows.Forms.DataGridView();
            this.DeletePlatformButton = new System.Windows.Forms.Button();
            this.InfoPlatformButton = new System.Windows.Forms.Button();
            this.AddPlatformButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ArgosProgramTabControl = new System.Windows.Forms.TabControl();
            this.GeneralTabPage = new System.Windows.Forms.TabPage();
            this.PlatformsTabPage = new System.Windows.Forms.TabPage();
            this.DownloadsTabPage = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.DownloadsDataGridView = new System.Windows.Forms.DataGridView();
            this.CloseButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PlatformsGridView)).BeginInit();
            this.ArgosProgramTabControl.SuspendLayout();
            this.GeneralTabPage.SuspendLayout();
            this.PlatformsTabPage.SuspendLayout();
            this.DownloadsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DownloadsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 117);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 13);
            this.label8.TabIndex = 56;
            this.label8.Text = "Start Date/Time:";
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StartDateTimePicker.Checked = false;
            this.StartDateTimePicker.CustomFormat = " ";
            this.StartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDateTimePicker.Location = new System.Drawing.Point(113, 111);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.ShowCheckBox = true;
            this.StartDateTimePicker.Size = new System.Drawing.Size(233, 20);
            this.StartDateTimePicker.TabIndex = 55;
            this.StartDateTimePicker.Value = new System.DateTime(2013, 4, 19, 0, 0, 0, 0);
            this.StartDateTimePicker.ValueChanged += new System.EventHandler(this.StartDateTimePicker_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 13);
            this.label7.TabIndex = 53;
            this.label7.Text = "Argos Password:";
            // 
            // UserNameTextBox
            // 
            this.UserNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserNameTextBox.Location = new System.Drawing.Point(113, 59);
            this.UserNameTextBox.Name = "UserNameTextBox";
            this.UserNameTextBox.Size = new System.Drawing.Size(233, 20);
            this.UserNameTextBox.TabIndex = 52;
            this.UserNameTextBox.TextChanged += new System.EventHandler(this.UserNameTextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 13);
            this.label6.TabIndex = 51;
            this.label6.Text = "Argos Username:";
            // 
            // ProgramNameTextBox
            // 
            this.ProgramNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramNameTextBox.Location = new System.Drawing.Point(113, 33);
            this.ProgramNameTextBox.Name = "ProgramNameTextBox";
            this.ProgramNameTextBox.Size = new System.Drawing.Size(233, 20);
            this.ProgramNameTextBox.TabIndex = 50;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 49;
            this.label5.Text = "Program Name:";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.Location = new System.Drawing.Point(6, 232);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 48;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Visible = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // EditSaveButton
            // 
            this.EditSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EditSaveButton.Location = new System.Drawing.Point(271, 232);
            this.EditSaveButton.Name = "EditSaveButton";
            this.EditSaveButton.Size = new System.Drawing.Size(75, 23);
            this.EditSaveButton.TabIndex = 47;
            this.EditSaveButton.Text = "Edit";
            this.EditSaveButton.UseVisualStyleBackColor = true;
            this.EditSaveButton.Click += new System.EventHandler(this.EditSaveButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 46;
            this.label4.Text = "Notes:";
            // 
            // NotesTextBox
            // 
            this.NotesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NotesTextBox.Location = new System.Drawing.Point(6, 183);
            this.NotesTextBox.Multiline = true;
            this.NotesTextBox.Name = "NotesTextBox";
            this.NotesTextBox.Size = new System.Drawing.Size(340, 43);
            this.NotesTextBox.TabIndex = 45;
            // 
            // ActiveCheckBox
            // 
            this.ActiveCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveCheckBox.AutoSize = true;
            this.ActiveCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ActiveCheckBox.Checked = true;
            this.ActiveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ActiveCheckBox.Location = new System.Drawing.Point(212, 163);
            this.ActiveCheckBox.Name = "ActiveCheckBox";
            this.ActiveCheckBox.Size = new System.Drawing.Size(134, 17);
            this.ActiveCheckBox.TabIndex = 44;
            this.ActiveCheckBox.Text = "Download this program";
            this.ActiveCheckBox.ThreeState = true;
            this.toolTip1.SetToolTip(this.ActiveCheckBox, "Yes, No, or defer to the individual platforms");
            this.ActiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // OwnerComboBox
            // 
            this.OwnerComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OwnerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OwnerComboBox.FormattingEnabled = true;
            this.OwnerComboBox.Location = new System.Drawing.Point(113, 6);
            this.OwnerComboBox.Name = "OwnerComboBox";
            this.OwnerComboBox.Size = new System.Drawing.Size(233, 21);
            this.OwnerComboBox.TabIndex = 43;
            this.OwnerComboBox.SelectedIndexChanged += new System.EventHandler(this.OwnerComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "Project Investigator:";
            // 
            // ProgramIdTextBox
            // 
            this.ProgramIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramIdTextBox.Enabled = false;
            this.ProgramIdTextBox.Location = new System.Drawing.Point(119, 12);
            this.ProgramIdTextBox.Name = "ProgramIdTextBox";
            this.ProgramIdTextBox.Size = new System.Drawing.Size(253, 20);
            this.ProgramIdTextBox.TabIndex = 41;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Program Id:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 39;
            this.label3.Text = "End Date/Time:";
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EndDateTimePicker.Checked = false;
            this.EndDateTimePicker.CustomFormat = " ";
            this.EndDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndDateTimePicker.Location = new System.Drawing.Point(113, 137);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.ShowCheckBox = true;
            this.EndDateTimePicker.Size = new System.Drawing.Size(233, 20);
            this.EndDateTimePicker.TabIndex = 38;
            this.EndDateTimePicker.Value = new System.DateTime(2013, 4, 19, 0, 0, 0, 0);
            this.EndDateTimePicker.ValueChanged += new System.EventHandler(this.EndDateTimePicker_ValueChanged);
            // 
            // PasswordMaskedTextBox
            // 
            this.PasswordMaskedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PasswordMaskedTextBox.Location = new System.Drawing.Point(113, 85);
            this.PasswordMaskedTextBox.Name = "PasswordMaskedTextBox";
            this.PasswordMaskedTextBox.Size = new System.Drawing.Size(233, 20);
            this.PasswordMaskedTextBox.TabIndex = 57;
            this.PasswordMaskedTextBox.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.PasswordMaskedTextBox_MaskInputRejected);
            // 
            // AddMissingPlatformsButton
            // 
            this.AddMissingPlatformsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddMissingPlatformsButton.Location = new System.Drawing.Point(226, 232);
            this.AddMissingPlatformsButton.Name = "AddMissingPlatformsButton";
            this.AddMissingPlatformsButton.Size = new System.Drawing.Size(123, 23);
            this.AddMissingPlatformsButton.TabIndex = 58;
            this.AddMissingPlatformsButton.Text = "Add Missing Platforms";
            this.AddMissingPlatformsButton.UseVisualStyleBackColor = true;
            this.AddMissingPlatformsButton.Click += new System.EventHandler(this.AddMissingPlatformsButton_Click);
            // 
            // PlatformsGridView
            // 
            this.PlatformsGridView.AllowUserToAddRows = false;
            this.PlatformsGridView.AllowUserToDeleteRows = false;
            this.PlatformsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PlatformsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.PlatformsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PlatformsGridView.Location = new System.Drawing.Point(3, 3);
            this.PlatformsGridView.Name = "PlatformsGridView";
            this.PlatformsGridView.ReadOnly = true;
            this.PlatformsGridView.RowHeadersVisible = false;
            this.PlatformsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.PlatformsGridView.Size = new System.Drawing.Size(346, 226);
            this.PlatformsGridView.TabIndex = 59;
            this.PlatformsGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PlatformsGridView_CellDoubleClick);
            this.PlatformsGridView.SelectionChanged += new System.EventHandler(this.PlatformsGridView_SelectedIndexChanged);
            // 
            // DeletePlatformButton
            // 
            this.DeletePlatformButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeletePlatformButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeletePlatformButton.Image = global::AnimalMovement.Properties.Resources.GenericDeleteRed16;
            this.DeletePlatformButton.Location = new System.Drawing.Point(33, 234);
            this.DeletePlatformButton.Name = "DeletePlatformButton";
            this.DeletePlatformButton.Size = new System.Drawing.Size(24, 24);
            this.DeletePlatformButton.TabIndex = 61;
            this.DeletePlatformButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.DeletePlatformButton.UseVisualStyleBackColor = true;
            this.DeletePlatformButton.Click += new System.EventHandler(this.DeletePlatformButton_Click);
            // 
            // InfoPlatformButton
            // 
            this.InfoPlatformButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InfoPlatformButton.FlatAppearance.BorderSize = 0;
            this.InfoPlatformButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoPlatformButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoPlatformButton.Image = global::AnimalMovement.Properties.Resources.GenericInformation_B_16;
            this.InfoPlatformButton.Location = new System.Drawing.Point(58, 234);
            this.InfoPlatformButton.Name = "InfoPlatformButton";
            this.InfoPlatformButton.Size = new System.Drawing.Size(24, 24);
            this.InfoPlatformButton.TabIndex = 62;
            this.InfoPlatformButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.InfoPlatformButton.UseVisualStyleBackColor = true;
            this.InfoPlatformButton.Click += new System.EventHandler(this.InfoPlatformButton_Click);
            // 
            // AddPlatformButton
            // 
            this.AddPlatformButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddPlatformButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddPlatformButton.Image = global::AnimalMovement.Properties.Resources.GenericAddGreen16;
            this.AddPlatformButton.Location = new System.Drawing.Point(3, 234);
            this.AddPlatformButton.Name = "AddPlatformButton";
            this.AddPlatformButton.Size = new System.Drawing.Size(24, 24);
            this.AddPlatformButton.TabIndex = 60;
            this.AddPlatformButton.UseVisualStyleBackColor = true;
            this.AddPlatformButton.Click += new System.EventHandler(this.AddPlatformButton_Click);
            // 
            // ArgosProgramTabControl
            // 
            this.ArgosProgramTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ArgosProgramTabControl.Controls.Add(this.GeneralTabPage);
            this.ArgosProgramTabControl.Controls.Add(this.PlatformsTabPage);
            this.ArgosProgramTabControl.Controls.Add(this.DownloadsTabPage);
            this.ArgosProgramTabControl.Location = new System.Drawing.Point(12, 38);
            this.ArgosProgramTabControl.Name = "ArgosProgramTabControl";
            this.ArgosProgramTabControl.SelectedIndex = 0;
            this.ArgosProgramTabControl.Size = new System.Drawing.Size(360, 287);
            this.ArgosProgramTabControl.TabIndex = 63;
            this.ArgosProgramTabControl.SelectedIndexChanged += new System.EventHandler(this.ArgosTabControl_SelectedIndexChanged);
            // 
            // GeneralTabPage
            // 
            this.GeneralTabPage.Controls.Add(this.EndDateTimePicker);
            this.GeneralTabPage.Controls.Add(this.label3);
            this.GeneralTabPage.Controls.Add(this.cancelButton);
            this.GeneralTabPage.Controls.Add(this.label2);
            this.GeneralTabPage.Controls.Add(this.EditSaveButton);
            this.GeneralTabPage.Controls.Add(this.PasswordMaskedTextBox);
            this.GeneralTabPage.Controls.Add(this.OwnerComboBox);
            this.GeneralTabPage.Controls.Add(this.label8);
            this.GeneralTabPage.Controls.Add(this.ActiveCheckBox);
            this.GeneralTabPage.Controls.Add(this.StartDateTimePicker);
            this.GeneralTabPage.Controls.Add(this.NotesTextBox);
            this.GeneralTabPage.Controls.Add(this.label7);
            this.GeneralTabPage.Controls.Add(this.label4);
            this.GeneralTabPage.Controls.Add(this.UserNameTextBox);
            this.GeneralTabPage.Controls.Add(this.label5);
            this.GeneralTabPage.Controls.Add(this.label6);
            this.GeneralTabPage.Controls.Add(this.ProgramNameTextBox);
            this.GeneralTabPage.Location = new System.Drawing.Point(4, 22);
            this.GeneralTabPage.Name = "GeneralTabPage";
            this.GeneralTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.GeneralTabPage.Size = new System.Drawing.Size(352, 261);
            this.GeneralTabPage.TabIndex = 0;
            this.GeneralTabPage.Text = "General";
            this.GeneralTabPage.UseVisualStyleBackColor = true;
            // 
            // PlatformsTabPage
            // 
            this.PlatformsTabPage.Controls.Add(this.PlatformsGridView);
            this.PlatformsTabPage.Controls.Add(this.DeletePlatformButton);
            this.PlatformsTabPage.Controls.Add(this.AddMissingPlatformsButton);
            this.PlatformsTabPage.Controls.Add(this.InfoPlatformButton);
            this.PlatformsTabPage.Controls.Add(this.AddPlatformButton);
            this.PlatformsTabPage.Location = new System.Drawing.Point(4, 22);
            this.PlatformsTabPage.Name = "PlatformsTabPage";
            this.PlatformsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.PlatformsTabPage.Size = new System.Drawing.Size(352, 261);
            this.PlatformsTabPage.TabIndex = 1;
            this.PlatformsTabPage.Text = "Argos Platforms";
            this.PlatformsTabPage.UseVisualStyleBackColor = true;
            // 
            // DownloadsTabPage
            // 
            this.DownloadsTabPage.Controls.Add(this.label9);
            this.DownloadsTabPage.Controls.Add(this.DownloadsDataGridView);
            this.DownloadsTabPage.Location = new System.Drawing.Point(4, 22);
            this.DownloadsTabPage.Name = "DownloadsTabPage";
            this.DownloadsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.DownloadsTabPage.Size = new System.Drawing.Size(352, 261);
            this.DownloadsTabPage.TabIndex = 2;
            this.DownloadsTabPage.Text = "Downloads";
            this.DownloadsTabPage.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(435, 13);
            this.label9.TabIndex = 61;
            this.label9.Text = "Automatic Argos website downloads for the entire Argos program (max of 10 days av" +
    "ailable)";
            // 
            // DownloadsDataGridView
            // 
            this.DownloadsDataGridView.AllowUserToAddRows = false;
            this.DownloadsDataGridView.AllowUserToDeleteRows = false;
            this.DownloadsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DownloadsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DownloadsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DownloadsDataGridView.Location = new System.Drawing.Point(3, 28);
            this.DownloadsDataGridView.Name = "DownloadsDataGridView";
            this.DownloadsDataGridView.ReadOnly = true;
            this.DownloadsDataGridView.RowHeadersVisible = false;
            this.DownloadsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DownloadsDataGridView.Size = new System.Drawing.Size(346, 230);
            this.DownloadsDataGridView.TabIndex = 60;
            this.DownloadsDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DownloadsDataGridView_CellDoubleClick);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(297, 331);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 58;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ArgosProgramDetailsForm
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(384, 366);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ProgramIdTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ArgosProgramTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(375, 375);
            this.Name = "ArgosProgramDetailsForm";
            this.Text = "Argos Program Details";
            ((System.ComponentModel.ISupportInitialize)(this.PlatformsGridView)).EndInit();
            this.ArgosProgramTabControl.ResumeLayout(false);
            this.GeneralTabPage.ResumeLayout(false);
            this.GeneralTabPage.PerformLayout();
            this.PlatformsTabPage.ResumeLayout(false);
            this.DownloadsTabPage.ResumeLayout(false);
            this.DownloadsTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DownloadsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker StartDateTimePicker;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox UserNameTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ProgramNameTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button EditSaveButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox NotesTextBox;
        private System.Windows.Forms.CheckBox ActiveCheckBox;
        private System.Windows.Forms.ComboBox OwnerComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ProgramIdTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker EndDateTimePicker;
        private System.Windows.Forms.MaskedTextBox PasswordMaskedTextBox;
        private System.Windows.Forms.Button AddMissingPlatformsButton;
        private System.Windows.Forms.DataGridView PlatformsGridView;
        private System.Windows.Forms.Button DeletePlatformButton;
        private System.Windows.Forms.Button InfoPlatformButton;
        private System.Windows.Forms.Button AddPlatformButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage GeneralTabPage;
        private System.Windows.Forms.TabPage PlatformsTabPage;
        private System.Windows.Forms.TabPage DownloadsTabPage;
        private System.Windows.Forms.DataGridView DownloadsDataGridView;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Label label9;
        internal System.Windows.Forms.TabControl ArgosProgramTabControl;
    }
}