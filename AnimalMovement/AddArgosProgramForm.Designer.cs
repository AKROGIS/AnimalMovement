namespace AnimalMovement
{
    partial class AddArgosProgramForm
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
            this.CreateButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.NotesTextBox = new System.Windows.Forms.TextBox();
            this.ActiveCheckBox = new System.Windows.Forms.CheckBox();
            this.OwnerComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ProgramIdTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.ProgramNameTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(12, 281);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 29;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // CreateButton
            // 
            this.CreateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateButton.Location = new System.Drawing.Point(247, 281);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(75, 23);
            this.CreateButton.TabIndex = 28;
            this.CreateButton.Text = "Create";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 196);
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
            this.NotesTextBox.Location = new System.Drawing.Point(12, 215);
            this.NotesTextBox.Multiline = true;
            this.NotesTextBox.Name = "NotesTextBox";
            this.NotesTextBox.Size = new System.Drawing.Size(310, 60);
            this.NotesTextBox.TabIndex = 26;
            // 
            // ActiveCheckBox
            // 
            this.ActiveCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveCheckBox.AutoSize = true;
            this.ActiveCheckBox.Checked = true;
            this.ActiveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ActiveCheckBox.Location = new System.Drawing.Point(266, 195);
            this.ActiveCheckBox.Name = "ActiveCheckBox";
            this.ActiveCheckBox.Size = new System.Drawing.Size(56, 17);
            this.ActiveCheckBox.TabIndex = 25;
            this.ActiveCheckBox.Text = "Active";
            this.ActiveCheckBox.ThreeState = true;
            this.ActiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // OwnerComboBox
            // 
            this.OwnerComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OwnerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OwnerComboBox.FormattingEnabled = true;
            this.OwnerComboBox.Location = new System.Drawing.Point(119, 12);
            this.OwnerComboBox.Name = "OwnerComboBox";
            this.OwnerComboBox.Size = new System.Drawing.Size(203, 21);
            this.OwnerComboBox.TabIndex = 24;
            this.OwnerComboBox.SelectedIndexChanged += new System.EventHandler(this.OwnerComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Project Investigator:";
            // 
            // ProgramIdTextBox
            // 
            this.ProgramIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramIdTextBox.Location = new System.Drawing.Point(119, 39);
            this.ProgramIdTextBox.Name = "ProgramIdTextBox";
            this.ProgramIdTextBox.Size = new System.Drawing.Size(203, 20);
            this.ProgramIdTextBox.TabIndex = 22;
            this.ProgramIdTextBox.TextChanged += new System.EventHandler(this.ProgramIdTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Program Id:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "End Date/Time:";
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EndDateTimePicker.Checked = false;
            this.EndDateTimePicker.CustomFormat = " ";
            this.EndDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndDateTimePicker.Location = new System.Drawing.Point(119, 169);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.ShowCheckBox = true;
            this.EndDateTimePicker.Size = new System.Drawing.Size(203, 20);
            this.EndDateTimePicker.TabIndex = 19;
            this.EndDateTimePicker.Value = new System.DateTime(2013, 4, 19, 0, 0, 0, 0);
            this.EndDateTimePicker.ValueChanged += new System.EventHandler(this.EndDateTimePicker_ValueChanged);
            // 
            // ProgramNameTextBox
            // 
            this.ProgramNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramNameTextBox.Location = new System.Drawing.Point(119, 65);
            this.ProgramNameTextBox.Name = "ProgramNameTextBox";
            this.ProgramNameTextBox.Size = new System.Drawing.Size(203, 20);
            this.ProgramNameTextBox.TabIndex = 31;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Program Name:";
            // 
            // UserNameTextBox
            // 
            this.UserNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserNameTextBox.Location = new System.Drawing.Point(119, 91);
            this.UserNameTextBox.Name = "UserNameTextBox";
            this.UserNameTextBox.Size = new System.Drawing.Size(203, 20);
            this.UserNameTextBox.TabIndex = 33;
            this.UserNameTextBox.TextChanged += new System.EventHandler(this.UserNameTextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "Argos Username:";
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PasswordTextBox.Location = new System.Drawing.Point(119, 117);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(203, 20);
            this.PasswordTextBox.TabIndex = 35;
            this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Argos Password:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(27, 149);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 13);
            this.label8.TabIndex = 37;
            this.label8.Text = "Start Date/Time:";
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StartDateTimePicker.Checked = false;
            this.StartDateTimePicker.CustomFormat = " ";
            this.StartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDateTimePicker.Location = new System.Drawing.Point(119, 143);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.ShowCheckBox = true;
            this.StartDateTimePicker.Size = new System.Drawing.Size(203, 20);
            this.StartDateTimePicker.TabIndex = 36;
            this.StartDateTimePicker.Value = new System.DateTime(2013, 4, 19, 0, 0, 0, 0);
            this.StartDateTimePicker.ValueChanged += new System.EventHandler(this.StartDateTimePicker_ValueChanged);
            // 
            // AddArgosProgramForm
            // 
            this.AcceptButton = this.CreateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(334, 316);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.StartDateTimePicker);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.UserNameTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ProgramNameTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.NotesTextBox);
            this.Controls.Add(this.ActiveCheckBox);
            this.Controls.Add(this.OwnerComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ProgramIdTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.EndDateTimePicker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 335);
            this.Name = "AddArgosProgramForm";
            this.Text = "Add Argos Program";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox NotesTextBox;
        private System.Windows.Forms.CheckBox ActiveCheckBox;
        private System.Windows.Forms.ComboBox OwnerComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ProgramIdTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker EndDateTimePicker;
        private System.Windows.Forms.TextBox ProgramNameTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox UserNameTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker StartDateTimePicker;
    }
}