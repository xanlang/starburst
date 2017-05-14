namespace KNEditor
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
            this.ButtonLoadFile = new System.Windows.Forms.Button();
            this.TreeViewKeyNote = new System.Windows.Forms.TreeView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // ButtonLoadFile
            // 
            this.ButtonLoadFile.Location = new System.Drawing.Point(12, 12);
            this.ButtonLoadFile.Name = "ButtonLoadFile";
            this.ButtonLoadFile.Size = new System.Drawing.Size(192, 35);
            this.ButtonLoadFile.TabIndex = 0;
            this.ButtonLoadFile.Text = "Load File";
            this.ButtonLoadFile.UseVisualStyleBackColor = true;
            this.ButtonLoadFile.Click += new System.EventHandler(this.ButtonLoadFile_Click);
            // 
            // TreeViewKeyNote
            // 
            this.TreeViewKeyNote.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TreeViewKeyNote.Location = new System.Drawing.Point(0, 84);
            this.TreeViewKeyNote.Name = "TreeViewKeyNote";
            this.TreeViewKeyNote.Size = new System.Drawing.Size(491, 311);
            this.TreeViewKeyNote.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(491, 395);
            this.Controls.Add(this.TreeViewKeyNote);
            this.Controls.Add(this.ButtonLoadFile);
            this.Name = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonLoadFile;
        private System.Windows.Forms.TreeView TreeViewKeyNote;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

