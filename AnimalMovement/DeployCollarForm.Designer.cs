namespace AnimalMovement
{
    partial class DeployCollarForm
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
            this.DeployButton = new System.Windows.Forms.Button();
            this.DeployDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.ProjectComboBox = new System.Windows.Forms.ComboBox();
            this.AnimalComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(136, 96);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(65, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // DeployButton
            // 
            this.DeployButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeployButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.DeployButton.Location = new System.Drawing.Point(207, 96);
            this.DeployButton.Name = "DeployButton";
            this.DeployButton.Size = new System.Drawing.Size(65, 23);
            this.DeployButton.TabIndex = 5;
            this.DeployButton.Text = "Deploy";
            this.DeployButton.UseVisualStyleBackColor = true;
            this.DeployButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // DeployDateTimePicker
            // 
            this.DeployDateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm";
            this.DeployDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DeployDateTimePicker.Location = new System.Drawing.Point(61, 66);
            this.DeployDateTimePicker.Name = "DeployDateTimePicker";
            this.DeployDateTimePicker.Size = new System.Drawing.Size(211, 20);
            this.DeployDateTimePicker.TabIndex = 3;
            this.DeployDateTimePicker.ValueChanged += new System.EventHandler(this.DeployDateTimePicker_ValueChanged);
            // 
            // ProjectComboBox
            // 
            this.ProjectComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProjectComboBox.FormattingEnabled = true;
            this.ProjectComboBox.Location = new System.Drawing.Point(61, 12);
            this.ProjectComboBox.Name = "ProjectComboBox";
            this.ProjectComboBox.Size = new System.Drawing.Size(211, 21);
            this.ProjectComboBox.TabIndex = 1;
            this.ProjectComboBox.SelectedIndexChanged += new System.EventHandler(this.ProjectComboBox_SelectedIndexChanged);
            // 
            // AnimalComboBox
            // 
            this.AnimalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AnimalComboBox.FormattingEnabled = true;
            this.AnimalComboBox.Location = new System.Drawing.Point(61, 39);
            this.AnimalComboBox.Name = "AnimalComboBox";
            this.AnimalComboBox.Size = new System.Drawing.Size(211, 21);
            this.AnimalComboBox.TabIndex = 2;
            this.AnimalComboBox.SelectedIndexChanged += new System.EventHandler(this.AnimalComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Project:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Animal:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Date:";
            // 
            // DeployCollarForm
            // 
            this.AcceptButton = this.DeployButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(284, 131);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AnimalComboBox);
            this.Controls.Add(this.ProjectComboBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.DeployButton);
            this.Controls.Add(this.DeployDateTimePicker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DeployCollarForm";
            this.Text = "Deploy Collar";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button DeployButton;
        private System.Windows.Forms.DateTimePicker DeployDateTimePicker;
        private System.Windows.Forms.ComboBox ProjectComboBox;
        private System.Windows.Forms.ComboBox AnimalComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}