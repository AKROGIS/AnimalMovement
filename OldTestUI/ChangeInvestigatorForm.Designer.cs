namespace TestUI
{
    partial class ChangeInvestigatorForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.LeadComboBox = new System.Windows.Forms.ComboBox();
            this.TransferCheckBox = new System.Windows.Forms.CheckBox();
            this.ChangeButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Warning: Once you give up the project lead,\r\nyou cannot reinstate yourself as the" +
    " project lead.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "New Project Lead:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(225, 26);
            this.label4.TabIndex = 3;
            this.label4.Text = "Contact the database administrator if you want\r\nto add a new project investigator" +
    " to the list.";
            // 
            // LeadComboBox
            // 
            this.LeadComboBox.FormattingEnabled = true;
            this.LeadComboBox.Location = new System.Drawing.Point(113, 81);
            this.LeadComboBox.Name = "LeadComboBox";
            this.LeadComboBox.Size = new System.Drawing.Size(160, 21);
            this.LeadComboBox.TabIndex = 4;
            // 
            // TransferCheckBox
            // 
            this.TransferCheckBox.AutoSize = true;
            this.TransferCheckBox.Location = new System.Drawing.Point(15, 108);
            this.TransferCheckBox.Name = "TransferCheckBox";
            this.TransferCheckBox.Size = new System.Drawing.Size(174, 17);
            this.TransferCheckBox.TabIndex = 5;
            this.TransferCheckBox.Text = "Transfer ownership of all collars";
            this.TransferCheckBox.UseVisualStyleBackColor = true;
            // 
            // ChangeButton
            // 
            this.ChangeButton.Location = new System.Drawing.Point(198, 136);
            this.ChangeButton.Name = "ChangeButton";
            this.ChangeButton.Size = new System.Drawing.Size(75, 23);
            this.ChangeButton.TabIndex = 6;
            this.ChangeButton.Text = "Change";
            this.ChangeButton.UseVisualStyleBackColor = true;
            this.ChangeButton.Click += new System.EventHandler(this.ChangeButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(117, 136);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 7;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ChangeInvestigatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 170);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ChangeButton);
            this.Controls.Add(this.TransferCheckBox);
            this.Controls.Add(this.LeadComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "ChangeInvestigatorForm";
            this.Text = "Relinquish Project Lead";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox LeadComboBox;
        private System.Windows.Forms.CheckBox TransferCheckBox;
        private System.Windows.Forms.Button ChangeButton;
        private System.Windows.Forms.Button CancelButton;
    }
}