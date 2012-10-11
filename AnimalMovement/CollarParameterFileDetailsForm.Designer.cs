namespace AnimalMovement
{
    partial class CollarParameterFileDetailsForm
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
            this.DoneCancelButton = new System.Windows.Forms.Button();
            this.EditSaveButton = new System.Windows.Forms.Button();
            this.CollarsDataGridView = new System.Windows.Forms.DataGridView();
            this.ShowContentsButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FileIdTextBox = new System.Windows.Forms.TextBox();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.UploadDateTextBox = new System.Windows.Forms.TextBox();
            this.OwnerTextBox = new System.Windows.Forms.TextBox();
            this.FormatTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.FileNameTextBox = new System.Windows.Forms.TextBox();
            this.CollarManufacturerColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CollarIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.CollarsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // DoneCancelButton
            // 
            this.DoneCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DoneCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneCancelButton.Location = new System.Drawing.Point(16, 231);
            this.DoneCancelButton.Name = "DoneCancelButton";
            this.DoneCancelButton.Size = new System.Drawing.Size(75, 23);
            this.DoneCancelButton.TabIndex = 70;
            this.DoneCancelButton.Text = "Done";
            this.DoneCancelButton.UseVisualStyleBackColor = true;
            this.DoneCancelButton.Click += new System.EventHandler(this.DoneCancelButton_Click);
            // 
            // EditSaveButton
            // 
            this.EditSaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EditSaveButton.BackColor = System.Drawing.SystemColors.Control;
            this.EditSaveButton.Location = new System.Drawing.Point(405, 231);
            this.EditSaveButton.Name = "EditSaveButton";
            this.EditSaveButton.Size = new System.Drawing.Size(75, 23);
            this.EditSaveButton.TabIndex = 71;
            this.EditSaveButton.Text = "Edit";
            this.EditSaveButton.UseVisualStyleBackColor = true;
            this.EditSaveButton.Click += new System.EventHandler(this.EditSaveButton_Click);
            // 
            // CollarsDataGridView
            // 
            this.CollarsDataGridView.AllowUserToAddRows = false;
            this.CollarsDataGridView.AllowUserToDeleteRows = false;
            this.CollarsDataGridView.AllowUserToOrderColumns = true;
            this.CollarsDataGridView.AllowUserToResizeRows = false;
            this.CollarsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CollarsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CollarsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CollarManufacturerColumn,
            this.CollarIdColumn,
            this.FileIdColumn,
            this.StartDateColumn,
            this.EndDateColumn});
            this.CollarsDataGridView.Location = new System.Drawing.Point(17, 95);
            this.CollarsDataGridView.MultiSelect = false;
            this.CollarsDataGridView.Name = "CollarsDataGridView";
            this.CollarsDataGridView.ReadOnly = true;
            this.CollarsDataGridView.RowHeadersVisible = false;
            this.CollarsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CollarsDataGridView.Size = new System.Drawing.Size(463, 127);
            this.CollarsDataGridView.TabIndex = 60;
            // 
            // ShowContentsButton
            // 
            this.ShowContentsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowContentsButton.Location = new System.Drawing.Point(186, 231);
            this.ShowContentsButton.Name = "ShowContentsButton";
            this.ShowContentsButton.Size = new System.Drawing.Size(116, 23);
            this.ShowContentsButton.TabIndex = 59;
            this.ShowContentsButton.Text = "Show Contents";
            this.ShowContentsButton.UseVisualStyleBackColor = true;
            this.ShowContentsButton.Click += new System.EventHandler(this.ShowContentsButton_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(241, 67);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 13);
            this.label9.TabIndex = 69;
            this.label9.Text = "Uploaded By:";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(242, 41);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 13);
            this.label8.TabIndex = 68;
            this.label8.Text = "Upload Date:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(274, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 66;
            this.label3.Text = "File Id:";
            // 
            // FileIdTextBox
            // 
            this.FileIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FileIdTextBox.Enabled = false;
            this.FileIdTextBox.Location = new System.Drawing.Point(318, 12);
            this.FileIdTextBox.Name = "FileIdTextBox";
            this.FileIdTextBox.Size = new System.Drawing.Size(162, 20);
            this.FileIdTextBox.TabIndex = 49;
            // 
            // UserNameTextBox
            // 
            this.UserNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UserNameTextBox.Enabled = false;
            this.UserNameTextBox.Location = new System.Drawing.Point(318, 64);
            this.UserNameTextBox.Name = "UserNameTextBox";
            this.UserNameTextBox.Size = new System.Drawing.Size(162, 20);
            this.UserNameTextBox.TabIndex = 56;
            // 
            // UploadDateTextBox
            // 
            this.UploadDateTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UploadDateTextBox.Enabled = false;
            this.UploadDateTextBox.Location = new System.Drawing.Point(318, 38);
            this.UploadDateTextBox.Name = "UploadDateTextBox";
            this.UploadDateTextBox.Size = new System.Drawing.Size(162, 20);
            this.UploadDateTextBox.TabIndex = 54;
            // 
            // OwnerTextBox
            // 
            this.OwnerTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OwnerTextBox.Enabled = false;
            this.OwnerTextBox.Location = new System.Drawing.Point(62, 38);
            this.OwnerTextBox.Name = "OwnerTextBox";
            this.OwnerTextBox.Size = new System.Drawing.Size(162, 20);
            this.OwnerTextBox.TabIndex = 55;
            // 
            // FormatTextBox
            // 
            this.FormatTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FormatTextBox.Enabled = false;
            this.FormatTextBox.Location = new System.Drawing.Point(62, 64);
            this.FormatTextBox.Name = "FormatTextBox";
            this.FormatTextBox.Size = new System.Drawing.Size(162, 20);
            this.FormatTextBox.TabIndex = 53;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 65;
            this.label6.Text = "Owner:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 62;
            this.label2.Text = "Format:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 61;
            this.label1.Text = "Name:";
            // 
            // FileNameTextBox
            // 
            this.FileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileNameTextBox.Enabled = false;
            this.FileNameTextBox.Location = new System.Drawing.Point(62, 12);
            this.FileNameTextBox.MaxLength = 255;
            this.FileNameTextBox.Name = "FileNameTextBox";
            this.FileNameTextBox.Size = new System.Drawing.Size(162, 20);
            this.FileNameTextBox.TabIndex = 51;
            // 
            // CollarManufacturerColumn
            // 
            this.CollarManufacturerColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CollarManufacturerColumn.DataPropertyName = "CollarManufacturer";
            this.CollarManufacturerColumn.HeaderText = "Manufacturer";
            this.CollarManufacturerColumn.MinimumWidth = 80;
            this.CollarManufacturerColumn.Name = "CollarManufacturerColumn";
            this.CollarManufacturerColumn.ReadOnly = true;
            this.CollarManufacturerColumn.Width = 95;
            // 
            // CollarIdColumn
            // 
            this.CollarIdColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CollarIdColumn.DataPropertyName = "CollarId";
            this.CollarIdColumn.HeaderText = "Collar";
            this.CollarIdColumn.MinimumWidth = 70;
            this.CollarIdColumn.Name = "CollarIdColumn";
            this.CollarIdColumn.ReadOnly = true;
            // 
            // FileIdColumn
            // 
            this.FileIdColumn.DataPropertyName = "FileId";
            this.FileIdColumn.HeaderText = "File Id";
            this.FileIdColumn.Name = "FileIdColumn";
            this.FileIdColumn.ReadOnly = true;
            this.FileIdColumn.Visible = false;
            // 
            // StartDateColumn
            // 
            this.StartDateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.StartDateColumn.DataPropertyName = "StartDate";
            this.StartDateColumn.HeaderText = "Start Date";
            this.StartDateColumn.MinimumWidth = 110;
            this.StartDateColumn.Name = "StartDateColumn";
            this.StartDateColumn.ReadOnly = true;
            this.StartDateColumn.Width = 110;
            // 
            // EndDateColumn
            // 
            this.EndDateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.EndDateColumn.DataPropertyName = "EndDate";
            this.EndDateColumn.HeaderText = "End Date";
            this.EndDateColumn.MinimumWidth = 110;
            this.EndDateColumn.Name = "EndDateColumn";
            this.EndDateColumn.ReadOnly = true;
            this.EndDateColumn.Width = 110;
            // 
            // CollarParameterFileDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 262);
            this.Controls.Add(this.DoneCancelButton);
            this.Controls.Add(this.EditSaveButton);
            this.Controls.Add(this.CollarsDataGridView);
            this.Controls.Add(this.ShowContentsButton);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.FileIdTextBox);
            this.Controls.Add(this.UserNameTextBox);
            this.Controls.Add(this.UploadDateTextBox);
            this.Controls.Add(this.OwnerTextBox);
            this.Controls.Add(this.FormatTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileNameTextBox);
            this.Name = "CollarParameterFileDetailsForm";
            this.Text = "Collar Parameter File Details";
            ((System.ComponentModel.ISupportInitialize)(this.CollarsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DoneCancelButton;
        private System.Windows.Forms.Button EditSaveButton;
        private System.Windows.Forms.DataGridView CollarsDataGridView;
        private System.Windows.Forms.Button ShowContentsButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FileIdTextBox;
        private System.Windows.Forms.TextBox UserNameTextBox;
        private System.Windows.Forms.TextBox UploadDateTextBox;
        private System.Windows.Forms.TextBox OwnerTextBox;
        private System.Windows.Forms.TextBox FormatTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FileNameTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollarManufacturerColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollarIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDateColumn;

    }
}