namespace AnimalMovement
{
    partial class UploadFilesForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FolderNameTextBox = new System.Windows.Forms.TextBox();
            this.AllowDuplicatesCheckBox = new System.Windows.Forms.CheckBox();
            this.FolderBrowserButton = new System.Windows.Forms.Button();
            this.FolderRadioButton = new System.Windows.Forms.RadioButton();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.FileBrowserButton = new System.Windows.Forms.Button();
            this.FileRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ClearCollarButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.InvestigatorComboBox = new System.Windows.Forms.ComboBox();
            this.CollarComboBox = new System.Windows.Forms.ComboBox();
            this.ProjectComboBox = new System.Windows.Forms.ComboBox();
            this.InvestigatorRadioButton = new System.Windows.Forms.RadioButton();
            this.ProjectRadioButton = new System.Windows.Forms.RadioButton();
            this.statusGroupBox = new System.Windows.Forms.GroupBox();
            this.StatusActiveRadioButton = new System.Windows.Forms.RadioButton();
            this.StatusInactiveRadioButton = new System.Windows.Forms.RadioButton();
            this.CancelButton = new System.Windows.Forms.Button();
            this.UploadButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.HelpButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.FolderNameTextBox);
            this.groupBox1.Controls.Add(this.AllowDuplicatesCheckBox);
            this.groupBox1.Controls.Add(this.FolderBrowserButton);
            this.groupBox1.Controls.Add(this.FolderRadioButton);
            this.groupBox1.Controls.Add(this.FileNameTextBox);
            this.groupBox1.Controls.Add(this.FileBrowserButton);
            this.groupBox1.Controls.Add(this.FileRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 93);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data source:";
            // 
            // FolderNameTextBox
            // 
            this.FolderNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FolderNameTextBox.Location = new System.Drawing.Point(69, 44);
            this.FolderNameTextBox.Name = "FolderNameTextBox";
            this.FolderNameTextBox.Size = new System.Drawing.Size(295, 20);
            this.FolderNameTextBox.TabIndex = 3;
            this.FolderNameTextBox.TextChanged += new System.EventHandler(this.FolderNameTextBox_TextChanged);
            // 
            // AllowDuplicatesCheckBox
            // 
            this.AllowDuplicatesCheckBox.AutoSize = true;
            this.AllowDuplicatesCheckBox.Location = new System.Drawing.Point(6, 70);
            this.AllowDuplicatesCheckBox.Name = "AllowDuplicatesCheckBox";
            this.AllowDuplicatesCheckBox.Size = new System.Drawing.Size(102, 17);
            this.AllowDuplicatesCheckBox.TabIndex = 5;
            this.AllowDuplicatesCheckBox.Text = "Allow duplicates";
            this.AllowDuplicatesCheckBox.UseVisualStyleBackColor = true;
            // 
            // FolderBrowserButton
            // 
            this.FolderBrowserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FolderBrowserButton.Location = new System.Drawing.Point(370, 41);
            this.FolderBrowserButton.Name = "FolderBrowserButton";
            this.FolderBrowserButton.Size = new System.Drawing.Size(24, 24);
            this.FolderBrowserButton.TabIndex = 4;
            this.FolderBrowserButton.Text = "...";
            this.FolderBrowserButton.UseVisualStyleBackColor = true;
            this.FolderBrowserButton.Click += new System.EventHandler(this.FolderBrowserButton_Click);
            // 
            // FolderRadioButton
            // 
            this.FolderRadioButton.AutoSize = true;
            this.FolderRadioButton.Location = new System.Drawing.Point(6, 45);
            this.FolderRadioButton.Name = "FolderRadioButton";
            this.FolderRadioButton.Size = new System.Drawing.Size(57, 17);
            this.FolderRadioButton.TabIndex = 3;
            this.FolderRadioButton.Text = "Folder:";
            this.FolderRadioButton.UseVisualStyleBackColor = true;
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileNameTextBox.Location = new System.Drawing.Point(69, 18);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(295, 20);
            this.FileNameTextBox.TabIndex = 1;
            this.FileNameTextBox.TextChanged += new System.EventHandler(this.FileNameTextBox_TextChanged);
            // 
            // FileBrowserButton
            // 
            this.FileBrowserButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FileBrowserButton.Location = new System.Drawing.Point(370, 15);
            this.FileBrowserButton.Name = "FileBrowserButton";
            this.FileBrowserButton.Size = new System.Drawing.Size(24, 24);
            this.FileBrowserButton.TabIndex = 2;
            this.FileBrowserButton.Text = "...";
            this.FileBrowserButton.UseVisualStyleBackColor = true;
            this.FileBrowserButton.Click += new System.EventHandler(this.FileBrowserButton_Click);
            // 
            // FileRadioButton
            // 
            this.FileRadioButton.AutoSize = true;
            this.FileRadioButton.Location = new System.Drawing.Point(6, 19);
            this.FileRadioButton.Name = "FileRadioButton";
            this.FileRadioButton.Size = new System.Drawing.Size(55, 17);
            this.FileRadioButton.TabIndex = 0;
            this.FileRadioButton.Text = "File(s):";
            this.FileRadioButton.UseVisualStyleBackColor = true;
            this.FileRadioButton.CheckedChanged += new System.EventHandler(this.FileRadioButton_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.ClearCollarButton);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.InvestigatorComboBox);
            this.groupBox2.Controls.Add(this.CollarComboBox);
            this.groupBox2.Controls.Add(this.ProjectComboBox);
            this.groupBox2.Controls.Add(this.InvestigatorRadioButton);
            this.groupBox2.Controls.Add(this.ProjectRadioButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 111);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(400, 98);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Associate file with:";
            // 
            // ClearCollarButton
            // 
            this.ClearCollarButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearCollarButton.Location = new System.Drawing.Point(349, 71);
            this.ClearCollarButton.Name = "ClearCollarButton";
            this.ClearCollarButton.Size = new System.Drawing.Size(45, 21);
            this.ClearCollarButton.TabIndex = 9;
            this.ClearCollarButton.Text = "Clear";
            this.ClearCollarButton.UseVisualStyleBackColor = true;
            this.ClearCollarButton.Click += new System.EventHandler(this.ClearCollarButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Collar:";
            // 
            // InvestigatorComboBox
            // 
            this.InvestigatorComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InvestigatorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InvestigatorComboBox.FormattingEnabled = true;
            this.InvestigatorComboBox.Location = new System.Drawing.Point(95, 44);
            this.InvestigatorComboBox.Name = "InvestigatorComboBox";
            this.InvestigatorComboBox.Size = new System.Drawing.Size(299, 21);
            this.InvestigatorComboBox.TabIndex = 7;
            this.InvestigatorComboBox.SelectedIndexChanged += new System.EventHandler(this.InvestigatorComboBox_SelectedIndexChanged);
            // 
            // CollarComboBox
            // 
            this.CollarComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CollarComboBox.FormattingEnabled = true;
            this.CollarComboBox.Location = new System.Drawing.Point(95, 71);
            this.CollarComboBox.Name = "CollarComboBox";
            this.CollarComboBox.Size = new System.Drawing.Size(248, 21);
            this.CollarComboBox.TabIndex = 8;
            this.CollarComboBox.SelectedIndexChanged += new System.EventHandler(this.CollarComboBox_SelectedIndexChanged);
            // 
            // ProjectComboBox
            // 
            this.ProjectComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProjectComboBox.FormattingEnabled = true;
            this.ProjectComboBox.Location = new System.Drawing.Point(95, 18);
            this.ProjectComboBox.Name = "ProjectComboBox";
            this.ProjectComboBox.Size = new System.Drawing.Size(299, 21);
            this.ProjectComboBox.TabIndex = 6;
            this.ProjectComboBox.SelectedIndexChanged += new System.EventHandler(this.ProjectComboBox_SelectedIndexChanged);
            // 
            // InvestigatorRadioButton
            // 
            this.InvestigatorRadioButton.AutoSize = true;
            this.InvestigatorRadioButton.Location = new System.Drawing.Point(6, 45);
            this.InvestigatorRadioButton.Name = "InvestigatorRadioButton";
            this.InvestigatorRadioButton.Size = new System.Drawing.Size(83, 17);
            this.InvestigatorRadioButton.TabIndex = 3;
            this.InvestigatorRadioButton.Text = "Investigator:";
            this.InvestigatorRadioButton.UseVisualStyleBackColor = true;
            // 
            // ProjectRadioButton
            // 
            this.ProjectRadioButton.AutoSize = true;
            this.ProjectRadioButton.Location = new System.Drawing.Point(6, 19);
            this.ProjectRadioButton.Name = "ProjectRadioButton";
            this.ProjectRadioButton.Size = new System.Drawing.Size(61, 17);
            this.ProjectRadioButton.TabIndex = 0;
            this.ProjectRadioButton.Text = "Project:";
            this.ProjectRadioButton.UseVisualStyleBackColor = true;
            this.ProjectRadioButton.CheckedChanged += new System.EventHandler(this.ProjectRadioButton_CheckedChanged);
            // 
            // statusGroupBox
            // 
            this.statusGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusGroupBox.Controls.Add(this.StatusActiveRadioButton);
            this.statusGroupBox.Controls.Add(this.StatusInactiveRadioButton);
            this.statusGroupBox.Location = new System.Drawing.Point(12, 215);
            this.statusGroupBox.Name = "statusGroupBox";
            this.statusGroupBox.Size = new System.Drawing.Size(400, 70);
            this.statusGroupBox.TabIndex = 8;
            this.statusGroupBox.TabStop = false;
            this.statusGroupBox.Text = "Status";
            // 
            // StatusActiveRadioButton
            // 
            this.StatusActiveRadioButton.AutoSize = true;
            this.StatusActiveRadioButton.Checked = true;
            this.StatusActiveRadioButton.Location = new System.Drawing.Point(7, 44);
            this.StatusActiveRadioButton.Name = "StatusActiveRadioButton";
            this.StatusActiveRadioButton.Size = new System.Drawing.Size(243, 17);
            this.StatusActiveRadioButton.TabIndex = 1;
            this.StatusActiveRadioButton.Tag = "A";
            this.StatusActiveRadioButton.Text = "Upload for archive and generation of locations";
            this.StatusActiveRadioButton.UseVisualStyleBackColor = true;
            // 
            // StatusInactiveRadioButton
            // 
            this.StatusInactiveRadioButton.AutoSize = true;
            this.StatusInactiveRadioButton.Location = new System.Drawing.Point(7, 20);
            this.StatusInactiveRadioButton.Name = "StatusInactiveRadioButton";
            this.StatusInactiveRadioButton.Size = new System.Drawing.Size(134, 17);
            this.StatusInactiveRadioButton.TabIndex = 0;
            this.StatusInactiveRadioButton.Tag = "I";
            this.StatusInactiveRadioButton.Text = "Upload for archive only";
            this.StatusInactiveRadioButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(256, 291);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 13;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // UploadButton
            // 
            this.UploadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UploadButton.Location = new System.Drawing.Point(337, 291);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(75, 23);
            this.UploadButton.TabIndex = 14;
            this.UploadButton.Text = "Upload";
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "csv";
            this.openFileDialog.FileName = "CollarData";
            this.openFileDialog.Filter = "CSV File|*.csv|DAT File|*.dat|TSV File|*.tsv|Text Files|*.txt|All Files|*.*";
            // 
            // HelpButton
            // 
            this.HelpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.HelpButton.Location = new System.Drawing.Point(175, 291);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(75, 23);
            this.HelpButton.TabIndex = 12;
            this.HelpButton.Text = "Help";
            this.HelpButton.UseVisualStyleBackColor = true;
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // UploadFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 326);
            this.Controls.Add(this.HelpButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.UploadButton);
            this.Controls.Add(this.statusGroupBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(300, 364);
            this.Name = "UploadFilesForm";
            this.Text = "Upload Collar Location Data";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusGroupBox.ResumeLayout(false);
            this.statusGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox FolderNameTextBox;
        private System.Windows.Forms.Button FolderBrowserButton;
        private System.Windows.Forms.RadioButton FolderRadioButton;
        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.Button FileBrowserButton;
        private System.Windows.Forms.RadioButton FileRadioButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox InvestigatorComboBox;
        private System.Windows.Forms.ComboBox ProjectComboBox;
        private System.Windows.Forms.RadioButton InvestigatorRadioButton;
        private System.Windows.Forms.RadioButton ProjectRadioButton;
        private System.Windows.Forms.CheckBox AllowDuplicatesCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CollarComboBox;
        private System.Windows.Forms.GroupBox statusGroupBox;
        private System.Windows.Forms.RadioButton StatusActiveRadioButton;
        private System.Windows.Forms.RadioButton StatusInactiveRadioButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button UploadButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button ClearCollarButton;
        private System.Windows.Forms.Button HelpButton;
    }
}