namespace AnimalMovement
{
    partial class AddDeploymentCsvForm
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
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.QuitButton = new System.Windows.Forms.Button();
            this.ProcessButton = new System.Windows.Forms.Button();
            this.InvestigatorComboBox = new System.Windows.Forms.ComboBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.ProgressTextBox = new System.Windows.Forms.TextBox();
            this.ExcelDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.ExcelDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.DefaultExt = "csv";
            this.OpenFileDialog.Filter = "CSV Files|*.csv|All Files|*.*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Project Investigator:";
            // 
            // QuitButton
            // 
            this.QuitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.QuitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.QuitButton.Location = new System.Drawing.Point(247, 310);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(75, 23);
            this.QuitButton.TabIndex = 6;
            this.QuitButton.Text = "Cancel";
            this.QuitButton.UseVisualStyleBackColor = true;
            this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // ProcessButton
            // 
            this.ProcessButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ProcessButton.Location = new System.Drawing.Point(328, 310);
            this.ProcessButton.Name = "ProcessButton";
            this.ProcessButton.Size = new System.Drawing.Size(75, 23);
            this.ProcessButton.TabIndex = 5;
            this.ProcessButton.Text = "Process";
            this.ProcessButton.UseVisualStyleBackColor = true;
            this.ProcessButton.Click += new System.EventHandler(this.ProcessButton_Click);
            // 
            // InvestigatorComboBox
            // 
            this.InvestigatorComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InvestigatorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InvestigatorComboBox.FormattingEnabled = true;
            this.InvestigatorComboBox.Location = new System.Drawing.Point(121, 12);
            this.InvestigatorComboBox.Name = "InvestigatorComboBox";
            this.InvestigatorComboBox.Size = new System.Drawing.Size(282, 21);
            this.InvestigatorComboBox.TabIndex = 4;
            this.InvestigatorComboBox.SelectedIndexChanged += new System.EventHandler(this.InvestigatorComboBox_SelectedIndexChanged);
            // 
            // BrowseButton
            // 
            this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseButton.Location = new System.Drawing.Point(377, 39);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(24, 23);
            this.BrowseButton.TabIndex = 29;
            this.BrowseButton.Text = "...";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "File:";
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileNameTextBox.Location = new System.Drawing.Point(50, 41);
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(321, 20);
            this.FileNameTextBox.TabIndex = 28;
            this.FileNameTextBox.TextChanged += new System.EventHandler(this.FileNameTextBox_TextChanged);
            // 
            // ProgressTextBox
            // 
            this.ProgressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressTextBox.Location = new System.Drawing.Point(11, 68);
            this.ProgressTextBox.Multiline = true;
            this.ProgressTextBox.Name = "ProgressTextBox";
            this.ProgressTextBox.Size = new System.Drawing.Size(390, 47);
            this.ProgressTextBox.TabIndex = 31;
            // 
            // ExcelDataGridView
            // 
            this.ExcelDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExcelDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ExcelDataGridView.Location = new System.Drawing.Point(11, 122);
            this.ExcelDataGridView.Name = "ExcelDataGridView";
            this.ExcelDataGridView.Size = new System.Drawing.Size(390, 182);
            this.ExcelDataGridView.TabIndex = 32;
            // 
            // AddDeploymentCsvForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.QuitButton;
            this.ClientSize = new System.Drawing.Size(413, 345);
            this.Controls.Add(this.ExcelDataGridView);
            this.Controls.Add(this.ProgressTextBox);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FileNameTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.QuitButton);
            this.Controls.Add(this.ProcessButton);
            this.Controls.Add(this.InvestigatorComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(275, 200);
            this.Name = "AddDeploymentCsvForm";
            this.Text = "Add CSV Deployment Data";
            ((System.ComponentModel.ISupportInitialize)(this.ExcelDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.Button ProcessButton;
        private System.Windows.Forms.ComboBox InvestigatorComboBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.TextBox ProgressTextBox;
        private System.Windows.Forms.DataGridView ExcelDataGridView;
    }
}