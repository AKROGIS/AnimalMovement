namespace AnimalMovement
{
    internal partial class ProjectDetailsForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.InvestigatorTextBox = new System.Windows.Forms.TextBox();
            this.UnitTextBox = new System.Windows.Forms.TextBox();
            this.DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.ProjectNameTextBox = new System.Windows.Forms.TextBox();
            this.ProjectCodeTextBox = new System.Windows.Forms.TextBox();
            this.EditInvestigatorButton = new System.Windows.Forms.Button();
            this.InvestigatorDetailsButton = new System.Windows.Forms.Button();
            this.EditSaveButton = new System.Windows.Forms.Button();
            this.DoneCancelButton = new System.Windows.Forms.Button();
            this.EditorsListBox = new System.Windows.Forms.ListBox();
            this.AddEditorButton = new System.Windows.Forms.Button();
            this.DeleteEditorButton = new System.Windows.Forms.Button();
            this.AnimalsListBox = new AnimalMovement.ColoredListBox();
            this.AddAnimalButton = new System.Windows.Forms.Button();
            this.DeleteAnimalsButton = new System.Windows.Forms.Button();
            this.InfoAnimalsButton = new System.Windows.Forms.Button();
            this.FilesListBox = new AnimalMovement.ColoredListBox();
            this.AddFilesButton = new System.Windows.Forms.Button();
            this.DeleteFilesButton = new System.Windows.Forms.Button();
            this.InfoFilesButton = new System.Windows.Forms.Button();
            this.ProjectTabs = new System.Windows.Forms.TabControl();
            this.AnimalsTabPage = new System.Windows.Forms.TabPage();
            this.FilesTabPage = new System.Windows.Forms.TabPage();
            this.ShowEmailFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.ShowDownloadFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.ShowDerivedFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.EditorTabPage = new System.Windows.Forms.TabPage();
            this.ReportsTabPage = new System.Windows.Forms.TabPage();
            this.ProjectTabs.SuspendLayout();
            this.AnimalsTabPage.SuspendLayout();
            this.FilesTabPage.SuspendLayout();
            this.EditorTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(404, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Code:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Name:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Investigator:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Description:";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(410, 41);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Unit:";
            // 
            // InvestigatorTextBox
            // 
            this.InvestigatorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InvestigatorTextBox.Enabled = false;
            this.InvestigatorTextBox.Location = new System.Drawing.Point(86, 38);
            this.InvestigatorTextBox.MaxLength = 128;
            this.InvestigatorTextBox.Name = "InvestigatorTextBox";
            this.InvestigatorTextBox.Size = new System.Drawing.Size(257, 20);
            this.InvestigatorTextBox.TabIndex = 3;
            // 
            // UnitTextBox
            // 
            this.UnitTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UnitTextBox.Enabled = false;
            this.UnitTextBox.Location = new System.Drawing.Point(445, 38);
            this.UnitTextBox.MaxLength = 4;
            this.UnitTextBox.Name = "UnitTextBox";
            this.UnitTextBox.Size = new System.Drawing.Size(101, 20);
            this.UnitTextBox.TabIndex = 6;
            // 
            // DescriptionTextBox
            // 
            this.DescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionTextBox.Enabled = false;
            this.DescriptionTextBox.Location = new System.Drawing.Point(15, 81);
            this.DescriptionTextBox.MaxLength = 2000;
            this.DescriptionTextBox.Multiline = true;
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.Size = new System.Drawing.Size(531, 65);
            this.DescriptionTextBox.TabIndex = 21;
            // 
            // ProjectNameTextBox
            // 
            this.ProjectNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectNameTextBox.Enabled = false;
            this.ProjectNameTextBox.Location = new System.Drawing.Point(86, 12);
            this.ProjectNameTextBox.MaxLength = 150;
            this.ProjectNameTextBox.Name = "ProjectNameTextBox";
            this.ProjectNameTextBox.Size = new System.Drawing.Size(312, 20);
            this.ProjectNameTextBox.TabIndex = 1;
            // 
            // ProjectCodeTextBox
            // 
            this.ProjectCodeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectCodeTextBox.Enabled = false;
            this.ProjectCodeTextBox.Location = new System.Drawing.Point(445, 12);
            this.ProjectCodeTextBox.MaxLength = 16;
            this.ProjectCodeTextBox.Name = "ProjectCodeTextBox";
            this.ProjectCodeTextBox.Size = new System.Drawing.Size(101, 20);
            this.ProjectCodeTextBox.TabIndex = 2;
            // 
            // EditInvestigatorButton
            // 
            this.EditInvestigatorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EditInvestigatorButton.Enabled = false;
            this.EditInvestigatorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditInvestigatorButton.Image = global::AnimalMovement.Properties.Resources.GenericPencil16;
            this.EditInvestigatorButton.Location = new System.Drawing.Point(349, 36);
            this.EditInvestigatorButton.Name = "EditInvestigatorButton";
            this.EditInvestigatorButton.Size = new System.Drawing.Size(24, 24);
            this.EditInvestigatorButton.TabIndex = 4;
            this.EditInvestigatorButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.EditInvestigatorButton.UseVisualStyleBackColor = true;
            this.EditInvestigatorButton.Click += new System.EventHandler(this.EditInvestigatorButton_Click);
            // 
            // InvestigatorDetailsButton
            // 
            this.InvestigatorDetailsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InvestigatorDetailsButton.FlatAppearance.BorderSize = 0;
            this.InvestigatorDetailsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InvestigatorDetailsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InvestigatorDetailsButton.Image = global::AnimalMovement.Properties.Resources.GenericInformation_B_16;
            this.InvestigatorDetailsButton.Location = new System.Drawing.Point(374, 35);
            this.InvestigatorDetailsButton.Name = "InvestigatorDetailsButton";
            this.InvestigatorDetailsButton.Size = new System.Drawing.Size(24, 24);
            this.InvestigatorDetailsButton.TabIndex = 5;
            this.InvestigatorDetailsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.InvestigatorDetailsButton.UseVisualStyleBackColor = true;
            this.InvestigatorDetailsButton.Click += new System.EventHandler(this.InvestigatorDetailsButton_Click);
            // 
            // EditSaveButton
            // 
            this.EditSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EditSaveButton.BackColor = System.Drawing.SystemColors.Control;
            this.EditSaveButton.Enabled = false;
            this.EditSaveButton.Location = new System.Drawing.Point(471, 421);
            this.EditSaveButton.Name = "EditSaveButton";
            this.EditSaveButton.Size = new System.Drawing.Size(75, 23);
            this.EditSaveButton.TabIndex = 10;
            this.EditSaveButton.Text = "Edit";
            this.EditSaveButton.UseVisualStyleBackColor = true;
            this.EditSaveButton.Click += new System.EventHandler(this.EditSaveButton_Click);
            // 
            // DoneCancelButton
            // 
            this.DoneCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DoneCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneCancelButton.Location = new System.Drawing.Point(15, 421);
            this.DoneCancelButton.Name = "DoneCancelButton";
            this.DoneCancelButton.Size = new System.Drawing.Size(75, 23);
            this.DoneCancelButton.TabIndex = 9;
            this.DoneCancelButton.Text = "Done";
            this.DoneCancelButton.UseVisualStyleBackColor = true;
            this.DoneCancelButton.Click += new System.EventHandler(this.DoneCancelButton_Click);
            // 
            // EditorsListBox
            // 
            this.EditorsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EditorsListBox.FormattingEnabled = true;
            this.EditorsListBox.IntegralHeight = false;
            this.EditorsListBox.Location = new System.Drawing.Point(3, 3);
            this.EditorsListBox.Name = "EditorsListBox";
            this.EditorsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.EditorsListBox.Size = new System.Drawing.Size(599, 173);
            this.EditorsListBox.TabIndex = 11;
            this.EditorsListBox.SelectedIndexChanged += new System.EventHandler(this.EditorsListBox_SelectedIndexChanged);
            // 
            // AddEditorButton
            // 
            this.AddEditorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddEditorButton.Enabled = false;
            this.AddEditorButton.Image = global::AnimalMovement.Properties.Resources.GenericAddGreen16;
            this.AddEditorButton.Location = new System.Drawing.Point(3, 181);
            this.AddEditorButton.Name = "AddEditorButton";
            this.AddEditorButton.Size = new System.Drawing.Size(24, 24);
            this.AddEditorButton.TabIndex = 12;
            this.AddEditorButton.UseVisualStyleBackColor = true;
            this.AddEditorButton.Click += new System.EventHandler(this.AddEditorButton_Click);
            // 
            // DeleteEditorButton
            // 
            this.DeleteEditorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteEditorButton.Enabled = false;
            this.DeleteEditorButton.Image = global::AnimalMovement.Properties.Resources.GenericDeleteRed16;
            this.DeleteEditorButton.Location = new System.Drawing.Point(30, 181);
            this.DeleteEditorButton.Name = "DeleteEditorButton";
            this.DeleteEditorButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteEditorButton.TabIndex = 13;
            this.DeleteEditorButton.UseVisualStyleBackColor = true;
            this.DeleteEditorButton.Click += new System.EventHandler(this.DeleteEditorButton_Click);
            // 
            // AnimalsListBox
            // 
            this.AnimalsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimalsListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.AnimalsListBox.FormattingEnabled = true;
            this.AnimalsListBox.IntegralHeight = false;
            this.AnimalsListBox.Location = new System.Drawing.Point(3, 3);
            this.AnimalsListBox.Name = "AnimalsListBox";
            this.AnimalsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.AnimalsListBox.Size = new System.Drawing.Size(599, 173);
            this.AnimalsListBox.TabIndex = 21;
            this.AnimalsListBox.SelectedIndexChanged += new System.EventHandler(this.AnimalsListBox_SelectedIndexChanged);
            this.AnimalsListBox.DoubleClick += new System.EventHandler(this.InfoAnimalsButton_Click);
            // 
            // AddAnimalButton
            // 
            this.AddAnimalButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddAnimalButton.Enabled = false;
            this.AddAnimalButton.Image = global::AnimalMovement.Properties.Resources.GenericAddGreen16;
            this.AddAnimalButton.Location = new System.Drawing.Point(3, 181);
            this.AddAnimalButton.Name = "AddAnimalButton";
            this.AddAnimalButton.Size = new System.Drawing.Size(24, 24);
            this.AddAnimalButton.TabIndex = 22;
            this.AddAnimalButton.UseVisualStyleBackColor = true;
            this.AddAnimalButton.Click += new System.EventHandler(this.AddAnimalButton_Click);
            // 
            // DeleteAnimalsButton
            // 
            this.DeleteAnimalsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteAnimalsButton.Enabled = false;
            this.DeleteAnimalsButton.Image = global::AnimalMovement.Properties.Resources.GenericDeleteRed16;
            this.DeleteAnimalsButton.Location = new System.Drawing.Point(30, 181);
            this.DeleteAnimalsButton.Name = "DeleteAnimalsButton";
            this.DeleteAnimalsButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteAnimalsButton.TabIndex = 23;
            this.DeleteAnimalsButton.UseVisualStyleBackColor = true;
            this.DeleteAnimalsButton.Click += new System.EventHandler(this.DeleteAnimalsButton_Click);
            // 
            // InfoAnimalsButton
            // 
            this.InfoAnimalsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InfoAnimalsButton.FlatAppearance.BorderSize = 0;
            this.InfoAnimalsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoAnimalsButton.Image = global::AnimalMovement.Properties.Resources.GenericInformation_B_16;
            this.InfoAnimalsButton.Location = new System.Drawing.Point(55, 181);
            this.InfoAnimalsButton.Name = "InfoAnimalsButton";
            this.InfoAnimalsButton.Size = new System.Drawing.Size(24, 24);
            this.InfoAnimalsButton.TabIndex = 24;
            this.InfoAnimalsButton.UseVisualStyleBackColor = true;
            this.InfoAnimalsButton.Click += new System.EventHandler(this.InfoAnimalsButton_Click);
            // 
            // FilesListBox
            // 
            this.FilesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilesListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.FilesListBox.FormattingEnabled = true;
            this.FilesListBox.IntegralHeight = false;
            this.FilesListBox.Location = new System.Drawing.Point(3, 3);
            this.FilesListBox.Name = "FilesListBox";
            this.FilesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.FilesListBox.Size = new System.Drawing.Size(517, 202);
            this.FilesListBox.TabIndex = 31;
            this.FilesListBox.SelectedIndexChanged += new System.EventHandler(this.FilesListBox_SelectedIndexChanged);
            this.FilesListBox.DoubleClick += new System.EventHandler(this.InfoFilesButton_Click);
            // 
            // AddFilesButton
            // 
            this.AddFilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddFilesButton.Enabled = false;
            this.AddFilesButton.Image = global::AnimalMovement.Properties.Resources.GenericAddGreen16;
            this.AddFilesButton.Location = new System.Drawing.Point(3, 210);
            this.AddFilesButton.Name = "AddFilesButton";
            this.AddFilesButton.Size = new System.Drawing.Size(24, 24);
            this.AddFilesButton.TabIndex = 32;
            this.AddFilesButton.UseVisualStyleBackColor = true;
            this.AddFilesButton.Click += new System.EventHandler(this.AddFilesButton_Click);
            // 
            // DeleteFilesButton
            // 
            this.DeleteFilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteFilesButton.Enabled = false;
            this.DeleteFilesButton.Image = global::AnimalMovement.Properties.Resources.GenericDeleteRed16;
            this.DeleteFilesButton.Location = new System.Drawing.Point(30, 210);
            this.DeleteFilesButton.Name = "DeleteFilesButton";
            this.DeleteFilesButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteFilesButton.TabIndex = 33;
            this.DeleteFilesButton.UseVisualStyleBackColor = true;
            this.DeleteFilesButton.Click += new System.EventHandler(this.DeleteFilesButton_Click);
            // 
            // InfoFilesButton
            // 
            this.InfoFilesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InfoFilesButton.FlatAppearance.BorderSize = 0;
            this.InfoFilesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoFilesButton.Image = global::AnimalMovement.Properties.Resources.GenericInformation_B_16;
            this.InfoFilesButton.Location = new System.Drawing.Point(58, 210);
            this.InfoFilesButton.Name = "InfoFilesButton";
            this.InfoFilesButton.Size = new System.Drawing.Size(24, 24);
            this.InfoFilesButton.TabIndex = 34;
            this.InfoFilesButton.UseVisualStyleBackColor = true;
            this.InfoFilesButton.Click += new System.EventHandler(this.InfoFilesButton_Click);
            // 
            // ProjectTabs
            // 
            this.ProjectTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectTabs.Controls.Add(this.AnimalsTabPage);
            this.ProjectTabs.Controls.Add(this.FilesTabPage);
            this.ProjectTabs.Controls.Add(this.EditorTabPage);
            this.ProjectTabs.Controls.Add(this.ReportsTabPage);
            this.ProjectTabs.Location = new System.Drawing.Point(15, 152);
            this.ProjectTabs.Name = "ProjectTabs";
            this.ProjectTabs.SelectedIndex = 0;
            this.ProjectTabs.Size = new System.Drawing.Size(531, 263);
            this.ProjectTabs.TabIndex = 22;
            this.ProjectTabs.SelectedIndexChanged += new System.EventHandler(this.ProjectTabs_SelectedIndexChanged);
            // 
            // AnimalsTabPage
            // 
            this.AnimalsTabPage.Controls.Add(this.AnimalsListBox);
            this.AnimalsTabPage.Controls.Add(this.AddAnimalButton);
            this.AnimalsTabPage.Controls.Add(this.DeleteAnimalsButton);
            this.AnimalsTabPage.Controls.Add(this.InfoAnimalsButton);
            this.AnimalsTabPage.Location = new System.Drawing.Point(4, 22);
            this.AnimalsTabPage.Name = "AnimalsTabPage";
            this.AnimalsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.AnimalsTabPage.Size = new System.Drawing.Size(523, 237);
            this.AnimalsTabPage.TabIndex = 1;
            this.AnimalsTabPage.Text = "Animals";
            this.AnimalsTabPage.UseVisualStyleBackColor = true;
            // 
            // FilesTabPage
            // 
            this.FilesTabPage.Controls.Add(this.ShowEmailFilesCheckBox);
            this.FilesTabPage.Controls.Add(this.ShowDownloadFilesCheckBox);
            this.FilesTabPage.Controls.Add(this.ShowDerivedFilesCheckBox);
            this.FilesTabPage.Controls.Add(this.FilesListBox);
            this.FilesTabPage.Controls.Add(this.AddFilesButton);
            this.FilesTabPage.Controls.Add(this.DeleteFilesButton);
            this.FilesTabPage.Controls.Add(this.InfoFilesButton);
            this.FilesTabPage.Location = new System.Drawing.Point(4, 22);
            this.FilesTabPage.Name = "FilesTabPage";
            this.FilesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.FilesTabPage.Size = new System.Drawing.Size(523, 237);
            this.FilesTabPage.TabIndex = 2;
            this.FilesTabPage.Text = "Files";
            this.FilesTabPage.UseVisualStyleBackColor = true;
            // 
            // ShowEmailFilesCheckBox
            // 
            this.ShowEmailFilesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ShowEmailFilesCheckBox.AutoSize = true;
            this.ShowEmailFilesCheckBox.ForeColor = System.Drawing.Color.MediumBlue;
            this.ShowEmailFilesCheckBox.Location = new System.Drawing.Point(88, 215);
            this.ShowEmailFilesCheckBox.Name = "ShowEmailFilesCheckBox";
            this.ShowEmailFilesCheckBox.Size = new System.Drawing.Size(86, 17);
            this.ShowEmailFilesCheckBox.TabIndex = 46;
            this.ShowEmailFilesCheckBox.Text = "Show Emails";
            this.ShowEmailFilesCheckBox.UseVisualStyleBackColor = true;
            this.ShowEmailFilesCheckBox.CheckedChanged += new System.EventHandler(this.ShowFilesCheckBox_CheckedChanged);
            // 
            // ShowDownloadFilesCheckBox
            // 
            this.ShowDownloadFilesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ShowDownloadFilesCheckBox.AutoSize = true;
            this.ShowDownloadFilesCheckBox.ForeColor = System.Drawing.Color.DarkMagenta;
            this.ShowDownloadFilesCheckBox.Location = new System.Drawing.Point(180, 215);
            this.ShowDownloadFilesCheckBox.Name = "ShowDownloadFilesCheckBox";
            this.ShowDownloadFilesCheckBox.Size = new System.Drawing.Size(109, 17);
            this.ShowDownloadFilesCheckBox.TabIndex = 47;
            this.ShowDownloadFilesCheckBox.Text = "Show Downloads";
            this.ShowDownloadFilesCheckBox.UseVisualStyleBackColor = true;
            this.ShowDownloadFilesCheckBox.CheckedChanged += new System.EventHandler(this.ShowFilesCheckBox_CheckedChanged);
            // 
            // ShowDerivedFilesCheckBox
            // 
            this.ShowDerivedFilesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ShowDerivedFilesCheckBox.AutoSize = true;
            this.ShowDerivedFilesCheckBox.ForeColor = System.Drawing.Color.Brown;
            this.ShowDerivedFilesCheckBox.Location = new System.Drawing.Point(297, 215);
            this.ShowDerivedFilesCheckBox.Name = "ShowDerivedFilesCheckBox";
            this.ShowDerivedFilesCheckBox.Size = new System.Drawing.Size(93, 17);
            this.ShowDerivedFilesCheckBox.TabIndex = 48;
            this.ShowDerivedFilesCheckBox.Text = "Show Derived";
            this.ShowDerivedFilesCheckBox.UseVisualStyleBackColor = true;
            this.ShowDerivedFilesCheckBox.CheckedChanged += new System.EventHandler(this.ShowFilesCheckBox_CheckedChanged);
            // 
            // EditorTabPage
            // 
            this.EditorTabPage.Controls.Add(this.EditorsListBox);
            this.EditorTabPage.Controls.Add(this.AddEditorButton);
            this.EditorTabPage.Controls.Add(this.DeleteEditorButton);
            this.EditorTabPage.Location = new System.Drawing.Point(4, 22);
            this.EditorTabPage.Name = "EditorTabPage";
            this.EditorTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.EditorTabPage.Size = new System.Drawing.Size(523, 237);
            this.EditorTabPage.TabIndex = 3;
            this.EditorTabPage.Text = "Editors";
            this.EditorTabPage.UseVisualStyleBackColor = true;
            // 
            // ReportsTabPage
            // 
            this.ReportsTabPage.Location = new System.Drawing.Point(4, 22);
            this.ReportsTabPage.Name = "ReportsTabPage";
            this.ReportsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ReportsTabPage.Size = new System.Drawing.Size(523, 237);
            this.ReportsTabPage.TabIndex = 4;
            this.ReportsTabPage.Text = "QC Reports";
            this.ReportsTabPage.UseVisualStyleBackColor = true;
            // 
            // ProjectDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DoneCancelButton;
            this.ClientSize = new System.Drawing.Size(564, 456);
            this.Controls.Add(this.ProjectTabs);
            this.Controls.Add(this.DoneCancelButton);
            this.Controls.Add(this.EditSaveButton);
            this.Controls.Add(this.ProjectCodeTextBox);
            this.Controls.Add(this.ProjectNameTextBox);
            this.Controls.Add(this.DescriptionTextBox);
            this.Controls.Add(this.UnitTextBox);
            this.Controls.Add(this.EditInvestigatorButton);
            this.Controls.Add(this.InvestigatorTextBox);
            this.Controls.Add(this.InvestigatorDetailsButton);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(450, 410);
            this.Name = "ProjectDetailsForm";
            this.Text = "Project Details";
            this.ProjectTabs.ResumeLayout(false);
            this.AnimalsTabPage.ResumeLayout(false);
            this.FilesTabPage.ResumeLayout(false);
            this.FilesTabPage.PerformLayout();
            this.EditorTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox EditorsListBox;
        private ColoredListBox FilesListBox;
        private ColoredListBox AnimalsListBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button AddEditorButton;
        private System.Windows.Forms.Button DeleteEditorButton;
        private System.Windows.Forms.Button DeleteFilesButton;
        private System.Windows.Forms.Button AddFilesButton;
        private System.Windows.Forms.Button DeleteAnimalsButton;
        private System.Windows.Forms.Button AddAnimalButton;
        private System.Windows.Forms.Button InvestigatorDetailsButton;
        private System.Windows.Forms.TextBox InvestigatorTextBox;
        private System.Windows.Forms.Button EditInvestigatorButton;
        private System.Windows.Forms.TextBox UnitTextBox;
        private System.Windows.Forms.TextBox DescriptionTextBox;
        private System.Windows.Forms.Button InfoAnimalsButton;
        private System.Windows.Forms.Button InfoFilesButton;
        private System.Windows.Forms.TextBox ProjectNameTextBox;
        private System.Windows.Forms.TextBox ProjectCodeTextBox;
        private System.Windows.Forms.Button EditSaveButton;
        private System.Windows.Forms.Button DoneCancelButton;
        private System.Windows.Forms.TabControl ProjectTabs;
        private System.Windows.Forms.TabPage AnimalsTabPage;
        private System.Windows.Forms.TabPage FilesTabPage;
        private System.Windows.Forms.TabPage EditorTabPage;
        private System.Windows.Forms.TabPage ReportsTabPage;
        private System.Windows.Forms.CheckBox ShowEmailFilesCheckBox;
        private System.Windows.Forms.CheckBox ShowDownloadFilesCheckBox;
        private System.Windows.Forms.CheckBox ShowDerivedFilesCheckBox;
    }
}