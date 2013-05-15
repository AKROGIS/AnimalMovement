namespace AnimalMovement
{
    partial class CollarDeploymentDetailsForm
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
            this.CollarComboBox = new System.Windows.Forms.ComboBox();
            this.FixItButton = new System.Windows.Forms.Button();
            this.ValidationTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.AnimalComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CollarComboBox
            // 
            this.CollarComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CollarComboBox.FormattingEnabled = true;
            this.CollarComboBox.Location = new System.Drawing.Point(102, 12);
            this.CollarComboBox.Name = "CollarComboBox";
            this.CollarComboBox.Size = new System.Drawing.Size(200, 21);
            this.CollarComboBox.TabIndex = 0;
            this.CollarComboBox.SelectedIndexChanged += new System.EventHandler(this.CollarComboBox_SelectedIndexChanged);
            // 
            // FixItButton
            // 
            this.FixItButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FixItButton.Location = new System.Drawing.Point(12, 159);
            this.FixItButton.Name = "FixItButton";
            this.FixItButton.Size = new System.Drawing.Size(75, 23);
            this.FixItButton.TabIndex = 4;
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
            this.ValidationTextBox.Location = new System.Drawing.Point(12, 118);
            this.ValidationTextBox.Multiline = true;
            this.ValidationTextBox.Name = "ValidationTextBox";
            this.ValidationTextBox.ReadOnly = true;
            this.ValidationTextBox.Size = new System.Drawing.Size(290, 35);
            this.ValidationTextBox.TabIndex = 35;
            this.ValidationTextBox.Tag = "";
            this.ValidationTextBox.Text = "Validation Error";
            this.ValidationTextBox.Visible = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "Start Date/Time:";
            // 
            // AnimalComboBox
            // 
            this.AnimalComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AnimalComboBox.FormattingEnabled = true;
            this.AnimalComboBox.Location = new System.Drawing.Point(102, 39);
            this.AnimalComboBox.Name = "AnimalComboBox";
            this.AnimalComboBox.Size = new System.Drawing.Size(200, 21);
            this.AnimalComboBox.TabIndex = 1;
            this.AnimalComboBox.SelectedIndexChanged += new System.EventHandler(this.AnimalComboBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "End Date/Time:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Animal:";
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StartDateTimePicker.Checked = false;
            this.StartDateTimePicker.CustomFormat = " MMM-d-yyyy h:mm tt";
            this.StartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDateTimePicker.Location = new System.Drawing.Point(102, 66);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.StartDateTimePicker.TabIndex = 2;
            this.StartDateTimePicker.ValueChanged += new System.EventHandler(this.StartDateTimePicker_ValueChanged);
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EndDateTimePicker.Checked = false;
            this.EndDateTimePicker.CustomFormat = " ";
            this.EndDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndDateTimePicker.Location = new System.Drawing.Point(102, 92);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.ShowCheckBox = true;
            this.EndDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.EndDateTimePicker.TabIndex = 3;
            this.EndDateTimePicker.Value = new System.DateTime(2013, 4, 19, 0, 0, 0, 0);
            this.EndDateTimePicker.ValueChanged += new System.EventHandler(this.EndDateTimePicker_ValueChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(146, 159);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(227, 159);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 6;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "Collar:";
            // 
            // CollarDeploymentDetailsForm
            // 
            this.AcceptButton = this.cancelButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(314, 196);
            this.Controls.Add(this.CollarComboBox);
            this.Controls.Add(this.FixItButton);
            this.Controls.Add(this.ValidationTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AnimalComboBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.StartDateTimePicker);
            this.Controls.Add(this.EndDateTimePicker);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CollarDeploymentDetailsForm";
            this.Text = "Edit Collar on Animal Deployment";
            this.Load += new System.EventHandler(this.CollarDeploymentDetailsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CollarComboBox;
        private System.Windows.Forms.Button FixItButton;
        private System.Windows.Forms.TextBox ValidationTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox AnimalComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker StartDateTimePicker;
        private System.Windows.Forms.DateTimePicker EndDateTimePicker;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label label1;
    }
}