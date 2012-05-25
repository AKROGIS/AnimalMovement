namespace TestUI
{
    partial class AddFileForm
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
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.FormatComboBox = new System.Windows.Forms.ComboBox();
            this.statusGroupBox = new System.Windows.Forms.GroupBox();
            this.StatusActiveRadioButton = new System.Windows.Forms.RadioButton();
            this.StatusInactiveRadioButton = new System.Windows.Forms.RadioButton();
            this.UploadButton = new System.Windows.Forms.Button();
            this.CollarMfgrComboBox = new System.Windows.Forms.ComboBox();
            this.ProjectComboBox = new System.Windows.Forms.ComboBox();
            this.CollarComboBox = new System.Windows.Forms.ComboBox();
            this.statusGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileNameTextBox.Location = new System.Drawing.Point(80, 12);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(262, 20);
            this.FileNameTextBox.TabIndex = 0;
            this.toolTip1.SetToolTip(this.FileNameTextBox, "File to import into the database");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "File:";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseButton.Location = new System.Drawing.Point(348, 10);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(24, 23);
            this.BrowseButton.TabIndex = 2;
            this.BrowseButton.Text = "...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "csv";
            this.openFileDialog.FileName = "CollarData";
            this.openFileDialog.Filter = "CSV File|*.csv|Text Files|*.txt|All Files|*.*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Format:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Collar Mfgr:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Collar Id:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Project:";
            // 
            // FormatComboBox
            // 
            this.FormatComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FormatComboBox.FormattingEnabled = true;
            this.FormatComboBox.Location = new System.Drawing.Point(81, 39);
            this.FormatComboBox.Name = "FormatComboBox";
            this.FormatComboBox.Size = new System.Drawing.Size(261, 21);
            this.FormatComboBox.TabIndex = 8;
            this.FormatComboBox.SelectedIndexChanged += new System.EventHandler(this.FormatComboBox_SelectedIndexChanged);
            // 
            // statusGroupBox
            // 
            this.statusGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusGroupBox.Controls.Add(this.StatusActiveRadioButton);
            this.statusGroupBox.Controls.Add(this.StatusInactiveRadioButton);
            this.statusGroupBox.Location = new System.Drawing.Point(80, 146);
            this.statusGroupBox.Name = "statusGroupBox";
            this.statusGroupBox.Size = new System.Drawing.Size(262, 70);
            this.statusGroupBox.TabIndex = 12;
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
            this.StatusActiveRadioButton.TabStop = true;
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
            // UploadButton
            // 
            this.UploadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UploadButton.Location = new System.Drawing.Point(296, 225);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(75, 23);
            this.UploadButton.TabIndex = 13;
            this.UploadButton.Text = "Upload";
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // CollarMfgrComboBox
            // 
            this.CollarMfgrComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarMfgrComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CollarMfgrComboBox.FormattingEnabled = true;
            this.CollarMfgrComboBox.Location = new System.Drawing.Point(80, 93);
            this.CollarMfgrComboBox.Name = "CollarMfgrComboBox";
            this.CollarMfgrComboBox.Size = new System.Drawing.Size(261, 21);
            this.CollarMfgrComboBox.TabIndex = 14;
            this.CollarMfgrComboBox.SelectedIndexChanged += new System.EventHandler(this.CollarMfgrComboBox_SelectedIndexChanged);
            // 
            // ProjectComboBox
            // 
            this.ProjectComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProjectComboBox.FormattingEnabled = true;
            this.ProjectComboBox.Location = new System.Drawing.Point(81, 66);
            this.ProjectComboBox.Name = "ProjectComboBox";
            this.ProjectComboBox.Size = new System.Drawing.Size(261, 21);
            this.ProjectComboBox.TabIndex = 15;
            this.ProjectComboBox.SelectionChangeCommitted += new System.EventHandler(this.ProjectComboBox_SelectionChangeCommitted);
            // 
            // CollarComboBox
            // 
            this.CollarComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CollarComboBox.FormattingEnabled = true;
            this.CollarComboBox.Location = new System.Drawing.Point(80, 119);
            this.CollarComboBox.Name = "CollarComboBox";
            this.CollarComboBox.Size = new System.Drawing.Size(261, 21);
            this.CollarComboBox.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 260);
            this.Controls.Add(this.CollarComboBox);
            this.Controls.Add(this.ProjectComboBox);
            this.Controls.Add(this.CollarMfgrComboBox);
            this.Controls.Add(this.UploadButton);
            this.Controls.Add(this.statusGroupBox);
            this.Controls.Add(this.FormatComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileNameTextBox);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 298);
            this.Name = "Form1";
            this.Text = "Add Collar Data File";
            this.statusGroupBox.ResumeLayout(false);
            this.statusGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox FormatComboBox;
        private System.Windows.Forms.GroupBox statusGroupBox;
        private System.Windows.Forms.RadioButton StatusActiveRadioButton;
        private System.Windows.Forms.RadioButton StatusInactiveRadioButton;
        private System.Windows.Forms.Button UploadButton;
        private System.Windows.Forms.ComboBox CollarMfgrComboBox;
        private System.Windows.Forms.ComboBox ProjectComboBox;
        private System.Windows.Forms.ComboBox CollarComboBox;
    }
}

