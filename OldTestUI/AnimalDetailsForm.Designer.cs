namespace TestUI
{
    partial class AnimalDetailsForm
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
            this.AnimalsListBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.InfoAnimalsButton = new System.Windows.Forms.Button();
            this.DeleteAnimalsButton = new System.Windows.Forms.Button();
            this.AddAnimalButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AnimalsListBox
            // 
            this.AnimalsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimalsListBox.FormattingEnabled = true;
            this.AnimalsListBox.Location = new System.Drawing.Point(12, 205);
            this.AnimalsListBox.Name = "AnimalsListBox";
            this.AnimalsListBox.Size = new System.Drawing.Size(126, 199);
            this.AnimalsListBox.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Collars";
            // 
            // InfoAnimalsButton
            // 
            this.InfoAnimalsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoAnimalsButton.FlatAppearance.BorderSize = 0;
            this.InfoAnimalsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoAnimalsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoAnimalsButton.Image = global::TestUI.Properties.Resources.GenericInformation_B_16;
            this.InfoAnimalsButton.Location = new System.Drawing.Point(67, 411);
            this.InfoAnimalsButton.Name = "InfoAnimalsButton";
            this.InfoAnimalsButton.Size = new System.Drawing.Size(24, 24);
            this.InfoAnimalsButton.TabIndex = 28;
            this.InfoAnimalsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.InfoAnimalsButton.UseVisualStyleBackColor = true;
            // 
            // DeleteAnimalsButton
            // 
            this.DeleteAnimalsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteAnimalsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteAnimalsButton.Image = global::TestUI.Properties.Resources.GenericDeleteRed16;
            this.DeleteAnimalsButton.Location = new System.Drawing.Point(42, 411);
            this.DeleteAnimalsButton.Name = "DeleteAnimalsButton";
            this.DeleteAnimalsButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteAnimalsButton.TabIndex = 27;
            this.DeleteAnimalsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.DeleteAnimalsButton.UseVisualStyleBackColor = true;
            // 
            // AddAnimalButton
            // 
            this.AddAnimalButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddAnimalButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddAnimalButton.Image = global::TestUI.Properties.Resources.GenericAddGreen16;
            this.AddAnimalButton.Location = new System.Drawing.Point(12, 411);
            this.AddAnimalButton.Name = "AddAnimalButton";
            this.AddAnimalButton.Size = new System.Drawing.Size(24, 24);
            this.AddAnimalButton.TabIndex = 26;
            this.AddAnimalButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.AddAnimalButton.UseVisualStyleBackColor = true;
            // 
            // AnimalDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 447);
            this.Controls.Add(this.InfoAnimalsButton);
            this.Controls.Add(this.DeleteAnimalsButton);
            this.Controls.Add(this.AddAnimalButton);
            this.Controls.Add(this.AnimalsListBox);
            this.Controls.Add(this.label2);
            this.Name = "AnimalDetailsForm";
            this.Text = "Animal Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button InfoAnimalsButton;
        private System.Windows.Forms.Button DeleteAnimalsButton;
        private System.Windows.Forms.Button AddAnimalButton;
        private System.Windows.Forms.ListBox AnimalsListBox;
        private System.Windows.Forms.Label label2;
    }
}