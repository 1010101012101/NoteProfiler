namespace Note_Profiler
{
    partial class frmPictureDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPictureDB));
            this.notesView = new Manina.Windows.Forms.ImageListView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabNotes = new System.Windows.Forms.TabPage();
            this.btnDelete = new System.Windows.Forms.Button();
            this.tabPages = new System.Windows.Forms.TabPage();
            this.pagesView = new Manina.Windows.Forms.ImageListView();
            this.tabControl1.SuspendLayout();
            this.tabNotes.SuspendLayout();
            this.tabPages.SuspendLayout();
            this.SuspendLayout();
            // 
            // notesView
            // 
            this.notesView.DefaultImage = ((System.Drawing.Image)(resources.GetObject("notesView.DefaultImage")));
            this.notesView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notesView.ErrorImage = ((System.Drawing.Image)(resources.GetObject("notesView.ErrorImage")));
            this.notesView.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.notesView.Location = new System.Drawing.Point(3, 3);
            this.notesView.Name = "notesView";
            this.notesView.Size = new System.Drawing.Size(882, 468);
            this.notesView.TabIndex = 2;
            this.notesView.Text = "";
            this.notesView.View = Manina.Windows.Forms.View.Pane;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabNotes);
            this.tabControl1.Controls.Add(this.tabPages);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(896, 500);
            this.tabControl1.TabIndex = 3;
            // 
            // tabNotes
            // 
            this.tabNotes.Controls.Add(this.btnDelete);
            this.tabNotes.Controls.Add(this.notesView);
            this.tabNotes.Location = new System.Drawing.Point(4, 22);
            this.tabNotes.Name = "tabNotes";
            this.tabNotes.Padding = new System.Windows.Forms.Padding(3);
            this.tabNotes.Size = new System.Drawing.Size(888, 474);
            this.tabNotes.TabIndex = 0;
            this.tabNotes.Text = "Notes";
            this.tabNotes.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(8, 445);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(78, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // tabPages
            // 
            this.tabPages.Controls.Add(this.pagesView);
            this.tabPages.Location = new System.Drawing.Point(4, 22);
            this.tabPages.Name = "tabPages";
            this.tabPages.Padding = new System.Windows.Forms.Padding(3);
            this.tabPages.Size = new System.Drawing.Size(888, 474);
            this.tabPages.TabIndex = 1;
            this.tabPages.Text = "Pages";
            this.tabPages.UseVisualStyleBackColor = true;
            // 
            // pagesView
            // 
            this.pagesView.DefaultImage = ((System.Drawing.Image)(resources.GetObject("pagesView.DefaultImage")));
            this.pagesView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pagesView.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pagesView.ErrorImage")));
            this.pagesView.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.pagesView.Location = new System.Drawing.Point(3, 3);
            this.pagesView.Name = "pagesView";
            this.pagesView.Size = new System.Drawing.Size(882, 468);
            this.pagesView.TabIndex = 3;
            this.pagesView.Text = "";
            this.pagesView.View = Manina.Windows.Forms.View.Pane;
            // 
            // frmPictureDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 500);
            this.Controls.Add(this.tabControl1);
            this.MinimizeBox = false;
            this.Name = "frmPictureDB";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Database Viewer";
            this.tabControl1.ResumeLayout(false);
            this.tabNotes.ResumeLayout(false);
            this.tabPages.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Manina.Windows.Forms.ImageListView notesView;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabNotes;
        private System.Windows.Forms.TabPage tabPages;
        private Manina.Windows.Forms.ImageListView pagesView;
        private System.Windows.Forms.Button btnDelete;
    }
}