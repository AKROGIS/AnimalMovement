namespace AD_test
{
    partial class Form1
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
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CheckButton = new System.Windows.Forms.Button();
            this.GetDomainsButton = new System.Windows.Forms.Button();
            this.ResultsListBox = new System.Windows.Forms.ListBox();
            this.ResultsGridView = new System.Windows.Forms.DataGridView();
            this.GetCatalogsButton = new System.Windows.Forms.Button();
            this.GetDCButton = new System.Windows.Forms.Button();
            this.GetGroupsButton = new System.Windows.Forms.Button();
            this.GetUsersButton = new System.Windows.Forms.Button();
            this.Name2TextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ResultsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // NameTextBox
            // 
            this.NameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameTextBox.Location = new System.Drawing.Point(95, 12);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(117, 20);
            this.NameTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Domain Name:";
            // 
            // CheckButton
            // 
            this.CheckButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckButton.Location = new System.Drawing.Point(218, 10);
            this.CheckButton.Name = "CheckButton";
            this.CheckButton.Size = new System.Drawing.Size(75, 23);
            this.CheckButton.TabIndex = 2;
            this.CheckButton.Text = "Check";
            this.CheckButton.UseVisualStyleBackColor = true;
            this.CheckButton.Click += new System.EventHandler(this.CheckButton_Click);
            // 
            // GetDomainsButton
            // 
            this.GetDomainsButton.Location = new System.Drawing.Point(12, 42);
            this.GetDomainsButton.Name = "GetDomainsButton";
            this.GetDomainsButton.Size = new System.Drawing.Size(75, 23);
            this.GetDomainsButton.TabIndex = 3;
            this.GetDomainsButton.Text = "Domains";
            this.GetDomainsButton.UseVisualStyleBackColor = true;
            this.GetDomainsButton.Click += new System.EventHandler(this.GetDOmainsButton_Click);
            // 
            // ResultsListBox
            // 
            this.ResultsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsListBox.FormattingEnabled = true;
            this.ResultsListBox.Location = new System.Drawing.Point(12, 71);
            this.ResultsListBox.Name = "ResultsListBox";
            this.ResultsListBox.Size = new System.Drawing.Size(477, 277);
            this.ResultsListBox.TabIndex = 4;
            // 
            // ResultsGridView
            // 
            this.ResultsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultsGridView.Location = new System.Drawing.Point(12, 354);
            this.ResultsGridView.Name = "ResultsGridView";
            this.ResultsGridView.Size = new System.Drawing.Size(476, 264);
            this.ResultsGridView.TabIndex = 5;
            // 
            // GetCatalogsButton
            // 
            this.GetCatalogsButton.Location = new System.Drawing.Point(95, 42);
            this.GetCatalogsButton.Name = "GetCatalogsButton";
            this.GetCatalogsButton.Size = new System.Drawing.Size(75, 23);
            this.GetCatalogsButton.TabIndex = 6;
            this.GetCatalogsButton.Text = "Catalogs";
            this.GetCatalogsButton.UseVisualStyleBackColor = true;
            this.GetCatalogsButton.Click += new System.EventHandler(this.GetCatalogsButton_Click);
            // 
            // GetDCButton
            // 
            this.GetDCButton.Location = new System.Drawing.Point(176, 42);
            this.GetDCButton.Name = "GetDCButton";
            this.GetDCButton.Size = new System.Drawing.Size(75, 23);
            this.GetDCButton.TabIndex = 7;
            this.GetDCButton.Text = "DC";
            this.GetDCButton.UseVisualStyleBackColor = true;
            this.GetDCButton.Click += new System.EventHandler(this.GetDCButton_Click);
            // 
            // GetGroupsButton
            // 
            this.GetGroupsButton.Location = new System.Drawing.Point(257, 42);
            this.GetGroupsButton.Name = "GetGroupsButton";
            this.GetGroupsButton.Size = new System.Drawing.Size(75, 23);
            this.GetGroupsButton.TabIndex = 8;
            this.GetGroupsButton.Text = "Groups";
            this.GetGroupsButton.UseVisualStyleBackColor = true;
            this.GetGroupsButton.Click += new System.EventHandler(this.GetGroupsButton_Click);
            // 
            // GetUsersButton
            // 
            this.GetUsersButton.Location = new System.Drawing.Point(338, 42);
            this.GetUsersButton.Name = "GetUsersButton";
            this.GetUsersButton.Size = new System.Drawing.Size(75, 23);
            this.GetUsersButton.TabIndex = 9;
            this.GetUsersButton.Text = "Users";
            this.GetUsersButton.UseVisualStyleBackColor = true;
            this.GetUsersButton.Click += new System.EventHandler(this.GetUsersButton_Click);
            // 
            // Name2TextBox
            // 
            this.Name2TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Name2TextBox.Location = new System.Drawing.Point(299, 13);
            this.Name2TextBox.Name = "Name2TextBox";
            this.Name2TextBox.Size = new System.Drawing.Size(178, 20);
            this.Name2TextBox.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 630);
            this.Controls.Add(this.Name2TextBox);
            this.Controls.Add(this.GetUsersButton);
            this.Controls.Add(this.GetGroupsButton);
            this.Controls.Add(this.GetDCButton);
            this.Controls.Add(this.GetCatalogsButton);
            this.Controls.Add(this.ResultsGridView);
            this.Controls.Add(this.ResultsListBox);
            this.Controls.Add(this.GetDomainsButton);
            this.Controls.Add(this.CheckButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NameTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ResultsGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CheckButton;
        private System.Windows.Forms.Button GetDomainsButton;
        private System.Windows.Forms.ListBox ResultsListBox;
        private System.Windows.Forms.DataGridView ResultsGridView;
        private System.Windows.Forms.Button GetCatalogsButton;
        private System.Windows.Forms.Button GetDCButton;
        private System.Windows.Forms.Button GetGroupsButton;
        private System.Windows.Forms.Button GetUsersButton;
        private System.Windows.Forms.TextBox Name2TextBox;
    }
}

