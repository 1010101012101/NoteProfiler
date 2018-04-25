using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static Note_Profiler.Note;

namespace Note_Profiler
{

    /// <summary>
    /// An adapter for the interface to use to access the database.
    /// While the inner classes and methods are all static - creating an instance of the adapter will initalize and populate those,
    /// and there are interfaces for the static classes for faster accesss.
    /// We originally wanted to have all the inner classes inherit from a generic class since they have a lot of common code, but never got around to it.
    /// </summary>
    public class DBAdapter
    {
        #region Global Variables
        SQLiteConnection dbConnection = new SQLiteConnection();
        #endregion

        /// <summary>
        /// Creates a connection to the database in a working folder in the user's ApplicationData folder.
        /// If it doesn't exist, copies an empty database from the app's main dir.
        /// If the database is of an older version, updates it by adding the missing fields.
        /// Finally, initalizes the inner classes.
        /// </summary>
        public DBAdapter()
        {
            SQLiteConnectionStringBuilder connectionBuilder = new SQLiteConnectionStringBuilder();
            DatabasePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NoteProfiler\\profiledNotes.db";
            if (!File.Exists(DatabasePath))
            {
                FileInfo fi = new FileInfo(DatabasePath);
                if (!fi.Directory.Exists)
                    fi.Directory.Create();
                File.Copy(Application.StartupPath + "\\Database\\ProfiledNotes.db", DatabasePath);
            }
            Console.Out.WriteLine("Database Path" + DatabasePath);
            connectionBuilder.DataSource = DatabasePath;
            connectionBuilder.FailIfMissing = true;
            dbConnection.ConnectionString = connectionBuilder.ConnectionString;
            DatabaseVersion = GetDatabaseVersion();
            if (DatabaseVersion < 2)
            {
                UpgradeDB(DatabaseVersion);
                DatabaseVersion = 2;
            }
            AuthorsAdapter.Init(dbConnection);
            NotesAdapter.Init(dbConnection);
            PageAdapter.Init(dbConnection);
            NoteTypeAdapter.Init(dbConnection);
        }

        #region Public Properties

        public string DatabasePath { get; }

        public SQLiteConnection SQLConnection
        {
            get { return dbConnection; }
        }

        public int DatabaseVersion
        {
            get;
        }

        /// <summary>
        /// Returns the number of profiled notes in the DB.
        /// </summary>
        public int NoteCount
        {
            get { return NotesAdapter.DataTable.Rows.Count; }
        }

        /// <summary>
        /// Returns the number of different pages in the DB.
        /// </summary>
        public int PageCount
        {
            get { return PageAdapter.DataTable.Rows.Count; }
        }

        public DataTable AuthorsTable
        {
            get { return AuthorsAdapter.DataTable; }
        }

        public DataTable NoteTypeTable
        {
            get { return NoteTypeAdapter.DataTable; }
        }

        /// <summary>
        /// Returns the list of authors currently in the DB.
        /// </summary>
        public List<Author> AuthorsList
        {
            get { return AuthorsAdapter.GetList(); }
        }

        /// <summary>
        /// Returns the list of Note Types currently in the DB.
        /// </summary>
        public List<NoteType> NoteTypeList
        {
            get { return NoteTypeAdapter.GetList(); }
        }

        /// <summary>
        /// Get the next ID in the Note Type table (used for auto-insertion in GUI).
        /// </summary>
        public int NextNoteType
        {
            get { return NoteTypeAdapter.GetNextID(); }
        }

        /// <summary>
        /// Get the next ID in the Author table (used for auto-insertion in GUI).
        /// </summary>
        public int NextAuthor
        {
            get { return AuthorsAdapter.GetNextID(); }
        }

        #endregion

        #region Public Methods
        public void ReloadAll()
        {
            AuthorsAdapter.LoadData();
            NotesAdapter.LoadData();
            PageAdapter.LoadData();
            NoteTypeAdapter.LoadData();
        }

        public void SaveAll()
        {
            AuthorsAdapter.SaveData();
            NotesAdapter.SaveData();
            PageAdapter.SaveData();
            NoteTypeAdapter.SaveData();
        }

        /// <summary>
        /// Swaps the sort order of two note types
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public void SwapNoteTyes(int first, int second)
        {
            NoteTypeAdapter.SwapSort(first, second);
            NoteTypeAdapter.SaveData();
        }


        /// <summary>
        /// Swaps the sort order of two Authors
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        public void SwapAuthorType(int first, int second)
        {
            AuthorsAdapter.SwapSort(first, second);
            AuthorsAdapter.SaveData();
        }

        public void AddNote(Note which, Mat image)
        {
            NotesAdapter.AddNote(which, image);            
        }

        public void DeleteNote(int ID)
        {
            NotesAdapter.DeleteNote(ID);
        }

        public void DeleteNote(Note which)
        {
            NotesAdapter.DeleteNote(which);
        }

        public void AddPage(Page which)
        {
            PageAdapter.AddPage(which);
        }

        public bool IsPageInDB(Page which)
        {
            DataRow result = PageAdapter.FindPageInDB(which.Hash);
            return result != null;
        }

        public List<Note> NotesInPage(Page which)
        {
            return NotesAdapter.FindByPage(which);
        }

        public List<DBPage> PageList()
        {
            return PageAdapter.GetList();
        }

        /// <summary>
        /// Returns all the notes currently in the database, but using the "DBNote" class rather than "Note" (since we don't need all the relevant data to create a proper note).
        /// </summary>
        /// <returns></returns>
        public List<DBNote> NotesInDB()
        {
            return NotesAdapter.GetAllInDB();
        }

        /// <summary>
        /// DEPRECATED - used previously to replaced faulty page images in the old corrupted database.
        /// It is no longer in use - but we kept it in the code for reference.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="image"></param>
        public void UpdatePage(string name, Bitmap image)
        {
            PageAdapter.UpdatePage(name, image);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the DB version
        /// </summary>
        /// <returns></returns>
        private int GetDatabaseVersion()
        {
            int reply = -1;
            dbConnection.Open();
            using (SQLiteCommand cmd = new SQLiteCommand("PRAGMA user_version", dbConnection))
            {
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        reply = rdr.GetInt32(0);
                    }
                }
            }
            dbConnection.Close();
            return reply;
        }

        /// <summary>
        /// Upgrades the DB to the most current version (which is 2) based on its current (old) version.
        /// It does so by adding the missing fields - which is why we need to know what's the old version.
        /// We COULD just TEST to see if those fields exist or not, but it's easier to just check one parameter each time.
        /// </summary>
        /// <param name="oldVersion">DB version of the DB to upgrade</param>
        private void UpgradeDB(int oldVersion)
        {
            DialogResult YesNo = MessageBox.Show(null, "Your database is about to be upgraded!\nIt is recommended that you back it up first.\nWould you like to do so now?", "Your database is about to be upgraded!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (YesNo == DialogResult.Yes)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.Filter = "SQLite Database File|*.db";
                DialogResult result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    File.Copy(DatabasePath, ofd.FileName, true);
                }
                ofd.Dispose();
            }
            dbConnection.Open();
            if (oldVersion < 1) //If upgrading from Version 0
            {
                using (SQLiteCommand cmd = new SQLiteCommand("ALTER TABLE Types ADD COLUMN Sort INTEGER", dbConnection))
                {
                    cmd.ExecuteNonQuery();

                }
            }
            if (oldVersion < 2) //If upgrading from either 0 or 1
            {
                using (SQLiteCommand cmd = new SQLiteCommand("ALTER TABLE Authors ADD COLUMN Sort INTEGER", dbConnection))
                {
                    cmd.ExecuteNonQuery();

                }
            }
            using (SQLiteCommand cmd = new SQLiteCommand("PRAGMA user_version = 2", dbConnection)) //Sets the new version
            {
                cmd.ExecuteNonQuery();
            }
            dbConnection.Close();
        }
        #endregion

        /// <summary>
        /// Allows access to the "Pages" table.
        /// This class and its methods are static, so it's best no to access them directly but use the interface methods of the DBAdapter class instead.
        /// </summary>
        protected static class PageAdapter
        {

            #region Private fields

            static SQLiteConnection dbConnection;
            static SQLiteDataAdapter sqlAdapter;
            static SQLiteCommandBuilder sqlCommand;
            static DataSet DS;
            static DataTable DT;

            #endregion

            /// <summary>
            // Set the inital properties for this adapter.
            /// </summary>
            /// <param name="connection"></param>
            public static void Init(SQLiteConnection connection)
            {
                dbConnection = connection;
                DS = new DataSet();
                sqlAdapter = new SQLiteDataAdapter("SELECT * FROM Pages", dbConnection);
                sqlCommand = new SQLiteCommandBuilder(sqlAdapter);
                sqlAdapter.InsertCommand = sqlCommand.GetInsertCommand();
                sqlAdapter.UpdateCommand = sqlCommand.GetUpdateCommand();
                sqlAdapter.DeleteCommand = sqlCommand.GetDeleteCommand();
            }

            #region Properties
            public static SQLiteDataAdapter DataAdapter
            {
                get { return sqlAdapter; }
            }

            public static DataSet DataSet
            {
                get { return DS; }
            }

            public static DataTable DataTable
            {
                get { return DT; }
            }

            #endregion

            #region Public Methods

            public static void LoadData()
            {
                dbConnection.Open();
                DS.Clear();
                sqlAdapter.Fill(DS);
                DT = DS.Tables[0];
                dbConnection.Close();
            }

            /// <summary>
            /// Adds a new page to the database (unless it already exists - which it determines by the page's Hash).
            /// </summary>
            /// <param name="which"></param>
            public static void AddPage(Page which)
            {
                string Hash = which.Hash;
                string Name = which.FileName;
                string Extension = which.ImageExt;

                DataRow row = FindPageInDB(which.Hash);
                if (row == null) //New note!
                {
                    Console.Out.WriteLine("Inserting new page!");
                    row = DT.NewRow();
                    byte[] array = Utilities.ImageToByteArray(which.Original.Bitmap);
                    row["Hash"] = Hash;
                    row["Name"] = Name;
                    row["Extension"] = Extension;
                    row["Image"] = array;
                    DT.Rows.Add(row);
                }
                else
                {
                    Console.Out.WriteLine("Page already exists!");
                }
                SaveData();
            }

            /// <summary>
            /// DEPRECATED - used previously to replaced faulty page images in the old corrupted database.
            /// It is no longer in use - but we kept it in the code for reference.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="bitmap"></param>
            public static void UpdatePage(string name, Bitmap bitmap)
            {
                DataRow[] target = DT.Select($"Name = '{name}'");
                if (target.Length == 0)
                {
                    Console.Out.WriteLine($"Page named '{name}' not found!");
                }
                else
                {
                    Console.Out.WriteLine("Updating page...");
                    byte[] array = Utilities.ImageToByteArray(bitmap);
                    target[0]["Image"] = array;
                    SaveData();
                }
            }

            public static void SaveData()
            {
                dbConnection.Open();
                sqlAdapter.Update(DT);
                DS.AcceptChanges();
                dbConnection.Close();
                LoadData();
            }


            /// <summary>
            /// Returns a page's datarow if its Hash is already in the DB.
            /// </summary>
            /// <param name="hash"></param>
            /// <returns></returns>
            public static DataRow FindPageInDB(string hash)
            {
                DataRow[] result = DT.Select($"Hash = '{hash}'");
                if (result.Length <= 0)
                    return null;
                else
                    return result[0];
            }

            /// <summary>
            /// Returns a list of pages existing in the database, in the "DBPage" class which also holds the image of the page.
            /// </summary>
            /// <returns></returns>
            public static List<DBPage> GetList()
            {
                List<DBPage> newList = new List<DBPage>();
                foreach (DataRow row in DT.Rows)
                {
                    byte[] array = row.Field<byte[]>("Image");
                    string name = row.Field<string>("Name");
                    Image image = Utilities.ByteArrayToImage(array);
                    DBPage newPage = new DBPage(image, name);
                    newList.Add(newPage);
                }
                return newList;
            }
            #endregion
        }

        /// <summary>
        /// Allows access to the "NoteType" table.
        /// This class and its methods are static, so it's best no to access them directly but use the interface methods of the DBAdapter class instead.
        /// </summary>
        protected static class NoteTypeAdapter
        {
            static SQLiteConnection dbConnection;
            static SQLiteDataAdapter sqlAdapter;
            static SQLiteCommandBuilder sqlCommand;
            static DataSet DS;
            static DataTable DT;

            /// <summary>
            /// Set the inital properties for this adapter.
            /// </summary>
            /// <param name="connection"></param>
            public static void Init(SQLiteConnection connection)
            {
                dbConnection = connection;
                DS = new DataSet();
                sqlAdapter = new SQLiteDataAdapter("SELECT * FROM Types ORDER BY Sort ASC", dbConnection);
                sqlCommand = new SQLiteCommandBuilder(sqlAdapter);
                sqlAdapter.InsertCommand = sqlCommand.GetInsertCommand();
                sqlAdapter.UpdateCommand = sqlCommand.GetUpdateCommand();
                sqlAdapter.DeleteCommand = sqlCommand.GetDeleteCommand();
            }

            #region Properties
            public static SQLiteDataAdapter DataAdapter
            {
                get { return sqlAdapter; }
            }

            public static DataSet DataSet
            {
                get { return DS; }
            }

            public static DataTable DataTable
            {
                get { return DT; }
            }

            #endregion

            /// <summary>
            /// Sets the sort value of each row depending on its actual order in the database.
            /// Used for upgrading older database versions which didn't support sorting.
            /// </summary>
            private static void SortFiller()
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DT.Rows[i]["Sort"] = i;
                }
            }

            #region Publid Methods

            /// <summary>
            /// Returns the next ID to be auto-filled by the DB.
            /// </summary>
            /// <returns></returns>
            public static int GetNextID()
            {
                SQLiteCommand cmdNewID = new SQLiteCommand("SELECT MAX(ID) + 1 FROM [Types]", dbConnection);
                dbConnection.Open();
                var cmdResult = cmdNewID.ExecuteScalar();
                int result = -1;
                if (cmdResult != DBNull.Value)
                    result = Convert.ToInt32(cmdResult);
                dbConnection.Close();
                return result;
            }

            /// <summary>
            /// Returns a new NoteType object that corresponds with the given ID.
            /// </summary>
            /// <param name="id">The ID to search for.</param>
            /// <returns></returns>
            public static NoteType TypeFromID(int id)
            {
                DataRow row = DT.Select("ID =" + id)[0];
                string description = row.Field<string>("Description");
                int sortid = Convert.ToInt32(row.Field<Int64>("Sort"));
                return new NoteType(id, description, sortid);
            }

            /// <summary>
            /// Given an existing NoteType object, finds a matching item in the database and returns its DB ID.
            /// Creates a new entry in the DB if a matching ID doesn't exist (we assume an ID is passed from the GUI, so that shouldn't happen in theory).
            /// </summary>
            /// <param name="which">The NoteType object to search for.</param>
            /// <returns></returns>
            public static int IDFromType(NoteType which)
            {
                if (which == null)
                    return -1;

                var row = DT.Select($"Description = '{which.Description}'");
                if (row.Length > 0)
                    return Convert.ToInt32(row[0].Field<Int64>("ID"));
                else
                {
                    int nextID = GetNextID();
                    DataRow newRow = DT.NewRow();
                    newRow["ID"] = nextID;
                    newRow["Description"] = which.Description;
                    newRow["Sort"] = nextID;
                    DT.Rows.Add(newRow);
                    return nextID;
                }
            }

            public static void SwapSort(int first, int second)
            {
                DataRow rowFirst = DT.Select("ID = " + first)[0];
                DataRow rowSecond = DT.Select("ID = " + second)[0];
                Int64 temp = (Int64)rowFirst["Sort"];
                rowFirst["Sort"] = rowSecond["Sort"];
                rowSecond["Sort"] = temp;
            }

            public static List<NoteType> GetList()
            {
                List<NoteType> newList = new List<NoteType>();
                foreach (DataRow row in DT.Rows)
                {
                    int id = Convert.ToInt32(row.Field<Int64>("ID"));
                    string desc = row.Field<string>("Description");
                    var sortid = row["Sort"];
                    if (sortid == DBNull.Value)
                        sortid = id;
                    sortid = Convert.ToInt32(sortid);
                    NoteType newType = new NoteType(id, desc, (int)sortid);
                    newList.Add(newType);
                }
                return newList;
            }

            public static void LoadData()
            {
                dbConnection.Open();
                DS.Clear();
                sqlAdapter.Fill(DS);
                DT = DS.Tables[0];
                dbConnection.Close();
                SortFiller();
            }

            public static void SaveData()
            {
                dbConnection.Open();
                sqlAdapter.Update(DT);
                DS.AcceptChanges();
                dbConnection.Close();
                LoadData();
            }
            #endregion
        }


        /// <summary>
        /// Allows access to the "Author" table.
        /// This class and its methods are static, so it's best no to access them directly but use the interface methods of the DBAdapter class instead.
        /// </summary>
        protected static class AuthorsAdapter
        {
            static SQLiteConnection dbConnection;
            static SQLiteDataAdapter sqlAdapter;
            static SQLiteCommandBuilder sqlCommand;
            static DataSet DS;
            static DataTable DT;

            /// <summary>
            /// Set the inital properties for this adapter.
            /// </summary>
            /// <param name="connection"></param>
            public static void Init(SQLiteConnection connection)
            {
                dbConnection = connection;
                DS = new DataSet();
                sqlAdapter = new SQLiteDataAdapter("SELECT * FROM Authors ORDER BY Sort ASC", dbConnection);
                sqlCommand = new SQLiteCommandBuilder(sqlAdapter);
                sqlAdapter.InsertCommand = sqlCommand.GetInsertCommand();
                sqlAdapter.UpdateCommand = sqlCommand.GetUpdateCommand();
                sqlAdapter.DeleteCommand = sqlCommand.GetDeleteCommand();
            }

            /// <summary>
            /// Sets the sort value of each row depending on its actual order in the database.
            /// Used for upgrading older database versions which didn't support sorting.
            /// </summary>
            private static void SortFiller()
            {
                foreach (DataRow row in DT.Rows)
                {
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        DT.Rows[i]["Sort"] = i;
                    }
                }
            }

            #region Properties
            public static SQLiteDataAdapter DataAdapter
            {
                get { return sqlAdapter; }
            }

            public static DataSet DataSet
            {
                get { return DS; }
            }

            public static DataTable DataTable
            {
                get { return DT; }
            }

            #endregion

            #region Publid Methods

            /// <summary>
            /// Returns the next ID to be auto-filled by the DB.
            /// </summary>
            /// <returns></returns>
            public static int GetNextID()
            {
                SQLiteCommand cmdNewID = new SQLiteCommand("SELECT MAX(ID) + 1 FROM [Authors]", dbConnection);
                dbConnection.Open();
                int result = -1;
                var cmdResult = cmdNewID.ExecuteScalar();
                if (cmdResult != DBNull.Value)
                    result = Convert.ToInt32(cmdResult);
                dbConnection.Close();
                return result;
            }

            /// <summary>
            /// Returns a new Author object based on its ID in the database.
            /// WARNING: Doesn't handle non-existing authors - probably should.
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public static Author AuthorFromID(int id)
            {
                DataRow row = DT.Select("ID =" + id)[0];
                string name = row.Field<string>("Name");
                int sortid = Convert.ToInt32(row.Field<Int64>("Sort"));
                return new Author(id, name, sortid);
            }

            /// <summary>
            /// Given an Author object, finds a corresponding one in the DB and returns its ID.
            /// If none is found - creates one in the DB.
            /// </summary>
            /// <param name="which">The author object to find in the DB</param>
            /// <returns></returns>
            public static int IDFromAuthor(Author which)
            {
                if (which == null)
                    return -1;
                var row = DT.Select($"Name = '{which.Name}'");
                if (row.Length > 0)
                    return Convert.ToInt32(row[0].Field<Int64>("ID"));
                else
                {
                    DataRow newRow = DT.NewRow();
                    int nextID = GetNextID();
                    newRow["ID"] = nextID;
                    newRow["Name"] = which.Name;
                    newRow["Sort"] = nextID;
                    DT.Rows.Add(newRow);
                    return GetNextID();
                }
            }

            public static List<Author> GetList()
            {
                DataSet ds = new DataSet();
                sqlAdapter.Fill(ds);
                List<Author> newList = new List<Author>();
                foreach (DataRow row in DT.Rows)
                {
                    int id = Convert.ToInt32(row.Field<Int64>("ID"));
                    string name = row.Field<string>("Name");
                    var sortid = row["Sort"];
                    if (sortid == DBNull.Value)
                        sortid = id;
                    sortid = Convert.ToInt32(sortid);
                    Author newAuthor = new Author(id, name, (int)sortid);
                    newList.Add(newAuthor);
                }
                return newList;
            }

            public static void LoadData()
            {
                dbConnection.Open();
                DS.Clear();
                sqlAdapter.Fill(DS);
                DT = DS.Tables[0];
                dbConnection.Close();
                SortFiller();
            }

            public static void SaveData()
            {
                dbConnection.Open();
                sqlAdapter.Update(DT);
                DS.AcceptChanges();
                dbConnection.Close();
                LoadData();
            }

            public static void SwapSort(int first, int second)
            {
                {
                    DataRow rowFirst = DT.Select("ID = " + first)[0];
                    DataRow rowSecond = DT.Select("ID = " + second)[0];
                    Int64 temp = (Int64)rowFirst["Sort"];
                    rowFirst["Sort"] = rowSecond["Sort"];
                    rowSecond["Sort"] = temp;
                }
            }
            #endregion
        }

        /// <summary>
        /// Allows access to the Notes table in the database.
        /// </summary>
        protected static class NotesAdapter
        {
            static SQLiteConnection dbConnection;
            static SQLiteDataAdapter sqlAdapter;
            static SQLiteCommandBuilder sqlCommand;
            static DataSet DS;
            static DataTable DT;

            /// <summary>
            /// Set the inital properties for this adapter.
            /// </summary>
            /// <param name="connection"></param>
            static public void Init(SQLiteConnection connection)
            {
                dbConnection = connection;
                DS = new DataSet();
                sqlAdapter = new SQLiteDataAdapter("SELECT * FROM Notes", dbConnection);
                sqlCommand = new SQLiteCommandBuilder(sqlAdapter);

                sqlAdapter.InsertCommand = sqlCommand.GetInsertCommand();
                sqlAdapter.UpdateCommand = sqlCommand.GetUpdateCommand();
                sqlAdapter.DeleteCommand = sqlCommand.GetDeleteCommand();
            }

            public static DataSet DataSet { get { return DS; } }
            public static DataTable DataTable { get { return DT; } }

            public static void LoadData()
            {
                dbConnection.Open();
                DS.Clear();
                sqlAdapter.Fill(DS);
                DT = DS.Tables[0];
                dbConnection.Close();
            }

            public static void SaveData()
            {
                dbConnection.Open();
                sqlAdapter.Update(DT);
                DS.AcceptChanges();
                dbConnection.Close();
                LoadData();
            }

            /// <summary>
            /// Creates a new note in the database.
            /// Also stores the byte-array of the original note (by 'cropping' the original image to the coordinates saved in the note's properties).
            /// If a note already exists in the database with the same properties (not including Author and Type), it updates it with the new Author and Type.
            /// (In the past - to fix corrupted databases - AFTER replacing the pages to non-corrupted ones, the update feature would also update the image).            
            /// </summary>
            /// <param name="which">Which note to save</param>
            /// <param name="OriginalImage">The original image from which the Note was profiled</param>
            public static void AddNote(Note which, Mat OriginalImage)
            {
                Image original = Utilities.OriginalNote(OriginalImage, which);                
                int authorID = AuthorsAdapter.IDFromAuthor(which.Author);
                int notetypeID = NoteTypeAdapter.IDFromType(which.Type);
                int x = which.Rectangle.X;
                int y = which.Rectangle.Y;
                int width = which.Rectangle.Width;
                int height = which.Rectangle.Height;
                byte[] originalStream = Utilities.ImageToByteArray(original);
                string ImageHash = which.PageHash;

                DataRow row = FindNoteInDB(which);
                if (row == null) //New note!
                {
                    Console.Out.WriteLine("Inserting new data");
                    row = DT.NewRow();
                    row["Author"] = authorID;
                    row["Type"] = notetypeID;
                    row["Original"] = originalStream;
                    row["X"] = x;
                    row["Y"] = y;
                    row["Width"] = width;
                    row["Height"] = height;
                    row["PageHash"] = ImageHash;
                    DT.Rows.Add(row);
                }
                else
                {
                    if (authorID == -1 || notetypeID == -1)
                        Console.Out.WriteLine("Upading Data");
                    row["Author"] = authorID;
                    row["Type"] = notetypeID;
                }
            }

            /// <summary>
            /// Deletes a note from the database based on a given ID number.
            /// </summary>
            /// <param name="ID">The ID number to delete</param>
            public static void DeleteNote(int ID)
            {
                DataRow[] rows = DT.Select($"ID = '{ID}'");
                if (rows.Length <= 0)
                {
                    Console.WriteLine($"Note No. {ID} not found!");
                    return;
                }                
                else
                {
                    rows[0].Delete();
                }
            }

            /// <summary>
            /// Deletes a note from database.
            /// </summary>
            /// <param name="which">which note to delete</param>
            public static void DeleteNote(Note which)
            {
                DataRow row = FindNoteInDB(which);
                if (row == null)
                {
                    Console.WriteLine("Note doens't exist in database anyway.");
                }
                else
                {
                    row.Delete();
                }
            }

            /// <summary>
            /// Returns all the notes that correspond with the given page.
            /// Used to retrieve previously profiled notes.
            /// </summary>
            /// <param name="which">The page to scan for notes.</param>
            /// <returns></returns>
            public static List<Note> FindByPage(Page which)
            {
                List<Note> returnList = new List<Note>();
                DataRow[] drows = DT.Select($"PageHash = '{which.Hash}'");
                foreach (DataRow dr in drows)
                {
                    Note newNote = new Note(which.Hash);
                    int x = Convert.ToInt32(dr.Field<Int64>("X"));
                    int y = Convert.ToInt32(dr.Field<Int64>("Y"));
                    int width = Convert.ToInt32(dr.Field<Int64>("Width"));
                    int height = Convert.ToInt32(dr.Field<Int64>("Height"));
                    int AuthodID = Convert.ToInt32(dr.Field<Int64>("Author"));
                    int TypeID = Convert.ToInt32(dr.Field<Int64>("Type"));
                    Author author = AuthorsAdapter.AuthorFromID(AuthodID);
                    NoteType type = NoteTypeAdapter.TypeFromID(TypeID);
                    newNote.Rectangle = new Rectangle(x, y, width, height);
                    newNote.Type = type;
                    newNote.Author = author;
                    returnList.Add(newNote);
                }
                return returnList;
            }

            /// <summary>
            /// Returns ALL the Notes in the DB, but in the "DBNote" class which is used for DB Browsing GUI.
            /// </summary>
            /// <returns></returns>
            public static List<DBNote> GetAllInDB()
            {
                List<DBNote> returnList = new List<DBNote>();
                foreach (DataRow dr in DT.Rows)
                {
                    int id = Convert.ToInt32(dr.Field<Int64>("ID"));
                    int x = Convert.ToInt32(dr.Field<Int64>("X"));
                    int y = Convert.ToInt32(dr.Field<Int64>("Y"));
                    int width = Convert.ToInt32(dr.Field<Int64>("Width"));
                    int height = Convert.ToInt32(dr.Field<Int64>("Height"));
                    int AuthodID = Convert.ToInt32(dr.Field<Int64>("Author"));
                    int TypeID = Convert.ToInt32(dr.Field<Int64>("Type"));
                    Author author = AuthorsAdapter.AuthorFromID(AuthodID);
                    NoteType type = NoteTypeAdapter.TypeFromID(TypeID);
                    byte[] array = dr.Field<byte[]>("Original");
                    Image image = Utilities.ByteArrayToImage(array);
                    DBNote newNote = new DBNote(image);
                    newNote.Rectangle = new Rectangle(x, y, width, height);
                    newNote.Type = type;
                    newNote.Author = author;
                    newNote.ID = id;
                    returnList.Add(newNote);
                }
                return returnList;
            }

            /// <summary>
            /// Finds a note in the database by comparing the Pagehash (which page it is from) AND its coordinates.
            /// If a note in the database exists with the same hash AND coordinates - we assume it was already profiled and return it.
            /// </summary>
            /// <param name="which"></param>
            /// <returns></returns>
            private static DataRow FindNoteInDB(Note which)
            {
                int x = which.Rectangle.X;
                int y = which.Rectangle.Y;
                int width = which.Rectangle.Width;
                int height = which.Rectangle.Height;
                string hash = which.PageHash;
                DataRow[] dr = DT.Select($"PageHash = '{hash}' AND X = {x} AND Y = {y} AND Width = {width} AND Height = {height}");
                if (dr.Length <= 0)
                    return null;
                else
                    return dr[0];
            }
        }
    }

    /// <summary>
    /// An extension of the Note class, which also saves the image of the profiled note.
    /// </summary>
    public class DBNote : Note
    {
        public DBNote(Image image) : base()
        {
            Image = image;
        }

        public Image Image { get; set; }

        public override string ToString()
        {
            return $"{ID}. {Type.Description} by {Author.Name}";
        }
    }

    /// <summary>
    /// An extension of the Page class, which also saves the image of the entire page.
    /// </summary>
    public class DBPage
    {
        public DBPage(Image original, string name)
        {
            Image = original;
            Name = name;
        }

        public Image Image { get; }
        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}


