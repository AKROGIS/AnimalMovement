namespace AnimalMovement
{
    sealed partial class FileContentsForm
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
            this.ContentRichTextBox = new System.Windows.Forms.RichTextBox();
            this.ContentContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CopyAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.ContentContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContentRichTextBox
            // 
            this.ContentRichTextBox.ContextMenuStrip = this.ContentContextMenu;
            this.ContentRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentRichTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContentRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.ContentRichTextBox.Name = "ContentRichTextBox";
            this.ContentRichTextBox.ReadOnly = true;
            this.ContentRichTextBox.Size = new System.Drawing.Size(427, 440);
            this.ContentRichTextBox.TabIndex = 0;
            this.ContentRichTextBox.Text = "";
            this.ContentRichTextBox.WordWrap = false;
            // 
            // ContentContextMenu
            // 
            this.ContentContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyAllToolStripMenuItem,
            this.SaveAsToolStripMenuItem});
            this.ContentContextMenu.Name = "ContentContextMenu";
            this.ContentContextMenu.Size = new System.Drawing.Size(124, 48);
            // 
            // CopyAllToolStripMenuItem
            // 
            this.CopyAllToolStripMenuItem.Name = "CopyAllToolStripMenuItem";
            this.CopyAllToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.CopyAllToolStripMenuItem.Text = "Copy All";
            this.CopyAllToolStripMenuItem.Click += new System.EventHandler(this.CopyAllToolStripMenuItem_Click);
            // 
            // SaveAsToolStripMenuItem
            // 
            this.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            this.SaveAsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.SaveAsToolStripMenuItem.Text = "Save As...";
            this.SaveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // SaveFileDialog
            // 
            this.SaveFileDialog.AddExtension = false;
            // 
            // FileContentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 440);
            this.Controls.Add(this.ContentRichTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FileContentsForm";
            this.Text = "<filename>";
            this.ContentContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox ContentRichTextBox;
        private System.Windows.Forms.ContextMenuStrip ContentContextMenu;
        private System.Windows.Forms.ToolStripMenuItem CopyAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
    }
}