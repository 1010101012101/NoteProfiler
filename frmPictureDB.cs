using Manina.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Note_Profiler
{
    /// <summary>
    /// A simple database viewer for Notes and Pages
    /// </summary>
    public partial class frmPictureDB : Form
    {
        DBAdapter db;
        BindingList<DBPage> pagesList;
        BindingList<DBNote> noteList;        

        public frmPictureDB()
        {
            InitializeComponent();
            db = new DBAdapter();
            pagesList = new BindingList<DBPage>(db.PageList());
            noteList = new BindingList<DBNote>(db.NotesInDB());
            foreach (DBNote note in noteList)
            {
                notesView.Items.Add(note, note.ToString(), note.Image); //Populates the image list. Note that if you do this twice, for some reason - it doesn't work.
            }
            foreach (DBPage page in pagesList)
            {             
                pagesView.Items.Add(page, page.ToString(), page.Image); //Populates the image list. Note that if you do this twice, for some reason - it doesn't work.
            }
        }        
            
        /// <summary>
        /// Deletes the selected image(s) from the database.
        /// Doesn't really reload the view - just deletes the items from the list as well.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult yesNo = MessageBox.Show(this, "Are you sure you want to delete this page from the database?\nThis action is non-reversible!", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            foreach (ImageListViewItem item in notesView.SelectedItems)
            {
                int id = ((DBNote)item.VirtualItemKey).ID;
                db.DeleteNote(id);
                notesView.Items.Remove(item);
                db.SaveAll();                                
            }
            
        }
    }    
}
