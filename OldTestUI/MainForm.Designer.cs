namespace TestUI
{
    partial class MainForm
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
            this.DeployButton = new System.Windows.Forms.Button();
            this.FilesButton = new System.Windows.Forms.Button();
            this.QuitButton = new System.Windows.Forms.Button();
            this.ReviewProjectsButton = new System.Windows.Forms.Button();
            this.GenerateMapButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // UploadButton
            // 
            this.UploadButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UploadButton.Location = new System.Drawing.Point(12, 41);
            this.UploadButton.Name = "UploadButton";
            this.UploadButton.Size = new System.Drawing.Size(158, 23);
            this.UploadButton.TabIndex = 0;
            this.UploadButton.Text = "Upload Collar Data";
            this.UploadButton.UseVisualStyleBackColor = true;
            this.UploadButton.Click += new System.EventHandler(this.UploadButton_Click);
            // 
            // DeployButton
            // 
            this.DeployButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DeployButton.Location = new System.Drawing.Point(12, 157);
            this.DeployButton.Name = "DeployButton";
            this.DeployButton.Size = new System.Drawing.Size(158, 23);
            this.DeployButton.TabIndex = 1;
            this.DeployButton.Text = "Manage Deployments";
            this.DeployButton.UseVisualStyleBackColor = true;
            this.DeployButton.Click += new System.EventHandler(this.DeployButton_Click);
            // 
            // FilesButton
            // 
            this.FilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilesButton.Location = new System.Drawing.Point(12, 128);
            this.FilesButton.Name = "FilesButton";
            this.FilesButton.Size = new System.Drawing.Size(158, 23);
            this.FilesButton.TabIndex = 2;
            this.FilesButton.Text = "Manage Collars";
            this.FilesButton.UseVisualStyleBackColor = true;
            this.FilesButton.Click += new System.EventHandler(this.FilesButton_Click);
            // 
            // QuitButton
            // 
            this.QuitButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QuitButton.Location = new System.Drawing.Point(12, 223);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(158, 23);
            this.QuitButton.TabIndex = 3;
            this.QuitButton.Text = "Quit";
            this.QuitButton.UseVisualStyleBackColor = true;
            this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // ReviewProjectsButton
            // 
            this.ReviewProjectsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReviewProjectsButton.Location = new System.Drawing.Point(12, 12);
            this.ReviewProjectsButton.Name = "ReviewProjectsButton";
            this.ReviewProjectsButton.Size = new System.Drawing.Size(158, 23);
            this.ReviewProjectsButton.TabIndex = 5;
            this.ReviewProjectsButton.Text = "Review Projects";
            this.ReviewProjectsButton.UseVisualStyleBackColor = true;
            this.ReviewProjectsButton.Click += new System.EventHandler(this.ReviewProjectsButton_Click);
            // 
            // GenerateMapButton
            // 
            this.GenerateMapButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GenerateMapButton.Location = new System.Drawing.Point(12, 70);
            this.GenerateMapButton.Name = "GenerateMapButton";
            this.GenerateMapButton.Size = new System.Drawing.Size(158, 23);
            this.GenerateMapButton.TabIndex = 6;
            this.GenerateMapButton.Text = "Generate Map File";
            this.GenerateMapButton.UseVisualStyleBackColor = true;
            this.GenerateMapButton.Click += new System.EventHandler(this.GenerateMapButton_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(12, 99);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Manage Files";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 258);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.GenerateMapButton);
            this.Controls.Add(this.ReviewProjectsButton);
            this.Controls.Add(this.QuitButton);
            this.Controls.Add(this.FilesButton);
            this.Controls.Add(this.DeployButton);
            this.Controls.Add(this.UploadButton);
            this.MinimumSize = new System.Drawing.Size(200, 175);
            this.Name = "MainForm";
            this.Text = "Animal Movement Test";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button UploadButton;
        private System.Windows.Forms.Button DeployButton;
        private System.Windows.Forms.Button FilesButton;
        private System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.Button ReviewProjectsButton;
        private System.Windows.Forms.Button GenerateMapButton;
        private System.Windows.Forms.Button button1;
    }
}