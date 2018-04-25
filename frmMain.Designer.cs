using System.Windows.Forms;

namespace Note_Profiler
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.numMin = new Note_Profiler.ToolStripNumericUpDown();
            this.numMax = new Note_Profiler.ToolStripNumericUpDown();
            this.listBoxFoundNotes = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.imageboxMain = new Cyotek.Windows.Forms.ImageBox();
            this.toolstripMain = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnSaveReadyToDB = new System.Windows.Forms.ToolStripButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnImageLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.btnRescan = new System.Windows.Forms.ToolStripButton();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPan = new System.Windows.Forms.ToolStripButton();
            this.btnSelectMultiple = new System.Windows.Forms.ToolStripButton();
            this.btnAddNote = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnMerge = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.comboAuthor = new System.Windows.Forms.ToolStripComboBox();
            this.btnManageAuthors = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.comboNoteType = new System.Windows.Forms.ToolStripComboBox();
            this.btnEditNoteTypes = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblDBVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblNoteInDB = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPagesInDB = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSelectedCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorkerScan = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuLoadImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNotes = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMultiSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.menuAddNote = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMergeNotes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveReady = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEditAuthors = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditNoteTypes = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuBackupDB = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRestoreDB = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClearDB = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.menuDBBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorkerLoadFromDB = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolstripMain.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numMin
            // 
            this.numMin.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numMin.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numMin.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numMin.Name = "numMin";
            this.numMin.Size = new System.Drawing.Size(41, 23);
            this.numMin.Text = "100";
            this.numMin.ToolTipText = "The minimum area to consider for detection";
            this.numMin.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numMin.TextChanged += new System.EventHandler(this.numMin_TextChanged);
            // 
            // numMax
            // 
            this.numMax.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numMax.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numMax.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numMax.Name = "numMax";
            this.numMax.Size = new System.Drawing.Size(41, 23);
            this.numMax.Text = "500";
            this.numMax.ToolTipText = "The maximum area to consider for detection";
            this.numMax.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numMax.Click += new System.EventHandler(this.numMax_Click);
            // 
            // listBoxFoundNotes
            // 
            this.listBoxFoundNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxFoundNotes.FormattingEnabled = true;
            this.listBoxFoundNotes.Location = new System.Drawing.Point(3, 16);
            this.listBoxFoundNotes.Name = "listBoxFoundNotes";
            this.listBoxFoundNotes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxFoundNotes.Size = new System.Drawing.Size(203, 524);
            this.listBoxFoundNotes.TabIndex = 3;
            this.listBoxFoundNotes.SelectedIndexChanged += new System.EventHandler(this.listBoxFoundNotes_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxFoundNotes);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(209, 543);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Recognized Objects";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 50);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 543);
            this.splitContainer1.SplitterDistance = 795;
            this.splitContainer1.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.imageboxMain);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(795, 543);
            this.panel1.TabIndex = 3;
            // 
            // imageboxMain
            // 
            this.imageboxMain.AllowDoubleClick = true;
            this.imageboxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageboxMain.Location = new System.Drawing.Point(0, 0);
            this.imageboxMain.Name = "imageboxMain";
            this.imageboxMain.Size = new System.Drawing.Size(795, 543);
            this.imageboxMain.TabIndex = 0;
            this.imageboxMain.Selected += new System.EventHandler<System.EventArgs>(this.imageboxMain_Selected);
            this.imageboxMain.Selecting += new System.EventHandler<Cyotek.Windows.Forms.ImageBoxCancelEventArgs>(this.imageboxMain_Selecting);
            this.imageboxMain.Click += new System.EventHandler(this.imageboxMain_clicked);
            this.imageboxMain.DoubleClick += new System.EventHandler(this.imageboxMain_DoubleClick);
            this.imageboxMain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.imageboxMain_KeyDown);
            this.imageboxMain.KeyUp += new System.Windows.Forms.KeyEventHandler(this.imageboxMain_KeyUp);
            this.imageboxMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageboxMain_MouseMove);
            // 
            // toolstripMain
            // 
            this.toolstripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolstripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave,
            this.btnSaveReadyToDB,
            this.btnOpen,
            this.toolStripSeparator2,
            this.btnImageLoad,
            this.toolStripLabel1,
            this.numMin,
            this.toolStripLabel2,
            this.numMax,
            this.btnRescan,
            this.btnClear,
            this.toolStripSeparator1,
            this.btnPan,
            this.btnSelectMultiple,
            this.btnAddNote,
            this.btnDelete,
            this.btnMerge,
            this.toolStripSeparator,
            this.comboAuthor,
            this.btnManageAuthors,
            this.toolStripSeparator7,
            this.comboNoteType,
            this.btnEditNoteTypes});
            this.toolstripMain.Location = new System.Drawing.Point(0, 24);
            this.toolstripMain.Name = "toolstripMain";
            this.toolstripMain.Size = new System.Drawing.Size(1008, 26);
            this.toolstripMain.TabIndex = 3;
            this.toolstripMain.Text = "toolStrip1";
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 23);
            this.btnSave.Text = "&Save";
            this.btnSave.ToolTipText = "Save profile";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveReadyToDB
            // 
            this.btnSaveReadyToDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSaveReadyToDB.Image = global::Note_Profiler.Properties.Resources.SaveTable_16x;
            this.btnSaveReadyToDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveReadyToDB.Name = "btnSaveReadyToDB";
            this.btnSaveReadyToDB.Size = new System.Drawing.Size(23, 23);
            this.btnSaveReadyToDB.Text = "Save Ready Notes to DB";
            this.btnSaveReadyToDB.Click += new System.EventHandler(this.btnSaveReadyToDB_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(23, 23);
            this.btnOpen.Text = "&Open";
            this.btnOpen.ToolTipText = "Load an existing profile";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 26);
            // 
            // btnImageLoad
            // 
            this.btnImageLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnImageLoad.Image = global::Note_Profiler.Properties.Resources.NewImage_16x;
            this.btnImageLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImageLoad.Name = "btnImageLoad";
            this.btnImageLoad.Size = new System.Drawing.Size(23, 23);
            this.btnImageLoad.Text = "&Load Image";
            this.btnImageLoad.ToolTipText = "Load new image";
            this.btnImageLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(31, 23);
            this.toolStripLabel1.Text = "Min:";
            this.toolStripLabel1.ToolTipText = "The minimum area to consider for detection";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(32, 23);
            this.toolStripLabel2.Text = "Max:";
            this.toolStripLabel2.ToolTipText = "The maximum area to consider for detection";
            // 
            // btnRescan
            // 
            this.btnRescan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRescan.Image = global::Note_Profiler.Properties.Resources.Refresh_16x;
            this.btnRescan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Size = new System.Drawing.Size(23, 23);
            this.btnRescan.Text = "Rescan";
            this.btnRescan.ToolTipText = "Rescan image for notes.\r\nNotes that already exist but don\'t fit the parameters wi" +
    "ll not be removed.";
            this.btnRescan.Click += new System.EventHandler(this.btnRescan_Click);
            // 
            // btnClear
            // 
            this.btnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClear.Image = global::Note_Profiler.Properties.Resources.ClearWindowContent_16x;
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(23, 23);
            this.btnClear.Text = "Clear All";
            this.btnClear.ToolTipText = "Clears notes";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 26);
            // 
            // btnPan
            // 
            this.btnPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPan.Image = global::Note_Profiler.Properties.Resources.PanTool_16x;
            this.btnPan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPan.Name = "btnPan";
            this.btnPan.Size = new System.Drawing.Size(23, 23);
            this.btnPan.Text = "Pan";
            this.btnPan.ToolTipText = "Pan around the picture using the mouse\r\n(Toggle: CTRL+D)";
            this.btnPan.Click += new System.EventHandler(this.btnPan_Click);
            // 
            // btnSelectMultiple
            // 
            this.btnSelectMultiple.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSelectMultiple.Image = global::Note_Profiler.Properties.Resources.RectangleSelectionTool_16x;
            this.btnSelectMultiple.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelectMultiple.Name = "btnSelectMultiple";
            this.btnSelectMultiple.Size = new System.Drawing.Size(23, 23);
            this.btnSelectMultiple.Text = "Select Multiple";
            this.btnSelectMultiple.ToolTipText = "Select multiple notes by dragging the mouse accross the image.\r\n(Toggle: CTRL+D)";
            this.btnSelectMultiple.Click += new System.EventHandler(this.btnSelectMultiple_Click);
            // 
            // btnAddNote
            // 
            this.btnAddNote.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddNote.Image = global::Note_Profiler.Properties.Resources.AddControl_16x;
            this.btnAddNote.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddNote.Name = "btnAddNote";
            this.btnAddNote.Size = new System.Drawing.Size(23, 23);
            this.btnAddNote.Text = "Manually Add Note";
            this.btnAddNote.ToolTipText = "Add a new note by selecting it on the image.\r\n(CTRL+A)";
            this.btnAddNote.Click += new System.EventHandler(this.btnAddNote_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Image = global::Note_Profiler.Properties.Resources.deleteControl;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(23, 23);
            this.btnDelete.Text = "Delete Note";
            this.btnDelete.ToolTipText = "Removes a note from the detection list.\r\n(DEL)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnMerge
            // 
            this.btnMerge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMerge.Image = global::Note_Profiler.Properties.Resources.Merge_24x;
            this.btnMerge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(23, 23);
            this.btnMerge.Text = "Merge Selected";
            this.btnMerge.ToolTipText = "Merge selected notes (Can not be undone!)\r\n(CTRL+M)";
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 26);
            // 
            // comboAuthor
            // 
            this.comboAuthor.Name = "comboAuthor";
            this.comboAuthor.Size = new System.Drawing.Size(121, 26);
            this.comboAuthor.Text = "Set Author...";            
            // 
            // btnManageAuthors
            // 
            this.btnManageAuthors.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnManageAuthors.Image = global::Note_Profiler.Properties.Resources.Table_16x;
            this.btnManageAuthors.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnManageAuthors.Name = "btnManageAuthors";
            this.btnManageAuthors.Size = new System.Drawing.Size(23, 23);
            this.btnManageAuthors.Text = "Manage Authors...";
            this.btnManageAuthors.Click += new System.EventHandler(this.btnManageAuthors_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 26);
            // 
            // comboNoteType
            // 
            this.comboNoteType.Name = "comboNoteType";
            this.comboNoteType.Size = new System.Drawing.Size(121, 26);
            this.comboNoteType.Text = "Set Note Type...";
            // 
            // btnEditNoteTypes
            // 
            this.btnEditNoteTypes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditNoteTypes.Image = global::Note_Profiler.Properties.Resources.Table_16x;
            this.btnEditNoteTypes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditNoteTypes.Name = "btnEditNoteTypes";
            this.btnEditNoteTypes.Size = new System.Drawing.Size(23, 23);
            this.btnEditNoteTypes.Text = "toolStripButton1";
            this.btnEditNoteTypes.Click += new System.EventHandler(this.btnEditNoteTypes_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblDBVersion,
            this.lblNoteInDB,
            this.lblPagesInDB,
            this.lblSelectedCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 593);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1008, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblDBVersion
            // 
            this.lblDBVersion.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblDBVersion.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblDBVersion.Name = "lblDBVersion";
            this.lblDBVersion.Size = new System.Drawing.Size(4, 17);
            // 
            // lblNoteInDB
            // 
            this.lblNoteInDB.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblNoteInDB.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblNoteInDB.Name = "lblNoteInDB";
            this.lblNoteInDB.Size = new System.Drawing.Size(4, 17);
            // 
            // lblPagesInDB
            // 
            this.lblPagesInDB.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblPagesInDB.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblPagesInDB.Name = "lblPagesInDB";
            this.lblPagesInDB.Size = new System.Drawing.Size(4, 17);
            // 
            // lblSelectedCount
            // 
            this.lblSelectedCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblSelectedCount.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblSelectedCount.Name = "lblSelectedCount";
            this.lblSelectedCount.Size = new System.Drawing.Size(4, 17);
            // 
            // backgroundWorkerScan
            // 
            this.backgroundWorkerScan.WorkerReportsProgress = true;
            this.backgroundWorkerScan.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerScan_DoWork);
            this.backgroundWorkerScan.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerScan_ProgressChanged);
            this.backgroundWorkerScan.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerScan_RunWorkerCompleted);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuNotes,
            this.menuDatabase,
            this.menuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNew,
            this.menuOpen,
            this.toolStripSeparator3,
            this.menuLoadImage,
            this.toolStripSeparator4,
            this.menuSave,
            this.menuSaveAs,
            this.toolStripSeparator5,
            this.menuExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // menuNew
            // 
            this.menuNew.Image = ((System.Drawing.Image)(resources.GetObject("menuNew.Image")));
            this.menuNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuNew.Name = "menuNew";
            this.menuNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuNew.Size = new System.Drawing.Size(185, 22);
            this.menuNew.Text = "&New";
            this.menuNew.Click += new System.EventHandler(this.menuNew_Click);
            // 
            // menuOpen
            // 
            this.menuOpen.Image = ((System.Drawing.Image)(resources.GetObject("menuOpen.Image")));
            this.menuOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuOpen.Name = "menuOpen";
            this.menuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuOpen.Size = new System.Drawing.Size(185, 22);
            this.menuOpen.Text = "&Open";
            this.menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(182, 6);
            // 
            // menuLoadImage
            // 
            this.menuLoadImage.Image = global::Note_Profiler.Properties.Resources.NewImage_16x;
            this.menuLoadImage.Name = "menuLoadImage";
            this.menuLoadImage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.menuLoadImage.Size = new System.Drawing.Size(185, 22);
            this.menuLoadImage.Text = "&Load Image...";
            this.menuLoadImage.Click += new System.EventHandler(this.menuLoadImage_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(182, 6);
            // 
            // menuSave
            // 
            this.menuSave.Image = ((System.Drawing.Image)(resources.GetObject("menuSave.Image")));
            this.menuSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.menuSave.Name = "menuSave";
            this.menuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuSave.Size = new System.Drawing.Size(185, 22);
            this.menuSave.Text = "&Save";
            this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.Size = new System.Drawing.Size(185, 22);
            this.menuSaveAs.Text = "Save &As";
            this.menuSaveAs.Click += new System.EventHandler(this.menuSaveAs_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(182, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(185, 22);
            this.menuExit.Text = "E&xit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuNotes
            // 
            this.menuNotes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuMultiSelect,
            this.toolStripSeparator10,
            this.menuAddNote,
            this.menuMergeNotes,
            this.menuDelete});
            this.menuNotes.Name = "menuNotes";
            this.menuNotes.Size = new System.Drawing.Size(50, 20);
            this.menuNotes.Text = "&Notes";
            // 
            // mnuMultiSelect
            // 
            this.mnuMultiSelect.CheckOnClick = true;
            this.mnuMultiSelect.Image = global::Note_Profiler.Properties.Resources.RectangleSelectionTool_16x;
            this.mnuMultiSelect.Name = "mnuMultiSelect";
            this.mnuMultiSelect.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.mnuMultiSelect.Size = new System.Drawing.Size(200, 22);
            this.mnuMultiSelect.Text = "Drag to select";
            this.mnuMultiSelect.Click += new System.EventHandler(this.mnuMultiSelect_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(197, 6);
            // 
            // menuAddNote
            // 
            this.menuAddNote.Image = global::Note_Profiler.Properties.Resources.AddControl_16x;
            this.menuAddNote.Name = "menuAddNote";
            this.menuAddNote.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.menuAddNote.Size = new System.Drawing.Size(200, 22);
            this.menuAddNote.Text = "&Add New Note";
            this.menuAddNote.Click += new System.EventHandler(this.menuNewNote_Click);
            // 
            // menuMergeNotes
            // 
            this.menuMergeNotes.Image = global::Note_Profiler.Properties.Resources.Merge_24x;
            this.menuMergeNotes.Name = "menuMergeNotes";
            this.menuMergeNotes.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.menuMergeNotes.Size = new System.Drawing.Size(200, 22);
            this.menuMergeNotes.Text = "&Merge Selected";
            this.menuMergeNotes.Click += new System.EventHandler(this.menuMergeNotes_Click);
            // 
            // menuDelete
            // 
            this.menuDelete.Image = global::Note_Profiler.Properties.Resources.deleteControl;
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.menuDelete.Size = new System.Drawing.Size(200, 22);
            this.menuDelete.Text = "&Delete Selected";
            this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
            // 
            // menuDatabase
            // 
            this.menuDatabase.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSaveReady,
            this.toolStripSeparator8,
            this.menuEditAuthors,
            this.menuEditNoteTypes,
            this.toolStripSeparator6,
            this.menuBackupDB,
            this.menuRestoreDB,
            this.menuClearDB,
            this.toolStripSeparator11,
            this.menuDBBrowser});
            this.menuDatabase.Name = "menuDatabase";
            this.menuDatabase.Size = new System.Drawing.Size(67, 20);
            this.menuDatabase.Text = "&Database";
            // 
            // menuSaveReady
            // 
            this.menuSaveReady.Image = global::Note_Profiler.Properties.Resources.SaveTable_16x;
            this.menuSaveReady.Name = "menuSaveReady";
            this.menuSaveReady.Size = new System.Drawing.Size(176, 22);
            this.menuSaveReady.Text = "&Save Ready Notes...";
            this.menuSaveReady.Click += new System.EventHandler(this.menuSaveReady_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(173, 6);
            // 
            // menuEditAuthors
            // 
            this.menuEditAuthors.Image = global::Note_Profiler.Properties.Resources.Table_16x;
            this.menuEditAuthors.Name = "menuEditAuthors";
            this.menuEditAuthors.Size = new System.Drawing.Size(176, 22);
            this.menuEditAuthors.Text = "Edit &Authors...";
            this.menuEditAuthors.Click += new System.EventHandler(this.mnuEditAuthors_Click);
            // 
            // menuEditNoteTypes
            // 
            this.menuEditNoteTypes.Image = global::Note_Profiler.Properties.Resources.Table_16x;
            this.menuEditNoteTypes.Name = "menuEditNoteTypes";
            this.menuEditNoteTypes.Size = new System.Drawing.Size(176, 22);
            this.menuEditNoteTypes.Text = "Edit &Note Types...";
            this.menuEditNoteTypes.Click += new System.EventHandler(this.menuEditNoteTypes_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(173, 6);
            // 
            // menuBackupDB
            // 
            this.menuBackupDB.Name = "menuBackupDB";
            this.menuBackupDB.Size = new System.Drawing.Size(176, 22);
            this.menuBackupDB.Text = "&Export Database...";
            this.menuBackupDB.Click += new System.EventHandler(this.menuBackupDB_Click);
            // 
            // menuRestoreDB
            // 
            this.menuRestoreDB.Name = "menuRestoreDB";
            this.menuRestoreDB.Size = new System.Drawing.Size(176, 22);
            this.menuRestoreDB.Text = "&Replace Database...";
            this.menuRestoreDB.Click += new System.EventHandler(this.menuRestoreDB_Click);
            // 
            // menuClearDB
            // 
            this.menuClearDB.Name = "menuClearDB";
            this.menuClearDB.Size = new System.Drawing.Size(176, 22);
            this.menuClearDB.Text = "&Clear Database...";
            this.menuClearDB.Click += new System.EventHandler(this.menuClearDB_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(173, 6);
            // 
            // menuDBBrowser
            // 
            this.menuDBBrowser.Name = "menuDBBrowser";
            this.menuDBBrowser.Size = new System.Drawing.Size(176, 22);
            this.menuDBBrowser.Text = "&Browse Database...";
            this.menuDBBrowser.Click += new System.EventHandler(this.menuDBBrowser_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator9,
            this.menuAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "&Help";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(113, 6);
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(116, 22);
            this.menuAbout.Text = "&About...";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // backgroundWorkerLoadFromDB
            // 
            this.backgroundWorkerLoadFromDB.WorkerReportsProgress = true;
            this.backgroundWorkerLoadFromDB.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerLoadFromDB_DoWork);
            this.backgroundWorkerLoadFromDB.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerLoadFromDB_ProgressChanged);
            this.backgroundWorkerLoadFromDB.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerLoadFromDB_RunWorkerCompleted);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 615);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolstripMain);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Note Profiler";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.toolstripMain.ResumeLayout(false);
            this.toolstripMain.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion        

        private System.Windows.Forms.ListBox listBoxFoundNotes;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolstripMain;
        private System.Windows.Forms.ToolStripButton btnImageLoad;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblSelectedCount;
        private System.Windows.Forms.Panel panel1;
        private ToolStripNumericUpDown numMin;
        private ToolStripNumericUpDown numMax;
        private Cyotek.Windows.Forms.ImageBox imageboxMain;
        private System.Windows.Forms.ToolStripButton btnSelectMultiple;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnAddNote;
        private System.Windows.Forms.ToolStripButton btnMerge;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;        
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;        
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnRescan;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.ComponentModel.BackgroundWorker backgroundWorkerScan;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuNew;
        private System.Windows.Forms.ToolStripMenuItem menuOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuSave;
        private System.Windows.Forms.ToolStripMenuItem menuSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuLoadImage;
        private System.Windows.Forms.ToolStripComboBox comboAuthor;
        private System.Windows.Forms.ToolStripButton btnManageAuthors;
        private System.Windows.Forms.ToolStripMenuItem menuDatabase;
        private System.Windows.Forms.ToolStripMenuItem menuEditAuthors;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem menuBackupDB;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem menuRestoreDB;
        private System.Windows.Forms.ToolStripMenuItem menuEditNoteTypes;
        private System.Windows.Forms.ToolStripComboBox comboNoteType;
        private System.Windows.Forms.ToolStripButton btnEditNoteTypes;
        private System.Windows.Forms.ToolStripStatusLabel lblNoteInDB;
        private ToolStripMenuItem menuNotes;
        private ToolStripMenuItem menuMergeNotes;
        private ToolStripMenuItem menuSaveReady;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem menuAddNote;
        private ToolStripMenuItem menuDelete;
        private ToolStripStatusLabel lblPagesInDB;
        private ToolStripMenuItem menuClearDB;
        private ToolStripButton btnSaveReadyToDB;
        private ToolStripButton btnPan;
        private ToolStripStatusLabel lblDBVersion;
        private ToolStripMenuItem mnuMultiSelect;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripMenuItem menuDBBrowser;
        private System.ComponentModel.BackgroundWorker backgroundWorkerLoadFromDB;
    }
}

