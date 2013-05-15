namespace AnimalMovement
{
    partial class CollarParametersDetailsForm
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
            this.ClearFileButton = new System.Windows.Forms.Button();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.Gen3TimeUnitComboBox = new System.Windows.Forms.ComboBox();
            this.Gen3PeriodTextBox = new System.Windows.Forms.TextBox();
            this.Gen3Label = new System.Windows.Forms.Label();
            this.FixItButton = new System.Windows.Forms.Button();
            this.ValidationTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.FileComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CollarComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // ClearFileButton
            // 
            this.ClearFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearFileButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ClearFileButton.Location = new System.Drawing.Point(246, 38);
            this.ClearFileButton.Name = "ClearFileButton";
            this.ClearFileButton.Size = new System.Drawing.Size(47, 23);
            this.ClearFileButton.TabIndex = 2;
            this.ClearFileButton.Text = "Clear";
            this.ClearFileButton.UseVisualStyleBackColor = true;
            this.ClearFileButton.Click += new System.EventHandler(this.ClearFileButton_Click);
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseButton.Location = new System.Drawing.Point(297, 38);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(25, 23);
            this.BrowseButton.TabIndex = 3;
            this.BrowseButton.Text = "...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // Gen3TimeUnitComboBox
            // 
            this.Gen3TimeUnitComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Gen3TimeUnitComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Gen3TimeUnitComboBox.Items.AddRange(new object[] {
            "Hours",
            "Minutes"});
            this.Gen3TimeUnitComboBox.Location = new System.Drawing.Point(247, 117);
            this.Gen3TimeUnitComboBox.Name = "Gen3TimeUnitComboBox";
            this.Gen3TimeUnitComboBox.Size = new System.Drawing.Size(75, 21);
            this.Gen3TimeUnitComboBox.TabIndex = 7;
            // 
            // Gen3PeriodTextBox
            // 
            this.Gen3PeriodTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Gen3PeriodTextBox.Location = new System.Drawing.Point(122, 117);
            this.Gen3PeriodTextBox.Name = "Gen3PeriodTextBox";
            this.Gen3PeriodTextBox.Size = new System.Drawing.Size(119, 20);
            this.Gen3PeriodTextBox.TabIndex = 6;
            this.Gen3PeriodTextBox.Text = "24";
            this.Gen3PeriodTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Gen3PeriodTextBox.TextChanged += new System.EventHandler(this.Gen3PeriodTextBox_TextChanged);
            // 
            // Gen3Label
            // 
            this.Gen3Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Gen3Label.AutoSize = true;
            this.Gen3Label.Location = new System.Drawing.Point(11, 120);
            this.Gen3Label.Name = "Gen3Label";
            this.Gen3Label.Size = new System.Drawing.Size(105, 13);
            this.Gen3Label.TabIndex = 42;
            this.Gen3Label.Text = "Time Between Fixes:";
            // 
            // FixItButton
            // 
            this.FixItButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FixItButton.Location = new System.Drawing.Point(12, 188);
            this.FixItButton.Name = "FixItButton";
            this.FixItButton.Size = new System.Drawing.Size(75, 23);
            this.FixItButton.TabIndex = 8;
            this.FixItButton.Text = "Fix It";
            this.FixItButton.UseVisualStyleBackColor = true;
            this.FixItButton.Visible = false;
            this.FixItButton.Click += new System.EventHandler(this.FixItButton_Click);
            // 
            // ValidationTextBox
            // 
            this.ValidationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ValidationTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.ValidationTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ValidationTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValidationTextBox.ForeColor = System.Drawing.Color.Red;
            this.ValidationTextBox.Location = new System.Drawing.Point(16, 147);
            this.ValidationTextBox.Multiline = true;
            this.ValidationTextBox.Name = "ValidationTextBox";
            this.ValidationTextBox.ReadOnly = true;
            this.ValidationTextBox.Size = new System.Drawing.Size(305, 35);
            this.ValidationTextBox.TabIndex = 40;
            this.ValidationTextBox.Tag = "";
            this.ValidationTextBox.Text = "Validation Error";
            this.ValidationTextBox.Visible = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "Start Date/Time:";
            // 
            // FileComboBox
            // 
            this.FileComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FileComboBox.FormattingEnabled = true;
            this.FileComboBox.Location = new System.Drawing.Point(121, 39);
            this.FileComboBox.Name = "FileComboBox";
            this.FileComboBox.Size = new System.Drawing.Size(119, 21);
            this.FileComboBox.TabIndex = 1;
            this.FileComboBox.SelectedIndexChanged += new System.EventHandler(this.FileComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "End Date/Time:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 36;
            this.label2.Text = "Parameter File:";
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StartDateTimePicker.Checked = false;
            this.StartDateTimePicker.CustomFormat = " ";
            this.StartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDateTimePicker.Location = new System.Drawing.Point(121, 65);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.ShowCheckBox = true;
            this.StartDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.StartDateTimePicker.TabIndex = 4;
            this.StartDateTimePicker.ValueChanged += new System.EventHandler(this.StartDateTimePicker_ValueChanged);
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EndDateTimePicker.Checked = false;
            this.EndDateTimePicker.CustomFormat = " ";
            this.EndDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndDateTimePicker.Location = new System.Drawing.Point(121, 91);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.ShowCheckBox = true;
            this.EndDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.EndDateTimePicker.TabIndex = 5;
            this.EndDateTimePicker.Value = new System.DateTime(2013, 4, 19, 0, 0, 0, 0);
            this.EndDateTimePicker.ValueChanged += new System.EventHandler(this.EndDateTimePicker_ValueChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(166, 188);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(247, 188);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 10;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Collar:";
            // 
            // CollarComboBox
            // 
            this.CollarComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CollarComboBox.FormattingEnabled = true;
            this.CollarComboBox.Location = new System.Drawing.Point(122, 12);
            this.CollarComboBox.Name = "CollarComboBox";
            this.CollarComboBox.Size = new System.Drawing.Size(200, 21);
            this.CollarComboBox.TabIndex = 0;
            this.CollarComboBox.SelectedIndexChanged += new System.EventHandler(this.CollarComboBox_SelectedIndexChanged);
            // 
            // CollarParametersDetailsForm
            // 
            this.AcceptButton = this.cancelButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(334, 224);
            this.Controls.Add(this.CollarComboBox);
            this.Controls.Add(this.ClearFileButton);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.Gen3TimeUnitComboBox);
            this.Controls.Add(this.Gen3PeriodTextBox);
            this.Controls.Add(this.Gen3Label);
            this.Controls.Add(this.FixItButton);
            this.Controls.Add(this.ValidationTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.FileComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.StartDateTimePicker);
            this.Controls.Add(this.EndDateTimePicker);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CollarParametersDetailsForm";
            this.Text = "Collar Parameters";
            this.Load += new System.EventHandler(this.CollarParametersDetailsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ClearFileButton;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.ComboBox Gen3TimeUnitComboBox;
        private System.Windows.Forms.TextBox Gen3PeriodTextBox;
        private System.Windows.Forms.Label Gen3Label;
        private System.Windows.Forms.Button FixItButton;
        private System.Windows.Forms.TextBox ValidationTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox FileComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker StartDateTimePicker;
        private System.Windows.Forms.DateTimePicker EndDateTimePicker;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CollarComboBox;
    }
}