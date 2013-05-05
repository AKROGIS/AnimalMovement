namespace AnimalMovement
{
    partial class ArgosPlatformDetailsForm
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
            this.DoneCancelButton = new System.Windows.Forms.Button();
            this.EditSaveButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.NotesTextBox = new System.Windows.Forms.TextBox();
            this.ActiveCheckBox = new System.Windows.Forms.CheckBox();
            this.ArgosProgramComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ArgosIdTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DisposalDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // DoneCancelButton
            // 
            this.DoneCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DoneCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneCancelButton.Location = new System.Drawing.Point(12, 181);
            this.DoneCancelButton.Name = "DoneCancelButton";
            this.DoneCancelButton.Size = new System.Drawing.Size(75, 23);
            this.DoneCancelButton.TabIndex = 29;
            this.DoneCancelButton.Text = "Done";
            this.DoneCancelButton.UseVisualStyleBackColor = true;
            this.DoneCancelButton.Click += new System.EventHandler(this.DoneCancelButton_Click);
            // 
            // EditSaveButton
            // 
            this.EditSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EditSaveButton.Location = new System.Drawing.Point(197, 181);
            this.EditSaveButton.Name = "EditSaveButton";
            this.EditSaveButton.Size = new System.Drawing.Size(75, 23);
            this.EditSaveButton.TabIndex = 28;
            this.EditSaveButton.Text = "Edit";
            this.EditSaveButton.UseVisualStyleBackColor = true;
            this.EditSaveButton.Click += new System.EventHandler(this.EditSaveButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Notes:";
            // 
            // NotesTextBox
            // 
            this.NotesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NotesTextBox.Location = new System.Drawing.Point(12, 111);
            this.NotesTextBox.Multiline = true;
            this.NotesTextBox.Name = "NotesTextBox";
            this.NotesTextBox.Size = new System.Drawing.Size(260, 64);
            this.NotesTextBox.TabIndex = 26;
            // 
            // ActiveCheckBox
            // 
            this.ActiveCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveCheckBox.AutoSize = true;
            this.ActiveCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ActiveCheckBox.Checked = true;
            this.ActiveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ActiveCheckBox.Location = new System.Drawing.Point(138, 91);
            this.ActiveCheckBox.Name = "ActiveCheckBox";
            this.ActiveCheckBox.Size = new System.Drawing.Size(134, 17);
            this.ActiveCheckBox.TabIndex = 25;
            this.ActiveCheckBox.Text = "Download this Platform";
            this.ActiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // ArgosProgramComboBox
            // 
            this.ArgosProgramComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ArgosProgramComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ArgosProgramComboBox.FormattingEnabled = true;
            this.ArgosProgramComboBox.Location = new System.Drawing.Point(119, 38);
            this.ArgosProgramComboBox.Name = "ArgosProgramComboBox";
            this.ArgosProgramComboBox.Size = new System.Drawing.Size(153, 21);
            this.ArgosProgramComboBox.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Argos Program:";
            // 
            // ArgosIdTextBox
            // 
            this.ArgosIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ArgosIdTextBox.Enabled = false;
            this.ArgosIdTextBox.Location = new System.Drawing.Point(119, 12);
            this.ArgosIdTextBox.Name = "ArgosIdTextBox";
            this.ArgosIdTextBox.Size = new System.Drawing.Size(153, 20);
            this.ArgosIdTextBox.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Argos Id:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Disposal Date/Time:";
            // 
            // DisposalDateTimePicker
            // 
            this.DisposalDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DisposalDateTimePicker.Checked = false;
            this.DisposalDateTimePicker.CustomFormat = " ";
            this.DisposalDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DisposalDateTimePicker.Location = new System.Drawing.Point(119, 65);
            this.DisposalDateTimePicker.Name = "DisposalDateTimePicker";
            this.DisposalDateTimePicker.ShowCheckBox = true;
            this.DisposalDateTimePicker.Size = new System.Drawing.Size(153, 20);
            this.DisposalDateTimePicker.TabIndex = 19;
            this.DisposalDateTimePicker.Value = new System.DateTime(2013, 4, 19, 0, 0, 0, 0);
            this.DisposalDateTimePicker.ValueChanged += new System.EventHandler(this.DisposalDateTimePicker_ValueChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipTitle = "Download state of the program  governs unless it is not set.";
            // 
            // ArgosPlatformDetailsForm
            // 
            this.AcceptButton = this.DoneCancelButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DoneCancelButton;
            this.ClientSize = new System.Drawing.Size(284, 216);
            this.Controls.Add(this.DoneCancelButton);
            this.Controls.Add(this.EditSaveButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.NotesTextBox);
            this.Controls.Add(this.ActiveCheckBox);
            this.Controls.Add(this.ArgosProgramComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ArgosIdTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DisposalDateTimePicker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(275, 225);
            this.Name = "ArgosPlatformDetailsForm";
            this.Text = "Argos Platform Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DoneCancelButton;
        private System.Windows.Forms.Button EditSaveButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox NotesTextBox;
        private System.Windows.Forms.CheckBox ActiveCheckBox;
        private System.Windows.Forms.ComboBox ArgosProgramComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ArgosIdTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker DisposalDateTimePicker;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}