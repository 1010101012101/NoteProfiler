using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Note_Profiler.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Note_Profiler
{
    public partial class frmMain : Form
    {
        /// <summary>
        /// The four different modes of the GUI.
        /// Determines how the mouse aces uppon dragging it accross an image.        
        /// </summary>
        enum SelectionMode { Default, Select, Create, Zoom }


        #region Global Variables

        BindingList<Note> noteList; //The list of notes detected within the current image
        List<Note> selectedNotes; //The list of notes currently highlighted
        Page currentPage; //The current loaded page - which includes the photo of the page and its name.

        private SelectionMode currentMode = SelectionMode.Default; //The current selection mode.

        Cursor curPen = Utilities.CreateCursor(Properties.Resources.pen, 0, 0); //A pen cursor object - used for creating new notes.
        Cursor groupSelect = Utilities.CreateCursor(Properties.Resources.GroupSelectCursor_16x, 0, 0); //The group selection cursor.
        List<ToolStripItem> imageTools; //A tool of objects related to image editing. Grouped in a list to allow fast endabling/disabling
        frmProgress progressDialog; //A progress dialog form - instance created dynamically.
        bool isListBoxUpdating = false; //A boolean that while true indicates that the note listbox might be updating and its gui shouldn't be changed
        Session currentSession; //A session object representing the current session - which includes open files, save states, etc.
        DBAdapter db; //A database adapter through which all the DB operations will be performed. Must be initialized!
        BindingList<Author> authorList; //A list of all the authors currently loaded - should be populated from the DB.
        BindingList<Note.NoteType> noteTypeList; //A list of all the note types currently loaded - should be populated from the DB.
        #endregion

        /// <summary>
        /// Loads the initial interface elements, instantiates the database and ties the different global variables to their initial values.
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
            if (Properties.Settings.Default.UpgradeRequired) //Upgrades the settings in case the version was upgraded. Required so settings aren't lost between sessions.
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
            }
            db = new DBAdapter(); //Creates a new DB instance.
            selectedNotes = new List<Note>();
            noteList = new BindingList<Note>();
            noteList.ListChanged += NoteList_ListChanged;
            comboAuthor.ComboBox.SelectionChangeCommitted += comboAuthor_SelectionChangeCommitted;
            comboNoteType.ComboBox.SelectionChangeCommitted += comboNoteType_SelectionChangeCommitted;
            listBoxFoundNotes.DataSource = noteList;
            InvalidateDatabase(); //Makes sure the database is fresh.

            //Create the list of interface elemnts to be tied to image editing and populate it:
            imageTools = new List<ToolStripItem>();
            imageTools.Add(btnAddNote);
            imageTools.Add(btnDelete);
            imageTools.Add(btnMerge);
            imageTools.Add(btnSelectMultiple);
            imageTools.Add(btnRescan);
            imageTools.Add(btnSave);
            imageTools.Add(btnClear);
            imageTools.Add(comboAuthor);
            imageTools.Add(comboNoteType);
            imageTools.Add(btnSaveReadyToDB);
            imageTools.Add(btnPan);
            setMode(SelectionMode.Default);
        }
        #region File Operations

        /// <summary>
        /// Loads an image (creates a new session for it too) to work on.
        /// Then scans it for notes, and eventually loads any pre-profiled notes from the Database.
        /// </summary>
        /// <param name="address"></param>
        private void LoadImage(string address)
        {
            if (currentSession != null)
                currentSession.EndSession();
            currentPage = new Page(address);
            currentSession = new Session(this, currentPage);
            imageboxMain.Image = currentPage.Working.Bitmap;
            setMode(SelectionMode.Default);
            ScanNotes();
            LoadNotesFromDB();
        }

        /// <summary>
        /// Save the current session (directly if already opened or saved, prompts for target if not).
        /// </summary>
        private void FileSave()
        {
            if (currentSession.ProfilePath == null)
            {
                FileSaveAs();
            }
            else
                SaveProfile(currentSession.ProfilePath);
        }

        /// <summary>
        /// Save current session to a new file determined by the user.
        /// </summary>
        private void FileSaveAs()
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "Note Profiler|*.npf";
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                SaveProfile(ofd.FileName);
            }
        }

        /// <summary>
        /// Start a new profileing session by clearing all the settings and lists.
        /// </summary>
        private void NewProfile()
        {
            if (currentSession != null)
                currentSession.EndSession();
            currentSession = null;
            noteList.Clear();
            selectedNotes.Clear();
            imageboxMain.Image = null;
            currentPage = null;
            setMode(SelectionMode.Default);
            InvalidateButtons();
            InvalidateDatabase();
        }

        /// <summary>
        /// Saves the current profile work to a file.
        /// Firstly it stores any detected notes to the database.
        /// The profile is then saved by serializing the currently detected notes and their values to an xml file.
        /// The names of the image and the session values are saved to respective files, as is the image itself.
        /// Those files are then zipped and the zipped file is renamed to whatever to user chose.
        /// </summary>
        /// <param name="fileName">What filename to save to</param>
        private void SaveProfile(string fileName)
        {
            SaveCompleted();
            File.WriteAllText(currentSession.WorkingDirectyory + "\\session", currentSession.Name);
            File.WriteAllText(currentSession.WorkingDirectyory + "\\imagename", currentPage.FileName);
            Serialize(noteList.ToList(), currentSession.WorkingDirectyory + "\\notes.xml");
            if (File.Exists(fileName))
                File.Delete(fileName);
            ZipFile.CreateFromDirectory(currentSession.WorkingDirectyory, fileName);
            currentSession.ProfilePath = fileName;
            currentSession.Dirty = false; //Indicate that the current session is saved and no longer requires saving.
        }

        /// <summary>
        /// Load a presaved file
        /// </summary>
        private void FileLoad()
        {
            if (!VerifySave())
                return;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Note Profiler|*.npf";
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (imageboxMain.Image != null)
                    NewProfile();
                LoadProfile(ofd.FileName);
            }
        }

        /// <summary>
        /// The reverse of saving a profile:
        /// First the file is unzipped, then the data in the xml is deserialized into the notelist.
        /// The session and image values are restored from the respective files, and the image is copied to a temp working directory.
        /// Finally - the interface is updated to reflect the loaded notes.
        /// </summary>
        /// <param name="address"></param>
        private void LoadProfile(string address)
        {
            string tempExtractDir = Path.GetTempPath() + "\\" + Path.GetFileNameWithoutExtension(address);
            if (Directory.Exists(tempExtractDir))
                Directory.Delete(tempExtractDir, true);
            ZipFile.ExtractToDirectory(address, tempExtractDir);
            string sessionName = File.ReadAllText(tempExtractDir + "\\session");
            string imagename = File.ReadAllText(tempExtractDir + "\\imagename");
            currentPage = new Page(tempExtractDir + "\\" + imagename);
            currentSession = new Session(this, currentPage, address);
            noteList = DeSerialize(tempExtractDir + "\\notes.xml");
            Directory.Delete(tempExtractDir, true);
            listBoxFoundNotes.DataSource = noteList;
            selectedNotes.Clear();
            SyncSelection(true, false);
            currentSession.Dirty = false;
        }

        /// <summary>
        /// Load a new image to work on.
        /// This will prompt the user to save their work, then load the image and start a new session for it.
        /// </summary>
        private void ImageLoad()
        {
            if (!VerifySave())
                return;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.jpg;*.png;*.gif;*.jpeg";
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (imageboxMain.Image != null)
                    NewProfile();
                LoadImage(ofd.FileName);
            }
        }

        /// <summary>
        /// Serializes notes into an XML file.
        /// </summary>
        /// <param name="list">The list of notes to serialize</param>
        /// <param name="fileName">Where to save the results</param>
        public void Serialize(List<Note> list, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Note>));
            using (TextWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, list);
            }
        }

        /// <summary>
        /// Deserializes notes from XML
        /// </summary>
        /// <param name="fileName">File to read from</param>
        /// <returns></returns>
        public BindingList<Note> DeSerialize(string fileName)
        {
            List<Note> returnList = new List<Note>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Note>));
            using (XmlReader reader = XmlReader.Create(fileName))
            {
                returnList = (List<Note>)serializer.Deserialize(reader);
            }
            return new BindingList<Note>(returnList);
        }

        #endregion
        #region Interface Functions

        #region Handlers

        #region Menus
        #region File
        private void menuLoadImage_Click(object sender, EventArgs e)
        {
            ImageLoad();
        }

        private void menuNew_Click(object sender, EventArgs e)
        {
            if (!VerifySave())
                return;
            NewProfile();
        }

        private void menuOpen_Click(object sender, EventArgs e)
        {
            FileLoad();
        }

        private void mnuLoadImage_Click(object sender, EventArgs e)
        {
            ImageLoad();
        }
        private void menuSave_Click(object sender, EventArgs e)
        {
            FileSave();
        }

        private void menuSaveAs_Click(object sender, EventArgs e)
        {
            FileSaveAs();
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #endregion
        #region Notes
        private void mnuMultiSelect_Click(object sender, EventArgs e)
        {
            if (currentMode == SelectionMode.Select)
            {
                setMode(SelectionMode.Default);
            }
            else
            {
                setMode(SelectionMode.Select);
            }
        }

        private void menuNewNote_Click(object sender, EventArgs e)
        {
            StartCreate();
        }

        private void menuMergeNotes_Click(object sender, EventArgs e)
        {
            MergeSelectedNotes();
        }

        private void menuDelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedNotes();
        }


        #endregion

        #region Database
        private void menuDBBrowser_Click(object sender, EventArgs e)
        {
            frmPictureDB newForm = new frmPictureDB();
            newForm.ShowDialog();
        }

        private void menuSaveReady_Click(object sender, EventArgs e)
        {
            SaveCompleted();
        }

        private void mnuEditAuthors_Click(object sender, EventArgs e)
        {
            frmDBEditor manageForm = new frmDBEditor(db, frmDBEditor.EditType.Author);
            manageForm.ShowDialog(this);
        }

        private void menuEditNoteTypes_Click(object sender, EventArgs e)
        {
            frmDBEditor manageForm = new frmDBEditor(db, frmDBEditor.EditType.NoteType);
            manageForm.ShowDialog(this);
        }


        private void menuRestoreDB_Click(object sender, EventArgs e)
        {
            DialogResult warning = MessageBox.Show(this, "This will overwrite the current database!\nAre you sure?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (warning == DialogResult.Yes)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "SQLite Database File|*.db";
                DialogResult result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    File.Copy(ofd.FileName, db.DatabasePath, true);
                }
                db = new DBAdapter();
                InvalidateDatabase();
            }
        }

        private void menuBackupDB_Click(object sender, EventArgs e)
        {
            BackupDatabase();
        }


        private void menuClearDB_Click(object sender, EventArgs e)
        {
            DialogResult warning = MessageBox.Show(this, "This will overwrite the current database!\nAre you sure?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (warning == DialogResult.Yes)
            {
                File.Copy(Application.StartupPath + "\\Database\\ProfiledNotes.db", db.DatabasePath, true);
                db = new DBAdapter();
                InvalidateDatabase();
            }
        }

        #endregion
        private void menuAbout_Click(object sender, EventArgs e)
        {
            frmAbout about = new frmAbout();
            about.ShowDialog();
        }
        #endregion
        #region Toolbar

        /// <summary>
        /// This is used to set the authors of selected note(s).
        /// The list is populated whenever the database is invalidated directly from the database.
        /// </summary>
        /// <param name="sender">The combobox</param>
        /// <param name="e">The parameters</param>
        private void comboAuthor_SelectionChangeCommitted(object sender, EventArgs e)
        {
            List<Note> backup = new List<Note>(selectedNotes);
            Author which = comboAuthor.SelectedIndex == 0 ? null : (Author)comboAuthor.SelectedItem;
            foreach (Note selected in selectedNotes.ToList()) //Allows multiple selection
            {
                selected.Author = which;
                selected.MarkedUpdate = true;
            }
            if (currentSession != null)
                currentSession.Dirty = true;
            selectedNotes = new List<Note>(backup);
            SyncSelection(false, false);
        }

        /// <summary>
        /// This is used to set the note type of selected note(s).
        /// The list is populated whenever the database is invalidated directly from the database.
        /// </summary>
        /// <param name="sender">The combobox</param>
        /// <param name="e">The parameters</param>
        private void comboNoteType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            List<Note> backup = new List<Note>(selectedNotes);
            Note.NoteType which = comboNoteType.SelectedIndex == 0 ? null : (Note.NoteType)comboNoteType.SelectedItem;
            foreach (Note selected in selectedNotes.ToList())
            {
                selected.Type = which;
                selected.MarkedUpdate = true;
            }
            if (currentSession != null)
                currentSession.Dirty = true;
            selectedNotes = new List<Note>(backup);
            SyncSelection(false, false);
        }

        /// <summary>
        /// Saves completed notes to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveReadyToDB_Click(object sender, EventArgs e)
        {
            SaveCompleted();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            FileLoad();
        }

        /// <summary>
        /// Handler for the "Open Image" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            ImageLoad();
        }
        /// <summary>
        /// Handle the "Select Multiple" button selection (toggle).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectMultiple_Click(object sender, EventArgs e)
        {
            if (currentMode == SelectionMode.Select)
            {
                setMode(SelectionMode.Default);
            }
            else
            {
                setMode(SelectionMode.Select);
            }
        }

        /// <summary>
        /// Starts the "Add new Note" proceedure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddNote_Click(object sender, EventArgs e)
        {
            StartCreate();
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            MergeSelectedNotes();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteSelectedNotes();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAllDetected();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FileSave();
        }

        /// <summary>
        /// Syncs the min and max values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numMin_TextChanged(object sender, EventArgs e)
        {
            if (numMin.Value > numMax.Value)
                numMax.Value = numMin.Value;
        }

        /// <summary>
        /// Syncs the min and max values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numMax_Click(object sender, EventArgs e)
        {
            if (numMax.Value < numMin.Value)
                numMin.Value = numMax.Value;
        }

        /// <summary>
        /// Rescans notes with the current min and max parameters.
        /// It does NOT erase any previously detected notes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRescan_Click(object sender, EventArgs e)
        {
            ScanNotes();
        }

        /// <summary>
        /// Loads the DB editor for authors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnManageAuthors_Click(object sender, EventArgs e)
        {
            frmDBEditor manageForm = new frmDBEditor(db, frmDBEditor.EditType.Author);
            manageForm.ShowDialog(this);
        }

        /// <summary>
        /// Toggles the "Pan" / "Select Multiple" mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPan_Click(object sender, EventArgs e)
        {
            if (currentMode == SelectionMode.Select)
            {
                setMode(SelectionMode.Default);
            }
            else
            {
                setMode(SelectionMode.Select);
            }
        }

        /// <summary>
        /// Loads the note type DB editor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditNoteTypes_Click(object sender, EventArgs e)
        {
            frmDBEditor manageForm = new frmDBEditor(db, frmDBEditor.EditType.NoteType);
            manageForm.ShowDialog(this);
        }

        #endregion
        #region Imagebox
        /// <summary>
        /// Handles clicks on the imagebox.
        /// If a note was clicked - selects or unselects it.
        /// Also handles modifiers (shift) for multiple selections.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageboxMain_clicked(object sender, EventArgs e)
        {
            if (imageboxMain.Image == null)
                return;

            Point clickedPoint = imageboxMain.PointToImage(((MouseEventArgs)e).Location);
            Note clickedNote = noteList.FirstOrDefault(s => s.Rectangle.Contains(clickedPoint));

            if (clickedNote != null) //If the user clicked a note
            {
                if (!selectedNotes.Contains(clickedNote)) //If we clicked a note that was previously unselected
                {
                    if (ModifierKeys != Keys.Shift) //If the SHIFT key is not pressed or multiple notes selected - clear first.
                    {
                        ClearSelection();
                    }
                    SelectNote(clickedNote);
                }
                else //If we pressed a note that was already selected
                {
                    if (selectedNotes.Count > 1) //If there is more than one note currently selected
                    {
                        if (ModifierKeys != Keys.Shift) //Shift is not pressed
                        {
                            ClearSelection();
                            SelectNote(clickedNote);
                        }
                        else //SHIFT is pressed
                        {
                            DeselectNote(clickedNote);
                        }
                    }
                    else //Only one note selected
                    {
                        DeselectNote(clickedNote);
                    }
                }
            }

        }

        /// <summary>
        /// Makes sure that while selecting notes, the listbox goes into sleep mode so it doesn't slow down the interface.
        /// The listbox will be updated after selection is finished.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageboxMain_Selecting(object sender, Cyotek.Windows.Forms.ImageBoxCancelEventArgs e)
        {
            isListBoxUpdating = true;
            listBoxFoundNotes.BeginUpdate();
        }

        /// <summary>
        /// When an area is FINISHED being selected on the image.
        /// Depending on the mode - either selects or creates notes.
        /// Afterwards, allows the listbox to be updated with the new state.
        /// </summary>
        /// <param name="sender">The imagebox sending the event</param>
        /// <param name="e">Eventargs</param>
        private void imageboxMain_Selected(object sender, EventArgs e)
        {
            switch (currentMode)
            {
                case SelectionMode.Select:
                    if (ModifierKeys != Keys.Shift) //If not shift, clear first.
                        ClearSelection();
                    selectedNotes.AddRange(noteList.Where(x => imageboxMain.SelectionRegion.Contains(x.Rectangle)));
                    SyncSelection(true, false);
                    break;

                case SelectionMode.Create:
                    Note newNote = new Note(currentPage.Hash);
                    newNote.Rectangle = Rectangle.Round(imageboxMain.SelectionRegion);
                    noteList.Add(newNote);
                    DrawRectangle(currentPage.Working, newNote.Rectangle, McvColor.Green);
                    SyncSelection(false, false);
                    break;

                default:
                    //this should not happen!
                    break;
            }
            imageboxMain.SelectNone();
            listBoxFoundNotes.EndUpdate();
            isListBoxUpdating = false;
        }

        /// <summary>
        /// If control is double clicked without an image, show open file dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageboxMain_DoubleClick(object sender, EventArgs e)
        {
            if (imageboxMain.Image == null)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Images|*.jpg;*.png;*.gif;*.jpeg";
                DialogResult result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    LoadImage(ofd.FileName);
                }
            }
        }

        /// <summary>
        /// Make sure the correct curser is showing depending on what's under it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageboxMain_MouseMove(object sender, MouseEventArgs e)
        {
            InvalidateCursor(e.Location);
        }

        private void imageboxMain_KeyUp(object sender, KeyEventArgs e)
        {
            InvalidateCursor(Cursor.Position);
        }

        private void imageboxMain_KeyDown(object sender, KeyEventArgs e)
        {
            InvalidateCursor(Cursor.Position);
        }
        /// <summary>
        /// Update the selection on the image if selection of the listbox changes.
        /// </summary>
        /// <param name="sender">The sending listbox</param>
        /// <param name="e">The Event Arguments</param>
        private void listBoxFoundNotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isListBoxUpdating)
                return;

            selectedNotes.Clear();
            selectedNotes = listBoxFoundNotes.SelectedItems.Cast<Note>().ToList();
            SyncSelection(true, true);
        }
        #endregion

        #region Form
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!VerifyExit())
                e.Cancel = true;
        }

        #endregion

        #endregion

        /// <summary>
        /// Sets the current interface mode for what happens when you drag the mouse on the image.
        /// </summary>
        /// <param name="newMode">The mode to set</param>
        private void setMode(SelectionMode newMode)
        {
            currentMode = newMode;
            switch (newMode)
            {
                case SelectionMode.Default:
                    imageboxMain.SelectNone();
                    imageboxMain.SelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.None;
                    break;
                case SelectionMode.Select:
                    imageboxMain.SelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.Rectangle;
                    imageboxMain.SelectionColor = Color.Red;
                    break;
                case SelectionMode.Create:
                    imageboxMain.SelectionMode = Cyotek.Windows.Forms.ImageBoxSelectionMode.Rectangle;
                    imageboxMain.SelectionColor = Color.Green;
                    break;
                case SelectionMode.Zoom:
                    throw new NotImplementedException(); //It was eventually not needed, as the ImageBox control supports zooming by scrolling.
            }
            InvalidateButtons(); //Refresh button states to match current modes
        }

        /// <summary>
        /// Sets the cursor to contextually match what's under it and what mode it's in.
        /// The function recieves global coordinates, so we use the ImageBox controller's helper function to convert it to image coordinates.
        /// </summary>
        /// <param name="mouseLocation">What are the cursor coordinates</param>
        private void InvalidateCursor(Point mouseLocation)
        {
            if (imageboxMain.Image != null)
            {
                switch (currentMode)
                {
                    case SelectionMode.Default:
                        if (ModifierKeys == Keys.Shift)
                        {
                            imageboxMain.Cursor = groupSelect;
                        }
                        else
                        {
                            Note highlightedNote = noteList.FirstOrDefault(s => s.Rectangle.Contains(imageboxMain.PointToImage(mouseLocation)));
                            if (highlightedNote != null)
                                imageboxMain.Cursor = Cursors.Hand;
                            else
                                imageboxMain.Cursor = DefaultCursor;
                        }
                        break;
                    case SelectionMode.Create:
                        imageboxMain.Cursor = curPen;
                        break;
                    case SelectionMode.Select:
                        imageboxMain.Cursor = Cursors.Cross;
                        break;
                }
            }
        }

        /// <summary>
        /// Syncs between the selection on the image and the selection in the listbox.
        /// </summary>
        private void SyncSelection(bool redraw, bool sentFromListBox)
        {            
            imageboxMain.Image = currentPage.Working.Bitmap;
            lblSelectedCount.Text = "Number of selected notes: " + selectedNotes.Count;
            isListBoxUpdating = true;

            if (!sentFromListBox)
            {
                listBoxFoundNotes.ClearSelected();
            }

            List<Author> commonAuthors = new List<Author>();
            List<Note.NoteType> commonTypes = new List<Note.NoteType>();
            foreach (Note item in selectedNotes)
            {
                if (!sentFromListBox)
                    listBoxFoundNotes.SelectedItems.Add(item);
                if (item.Author != null && commonAuthors.FirstOrDefault(a => a.ID == item.Author.ID && a.Name.Equals(item.Author.Name)) == null)
                    commonAuthors.Add(item.Author);
                if (item.Type != null && commonTypes.FirstOrDefault(t => t.ID == item.Type.ID && t.Description.Equals(item.Type.Description)) == null)
                    commonTypes.Add(item.Type);
            }
            if (commonAuthors.Count == 1 && commonAuthors[0] != null)
                comboAuthor.ComboBox.SelectedItem = authorList.FirstOrDefault(l => l.ID == commonAuthors[0].ID);
            else
            {
                comboAuthor.ComboBox.SelectedIndex = 0;
                comboAuthor.Text = "Set Author...";
            }
            if (commonTypes.Count == 1 && commonTypes[0] != null)
                comboNoteType.ComboBox.SelectedItem = noteTypeList.FirstOrDefault(t => t.ID == commonTypes[0].ID);
            else
            {
                comboNoteType.ComboBox.SelectedIndex = 0;
                comboNoteType.Text = "Set Note Type...";
            }
            isListBoxUpdating = false;
            if (redraw)
                RedrawAllRectangles();
            InvalidateButtons();
        }

        /// <summary>
        /// Sets all butotn states (highlighted, enabled, etc.) to match the current state of the form.
        /// </summary>
        private void InvalidateButtons()
        {
            bool isLoaded = (imageboxMain.Image != null);
            if (!isLoaded)
            {
                foreach (ToolStripItem item in imageTools)
                {
                    item.Enabled = false;
                    if (item is ToolStripButton)
                    {
                        ((ToolStripButton)item).Checked = false;
                    }
                    else if (item is ToolStripComboBox)
                    {
                        ((ToolStripComboBox)item).ComboBox.SelectedIndex = 0;
                    }
                }
            }
            else
            {
                foreach (ToolStripItem item in imageTools)
                {
                    item.Enabled = true;
                    if (item is ToolStripComboBox)
                    {
                        item.Enabled = (selectedNotes.Count > 0);
                    }
                }
                btnDelete.Enabled = (selectedNotes.Count > 0);
                menuDelete.Enabled = (selectedNotes.Count > 0);
                btnMerge.Enabled = (selectedNotes.Count > 1);
                menuMergeNotes.Enabled = (selectedNotes.Count > 1);
                btnAddNote.Checked = (currentMode == SelectionMode.Create);
                menuAddNote.Checked = (currentMode == SelectionMode.Create);
                btnSelectMultiple.Checked = (currentMode == SelectionMode.Select);
                mnuMultiSelect.Checked = (currentMode == SelectionMode.Select);
                btnPan.Checked = (currentMode == SelectionMode.Default);
            }
            btnSave.Enabled = (currentSession != null && currentSession.Dirty);
            menuNotes.Enabled = isLoaded;
            menuSaveReady.Enabled = isLoaded;
        }
        #endregion

        #region Note Operations
        #region Selection

        /// <summary>
        /// Select a note by making its bounding rectangle red and adding it to the relevant lists.
        /// Also highlights it in the listbox if neccesary
        /// </summary>
        /// <param name="which">Note object to select</param>
        private void SelectNote(Note which)
        {
            DrawRectangle(currentPage.Working, which.Rectangle, McvColor.Red);
            selectedNotes.Add(which);

            SyncSelection(false, false);
        }

        /// <summary>
        /// Un-Select a note by making its bounding rectangle green and removing it from the relevant lists.
        /// Also un-highlights it in the listbox if neccesary
        /// </summary>
        /// <param name="which">A Note object to deselect</param>        
        private void DeselectNote(Note which)
        {
            DrawRectangle(currentPage.Working, which.Rectangle, which.isComplete() ? McvColor.Blue : McvColor.Green);
            selectedNotes.Remove(which); //This HAS to come before changing the listbox selection to avoid looping           
            SyncSelection(false, false);
        }

        /// <summary>
        /// Resets the selection of any notes both graphically and logically
        /// </summary>
        private void ClearSelection()
        {
            selectedNotes.Clear();
            SyncSelection(true, false);
        }
        /// <summary>
        /// A "Last Resort" function - goes over all the notes and redraws them according whether or not they're in the selected list.
        /// </summary>
        private void RedrawAllRectangles()
        {
            currentPage.Working = currentPage.Original.Clone();
            foreach (Note note in noteList)
            {
                if (selectedNotes.Contains(note))
                    DrawRectangle(currentPage.Working, note.Rectangle, McvColor.Red);
                else if (note.isComplete())
                    DrawRectangle(currentPage.Working, note.Rectangle, McvColor.Blue);
                else
                    DrawRectangle(currentPage.Working, note.Rectangle, McvColor.Green);
            }
            imageboxMain.Image = currentPage.Working.Bitmap;
        }

        /// <summary>
        /// Draws a rectengle on an image
        /// </summary>
        /// <param name="img">What image to draw on</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color to draw in</param>
        private void DrawRectangle(Mat img, Rectangle rect, MCvScalar color)
        {
            CvInvoke.Rectangle(img, rect, color, 2);
        }
        #endregion

        #region Note Editing

        /// <summary>
        /// When creating a note - first sets the correct mode.
        /// The actual creation will be handled by the Selecting method.
        /// </summary>
        public void StartCreate()
        {
            if (currentMode != SelectionMode.Create)
            {
                setMode(SelectionMode.Create);
            }
            else
            {
                setMode(SelectionMode.Default);
            }
        }

        /// <summary>
        /// Deletes whatever notes are currently selected.
        /// </summary>
        public void DeleteSelectedNotes()
        {
            if (selectedNotes.Count <= 0) //Shouldn't happen
                return;

            string message = "Are you sure you want to delete the selected note(s)?\nTHIS CAN NOT BE UNDONE!";
            DialogResult confirm = MessageBox.Show(this, message, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                listBoxFoundNotes.BeginUpdate();
                isListBoxUpdating = true;
                foreach (Note note in selectedNotes.ToList()) //TODO: Investigate whether performence here can be improved
                {
                    noteList.Remove(note);
                }
                selectedNotes.Clear();
                listBoxFoundNotes.EndUpdate();
                isListBoxUpdating = false;
                SyncSelection(true, false);
            }
            currentSession.Dirty = true;
        }

        /// <summary>
        /// Merges all the selected notes into one - which is the bounding box of the selection.
        /// </summary>
        public void MergeSelectedNotes()
        {
            listBoxFoundNotes.BeginUpdate();
            isListBoxUpdating = true;
            Note mergedNote = new Note(currentPage.Hash);
            int newX = int.MaxValue, newY = int.MaxValue, newBottom = 0, newRight = 0;
            List<Author> differentAuthors = new List<Author>();
            List<Note.NoteType> differentTypes = new List<Note.NoteType>();
            foreach (Note selected in selectedNotes.ToList())
            {
                if (!differentAuthors.Contains(selected.Author))
                    differentAuthors.Add(selected.Author);
                if (!differentTypes.Contains(selected.Type))
                    differentTypes.Add(selected.Type);
                newX = Math.Min(newX, selected.Rectangle.X);
                newY = Math.Min(newY, selected.Rectangle.Y);
                newBottom = Math.Max(selected.Rectangle.Bottom, newBottom);
                newRight = Math.Max(selected.Rectangle.Right, newRight);
                noteList.Remove(selected);
            }
            Rectangle newRect = new Rectangle(newX, newY, newRight - newX, newBottom - newY);
            mergedNote.Rectangle = newRect;

            if (differentTypes.Count == 1)
                mergedNote.Type = differentTypes[0];
            if (differentAuthors.Count == 1)
                mergedNote.Author = differentAuthors[0];

            noteList.Add(mergedNote);
            selectedNotes.Clear();
            selectedNotes.Add(mergedNote);
            listBoxFoundNotes.EndUpdate();
            isListBoxUpdating = false;
            SyncSelection(true, false);
            currentSession.Dirty = true;
        }

        /// <summary>
        /// Clears all detected notes.
        /// </summary>
        public void ClearAllDetected()
        {
            string message = "All your work will be lost!\nAre you sure you want to clear all detected notes?";
            DialogResult confirm = MessageBox.Show(this, message, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                selectedNotes.Clear();
                noteList.Clear();
            }
            currentSession.Dirty = true;
        }

        #endregion

        #region Note Scanning

        /// <summary>
        /// Calls for a background worker to do the actual scanning in a different thread because this process is a bit lengthy and can make the GUI freeze.
        /// This way it shows a nice progress bar instead so the user doesn't have to wonder if the program crashed.
        /// </summary>
        private void ScanNotes()
        {
            listBoxFoundNotes.BeginUpdate();
            progressDialog = new frmProgress();
            backgroundWorkerScan.RunWorkerAsync();
            progressDialog.ShowDialog();
        }

        /// <summary>
        /// The actual scanning method. Runs async from the main thread so can only be called by a background worker.
        /// Uses EMGU (OpenCV for C#).
        /// After some pre-processing (turning the image to gray then increasing contrast by using a threshold), detects countours (based on the min/max values set by the user),
        /// then adds all the bounding boxes of these contours to the list of notes (using a delegate method).
        /// </summary>
        private void ScanNotesAsync()
        {
            if (imageboxMain.Image == null) //shouldn't happen
                return;
            Mat gray = new Mat();
            Mat thresh = new Mat();
            VectorOfVectorOfPoint conts = new VectorOfVectorOfPoint();
            Mat hirarchy = new Mat();
            CvInvoke.CvtColor(currentPage.Original, gray, ColorConversion.Bgr2Gray);
            CvInvoke.Threshold(gray, thresh, 0, 255, ThresholdType.Otsu);
            CvInvoke.FindContours(thresh, conts, hirarchy, RetrType.List, ChainApproxMethod.ChainApproxSimple);
            isListBoxUpdating = true;
            for (int i = conts.Size - 1; i >= 0; i--)
            {
                if (backgroundWorkerScan.IsBusy)
                {
                    backgroundWorkerScan.ReportProgress(i, conts.Size);
                }
                if (CvInvoke.ContourArea(conts[i]) > (double)numMin.Value && CvInvoke.ContourArea(conts[i]) < (double)numMax.Value)
                {
                    Rectangle contRect = CvInvoke.BoundingRectangle(conts[i]);
                    Note newNote = new Note(currentPage.Hash);
                    newNote.Rectangle = contRect;
                    listBoxFoundNotes.Invoke(new UpdateUIDelegate(AddToListAsync), newNote);
                }
            }
            isListBoxUpdating = false;
        }

        /// <summary>
        /// A delegate method that allows updating the note list from a different thread than the one that created it.
        /// </summary>
        /// <param name="which"></param>
        private delegate void UpdateUIDelegate(Note which);
        private void AddToListAsync(Note which)
        {
            IEnumerable<Note> duplicate = noteList.Where(n => n.Rectangle == which.Rectangle);
            if (duplicate.Count() == 0) //Not a duplicate
            {
                noteList.Add(which);
            }
            else
            {
                foreach (Note dup in duplicate.ToList())
                    noteList.Remove(dup);

                noteList.Add(which);
            }
        }

        /// <summary>
        /// If the note list changes - makes the session "dirty" so it prompts the user to save if they try to end it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoteList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (currentSession != null)
                currentSession.Dirty = true;
        }

        /// <summary>
        /// Asks the user if they want to save their current session if it's dirty.
        /// Will return true even if the user didn't want to save - because as far as we care it means a new session can go on.
        /// </summary>
        /// <returns>True if the user saved or didn't want to save - false if the user cancelled.</returns>
        public bool VerifySave()
        {
            if (currentSession != null && currentSession.Dirty)
            {
                DialogResult yesNoCancel = MessageBox.Show(this, "You have unsaved changed!\nDo you want to save them now?", "You have unsaved changed!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                switch (yesNoCancel)
                {
                    case DialogResult.Yes:
                        FileSave();
                        return true;
                    case DialogResult.No:
                        return true;
                    case DialogResult.Cancel:
                    default:
                        return false;
                }
            }
            else
                return true;
        }

        #endregion

        #endregion
        #region Background Workers

        /// <summary>
        /// If a note exists in the current image that also exists in the database, load its info from the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerLoadFromDB_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Note> detectedNotes = db.NotesInPage(currentPage);
            int i = 0;
            foreach (Note note in detectedNotes)
            {
                backgroundWorkerLoadFromDB.ReportProgress(i, detectedNotes.Count);
                i++;
                listBoxFoundNotes.Invoke(new UpdateUIDelegate(AddToListAsync), new Note(note));
            }
        }

        private void backgroundWorkerLoadFromDB_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (progressDialog.Max < (int)e.UserState)
            {
                progressDialog.Max = (int)e.UserState;
            }
            progressDialog.Progress = e.ProgressPercentage;
            progressDialog.Message = $"Comparings with notes in Database: {e.ProgressPercentage} / {e.UserState.ToString()}";
        }

        /// <summary>
        /// Update all the GUI elements to the new list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerLoadFromDB_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressDialog.Close();
            progressDialog.Dispose();
            isListBoxUpdating = false;
            listBoxFoundNotes.EndUpdate();
            listBoxFoundNotes.Refresh();
            SyncSelection(true, false);
            setMode(SelectionMode.Default);
            imageboxMain.Cursor = Cursors.SizeAll;
            listBoxFoundNotes.DataSource = noteList;
        }

        /// <summary>
        /// Scans the image for notes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerScan_DoWork(object sender, DoWorkEventArgs e)
        {
            ScanNotesAsync();
        }

        /// <summary>
        /// Update all the GUI elements to the new list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerScan_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressDialog.Close();
            progressDialog.Dispose();
            listBoxFoundNotes.EndUpdate();
            listBoxFoundNotes.Refresh();
            SyncSelection(true, false);
            setMode(SelectionMode.Default);
            imageboxMain.Cursor = Cursors.SizeAll;
            listBoxFoundNotes.DataSource = noteList;
        }

        private void backgroundWorkerScan_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (progressDialog.Max < (int)e.UserState)
            {
                progressDialog.Max = (int)e.UserState;
            }
            progressDialog.Progress = (int)e.UserState - e.ProgressPercentage;
            progressDialog.Message = $"Processing: {(int)e.UserState - e.ProgressPercentage} / {(int)e.UserState}.";
        }

        #endregion
        #region DB Operations

        /// <summary>
        /// If a note exists in the current image that also exists in the database, load its info from the database.
        /// Does so async using a background worker.
        /// </summary>
        private void LoadNotesFromDB()
        {
            if (db.IsPageInDB(currentPage))
            {
                isListBoxUpdating = true;
                listBoxFoundNotes.BeginUpdate();
                progressDialog = new frmProgress();
                progressDialog.Text = "Just a little bit longer...";
                backgroundWorkerLoadFromDB.RunWorkerAsync();
                progressDialog.ShowDialog();
            }
        }

        /// <summary>
        /// Allows exporting the currently working database to a file.
        /// Just creates a copy wherever the user selects.
        /// </summary>
        public void BackupDatabase()
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "SQLite Database File|*.db";
            DialogResult result = ofd.ShowDialog();
            if (result == DialogResult.OK)
            {
                File.Copy(db.DatabasePath, ofd.FileName, true);
            }
        }

        /// <summary>
        /// Deloads the database, then repopulates all database dependent interface elements.
        /// </summary>
        public void InvalidateDatabase()
        {
            db.ReloadAll();
            authorList = new BindingList<Author>(db.AuthorsList);
            noteTypeList = new BindingList<Note.NoteType>(db.NoteTypeList);
            authorList.Insert(0, new Author(-1, null, -1));
            noteTypeList.Insert(0, new Note.NoteType(-1, null, -1));
            comboAuthor.ComboBox.DataSource = authorList;
            comboNoteType.ComboBox.DataSource = noteTypeList;
            comboAuthor.ComboBox.Invalidate();
            comboNoteType.ComboBox.Invalidate();
            comboAuthor.SelectedIndex = 0;
            comboNoteType.SelectedIndex = 0;
            lblNoteInDB.Text = $"Profiled notes in database: {db.NoteCount}";
            lblPagesInDB.Text = $"Pages in database: {db.PageCount}";
            lblDBVersion.Text = $"Database Version: {db.DatabaseVersion}";
        }

        /// <summary>
        /// Saves completed notes (with both a type and an author) to the database.
        /// </summary>
        private void SaveCompleted()
        {
            foreach (Note note in noteList)
            {
                if (note.isComplete()) //Enters here if complete (marked update optional)
                {
                    db.AddNote(note, currentPage.Original);
                    note.MarkedUpdate = false;
                }                    
                else if (note.MarkedUpdate) //Only enters here if marked update but not complete
                {
                    db.DeleteNote(note);
                    note.MarkedUpdate = false;
                }
                    
            }
            db.AddPage(currentPage);
            db.SaveAll();
            InvalidateDatabase();
        }
        #endregion

        #region Error Handling
        public void UnhandledThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            this.HandleUnhandledException(e.Exception);
        }

        /// <summary>
        /// Allows the user to save the trace of unhandled exceptions for easier reporting to the developers.
        /// Also allows the user to continue working - despite that being generally a bad idea - but that's on the user, a warning IS displayed.
        /// </summary>
        /// <param name="e"></param>
        public void HandleUnhandledException(Exception e)
        {
            DialogResult YesNo = MessageBox.Show(this, "An unexpected error has occured!\nYou can save the error log to a file so you can send it to the developer.\nWould you like to do so?", "An unexpected error has occured", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (YesNo == DialogResult.Yes)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.Filter = "Error Log|*.log";
                DialogResult result = ofd.ShowDialog();
                string Error = "ERROR!\n" + e.Message + "\n" + e.StackTrace;
                if (result == DialogResult.OK)
                {
                    File.WriteAllText(ofd.FileName, Error);
                }
            }
            DialogResult Continue = MessageBox.Show(this, "Would you like to try to continue working? *Not recommended!*", "Would you like to contine?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (Continue == DialogResult.Yes)
            {
                return;
            }
            else
            {
                Application.Exit();
            }
        }
        #endregion

        private bool VerifyExit()
        {
            if (currentSession != null && currentSession.Dirty)
                return VerifySave();
            else
            {
                DialogResult yesno = MessageBox.Show(this, "Are you sure you want to exit the program?", "Are you sure you want to exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return (yesno == DialogResult.Yes);
            }
        }

        /// <summary>
        /// A class that represents a current working session.
        /// Makes not of the currently open page, handles ID numbering for new notes and holds the paths for all the temporary files either created or loaded from a saved profile.
        /// </summary>
        private class Session
        {
            private bool _dirty;
            private frmMain _owner;

            public Session(frmMain owner, Page openPage)
            {
                _owner = owner;
                OpenPage = openPage;
                Name = Path.GetFileNameWithoutExtension(OpenPage.FileName);
                WorkingDirectyory = Path.GetTempPath() + "\\" + Name;
                if (Directory.Exists(WorkingDirectyory))
                    Directory.Delete(WorkingDirectyory, true);
                Directory.CreateDirectory(WorkingDirectyory);
                File.Copy(OpenPage.FullPath, WorkingDirectyory + "\\" + OpenPage.FileName);
                Note.ResetID();
                Dirty = true;
            }

            public Session(frmMain owner, Page openPage, string profilePath) : this(owner, openPage)
            {
                ProfilePath = profilePath;
                Dirty = false;
            }

            /// <summary>
            /// Whether or not the session has been updated since it's last been saved.
            /// </summary>
            public bool Dirty
            {
                get { return _dirty; }
                set
                {
                    _dirty = value;
                    _owner.btnSave.Enabled = value;
                }
            }

            /// <summary>
            /// The path of the profile file (.npf) if saved or loaded
            /// </summary>
            public string ProfilePath { get; set; }

            public string Name { get; }

            public string WorkingDirectyory { get; }

            public Page OpenPage { get; }

            public void EndSession()
            {
                if (Directory.Exists(WorkingDirectyory))
                    Directory.Delete(WorkingDirectyory, true);
            }
        }
    }



    /// <summary>
    /// A class representing a single note object.
    /// </summary>
    public class Note : NotifyProperyChangedBase
    {
        private static int ID_Dispencer = 0;

        private Author _author;
        private NoteType _type;

        public bool MarkedUpdate { get; set; }

        public Note() { }

        public Note(Note other)
        {
            Rectangle = other.Rectangle;
            ID = other.ID;
            Author = other.Author;
            Type = other.Type;
            PageHash = other.PageHash;
        }

        public Note(string pageHash)
        {
            ID = ID_Dispencer++;
            PageHash = pageHash;
        }

        /// <summary>
        /// The notes coordinates (on the image)
        /// </summary>
        public Rectangle Rectangle { get; set; }

        public Author Author
        {
            get
            {
                return _author;
            }
            set
            {
                if (this.CheckPropertyChanged<Author>("Author", ref _author, ref value))
                {
                    this.ToStringChanged();
                }
            }
        }

        public NoteType Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (this.CheckPropertyChanged<NoteType>("Type", ref _type, ref value))
                {
                    this.ToStringChanged();
                }
            }
        }

        private void ToStringChanged()
        {
            this.FirePropertyChanged("ToString");
        }

        /// <summary>
        /// The hash of the page the note was profiled from
        /// </summary>
        public string PageHash { get; set; }

        public int ID { get; set; }

        override public string ToString()
        {
            string type = Type == null ? "Unprofiled Note" : Type.Description;
            string name = Author == null ? "Uknown Author" : Author.Name;
            return $"{type} by {name}";
        }

        public bool isComplete()
        {
            return (Author != null && Type != null);
        }

        /// <summary>
        /// A static class the resets the ID dispencer
        /// </summary>
        public static void ResetID()
        {
            ID_Dispencer = 0;
        }

        /// <summary>
        /// A class representing a note type (half, eight, etc.)
        /// </summary>
        public class NoteType
        {
            public NoteType() { }

            public NoteType(int id, string description, int sortOrder)
            {
                ID = id;
                Description = description;
                SortOrder = sortOrder;
            }

            public int ID { get; set; }
            public string Description { get; set; }

            public int SortOrder { get; set; }

            public override string ToString()
            {
                if (ID == -1)
                    return "<Clear>";
                if (Description == null || Description.Equals(string.Empty))
                    Description = "Unknown Type";
                return Description;
            }
        }
    }

    /// <summary>
    /// A class representing an author.
    /// </summary>
    public class Author
    {
        public Author() { }

        public Author(int id, string name, int sortid)
        {
            ID = id;
            Name = name;
        }
        public int ID { get; set; }
        public string Name { get; set; }

        public int SortOrder { get; set; }

        public override string ToString()
        {
            if (ID == -1)
                return "<Clear>";

            if (Name == null || Name.Equals(string.Empty))
                return "Unknown Author";

            return $"{ID}. {Name}";
        }

    }

    /// <summary>
    /// A class representing a page.
    /// A page includes an original copy of the used image, a working image (on which we draw rectengles), and properties of where the image was loaded from.
    /// </summary>
    public class Page
    {
        public Page(string address)
        {
            Original = CvInvoke.Imread(address, LoadImageType.AnyColor);
            Working = Original.Clone();
            Hash = Utilities.FileHash(address);
            FileName = Path.GetFileName(address);
            ImageExt = Path.GetExtension(address);
            FullPath = address;
        }

        public Mat Original { get; }

        public Mat Working { get; set; }

        public string Hash { get; }

        public string ImageExt { get; }

        public string FileName { get; }

        public string FullPath { get; }

        public override string ToString()
        {
            return FileName;
        }

    }


    /// <summary>
    /// The colors we use to draw rectengles.
    /// </summary>
    static class McvColor
    {
        public static MCvScalar Red = new MCvScalar(0, 0, 255);
        public static MCvScalar Green = new MCvScalar(0, 255, 0);
        public static MCvScalar Blue = new MCvScalar(255, 0, 0);
    }

    public abstract class NotifyProperyChangedBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region methods
        protected bool CheckPropertyChanged<T>
        (string propertyName, ref T oldValue, ref T newValue)
        {
            if (oldValue == null && newValue == null)
            {
                return false;
            }

            if ((oldValue == null && newValue != null) || !oldValue.Equals((T)newValue))
            {
                oldValue = newValue;
                FirePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected void FirePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
