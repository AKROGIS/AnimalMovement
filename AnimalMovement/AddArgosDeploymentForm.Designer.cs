namespace AnimalMovement
{
    partial class AddArgosDeploymentForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.CreateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ArgosComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ValidationTextBox = new System.Windows.Forms.TextBox();
            this.FixItButton = new System.Windows.Forms.Button();
            this.CollarComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Collar:";
            // 
            // CreateButton
            // 
            this.CreateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateButton.Location = new System.Drawing.Point(227, 158);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(75, 23);
            this.CreateButton.TabIndex = 2;
            this.CreateButton.Text = "Create";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(146, 158);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EndDateTimePicker.Checked = false;
            this.EndDateTimePicker.CustomFormat = " ";
            this.EndDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndDateTimePicker.Location = new System.Drawing.Point(102, 91);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.ShowCheckBox = true;
            this.EndDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.EndDateTimePicker.TabIndex = 4;
            this.EndDateTimePicker.Value = new System.DateTime(2013, 4, 19, 0, 0, 0, 0);
            this.EndDateTimePicker.ValueChanged += new System.EventHandler(this.EndDateTimePicker_ValueChanged);
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StartDateTimePicker.Checked = false;
            this.StartDateTimePicker.CustomFormat = " ";
            this.StartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDateTimePicker.Location = new System.Drawing.Point(102, 65);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.ShowCheckBox = true;
            this.StartDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.StartDateTimePicker.TabIndex = 5;
            this.StartDateTimePicker.ValueChanged += new System.EventHandler(this.StartDateTimePicker_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Argos Id:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "End Date/Time:";
            // 
            // ArgosComboBox
            // 
            this.ArgosComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ArgosComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ArgosComboBox.FormattingEnabled = true;
            this.ArgosComboBox.Location = new System.Drawing.Point(102, 38);
            this.ArgosComboBox.Name = "ArgosComboBox";
            this.ArgosComboBox.Size = new System.Drawing.Size(200, 21);
            this.ArgosComboBox.TabIndex = 8;
            this.ArgosComboBox.SelectedIndexChanged += new System.EventHandler(this.ArgosComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Start Date/Time:";
            // 
            // ValidationTextBox
            // 
            this.ValidationTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.ValidationTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ValidationTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValidationTextBox.ForeColor = System.Drawing.Color.Red;
            this.ValidationTextBox.Location = new System.Drawing.Point(12, 117);
            this.ValidationTextBox.Multiline = true;
            this.ValidationTextBox.Name = "ValidationTextBox";
            this.ValidationTextBox.ReadOnly = true;
            this.ValidationTextBox.Size = new System.Drawing.Size(290, 35);
            this.ValidationTextBox.TabIndex = 11;
            this.ValidationTextBox.Tag = "";
            this.ValidationTextBox.Text = "Validation Error";
            this.ValidationTextBox.Visible = false;
            // 
            // FixItButton
            // 
            this.FixItButton.Location = new System.Drawing.Point(12, 158);
            this.FixItButton.Name = "FixItButton";
            this.FixItButton.Size = new System.Drawing.Size(75, 23);
            this.FixItButton.TabIndex = 12;
            this.FixItButton.Text = "Fix It";
            this.FixItButton.UseVisualStyleBackColor = true;
            this.FixItButton.Visible = false;
            this.FixItButton.Click += new System.EventHandler(this.FixItButton_Click);
            // 
            // CollarComboBox
            // 
            this.CollarComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CollarComboBox.FormattingEnabled = true;
            this.CollarComboBox.Location = new System.Drawing.Point(102, 12);
            this.CollarComboBox.Name = "CollarComboBox";
            this.CollarComboBox.Size = new System.Drawing.Size(200, 21);
            this.CollarComboBox.TabIndex = 13;
            this.CollarComboBox.SelectedIndexChanged += new System.EventHandler(this.CollarComboBox_SelectedIndexChanged);
            // 
            // AddArgosDeploymentForm
            // 
            this.AcceptButton = this.CreateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(314, 193);
            this.Controls.Add(this.CollarComboBox);
            this.Controls.Add(this.FixItButton);
            this.Controls.Add(this.ValidationTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ArgosComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.StartDateTimePicker);
            this.Controls.Add(this.EndDateTimePicker);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AddArgosDeploymentForm";
            this.Text = "Add Argos on Collar Deployment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.DateTimePicker EndDateTimePicker;
        private System.Windows.Forms.DateTimePicker StartDateTimePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ArgosComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ValidationTextBox;
        private System.Windows.Forms.Button FixItButton;
        private System.Windows.Forms.ComboBox CollarComboBox;
    }
}