namespace AnimalMovement
{
    partial class CreateQueryLayerForm
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
            this.label4 = new System.Windows.Forms.Label();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.DatabaseComboBox = new System.Windows.Forms.ComboBox();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.UseEarliestDateCheckBox = new System.Windows.Forms.CheckBox();
            this.UseLatestDateCheckBox = new System.Windows.Forms.CheckBox();
            this.CreateLocationsCheckBox = new System.Windows.Forms.CheckBox();
            this.CreateMovementsCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FilterByDatesCheckBox = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.AllProjectsButton = new System.Windows.Forms.Button();
            this.ClearProjectsButton = new System.Windows.Forms.Button();
            this.ProjectsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.FilterByProjectsCheckBox = new System.Windows.Forms.CheckBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.SpeciesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.AllSpeciesButton = new System.Windows.Forms.Button();
            this.ClearSpeciesButton = new System.Windows.Forms.Button();
            this.FilterBySpeciesCheckBox = new System.Windows.Forms.CheckBox();
            this.AnimalsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.AllAnimalsButton = new System.Windows.Forms.Button();
            this.FilterByAnimalsCheckBox = new System.Windows.Forms.CheckBox();
            this.ClearAnimalsButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Database:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 227);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Start Date:";
            // 
            // GenerateButton
            // 
            this.GenerateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GenerateButton.Location = new System.Drawing.Point(392, 350);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(110, 23);
            this.GenerateButton.TabIndex = 7;
            this.GenerateButton.Text = "Generate";
            this.GenerateButton.UseVisualStyleBackColor = true;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // DatabaseComboBox
            // 
            this.DatabaseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DatabaseComboBox.FormattingEnabled = true;
            this.DatabaseComboBox.Location = new System.Drawing.Point(71, 6);
            this.DatabaseComboBox.Name = "DatabaseComboBox";
            this.DatabaseComboBox.Size = new System.Drawing.Size(208, 21);
            this.DatabaseComboBox.TabIndex = 19;
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StartDateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm";
            this.StartDateTimePicker.Enabled = false;
            this.StartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDateTimePicker.Location = new System.Drawing.Point(67, 224);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.Size = new System.Drawing.Size(328, 20);
            this.StartDateTimePicker.TabIndex = 20;
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EndDateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm";
            this.EndDateTimePicker.Enabled = false;
            this.EndDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndDateTimePicker.Location = new System.Drawing.Point(67, 250);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.Size = new System.Drawing.Size(328, 20);
            this.EndDateTimePicker.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 252);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "End Date:";
            // 
            // UseEarliestDateCheckBox
            // 
            this.UseEarliestDateCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UseEarliestDateCheckBox.AutoSize = true;
            this.UseEarliestDateCheckBox.Enabled = false;
            this.UseEarliestDateCheckBox.Location = new System.Drawing.Point(401, 226);
            this.UseEarliestDateCheckBox.Name = "UseEarliestDateCheckBox";
            this.UseEarliestDateCheckBox.Size = new System.Drawing.Size(82, 17);
            this.UseEarliestDateCheckBox.TabIndex = 23;
            this.UseEarliestDateCheckBox.Text = "Use Earliest";
            this.UseEarliestDateCheckBox.UseVisualStyleBackColor = true;
            this.UseEarliestDateCheckBox.CheckedChanged += new System.EventHandler(this.FilterByDatesCheckBox_CheckedChanged);
            // 
            // UseLatestDateCheckBox
            // 
            this.UseLatestDateCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UseLatestDateCheckBox.AutoSize = true;
            this.UseLatestDateCheckBox.Enabled = false;
            this.UseLatestDateCheckBox.Location = new System.Drawing.Point(401, 252);
            this.UseLatestDateCheckBox.Name = "UseLatestDateCheckBox";
            this.UseLatestDateCheckBox.Size = new System.Drawing.Size(77, 17);
            this.UseLatestDateCheckBox.TabIndex = 24;
            this.UseLatestDateCheckBox.Text = "Use Latest";
            this.UseLatestDateCheckBox.UseVisualStyleBackColor = true;
            this.UseLatestDateCheckBox.CheckedChanged += new System.EventHandler(this.FilterByDatesCheckBox_CheckedChanged);
            // 
            // CreateLocationsCheckBox
            // 
            this.CreateLocationsCheckBox.AutoSize = true;
            this.CreateLocationsCheckBox.Checked = true;
            this.CreateLocationsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CreateLocationsCheckBox.Location = new System.Drawing.Point(71, 33);
            this.CreateLocationsCheckBox.Name = "CreateLocationsCheckBox";
            this.CreateLocationsCheckBox.Size = new System.Drawing.Size(99, 17);
            this.CreateLocationsCheckBox.TabIndex = 25;
            this.CreateLocationsCheckBox.Text = "Location Points";
            this.CreateLocationsCheckBox.UseVisualStyleBackColor = true;
            // 
            // CreateMovementsCheckBox
            // 
            this.CreateMovementsCheckBox.AutoSize = true;
            this.CreateMovementsCheckBox.Checked = true;
            this.CreateMovementsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CreateMovementsCheckBox.Location = new System.Drawing.Point(176, 33);
            this.CreateMovementsCheckBox.Name = "CreateMovementsCheckBox";
            this.CreateMovementsCheckBox.Size = new System.Drawing.Size(104, 17);
            this.CreateMovementsCheckBox.TabIndex = 26;
            this.CreateMovementsCheckBox.Text = "Movement Lines";
            this.CreateMovementsCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.FilterByDatesCheckBox);
            this.groupBox1.Controls.Add(this.splitContainer1);
            this.groupBox1.Controls.Add(this.UseLatestDateCheckBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.StartDateTimePicker);
            this.groupBox1.Controls.Add(this.EndDateTimePicker);
            this.groupBox1.Controls.Add(this.UseEarliestDateCheckBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Location = new System.Drawing.Point(12, 60);
            this.groupBox1.MinimumSize = new System.Drawing.Size(351, 240);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(490, 278);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter By";
            // 
            // FilterByDatesCheckBox
            // 
            this.FilterByDatesCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FilterByDatesCheckBox.AutoSize = true;
            this.FilterByDatesCheckBox.Location = new System.Drawing.Point(6, 207);
            this.FilterByDatesCheckBox.Name = "FilterByDatesCheckBox";
            this.FilterByDatesCheckBox.Size = new System.Drawing.Size(54, 17);
            this.FilterByDatesCheckBox.TabIndex = 32;
            this.FilterByDatesCheckBox.Text = "Dates";
            this.FilterByDatesCheckBox.UseVisualStyleBackColor = true;
            this.FilterByDatesCheckBox.CheckedChanged += new System.EventHandler(this.FilterByDatesCheckBox_CheckedChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 19);
            this.splitContainer1.MinimumSize = new System.Drawing.Size(344, 144);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.AllProjectsButton);
            this.splitContainer1.Panel1.Controls.Add(this.ClearProjectsButton);
            this.splitContainer1.Panel1.Controls.Add(this.ProjectsCheckedListBox);
            this.splitContainer1.Panel1.Controls.Add(this.FilterByProjectsCheckBox);
            this.splitContainer1.Panel1MinSize = 112;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2MinSize = 228;
            this.splitContainer1.Size = new System.Drawing.Size(483, 175);
            this.splitContainer1.SplitterDistance = 185;
            this.splitContainer1.TabIndex = 30;
            // 
            // AllProjectsButton
            // 
            this.AllProjectsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AllProjectsButton.Enabled = false;
            this.AllProjectsButton.Location = new System.Drawing.Point(3, 149);
            this.AllProjectsButton.Name = "AllProjectsButton";
            this.AllProjectsButton.Size = new System.Drawing.Size(50, 23);
            this.AllProjectsButton.TabIndex = 17;
            this.AllProjectsButton.Text = "All";
            this.AllProjectsButton.UseVisualStyleBackColor = true;
            this.AllProjectsButton.Click += new System.EventHandler(this.AllAnimalsButton_Click);
            // 
            // ClearProjectsButton
            // 
            this.ClearProjectsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearProjectsButton.Enabled = false;
            this.ClearProjectsButton.Location = new System.Drawing.Point(132, 149);
            this.ClearProjectsButton.Name = "ClearProjectsButton";
            this.ClearProjectsButton.Size = new System.Drawing.Size(50, 23);
            this.ClearProjectsButton.TabIndex = 18;
            this.ClearProjectsButton.Text = "Clear";
            this.ClearProjectsButton.UseVisualStyleBackColor = true;
            this.ClearProjectsButton.Click += new System.EventHandler(this.ClearAnimalsButton_Click);
            // 
            // ProjectsCheckedListBox
            // 
            this.ProjectsCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectsCheckedListBox.CheckOnClick = true;
            this.ProjectsCheckedListBox.Enabled = false;
            this.ProjectsCheckedListBox.FormattingEnabled = true;
            this.ProjectsCheckedListBox.Location = new System.Drawing.Point(3, 20);
            this.ProjectsCheckedListBox.Name = "ProjectsCheckedListBox";
            this.ProjectsCheckedListBox.Size = new System.Drawing.Size(179, 124);
            this.ProjectsCheckedListBox.TabIndex = 16;
            // 
            // FilterByProjectsCheckBox
            // 
            this.FilterByProjectsCheckBox.AutoSize = true;
            this.FilterByProjectsCheckBox.Location = new System.Drawing.Point(3, 3);
            this.FilterByProjectsCheckBox.Name = "FilterByProjectsCheckBox";
            this.FilterByProjectsCheckBox.Size = new System.Drawing.Size(64, 17);
            this.FilterByProjectsCheckBox.TabIndex = 28;
            this.FilterByProjectsCheckBox.Text = "Projects";
            this.FilterByProjectsCheckBox.UseVisualStyleBackColor = true;
            this.FilterByProjectsCheckBox.CheckedChanged += new System.EventHandler(this.FilterByProjectCheckBox_CheckedChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.SpeciesCheckedListBox);
            this.splitContainer2.Panel1.Controls.Add(this.AllSpeciesButton);
            this.splitContainer2.Panel1.Controls.Add(this.ClearSpeciesButton);
            this.splitContainer2.Panel1.Controls.Add(this.FilterBySpeciesCheckBox);
            this.splitContainer2.Panel1MinSize = 112;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.AnimalsCheckedListBox);
            this.splitContainer2.Panel2.Controls.Add(this.AllAnimalsButton);
            this.splitContainer2.Panel2.Controls.Add(this.FilterByAnimalsCheckBox);
            this.splitContainer2.Panel2.Controls.Add(this.ClearAnimalsButton);
            this.splitContainer2.Panel2MinSize = 112;
            this.splitContainer2.Size = new System.Drawing.Size(294, 175);
            this.splitContainer2.SplitterDistance = 112;
            this.splitContainer2.TabIndex = 0;
            // 
            // SpeciesCheckedListBox
            // 
            this.SpeciesCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpeciesCheckedListBox.CheckOnClick = true;
            this.SpeciesCheckedListBox.Enabled = false;
            this.SpeciesCheckedListBox.FormattingEnabled = true;
            this.SpeciesCheckedListBox.Location = new System.Drawing.Point(3, 20);
            this.SpeciesCheckedListBox.Name = "SpeciesCheckedListBox";
            this.SpeciesCheckedListBox.Size = new System.Drawing.Size(106, 124);
            this.SpeciesCheckedListBox.TabIndex = 8;
            // 
            // AllSpeciesButton
            // 
            this.AllSpeciesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AllSpeciesButton.Enabled = false;
            this.AllSpeciesButton.Location = new System.Drawing.Point(3, 149);
            this.AllSpeciesButton.Name = "AllSpeciesButton";
            this.AllSpeciesButton.Size = new System.Drawing.Size(50, 23);
            this.AllSpeciesButton.TabIndex = 9;
            this.AllSpeciesButton.Text = "All";
            this.AllSpeciesButton.UseVisualStyleBackColor = true;
            this.AllSpeciesButton.Click += new System.EventHandler(this.AllSpeciesButton_Click);
            // 
            // ClearSpeciesButton
            // 
            this.ClearSpeciesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearSpeciesButton.Enabled = false;
            this.ClearSpeciesButton.Location = new System.Drawing.Point(59, 149);
            this.ClearSpeciesButton.Name = "ClearSpeciesButton";
            this.ClearSpeciesButton.Size = new System.Drawing.Size(50, 23);
            this.ClearSpeciesButton.TabIndex = 10;
            this.ClearSpeciesButton.Text = "Clear";
            this.ClearSpeciesButton.UseVisualStyleBackColor = true;
            this.ClearSpeciesButton.Click += new System.EventHandler(this.ClearSpeciesButton_Click);
            // 
            // FilterBySpeciesCheckBox
            // 
            this.FilterBySpeciesCheckBox.AutoSize = true;
            this.FilterBySpeciesCheckBox.Location = new System.Drawing.Point(3, 3);
            this.FilterBySpeciesCheckBox.Name = "FilterBySpeciesCheckBox";
            this.FilterBySpeciesCheckBox.Size = new System.Drawing.Size(64, 17);
            this.FilterBySpeciesCheckBox.TabIndex = 27;
            this.FilterBySpeciesCheckBox.Text = "Species";
            this.FilterBySpeciesCheckBox.UseVisualStyleBackColor = true;
            this.FilterBySpeciesCheckBox.CheckedChanged += new System.EventHandler(this.FilterBySpeciesCheckBox_CheckedChanged);
            // 
            // AnimalsCheckedListBox
            // 
            this.AnimalsCheckedListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AnimalsCheckedListBox.CheckOnClick = true;
            this.AnimalsCheckedListBox.Enabled = false;
            this.AnimalsCheckedListBox.FormattingEnabled = true;
            this.AnimalsCheckedListBox.Location = new System.Drawing.Point(3, 20);
            this.AnimalsCheckedListBox.Name = "AnimalsCheckedListBox";
            this.AnimalsCheckedListBox.Size = new System.Drawing.Size(172, 124);
            this.AnimalsCheckedListBox.TabIndex = 12;
            // 
            // AllAnimalsButton
            // 
            this.AllAnimalsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AllAnimalsButton.Enabled = false;
            this.AllAnimalsButton.Location = new System.Drawing.Point(3, 149);
            this.AllAnimalsButton.Name = "AllAnimalsButton";
            this.AllAnimalsButton.Size = new System.Drawing.Size(50, 23);
            this.AllAnimalsButton.TabIndex = 13;
            this.AllAnimalsButton.Text = "All";
            this.AllAnimalsButton.UseVisualStyleBackColor = true;
            this.AllAnimalsButton.Click += new System.EventHandler(this.AllProjectsButton_Click);
            // 
            // FilterByAnimalsCheckBox
            // 
            this.FilterByAnimalsCheckBox.AutoSize = true;
            this.FilterByAnimalsCheckBox.Location = new System.Drawing.Point(3, 3);
            this.FilterByAnimalsCheckBox.Name = "FilterByAnimalsCheckBox";
            this.FilterByAnimalsCheckBox.Size = new System.Drawing.Size(62, 17);
            this.FilterByAnimalsCheckBox.TabIndex = 29;
            this.FilterByAnimalsCheckBox.Text = "Animals";
            this.FilterByAnimalsCheckBox.UseVisualStyleBackColor = true;
            this.FilterByAnimalsCheckBox.CheckedChanged += new System.EventHandler(this.FilterByAnimalCheckBox_CheckedChanged);
            // 
            // ClearAnimalsButton
            // 
            this.ClearAnimalsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearAnimalsButton.Enabled = false;
            this.ClearAnimalsButton.Location = new System.Drawing.Point(125, 149);
            this.ClearAnimalsButton.Name = "ClearAnimalsButton";
            this.ClearAnimalsButton.Size = new System.Drawing.Size(50, 23);
            this.ClearAnimalsButton.TabIndex = 14;
            this.ClearAnimalsButton.Text = "Clear";
            this.ClearAnimalsButton.UseVisualStyleBackColor = true;
            this.ClearAnimalsButton.Click += new System.EventHandler(this.ClearProjectsButton_Click);
            // 
            // CreateQueryLayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 385);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CreateMovementsCheckBox);
            this.Controls.Add(this.CreateLocationsCheckBox);
            this.Controls.Add(this.DatabaseComboBox);
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(392, 385);
            this.Name = "CreateQueryLayerForm";
            this.Text = "Create Map File";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckedListBox SpeciesCheckedListBox;
        private System.Windows.Forms.Button AllSpeciesButton;
        private System.Windows.Forms.Button ClearSpeciesButton;
        private System.Windows.Forms.Button ClearAnimalsButton;
        private System.Windows.Forms.Button AllAnimalsButton;
        private System.Windows.Forms.CheckedListBox AnimalsCheckedListBox;
        private System.Windows.Forms.CheckedListBox ProjectsCheckedListBox;
        private System.Windows.Forms.Button ClearProjectsButton;
        private System.Windows.Forms.Button AllProjectsButton;
        private System.Windows.Forms.ComboBox DatabaseComboBox;
        private System.Windows.Forms.DateTimePicker StartDateTimePicker;
        private System.Windows.Forms.DateTimePicker EndDateTimePicker;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox UseEarliestDateCheckBox;
        private System.Windows.Forms.CheckBox UseLatestDateCheckBox;
        private System.Windows.Forms.CheckBox CreateLocationsCheckBox;
        private System.Windows.Forms.CheckBox CreateMovementsCheckBox;
        private System.Windows.Forms.CheckBox FilterBySpeciesCheckBox;
        private System.Windows.Forms.CheckBox FilterByProjectsCheckBox;
        private System.Windows.Forms.CheckBox FilterByAnimalsCheckBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox FilterByDatesCheckBox;
    }
}