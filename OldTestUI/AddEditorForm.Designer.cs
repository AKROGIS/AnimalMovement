namespace TestUI
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
            this.AddEditorButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Editor:";
            // 
            // EditorTextBox
            // 
            this.EditorTextBox.Location = new System.Drawing.Point(57, 20);
            this.EditorTextBox.Name = "EditorTextBox";
            this.EditorTextBox.Size = new System.Drawing.Size(182, 20);
            this.EditorTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "Must be a windows login name with domain\r\nFor Example:  NPS\\RESarwas";
            // 
            // AddEditorButton
            // 
            this.AddEditorButton.Location = new System.Drawing.Point(164, 85);
            this.AddEditorButton.Name = "AddEditorButton";
            this.AddEditorButton.Size = new System.Drawing.Size(75, 23);
            this.AddEditorButton.TabIndex = 3;
            this.AddEditorButton.Text = "Add";
            this.AddEditorButton.UseVisualStyleBackColor = true;
            this.AddEditorButton.Click += new System.EventHandler(this.AddEditorButton_Click);
            // 
            // AddEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 120);
            this.Controls.Add(this.AddEditorButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.EditorTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditorForm";
            this.Text = "Add Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox EditorTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button AddEditorButton;
    }
}