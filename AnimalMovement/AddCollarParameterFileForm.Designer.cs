namespace AnimalMovement
{
    partial class AddCollarParameterFileForm
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.UploadButton = new System.Windows.Forms.Button();
            this.FormatComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.OwnerComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.StatusComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CreateParametersCheckBox = new System.Windows.Forms.CheckBox();
            this.CreateCollarsCheckBox = new System.Windows.Forms.CheckBox();
            this.IgnoreSuffixCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(116, 201);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // UploadButton
            // 
            this.UploadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UploadButton.Location = new System.Drawing.Point(197, 201);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(75, 23);
            this.UploadButton.TabIndex = 9;
            this.UploadButton.Text = "Upload";
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // FormatComboBox
            // 
            this.FormatComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FormatComboBox.FormattingEnabled = true;
            this.FormatComboBox.Location = new System.Drawing.Point(51, 39);
            this.FormatComboBox.Name = "FormatComboBox";
            this.FormatComboBox.Size = new System.Drawing.Size(221, 21);
            this.FormatComboBox.TabIndex = 1;
            this.FormatComboBox.SelectedIndexChanged += new System.EventHandler(this.FormatComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 29;
            this.label2.Text = "Format:";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseButton.Location = new System.Drawing.Point(248, 64);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(24, 23);
            this.BrowseButton.TabIndex = 3;
            this.BrowseButton.Text = "...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "File(s):";
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileNameTextBox.Location = new System.Drawing.Point(50, 66);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(192, 20);
            this.FileNameTextBox.TabIndex = 2;
            this.FileNameTextBox.TextChanged += new System.EventHandler(this.FileNameTextBox_TextChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "myfile.tpf";
            // 
            // OwnerComboBox
            // 
            this.OwnerComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OwnerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OwnerComboBox.FormattingEnabled = true;
            this.OwnerComboBox.Location = new System.Drawing.Point(51, 12);
            this.OwnerComboBox.Name = "OwnerComboBox";
            this.OwnerComboBox.Size = new System.Drawing.Size(221, 21);
            this.OwnerComboBox.TabIndex = 0;
            this.OwnerComboBox.SelectedIndexChanged += new System.EventHandler(this.OwnerComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Owner:";
            // 
            // StatusComboBox
            // 
            this.StatusComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StatusComboBox.FormattingEnabled = true;
            this.StatusComboBox.Location = new System.Drawing.Point(51, 91);
            this.StatusComboBox.Name = "StatusComboBox";
            this.StatusComboBox.Size = new System.Drawing.Size(221, 21);
            this.StatusComboBox.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "Status:";
            // 
            // CreateParametersCheckBox
            // 
            this.CreateParametersCheckBox.AutoSize = true;
            this.CreateParametersCheckBox.Checked = true;
            this.CreateParametersCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CreateParametersCheckBox.Location = new System.Drawing.Point(50, 118);
            this.CreateParametersCheckBox.Name = "CreateParametersCheckBox";
            this.CreateParametersCheckBox.Size = new System.Drawing.Size(179, 17);
            this.CreateParametersCheckBox.TabIndex = 5;
            this.CreateParametersCheckBox.Text = "Match TPF parameters to collars";
            this.CreateParametersCheckBox.UseVisualStyleBackColor = true;
            this.CreateParametersCheckBox.CheckedChanged += new System.EventHandler(this.CreateParametersCheckBox_CheckedChanged);
            // 
            // CreateCollarsCheckBox
            // 
            this.CreateCollarsCheckBox.AutoSize = true;
            this.CreateCollarsCheckBox.Checked = true;
            this.CreateCollarsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CreateCollarsCheckBox.Location = new System.Drawing.Point(72, 141);
            this.CreateCollarsCheckBox.Name = "CreateCollarsCheckBox";
            this.CreateCollarsCheckBox.Size = new System.Drawing.Size(135, 17);
            this.CreateCollarsCheckBox.TabIndex = 6;
            this.CreateCollarsCheckBox.Text = "Create collars if missing";
            this.CreateCollarsCheckBox.UseVisualStyleBackColor = true;
            // 
            // IgnoreSuffixCheckBox
            // 
            this.IgnoreSuffixCheckBox.AutoSize = true;
            this.IgnoreSuffixCheckBox.Checked = true;
            this.IgnoreSuffixCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IgnoreSuffixCheckBox.Location = new System.Drawing.Point(72, 164);
            this.IgnoreSuffixCheckBox.Name = "IgnoreSuffixCheckBox";
            this.IgnoreSuffixCheckBox.Size = new System.Drawing.Size(176, 17);
            this.IgnoreSuffixCheckBox.TabIndex = 7;
            this.IgnoreSuffixCheckBox.Text = "Ignore suffix on CTN for CollarId";
            this.IgnoreSuffixCheckBox.UseVisualStyleBackColor = true;
            // 
            // AddCollarParameterFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(284, 236);
            this.Controls.Add(this.IgnoreSuffixCheckBox);
            this.Controls.Add(this.CreateCollarsCheckBox);
            this.Controls.Add(this.CreateParametersCheckBox);
            this.Controls.Add(this.StatusComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.OwnerComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.UploadButton);
            this.Controls.Add(this.FormatComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileNameTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(281, 260);
            this.Name = "AddCollarParameterFileForm";
            this.Text = "Upload Collar Parameters";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button UploadButton;
        private System.Windows.Forms.ComboBox FormatComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ComboBox OwnerComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox StatusComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox CreateParametersCheckBox;
        private System.Windows.Forms.CheckBox CreateCollarsCheckBox;
        private System.Windows.Forms.CheckBox IgnoreSuffixCheckBox;

    }
}