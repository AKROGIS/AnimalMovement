namespace AnimalMovement
{
    partial class InvestigatorForm
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
            this.LoginTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.PhoneTextBox = new System.Windows.Forms.TextBox();
            this.EmailTextBox = new System.Windows.Forms.TextBox();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.DoneCancelButton = new System.Windows.Forms.Button();
            this.EditSaveButton = new System.Windows.Forms.Button();
            this.InfoCollarButton = new System.Windows.Forms.Button();
            this.InfoProjectButton = new System.Windows.Forms.Button();
            this.DeleteProjectsButton = new System.Windows.Forms.Button();
            this.AddProjectButton = new System.Windows.Forms.Button();
            this.DeleteCollarsButton = new System.Windows.Forms.Button();
            this.AddCollarButton = new System.Windows.Forms.Button();
            this.ProjectsListBox = new System.Windows.Forms.ListBox();
            this.CollarsListBox = new AnimalMovement.ColoredListBox();
            this.FilesListBox = new AnimalMovement.ColoredListBox();
            this.InfoFileButton = new System.Windows.Forms.Button();
            this.DeleteFilesButton = new System.Windows.Forms.Button();
            this.AddFileButton = new System.Windows.Forms.Button();
            this.ProjectInvestigatorTabs = new System.Windows.Forms.TabControl();
            this.ProjectsTab = new System.Windows.Forms.TabPage();
            this.CollarsTab = new System.Windows.Forms.TabPage();
            this.FilesTab = new System.Windows.Forms.TabPage();
            this.ParameterFilesTab = new System.Windows.Forms.TabPage();
            this.ReportsTab = new System.Windows.Forms.TabPage();
            this.ProjectInvestigatorTabs.SuspendLayout();
            this.ProjectsTab.SuspendLayout();
            this.CollarsTab.SuspendLayout();
            this.ParameterFilesTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // LoginTextBox
            // 
            this.LoginTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoginTextBox.Enabled = false;
            this.LoginTextBox.Location = new System.Drawing.Point(93, 12);
            this.LoginTextBox.Name = "LoginTextBox";
            this.LoginTextBox.Size = new System.Drawing.Size(591, 20);
            this.LoginTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Domain Login:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Email:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Phone:";
            // 
            // PhoneTextBox
            // 
            this.PhoneTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PhoneTextBox.Enabled = false;
            this.PhoneTextBox.Location = new System.Drawing.Point(93, 90);
            this.PhoneTextBox.MaxLength = 100;
            this.PhoneTextBox.Name = "PhoneTextBox";
            this.PhoneTextBox.Size = new System.Drawing.Size(591, 20);
            this.PhoneTextBox.TabIndex = 3;
            // 
            // EmailTextBox
            // 
            this.EmailTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EmailTextBox.Enabled = false;
            this.EmailTextBox.Location = new System.Drawing.Point(93, 64);
            this.EmailTextBox.MaxLength = 200;
            this.EmailTextBox.Name = "EmailTextBox";
            this.EmailTextBox.Size = new System.Drawing.Size(591, 20);
            this.EmailTextBox.TabIndex = 2;
            // 
            // NameTextBox
            // 
            this.NameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameTextBox.Enabled = false;
            this.NameTextBox.Location = new System.Drawing.Point(93, 38);
            this.NameTextBox.MaxLength = 100;
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(591, 20);
            this.NameTextBox.TabIndex = 1;
            // 
            // DoneCancelButton
            // 
            this.DoneCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DoneCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneCancelButton.Location = new System.Drawing.Point(18, 496);
            this.DoneCancelButton.Name = "DoneCancelButton";
            this.DoneCancelButton.Size = new System.Drawing.Size(75, 23);
            this.DoneCancelButton.TabIndex = 5;
            this.DoneCancelButton.Text = "Done";
            this.DoneCancelButton.UseVisualStyleBackColor = true;
            this.DoneCancelButton.Click += new System.EventHandler(this.DoneCancelButton_Click);
            // 
            // EditSaveButton
            // 
            this.EditSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EditSaveButton.BackColor = System.Drawing.SystemColors.Control;
            this.EditSaveButton.Location = new System.Drawing.Point(609, 496);
            this.EditSaveButton.Name = "EditSaveButton";
            this.EditSaveButton.Size = new System.Drawing.Size(75, 23);
            this.EditSaveButton.TabIndex = 6;
            this.EditSaveButton.Text = "Edit";
            this.EditSaveButton.UseVisualStyleBackColor = true;
            this.EditSaveButton.Click += new System.EventHandler(this.EditSaveButton_Click);
            // 
            // InfoCollarButton
            // 
            this.InfoCollarButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InfoCollarButton.FlatAppearance.BorderSize = 0;
            this.InfoCollarButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoCollarButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoCollarButton.Image = global::AnimalMovement.Properties.Resources.GenericInformation_B_16;
            this.InfoCollarButton.Location = new System.Drawing.Point(61, 318);
            this.InfoCollarButton.Name = "InfoCollarButton";
            this.InfoCollarButton.Size = new System.Drawing.Size(24, 24);
            this.InfoCollarButton.TabIndex = 4;
            this.InfoCollarButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.InfoCollarButton.UseVisualStyleBackColor = true;
            this.InfoCollarButton.Click += new System.EventHandler(this.InfoCollarButton_Click);
            // 
            // InfoProjectButton
            // 
            this.InfoProjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InfoProjectButton.FlatAppearance.BorderSize = 0;
            this.InfoProjectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoProjectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoProjectButton.Image = global::AnimalMovement.Properties.Resources.GenericInformation_B_16;
            this.InfoProjectButton.Location = new System.Drawing.Point(61, 318);
            this.InfoProjectButton.Name = "InfoProjectButton";
            this.InfoProjectButton.Size = new System.Drawing.Size(24, 24);
            this.InfoProjectButton.TabIndex = 4;
            this.InfoProjectButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.InfoProjectButton.UseVisualStyleBackColor = true;
            this.InfoProjectButton.Click += new System.EventHandler(this.InfoProjectButton_Click);
            // 
            // DeleteProjectsButton
            // 
            this.DeleteProjectsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteProjectsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteProjectsButton.Image = global::AnimalMovement.Properties.Resources.GenericDeleteRed16;
            this.DeleteProjectsButton.Location = new System.Drawing.Point(36, 318);
            this.DeleteProjectsButton.Name = "DeleteProjectsButton";
            this.DeleteProjectsButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteProjectsButton.TabIndex = 3;
            this.DeleteProjectsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.DeleteProjectsButton.UseVisualStyleBackColor = true;
            this.DeleteProjectsButton.Click += new System.EventHandler(this.DeleteProjectsButton_Click);
            // 
            // AddProjectButton
            // 
            this.AddProjectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddProjectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddProjectButton.Image = global::AnimalMovement.Properties.Resources.GenericAddGreen16;
            this.AddProjectButton.Location = new System.Drawing.Point(6, 318);
            this.AddProjectButton.Name = "AddProjectButton";
            this.AddProjectButton.Size = new System.Drawing.Size(24, 24);
            this.AddProjectButton.TabIndex = 2;
            this.AddProjectButton.UseVisualStyleBackColor = true;
            this.AddProjectButton.Click += new System.EventHandler(this.AddProjectButton_Click);
            // 
            // DeleteCollarsButton
            // 
            this.DeleteCollarsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteCollarsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteCollarsButton.Image = global::AnimalMovement.Properties.Resources.GenericDeleteRed16;
            this.DeleteCollarsButton.Location = new System.Drawing.Point(36, 318);
            this.DeleteCollarsButton.Name = "DeleteCollarsButton";
            this.DeleteCollarsButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteCollarsButton.TabIndex = 3;
            this.DeleteCollarsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.DeleteCollarsButton.UseVisualStyleBackColor = true;
            this.DeleteCollarsButton.Click += new System.EventHandler(this.DeleteCollarsButton_Click);
            // 
            // AddCollarButton
            // 
            this.AddCollarButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddCollarButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddCollarButton.Image = global::AnimalMovement.Properties.Resources.GenericAddGreen16;
            this.AddCollarButton.Location = new System.Drawing.Point(6, 318);
            this.AddCollarButton.Name = "AddCollarButton";
            this.AddCollarButton.Size = new System.Drawing.Size(24, 24);
            this.AddCollarButton.TabIndex = 2;
            this.AddCollarButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.AddCollarButton.UseVisualStyleBackColor = true;
            this.AddCollarButton.Click += new System.EventHandler(this.AddCollarButton_Click);
            // 
            // ProjectsListBox
            // 
            this.ProjectsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectsListBox.FormattingEnabled = true;
            this.ProjectsListBox.IntegralHeight = false;
            this.ProjectsListBox.Location = new System.Drawing.Point(6, 6);
            this.ProjectsListBox.Name = "ProjectsListBox";
            this.ProjectsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ProjectsListBox.Size = new System.Drawing.Size(649, 306);
            this.ProjectsListBox.TabIndex = 1;
            this.ProjectsListBox.SelectedIndexChanged += new System.EventHandler(this.ProjectsListBox_SelectedIndexChanged);
            this.ProjectsListBox.DoubleClick += new System.EventHandler(this.InfoProjectButton_Click);
            // 
            // CollarsListBox
            // 
            this.CollarsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarsListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CollarsListBox.FormattingEnabled = true;
            this.CollarsListBox.IntegralHeight = false;
            this.CollarsListBox.Location = new System.Drawing.Point(6, 6);
            this.CollarsListBox.Name = "CollarsListBox";
            this.CollarsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.CollarsListBox.Size = new System.Drawing.Size(649, 306);
            this.CollarsListBox.TabIndex = 1;
            this.CollarsListBox.SelectedIndexChanged += new System.EventHandler(this.CollarsListBox_SelectedIndexChanged);
            this.CollarsListBox.DoubleClick += new System.EventHandler(this.InfoCollarButton_Click);
            // 
            // FilesListBox
            // 
            this.FilesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilesListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.FilesListBox.FormattingEnabled = true;
            this.FilesListBox.IntegralHeight = false;
            this.FilesListBox.Location = new System.Drawing.Point(6, 6);
            this.FilesListBox.Name = "FilesListBox";
            this.FilesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.FilesListBox.Size = new System.Drawing.Size(649, 306);
            this.FilesListBox.TabIndex = 35;
            this.FilesListBox.SelectedIndexChanged += new System.EventHandler(this.FilesListBox_SelectedIndexChanged);
            this.FilesListBox.DoubleClick += new System.EventHandler(this.InfoFileButton_Click);
            // 
            // InfoFileButton
            // 
            this.InfoFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InfoFileButton.FlatAppearance.BorderSize = 0;
            this.InfoFileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoFileButton.Image = global::AnimalMovement.Properties.Resources.GenericInformation_B_16;
            this.InfoFileButton.Location = new System.Drawing.Point(61, 318);
            this.InfoFileButton.Name = "InfoFileButton";
            this.InfoFileButton.Size = new System.Drawing.Size(24, 24);
            this.InfoFileButton.TabIndex = 38;
            this.InfoFileButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.InfoFileButton.UseVisualStyleBackColor = true;
            this.InfoFileButton.Click += new System.EventHandler(this.InfoFileButton_Click);
            // 
            // DeleteFilesButton
            // 
            this.DeleteFilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteFilesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteFilesButton.Image = global::AnimalMovement.Properties.Resources.GenericDeleteRed16;
            this.DeleteFilesButton.Location = new System.Drawing.Point(36, 318);
            this.DeleteFilesButton.Name = "DeleteFilesButton";
            this.DeleteFilesButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteFilesButton.TabIndex = 37;
            this.DeleteFilesButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.DeleteFilesButton.UseVisualStyleBackColor = true;
            this.DeleteFilesButton.Click += new System.EventHandler(this.DeleteFilesButton_Click);
            // 
            // AddFileButton
            // 
            this.AddFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddFileButton.Image = global::AnimalMovement.Properties.Resources.GenericAddGreen16;
            this.AddFileButton.Location = new System.Drawing.Point(6, 318);
            this.AddFileButton.Name = "AddFileButton";
            this.AddFileButton.Size = new System.Drawing.Size(24, 24);
            this.AddFileButton.TabIndex = 36;
            this.AddFileButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.AddFileButton.UseVisualStyleBackColor = true;
            this.AddFileButton.Click += new System.EventHandler(this.AddFileButton_Click);
            // 
            // ProjectInvestigatorTabs
            // 
            this.ProjectInvestigatorTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectInvestigatorTabs.Controls.Add(this.ProjectsTab);
            this.ProjectInvestigatorTabs.Controls.Add(this.CollarsTab);
            this.ProjectInvestigatorTabs.Controls.Add(this.FilesTab);
            this.ProjectInvestigatorTabs.Controls.Add(this.ParameterFilesTab);
            this.ProjectInvestigatorTabs.Controls.Add(this.ReportsTab);
            this.ProjectInvestigatorTabs.Location = new System.Drawing.Point(18, 116);
            this.ProjectInvestigatorTabs.Name = "ProjectInvestigatorTabs";
            this.ProjectInvestigatorTabs.SelectedIndex = 0;
            this.ProjectInvestigatorTabs.Size = new System.Drawing.Size(669, 374);
            this.ProjectInvestigatorTabs.TabIndex = 8;
            // 
            // ProjectsTab
            // 
            this.ProjectsTab.Controls.Add(this.ProjectsListBox);
            this.ProjectsTab.Controls.Add(this.DeleteProjectsButton);
            this.ProjectsTab.Controls.Add(this.InfoProjectButton);
            this.ProjectsTab.Controls.Add(this.AddProjectButton);
            this.ProjectsTab.Location = new System.Drawing.Point(4, 22);
            this.ProjectsTab.Name = "ProjectsTab";
            this.ProjectsTab.Padding = new System.Windows.Forms.Padding(3);
            this.ProjectsTab.Size = new System.Drawing.Size(661, 348);
            this.ProjectsTab.TabIndex = 0;
            this.ProjectsTab.Text = "Projects";
            this.ProjectsTab.UseVisualStyleBackColor = true;
            // 
            // CollarsTab
            // 
            this.CollarsTab.Controls.Add(this.CollarsListBox);
            this.CollarsTab.Controls.Add(this.InfoCollarButton);
            this.CollarsTab.Controls.Add(this.AddCollarButton);
            this.CollarsTab.Controls.Add(this.DeleteCollarsButton);
            this.CollarsTab.Location = new System.Drawing.Point(4, 22);
            this.CollarsTab.Name = "CollarsTab";
            this.CollarsTab.Padding = new System.Windows.Forms.Padding(3);
            this.CollarsTab.Size = new System.Drawing.Size(661, 348);
            this.CollarsTab.TabIndex = 1;
            this.CollarsTab.Text = "Collars";
            this.CollarsTab.UseVisualStyleBackColor = true;
            // 
            // FilesTab
            // 
            this.FilesTab.Location = new System.Drawing.Point(4, 22);
            this.FilesTab.Name = "FilesTab";
            this.FilesTab.Padding = new System.Windows.Forms.Padding(3);
            this.FilesTab.Size = new System.Drawing.Size(661, 348);
            this.FilesTab.TabIndex = 2;
            this.FilesTab.Text = "Files";
            this.FilesTab.UseVisualStyleBackColor = true;
            // 
            // ParameterFilesTab
            // 
            this.ParameterFilesTab.Controls.Add(this.FilesListBox);
            this.ParameterFilesTab.Controls.Add(this.InfoFileButton);
            this.ParameterFilesTab.Controls.Add(this.AddFileButton);
            this.ParameterFilesTab.Controls.Add(this.DeleteFilesButton);
            this.ParameterFilesTab.Location = new System.Drawing.Point(4, 22);
            this.ParameterFilesTab.Name = "ParameterFilesTab";
            this.ParameterFilesTab.Padding = new System.Windows.Forms.Padding(3);
            this.ParameterFilesTab.Size = new System.Drawing.Size(661, 348);
            this.ParameterFilesTab.TabIndex = 3;
            this.ParameterFilesTab.Text = "Parameter Files";
            this.ParameterFilesTab.UseVisualStyleBackColor = true;
            // 
            // ReportsTab
            // 
            this.ReportsTab.Location = new System.Drawing.Point(4, 22);
            this.ReportsTab.Name = "ReportsTab";
            this.ReportsTab.Padding = new System.Windows.Forms.Padding(3);
            this.ReportsTab.Size = new System.Drawing.Size(661, 348);
            this.ReportsTab.TabIndex = 4;
            this.ReportsTab.Text = "QC Reports";
            this.ReportsTab.UseVisualStyleBackColor = true;
            // 
            // InvestigatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DoneCancelButton;
            this.ClientSize = new System.Drawing.Size(699, 531);
            this.Controls.Add(this.ProjectInvestigatorTabs);
            this.Controls.Add(this.DoneCancelButton);
            this.Controls.Add(this.EditSaveButton);
            this.Controls.Add(this.NameTextBox);
            this.Controls.Add(this.EmailTextBox);
            this.Controls.Add(this.PhoneTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LoginTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(350, 350);
            this.Name = "InvestigatorForm";
            this.Text = "Project Investigator Details";
            this.Load += new System.EventHandler(this.InvestigatorForm_Load);
            this.ProjectInvestigatorTabs.ResumeLayout(false);
            this.ProjectsTab.ResumeLayout(false);
            this.CollarsTab.ResumeLayout(false);
            this.ParameterFilesTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LoginTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PhoneTextBox;
        private System.Windows.Forms.TextBox EmailTextBox;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Button DoneCancelButton;
        private System.Windows.Forms.Button EditSaveButton;
        private System.Windows.Forms.Button InfoCollarButton;
        private System.Windows.Forms.Button InfoProjectButton;
        private System.Windows.Forms.Button DeleteProjectsButton;
        private System.Windows.Forms.Button AddProjectButton;
        private System.Windows.Forms.Button DeleteCollarsButton;
        private System.Windows.Forms.Button AddCollarButton;
        private System.Windows.Forms.ListBox ProjectsListBox;
        private ColoredListBox CollarsListBox;
        private ColoredListBox FilesListBox;
        private System.Windows.Forms.Button InfoFileButton;
        private System.Windows.Forms.Button DeleteFilesButton;
        private System.Windows.Forms.Button AddFileButton;
        private System.Windows.Forms.TabControl ProjectInvestigatorTabs;
        private System.Windows.Forms.TabPage ProjectsTab;
        private System.Windows.Forms.TabPage CollarsTab;
        private System.Windows.Forms.TabPage FilesTab;
        private System.Windows.Forms.TabPage ParameterFilesTab;
        private System.Windows.Forms.TabPage ReportsTab;
    }
}