namespace AnimalMovement
{
    partial class InvestigatorsForm
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
            this.InvestigatorsGridView = new System.Windows.Forms.DataGridView();
            this.InfoInvestigatorButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.InvestigatorsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // InvestigatorsGridView
            // 
            this.InvestigatorsGridView.AllowUserToAddRows = false;
            this.InvestigatorsGridView.AllowUserToDeleteRows = false;
            this.InvestigatorsGridView.AllowUserToOrderColumns = true;
            this.InvestigatorsGridView.AllowUserToResizeRows = false;
            this.InvestigatorsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InvestigatorsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.InvestigatorsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InvestigatorsGridView.Location = new System.Drawing.Point(12, 12);
            this.InvestigatorsGridView.Name = "InvestigatorsGridView";
            this.InvestigatorsGridView.ReadOnly = true;
            this.InvestigatorsGridView.RowHeadersWidth = 25;
            this.InvestigatorsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.InvestigatorsGridView.Size = new System.Drawing.Size(410, 160);
            this.InvestigatorsGridView.TabIndex = 7;
            this.InvestigatorsGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.InvestigatorsGridView_CellContentClick);
            this.InvestigatorsGridView.SelectionChanged += new System.EventHandler(this.InvestigatorsGridView_SelectionChanged);
            // 
            // InfoInvestigatorButton
            // 
            this.InfoInvestigatorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InfoInvestigatorButton.FlatAppearance.BorderSize = 0;
            this.InfoInvestigatorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoInvestigatorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoInvestigatorButton.Image = global::AnimalMovement.Properties.Resources.GenericInformation_B_16;
            this.InfoInvestigatorButton.Location = new System.Drawing.Point(12, 178);
            this.InfoInvestigatorButton.Name = "InfoInvestigatorButton";
            this.InfoInvestigatorButton.Size = new System.Drawing.Size(24, 24);
            this.InfoInvestigatorButton.TabIndex = 10;
            this.InfoInvestigatorButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.InfoInvestigatorButton.UseVisualStyleBackColor = true;
            this.InfoInvestigatorButton.Click += new System.EventHandler(this.InfoInvestigatorButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 185);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(300, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "PIs can only be created/deleted by the database administrator";
            // 
            // InvestigatorsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 211);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InvestigatorsGridView);
            this.Controls.Add(this.InfoInvestigatorButton);
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "InvestigatorsForm";
            this.Text = "Project Investigators List";
            ((System.ComponentModel.ISupportInitialize)(this.InvestigatorsGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView InvestigatorsGridView;
        private System.Windows.Forms.Button InfoInvestigatorButton;
        private System.Windows.Forms.Label label1;
    }
}