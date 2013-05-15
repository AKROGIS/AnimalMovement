namespace AnimalMovement
{
    internal partial class MainForm
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
            this.UploadButton = new System.Windows.Forms.Button();
            this.QuitButton = new System.Windows.Forms.Button();
            this.ProjectsButton = new System.Windows.Forms.Button();
            this.GenerateMapButton = new System.Windows.Forms.Button();
            this.MyProfileButton = new System.Windows.Forms.Button();
            this.QuickStartWizardButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // UploadButton
            // 
            this.UploadButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UploadButton.Enabled = false;
            this.UploadButton.Location = new System.Drawing.Point(12, 70);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(205, 23);
            this.UploadButton.TabIndex = 2;
            this.UploadButton.Text = "Upload Collar Data";
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // QuitButton
            // 
            this.QuitButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QuitButton.Location = new System.Drawing.Point(12, 177);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(205, 23);
            this.QuitButton.TabIndex = 5;
            this.QuitButton.Text = "Quit";
            this.QuitButton.UseVisualStyleBackColor = true;
            this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // ProjectsButton
            // 
            this.ProjectsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectsButton.Enabled = false;
            this.ProjectsButton.Location = new System.Drawing.Point(12, 41);
            this.ProjectsButton.Name = "ProjectsButton";
            this.ProjectsButton.Size = new System.Drawing.Size(205, 23);
            this.ProjectsButton.TabIndex = 1;
            this.ProjectsButton.Text = "Project List";
            this.ProjectsButton.UseVisualStyleBackColor = true;
            this.ProjectsButton.Click += new System.EventHandler(this.ProjectsButton_Click);
            // 
            // GenerateMapButton
            // 
            this.GenerateMapButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GenerateMapButton.Enabled = false;
            this.GenerateMapButton.Location = new System.Drawing.Point(12, 99);
            this.GenerateMapButton.Name = "GenerateMapButton";
            this.GenerateMapButton.Size = new System.Drawing.Size(205, 23);
            this.GenerateMapButton.TabIndex = 3;
            this.GenerateMapButton.Text = "Create Map File";
            this.GenerateMapButton.UseVisualStyleBackColor = true;
            this.GenerateMapButton.Click += new System.EventHandler(this.GenerateMapButton_Click);
            // 
            // MyProfileButton
            // 
            this.MyProfileButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MyProfileButton.Location = new System.Drawing.Point(12, 12);
            this.MyProfileButton.Name = "MyProfileButton";
            this.MyProfileButton.Size = new System.Drawing.Size(205, 23);
            this.MyProfileButton.TabIndex = 0;
            this.MyProfileButton.Text = "Connecting...";
            this.MyProfileButton.UseVisualStyleBackColor = true;
            this.MyProfileButton.Click += new System.EventHandler(this.MyProfileButton_Click);
            // 
            // QuickStartWizardButton
            // 
            this.QuickStartWizardButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QuickStartWizardButton.Enabled = false;
            this.QuickStartWizardButton.Location = new System.Drawing.Point(12, 128);
            this.QuickStartWizardButton.Name = "QuickStartWizardButton";
            this.QuickStartWizardButton.Size = new System.Drawing.Size(205, 23);
            this.QuickStartWizardButton.TabIndex = 4;
            this.QuickStartWizardButton.Text = "Quick Start Wizard";
            this.QuickStartWizardButton.UseVisualStyleBackColor = true;
            this.QuickStartWizardButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 212);
            this.Controls.Add(this.QuickStartWizardButton);
            this.Controls.Add(this.MyProfileButton);
            this.Controls.Add(this.GenerateMapButton);
            this.Controls.Add(this.ProjectsButton);
            this.Controls.Add(this.QuitButton);
            this.Controls.Add(this.UploadButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(200, 202);
            this.Name = "MainForm";
            this.Text = "Animal Movement";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button UploadButton;
        private System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.Button ProjectsButton;
        private System.Windows.Forms.Button GenerateMapButton;
        private System.Windows.Forms.Button MyProfileButton;
        private System.Windows.Forms.Button QuickStartWizardButton;
    }
}