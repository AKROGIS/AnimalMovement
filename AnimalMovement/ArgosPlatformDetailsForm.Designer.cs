namespace AnimalMovement
{
    partial class ArgosPlatformDetailsForm
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
            this.components = new System.ComponentModel.Container();
            this.cancelButton = new System.Windows.Forms.Button();
            this.EditSaveButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.NotesTextBox = new System.Windows.Forms.TextBox();
            this.ActiveCheckBox = new System.Windows.Forms.CheckBox();
            this.ArgosProgramComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ArgosIdTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DisposalDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ArgosTabControl = new System.Windows.Forms.TabControl();
            this.DetailsTabPage = new System.Windows.Forms.TabPage();
            this.CollarsTabPage = new System.Windows.Forms.TabPage();
            this.EditCollarButton = new System.Windows.Forms.Button();
            this.AddCollarButton = new System.Windows.Forms.Button();
            this.DeleteCollarButton = new System.Windows.Forms.Button();
            this.InfoCollarButton = new System.Windows.Forms.Button();
            this.CollarsDataGridView = new System.Windows.Forms.DataGridView();
            this.ParametersTabPage = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.ParametersDataGridView = new System.Windows.Forms.DataGridView();
            this.DownloadsTabPage = new System.Windows.Forms.TabPage();
            this.DownloadsDataGridView = new System.Windows.Forms.DataGridView();
            this.ProgramDownloadsButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.TransmissionsTabPage = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.TransmissionsDataGridView = new System.Windows.Forms.DataGridView();
            this.ProcessingIssuesTabPage = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.ProcessingIssuesDataGridView = new System.Windows.Forms.DataGridView();
            this.DerivedDataTabPage = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.DerivedDataGridView = new System.Windows.Forms.DataGridView();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ArgosTabControl.SuspendLayout();
            this.DetailsTabPage.SuspendLayout();
            this.CollarsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CollarsDataGridView)).BeginInit();
            this.ParametersTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParametersDataGridView)).BeginInit();
            this.DownloadsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DownloadsDataGridView)).BeginInit();
            this.TransmissionsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TransmissionsDataGridView)).BeginInit();
            this.ProcessingIssuesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessingIssuesDataGridView)).BeginInit();
            this.DerivedDataTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DerivedDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.Location = new System.Drawing.Point(6, 232);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 29;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Visible = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // EditSaveButton
            // 
            this.EditSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EditSaveButton.Location = new System.Drawing.Point(421, 232);
            this.EditSaveButton.Name = "EditSaveButton";
            this.EditSaveButton.Size = new System.Drawing.Size(75, 23);
            this.EditSaveButton.TabIndex = 28;
            this.EditSaveButton.Text = "Edit";
            this.EditSaveButton.UseVisualStyleBackColor = true;
            this.EditSaveButton.Click += new System.EventHandler(this.EditSaveButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "Notes:";
            // 
            // NotesTextBox
            // 
            this.NotesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NotesTextBox.Location = new System.Drawing.Point(6, 82);
            this.NotesTextBox.Multiline = true;
            this.NotesTextBox.Name = "NotesTextBox";
            this.NotesTextBox.Size = new System.Drawing.Size(490, 144);
            this.NotesTextBox.TabIndex = 26;
            // 
            // ActiveCheckBox
            // 
            this.ActiveCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveCheckBox.AutoSize = true;
            this.ActiveCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ActiveCheckBox.Checked = true;
            this.ActiveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ActiveCheckBox.Location = new System.Drawing.Point(362, 59);
            this.ActiveCheckBox.Name = "ActiveCheckBox";
            this.ActiveCheckBox.Size = new System.Drawing.Size(134, 17);
            this.ActiveCheckBox.TabIndex = 25;
            this.ActiveCheckBox.Text = "Download this Platform";
            this.ActiveCheckBox.UseVisualStyleBackColor = true;
            // 
            // ArgosProgramComboBox
            // 
            this.ArgosProgramComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ArgosProgramComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ArgosProgramComboBox.FormattingEnabled = true;
            this.ArgosProgramComboBox.Location = new System.Drawing.Point(116, 6);
            this.ArgosProgramComboBox.Name = "ArgosProgramComboBox";
            this.ArgosProgramComboBox.Size = new System.Drawing.Size(380, 21);
            this.ArgosProgramComboBox.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Argos Program:";
            // 
            // ArgosIdTextBox
            // 
            this.ArgosIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ArgosIdTextBox.Enabled = false;
            this.ArgosIdTextBox.Location = new System.Drawing.Point(67, 12);
            this.ArgosIdTextBox.Name = "ArgosIdTextBox";
            this.ArgosIdTextBox.Size = new System.Drawing.Size(455, 20);
            this.ArgosIdTextBox.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Argos Id:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Disposal Date/Time:";
            // 
            // DisposalDateTimePicker
            // 
            this.DisposalDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DisposalDateTimePicker.Checked = false;
            this.DisposalDateTimePicker.CustomFormat = " ";
            this.DisposalDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DisposalDateTimePicker.Location = new System.Drawing.Point(116, 33);
            this.DisposalDateTimePicker.Name = "DisposalDateTimePicker";
            this.DisposalDateTimePicker.ShowCheckBox = true;
            this.DisposalDateTimePicker.Size = new System.Drawing.Size(380, 20);
            this.DisposalDateTimePicker.TabIndex = 19;
            this.DisposalDateTimePicker.Value = new System.DateTime(2013, 4, 19, 0, 0, 0, 0);
            this.DisposalDateTimePicker.ValueChanged += new System.EventHandler(this.DisposalDateTimePicker_ValueChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipTitle = "Download state of the program  governs unless it is not set.";
            // 
            // ArgosTabControl
            // 
            this.ArgosTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ArgosTabControl.Controls.Add(this.DetailsTabPage);
            this.ArgosTabControl.Controls.Add(this.CollarsTabPage);
            this.ArgosTabControl.Controls.Add(this.ParametersTabPage);
            this.ArgosTabControl.Controls.Add(this.DownloadsTabPage);
            this.ArgosTabControl.Controls.Add(this.TransmissionsTabPage);
            this.ArgosTabControl.Controls.Add(this.ProcessingIssuesTabPage);
            this.ArgosTabControl.Controls.Add(this.DerivedDataTabPage);
            this.ArgosTabControl.Location = new System.Drawing.Point(12, 38);
            this.ArgosTabControl.Name = "ArgosTabControl";
            this.ArgosTabControl.SelectedIndex = 0;
            this.ArgosTabControl.Size = new System.Drawing.Size(510, 287);
            this.ArgosTabControl.TabIndex = 30;
            this.ArgosTabControl.SelectedIndexChanged += new System.EventHandler(this.ArgosTabControl_SelectedIndexChanged);
            // 
            // DetailsTabPage
            // 
            this.DetailsTabPage.Controls.Add(this.NotesTextBox);
            this.DetailsTabPage.Controls.Add(this.cancelButton);
            this.DetailsTabPage.Controls.Add(this.DisposalDateTimePicker);
            this.DetailsTabPage.Controls.Add(this.EditSaveButton);
            this.DetailsTabPage.Controls.Add(this.label3);
            this.DetailsTabPage.Controls.Add(this.label4);
            this.DetailsTabPage.Controls.Add(this.label2);
            this.DetailsTabPage.Controls.Add(this.ArgosProgramComboBox);
            this.DetailsTabPage.Controls.Add(this.ActiveCheckBox);
            this.DetailsTabPage.Location = new System.Drawing.Point(4, 22);
            this.DetailsTabPage.Name = "DetailsTabPage";
            this.DetailsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.DetailsTabPage.Size = new System.Drawing.Size(502, 261);
            this.DetailsTabPage.TabIndex = 0;
            this.DetailsTabPage.Text = "Details";
            this.DetailsTabPage.UseVisualStyleBackColor = true;
            // 
            // CollarsTabPage
            // 
            this.CollarsTabPage.Controls.Add(this.EditCollarButton);
            this.CollarsTabPage.Controls.Add(this.AddCollarButton);
            this.CollarsTabPage.Controls.Add(this.DeleteCollarButton);
            this.CollarsTabPage.Controls.Add(this.InfoCollarButton);
            this.CollarsTabPage.Controls.Add(this.CollarsDataGridView);
            this.CollarsTabPage.Location = new System.Drawing.Point(4, 22);
            this.CollarsTabPage.Name = "CollarsTabPage";
            this.CollarsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.CollarsTabPage.Size = new System.Drawing.Size(502, 261);
            this.CollarsTabPage.TabIndex = 1;
            this.CollarsTabPage.Text = "Collars";
            this.CollarsTabPage.UseVisualStyleBackColor = true;
            // 
            // EditCollarButton
            // 
            this.EditCollarButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.EditCollarButton.FlatAppearance.BorderSize = 0;
            this.EditCollarButton.Image = global::AnimalMovement.Properties.Resources.GenericPencil16;
            this.EditCollarButton.Location = new System.Drawing.Point(57, 234);
            this.EditCollarButton.Name = "EditCollarButton";
            this.EditCollarButton.Size = new System.Drawing.Size(24, 24);
            this.EditCollarButton.TabIndex = 34;
            this.EditCollarButton.UseVisualStyleBackColor = true;
            this.EditCollarButton.Click += new System.EventHandler(this.EditCollarButton_Click);
            // 
            // AddCollarButton
            // 
            this.AddCollarButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddCollarButton.Enabled = false;
            this.AddCollarButton.Image = global::AnimalMovement.Properties.Resources.GenericAddGreen16;
            this.AddCollarButton.Location = new System.Drawing.Point(3, 234);
            this.AddCollarButton.Name = "AddCollarButton";
            this.AddCollarButton.Size = new System.Drawing.Size(24, 24);
            this.AddCollarButton.TabIndex = 31;
            this.AddCollarButton.UseVisualStyleBackColor = true;
            this.AddCollarButton.Click += new System.EventHandler(this.AddCollarButton_Click);
            // 
            // DeleteCollarButton
            // 
            this.DeleteCollarButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DeleteCollarButton.Enabled = false;
            this.DeleteCollarButton.Image = global::AnimalMovement.Properties.Resources.GenericDeleteRed16;
            this.DeleteCollarButton.Location = new System.Drawing.Point(30, 234);
            this.DeleteCollarButton.Name = "DeleteCollarButton";
            this.DeleteCollarButton.Size = new System.Drawing.Size(24, 24);
            this.DeleteCollarButton.TabIndex = 32;
            this.DeleteCollarButton.UseVisualStyleBackColor = true;
            this.DeleteCollarButton.Click += new System.EventHandler(this.DeleteCollarButton_Click);
            // 
            // InfoCollarButton
            // 
            this.InfoCollarButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InfoCollarButton.FlatAppearance.BorderSize = 0;
            this.InfoCollarButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoCollarButton.Image = global::AnimalMovement.Properties.Resources.GenericInformation_B_16;
            this.InfoCollarButton.Location = new System.Drawing.Point(81, 234);
            this.InfoCollarButton.Name = "InfoCollarButton";
            this.InfoCollarButton.Size = new System.Drawing.Size(24, 24);
            this.InfoCollarButton.TabIndex = 33;
            this.InfoCollarButton.UseVisualStyleBackColor = true;
            this.InfoCollarButton.Click += new System.EventHandler(this.InfoCollarButton_Click);
            // 
            // CollarsDataGridView
            // 
            this.CollarsDataGridView.AllowUserToOrderColumns = true;
            this.CollarsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.CollarsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CollarsDataGridView.Location = new System.Drawing.Point(3, 3);
            this.CollarsDataGridView.Name = "CollarsDataGridView";
            this.CollarsDataGridView.ReadOnly = true;
            this.CollarsDataGridView.RowHeadersVisible = false;
            this.CollarsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CollarsDataGridView.Size = new System.Drawing.Size(496, 225);
            this.CollarsDataGridView.TabIndex = 4;
            this.CollarsDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.CollarDataGridView_CellDoubleClick);
            this.CollarsDataGridView.SelectionChanged += new System.EventHandler(this.CollarDataGridView_SelectionChanged);
            // 
            // ParametersTabPage
            // 
            this.ParametersTabPage.Controls.Add(this.label9);
            this.ParametersTabPage.Controls.Add(this.ParametersDataGridView);
            this.ParametersTabPage.Location = new System.Drawing.Point(4, 22);
            this.ParametersTabPage.Name = "ParametersTabPage";
            this.ParametersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ParametersTabPage.Size = new System.Drawing.Size(502, 261);
            this.ParametersTabPage.TabIndex = 2;
            this.ParametersTabPage.Text = "Parameter Files";
            this.ParametersTabPage.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(308, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Telonics Gen4 Parameter Files which include this Argos platform";
            // 
            // ParametersDataGridView
            // 
            this.ParametersDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ParametersDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ParametersDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ParametersDataGridView.Location = new System.Drawing.Point(3, 28);
            this.ParametersDataGridView.Name = "ParametersDataGridView";
            this.ParametersDataGridView.ReadOnly = true;
            this.ParametersDataGridView.RowHeadersVisible = false;
            this.ParametersDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ParametersDataGridView.Size = new System.Drawing.Size(496, 230);
            this.ParametersDataGridView.TabIndex = 4;
            this.ParametersDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ParametersDataGridView_CellDoubleClick);
            // 
            // DownloadsTabPage
            // 
            this.DownloadsTabPage.Controls.Add(this.DownloadsDataGridView);
            this.DownloadsTabPage.Controls.Add(this.ProgramDownloadsButton);
            this.DownloadsTabPage.Controls.Add(this.label5);
            this.DownloadsTabPage.Location = new System.Drawing.Point(4, 22);
            this.DownloadsTabPage.Name = "DownloadsTabPage";
            this.DownloadsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.DownloadsTabPage.Size = new System.Drawing.Size(502, 261);
            this.DownloadsTabPage.TabIndex = 6;
            this.DownloadsTabPage.Text = "Downloads";
            this.DownloadsTabPage.UseVisualStyleBackColor = true;
            // 
            // DownloadsDataGridView
            // 
            this.DownloadsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DownloadsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DownloadsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DownloadsDataGridView.Location = new System.Drawing.Point(3, 28);
            this.DownloadsDataGridView.Name = "DownloadsDataGridView";
            this.DownloadsDataGridView.ReadOnly = true;
            this.DownloadsDataGridView.RowHeadersVisible = false;
            this.DownloadsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DownloadsDataGridView.Size = new System.Drawing.Size(496, 197);
            this.DownloadsDataGridView.TabIndex = 2;
            this.DownloadsDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DownloadsDataGridView_CellDoubleClick);
            // 
            // ProgramDownloadsButton
            // 
            this.ProgramDownloadsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ProgramDownloadsButton.Location = new System.Drawing.Point(6, 231);
            this.ProgramDownloadsButton.Name = "ProgramDownloadsButton";
            this.ProgramDownloadsButton.Size = new System.Drawing.Size(243, 23);
            this.ProgramDownloadsButton.TabIndex = 1;
            this.ProgramDownloadsButton.Text = "Show Downloads for the Entire Argos Program";
            this.ProgramDownloadsButton.UseVisualStyleBackColor = true;
            this.ProgramDownloadsButton.Click += new System.EventHandler(this.ProgramDownloadsButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(466, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Automatic downloads from the Argos website for just this Argos platform (max of 1" +
    "0 days available)";
            // 
            // TransmissionsTabPage
            // 
            this.TransmissionsTabPage.Controls.Add(this.label7);
            this.TransmissionsTabPage.Controls.Add(this.TransmissionsDataGridView);
            this.TransmissionsTabPage.Location = new System.Drawing.Point(4, 22);
            this.TransmissionsTabPage.Name = "TransmissionsTabPage";
            this.TransmissionsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.TransmissionsTabPage.Size = new System.Drawing.Size(502, 261);
            this.TransmissionsTabPage.TabIndex = 4;
            this.TransmissionsTabPage.Text = "Transmissions";
            this.TransmissionsTabPage.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(254, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Collar files containing raw data for this Argos platform";
            // 
            // TransmissionsDataGridView
            // 
            this.TransmissionsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TransmissionsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.TransmissionsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TransmissionsDataGridView.Location = new System.Drawing.Point(3, 28);
            this.TransmissionsDataGridView.Name = "TransmissionsDataGridView";
            this.TransmissionsDataGridView.ReadOnly = true;
            this.TransmissionsDataGridView.RowHeadersVisible = false;
            this.TransmissionsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.TransmissionsDataGridView.Size = new System.Drawing.Size(496, 230);
            this.TransmissionsDataGridView.TabIndex = 4;
            this.TransmissionsDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TransmissionsDataGridView_CellDoubleClick);
            // 
            // ProcessingIssuesTabPage
            // 
            this.ProcessingIssuesTabPage.Controls.Add(this.label8);
            this.ProcessingIssuesTabPage.Controls.Add(this.ProcessingIssuesDataGridView);
            this.ProcessingIssuesTabPage.Location = new System.Drawing.Point(4, 22);
            this.ProcessingIssuesTabPage.Name = "ProcessingIssuesTabPage";
            this.ProcessingIssuesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ProcessingIssuesTabPage.Size = new System.Drawing.Size(502, 261);
            this.ProcessingIssuesTabPage.TabIndex = 3;
            this.ProcessingIssuesTabPage.Text = "Processing Issues";
            this.ProcessingIssuesTabPage.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(443, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Results (or problems) encountered when trying to process the raw data for this Ar" +
    "gos platform";
            // 
            // ProcessingIssuesDataGridView
            // 
            this.ProcessingIssuesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProcessingIssuesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ProcessingIssuesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ProcessingIssuesDataGridView.Location = new System.Drawing.Point(3, 28);
            this.ProcessingIssuesDataGridView.Name = "ProcessingIssuesDataGridView";
            this.ProcessingIssuesDataGridView.ReadOnly = true;
            this.ProcessingIssuesDataGridView.RowHeadersVisible = false;
            this.ProcessingIssuesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ProcessingIssuesDataGridView.Size = new System.Drawing.Size(496, 230);
            this.ProcessingIssuesDataGridView.TabIndex = 3;
            this.ProcessingIssuesDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ProcessingIssuesDataGridView_CellDoubleClick);
            // 
            // DerivedDataTabPage
            // 
            this.DerivedDataTabPage.Controls.Add(this.label6);
            this.DerivedDataTabPage.Controls.Add(this.DerivedDataGridView);
            this.DerivedDataTabPage.Location = new System.Drawing.Point(4, 22);
            this.DerivedDataTabPage.Name = "DerivedDataTabPage";
            this.DerivedDataTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.DerivedDataTabPage.Size = new System.Drawing.Size(502, 261);
            this.DerivedDataTabPage.TabIndex = 5;
            this.DerivedDataTabPage.Text = "Derived Data";
            this.DerivedDataTabPage.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(275, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Derived collar data files dependent on this Argos platform";
            // 
            // DerivedDataGridView
            // 
            this.DerivedDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DerivedDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DerivedDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DerivedDataGridView.Location = new System.Drawing.Point(3, 28);
            this.DerivedDataGridView.Name = "DerivedDataGridView";
            this.DerivedDataGridView.ReadOnly = true;
            this.DerivedDataGridView.RowHeadersVisible = false;
            this.DerivedDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DerivedDataGridView.Size = new System.Drawing.Size(496, 230);
            this.DerivedDataGridView.TabIndex = 4;
            this.DerivedDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DerivedDataGridView_CellDoubleClick);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(447, 331);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 30;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // ArgosPlatformDetailsForm
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(534, 366);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ArgosTabControl);
            this.Controls.Add(this.ArgosIdTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "ArgosPlatformDetailsForm";
            this.Text = "Argos Platform Details";
            this.ArgosTabControl.ResumeLayout(false);
            this.DetailsTabPage.ResumeLayout(false);
            this.DetailsTabPage.PerformLayout();
            this.CollarsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CollarsDataGridView)).EndInit();
            this.ParametersTabPage.ResumeLayout(false);
            this.ParametersTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParametersDataGridView)).EndInit();
            this.DownloadsTabPage.ResumeLayout(false);
            this.DownloadsTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DownloadsDataGridView)).EndInit();
            this.TransmissionsTabPage.ResumeLayout(false);
            this.TransmissionsTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TransmissionsDataGridView)).EndInit();
            this.ProcessingIssuesTabPage.ResumeLayout(false);
            this.ProcessingIssuesTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessingIssuesDataGridView)).EndInit();
            this.DerivedDataTabPage.ResumeLayout(false);
            this.DerivedDataTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DerivedDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button EditSaveButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox NotesTextBox;
        private System.Windows.Forms.CheckBox ActiveCheckBox;
        private System.Windows.Forms.ComboBox ArgosProgramComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ArgosIdTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker DisposalDateTimePicker;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabControl ArgosTabControl;
        private System.Windows.Forms.TabPage DetailsTabPage;
        private System.Windows.Forms.TabPage CollarsTabPage;
        private System.Windows.Forms.TabPage ParametersTabPage;
        private System.Windows.Forms.TabPage ProcessingIssuesTabPage;
        private System.Windows.Forms.TabPage TransmissionsTabPage;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TabPage DerivedDataTabPage;
        private System.Windows.Forms.TabPage DownloadsTabPage;
        private System.Windows.Forms.Button ProgramDownloadsButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView DownloadsDataGridView;
        private System.Windows.Forms.DataGridView ProcessingIssuesDataGridView;
        private System.Windows.Forms.DataGridView CollarsDataGridView;
        private System.Windows.Forms.DataGridView ParametersDataGridView;
        private System.Windows.Forms.DataGridView TransmissionsDataGridView;
        private System.Windows.Forms.DataGridView DerivedDataGridView;
        private System.Windows.Forms.Button EditCollarButton;
        private System.Windows.Forms.Button AddCollarButton;
        private System.Windows.Forms.Button DeleteCollarButton;
        private System.Windows.Forms.Button InfoCollarButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}