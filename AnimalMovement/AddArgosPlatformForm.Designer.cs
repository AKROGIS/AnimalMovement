namespace AnimalMovement
{
    partial class AddArgosPlatformForm
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
            this.CreateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.NotesTextBox = new System.Windows.Forms.TextBox();
            this.ActiveCheckBox = new System.Windows.Forms.CheckBox();
            this.ArgosProgramComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ArgosIdTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DisposalDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // CreateButton
            // 
            this.CreateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateButton.Location = new System.Drawing.Point(197, 181);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(75, 23);
            this.CreateButton.TabIndex = 6;
            this.CreateButton.Text = "Create";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(12, 181);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 16;
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
            this.NotesTextBox.TabIndex = 4;
            // 
            // ActiveCheckBox
            // 
            this.ActiveCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveCheckBox.AutoSize = true;
            this.ActiveCheckBox.Checked = true;
            this.ActiveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ActiveCheckBox.Location = new System.Drawing.Point(216, 91);
            this.ActiveCheckBox.Name = "ActiveCheckBox";
            this.ActiveCheckBox.Size = new System.Drawing.Size(56, 17);
            this.ActiveCheckBox.TabIndex = 3;
            this.ActiveCheckBox.Text = "Active";
            this.ActiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // ArgosProgramComboBox
            // 
            this.ArgosProgramComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ArgosProgramComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ArgosProgramComboBox.FormattingEnabled = true;
            this.ArgosProgramComboBox.Location = new System.Drawing.Point(119, 12);
            this.ArgosProgramComboBox.Name = "ArgosProgramComboBox";
            this.ArgosProgramComboBox.Size = new System.Drawing.Size(153, 21);
            this.ArgosProgramComboBox.TabIndex = 0;
            this.ArgosProgramComboBox.SelectedIndexChanged += new System.EventHandler(this.ArgosProgramComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Argos Program:";
            // 
            // ArgosIdTextBox
            // 
            this.ArgosIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ArgosIdTextBox.Location = new System.Drawing.Point(119, 39);
            this.ArgosIdTextBox.Name = "ArgosIdTextBox";
            this.ArgosIdTextBox.Size = new System.Drawing.Size(153, 20);
            this.ArgosIdTextBox.TabIndex = 1;
            this.ArgosIdTextBox.TextChanged += new System.EventHandler(this.ArgosIdTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Argos Id:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 9;
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
            this.DisposalDateTimePicker.TabIndex = 2;
            this.DisposalDateTimePicker.Value = new System.DateTime(2013, 4, 19, 0, 0, 0, 0);
            this.DisposalDateTimePicker.ValueChanged += new System.EventHandler(this.DisposalDateTimePicker_ValueChanged);
            // 
            // AddArgosPlatformForm
            // 
            this.AcceptButton = this.CreateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(284, 216);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.CreateButton);
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
            this.Name = "AddArgosPlatformForm";
            this.Text = "Add Argos Platform";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker DisposalDateTimePicker;
        private System.Windows.Forms.ComboBox ArgosProgramComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ArgosIdTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ActiveCheckBox;
        private System.Windows.Forms.TextBox NotesTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button CreateButton;
    }
}