namespace AnimalMovement
{
    partial class AddEditorForm
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
            this.EditorTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CreateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SelectorComboBox = new System.Windows.Forms.ComboBox();
            this.InvestigatorLabel = new System.Windows.Forms.Label();
            this.FindButton = new System.Windows.Forms.Button();
            this.ResultsListBox = new System.Windows.Forms.ListBox();
            this.ProjectLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Editor:";
            // 
            // EditorTextBox
            // 
            this.EditorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EditorTextBox.Location = new System.Drawing.Point(51, 94);
            this.EditorTextBox.Name = "EditorTextBox";
            this.EditorTextBox.Size = new System.Drawing.Size(289, 20);
            this.EditorTextBox.TabIndex = 1;
            this.EditorTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditorTextBox_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(249, 39);
            this.label2.TabIndex = 2;
            this.label2.Text = "Enter as much of the user\'s last name as you know,\r\nthen press enter or click Fin" +
    "d.  Finally select the\r\ncorrect person from the list and click Create.";
            // 
            // CreateButton
            // 
            this.CreateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateButton.Location = new System.Drawing.Point(316, 209);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(75, 23);
            this.CreateButton.TabIndex = 5;
            this.CreateButton.Text = "Create";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(11, 209);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SelectorComboBox
            // 
            this.SelectorComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectorComboBox.FormattingEnabled = true;
            this.SelectorComboBox.Location = new System.Drawing.Point(119, 12);
            this.SelectorComboBox.Name = "SelectorComboBox";
            this.SelectorComboBox.Size = new System.Drawing.Size(272, 21);
            this.SelectorComboBox.TabIndex = 0;
            this.SelectorComboBox.SelectedIndexChanged += new System.EventHandler(this.ProjectComboBox_SelectedIndexChanged);
            // 
            // InvestigatorLabel
            // 
            this.InvestigatorLabel.AutoSize = true;
            this.InvestigatorLabel.Location = new System.Drawing.Point(12, 15);
            this.InvestigatorLabel.Name = "InvestigatorLabel";
            this.InvestigatorLabel.Size = new System.Drawing.Size(101, 13);
            this.InvestigatorLabel.TabIndex = 6;
            this.InvestigatorLabel.Text = "Project Investigator:";
            // 
            // FindButton
            // 
            this.FindButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindButton.Location = new System.Drawing.Point(346, 92);
            this.FindButton.Name = "FindButton";
            this.FindButton.Size = new System.Drawing.Size(45, 23);
            this.FindButton.TabIndex = 2;
            this.FindButton.Text = "Find";
            this.FindButton.UseVisualStyleBackColor = true;
            this.FindButton.Click += new System.EventHandler(this.FindButton_Click);
            // 
            // ResultsListBox
            // 
            this.ResultsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsListBox.FormattingEnabled = true;
            this.ResultsListBox.Location = new System.Drawing.Point(11, 121);
            this.ResultsListBox.Name = "ResultsListBox";
            this.ResultsListBox.Size = new System.Drawing.Size(380, 82);
            this.ResultsListBox.TabIndex = 3;
            this.ResultsListBox.SelectedIndexChanged += new System.EventHandler(this.ResultsListBox_SelectedIndexChanged);
            // 
            // ProjectLabel
            // 
            this.ProjectLabel.AutoSize = true;
            this.ProjectLabel.Location = new System.Drawing.Point(70, 15);
            this.ProjectLabel.Name = "ProjectLabel";
            this.ProjectLabel.Size = new System.Drawing.Size(43, 13);
            this.ProjectLabel.TabIndex = 11;
            this.ProjectLabel.Text = "Project:";
            // 
            // AddEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(405, 246);
            this.Controls.Add(this.ProjectLabel);
            this.Controls.Add(this.ResultsListBox);
            this.Controls.Add(this.FindButton);
            this.Controls.Add(this.InvestigatorLabel);
            this.Controls.Add(this.SelectorComboBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.EditorTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(275, 250);
            this.Name = "AddEditorForm";
            this.Text = "Add Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox EditorTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox SelectorComboBox;
        private System.Windows.Forms.Label InvestigatorLabel;
        private System.Windows.Forms.Button FindButton;
        private System.Windows.Forms.ListBox ResultsListBox;
        private System.Windows.Forms.Label ProjectLabel;
    }
}