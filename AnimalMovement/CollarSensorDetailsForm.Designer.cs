namespace AnimalMovement
{
    partial class CollarSensorDetailsForm
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
            this.CollarIdTextBox = new System.Windows.Forms.TextBox();
            this.CollarManufacturerTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CreateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.IsActiveCheckBox = new System.Windows.Forms.CheckBox();
            this.CollarSensorTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CollarIdTextBox
            // 
            this.CollarIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarIdTextBox.Enabled = false;
            this.CollarIdTextBox.Location = new System.Drawing.Point(93, 39);
            this.CollarIdTextBox.MaxLength = 16;
            this.CollarIdTextBox.Name = "CollarIdTextBox";
            this.CollarIdTextBox.Size = new System.Drawing.Size(186, 20);
            this.CollarIdTextBox.TabIndex = 1;
            // 
            // CollarManufacturerTextBox
            // 
            this.CollarManufacturerTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarManufacturerTextBox.Enabled = false;
            this.CollarManufacturerTextBox.Location = new System.Drawing.Point(93, 12);
            this.CollarManufacturerTextBox.MaxLength = 500;
            this.CollarManufacturerTextBox.Name = "CollarManufacturerTextBox";
            this.CollarManufacturerTextBox.Size = new System.Drawing.Size(186, 20);
            this.CollarManufacturerTextBox.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(42, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 31;
            this.label8.Text = "Active:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Sensor:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Collar Id:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Manufacturer:";
            // 
            // CreateButton
            // 
            this.CreateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateButton.Location = new System.Drawing.Point(204, 126);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(75, 23);
            this.CreateButton.TabIndex = 8;
            this.CreateButton.Text = "Update";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(123, 126);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // IsActiveCheckBox
            // 
            this.IsActiveCheckBox.AutoSize = true;
            this.IsActiveCheckBox.Checked = true;
            this.IsActiveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsActiveCheckBox.Location = new System.Drawing.Point(93, 93);
            this.IsActiveCheckBox.Name = "IsActiveCheckBox";
            this.IsActiveCheckBox.Size = new System.Drawing.Size(15, 14);
            this.IsActiveCheckBox.TabIndex = 32;
            this.IsActiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // CollarSensorTextBox
            // 
            this.CollarSensorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarSensorTextBox.Enabled = false;
            this.CollarSensorTextBox.Location = new System.Drawing.Point(93, 64);
            this.CollarSensorTextBox.MaxLength = 16;
            this.CollarSensorTextBox.Name = "CollarSensorTextBox";
            this.CollarSensorTextBox.Size = new System.Drawing.Size(186, 20);
            this.CollarSensorTextBox.TabIndex = 33;
            // 
            // CollarSensorDetailsForm
            // 
            this.AcceptButton = this.CreateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(296, 161);
            this.Controls.Add(this.CollarSensorTextBox);
            this.Controls.Add(this.IsActiveCheckBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.CollarIdTextBox);
            this.Controls.Add(this.CollarManufacturerTextBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(260, 200);
            this.Name = "CollarSensorDetailsForm";
            this.Text = "Sensor Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox CollarIdTextBox;
        private System.Windows.Forms.TextBox CollarManufacturerTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox IsActiveCheckBox;
        private System.Windows.Forms.TextBox CollarSensorTextBox;
    }
}