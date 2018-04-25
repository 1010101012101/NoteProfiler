using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Note_Profiler
{
    /// <summary>
    /// An editor for basic database tables such as "Note Type" and "Author".
    /// </summary>
    public partial class frmDBEditor : Form
    {
        DBAdapter db;
        DataGridViewCellStyle cellStyleModified = new DataGridViewCellStyle();       
        DataGridViewCellStyle cellStyleNew = new DataGridViewCellStyle();
        public enum EditType { Author, NoteType };
        EditType DBType;

        /// <summary>
        /// Init the interface.
        /// </summary>
        /// <param name="dbAdapter">The DB adapter to connect to (passed from the main form).</param>
        /// <param name="type">The type of DB to display (Author or NoteType)</param>
        public frmDBEditor(DBAdapter dbAdapter, EditType type)
        {
            InitializeComponent();
            db = dbAdapter;
            DBType = type;
            cellStyleModified.Font = new Font(gridviewDB.Font, FontStyle.Bold);
            cellStyleModified.ForeColor = Color.Blue;            
            cellStyleNew.Font = new Font(gridviewDB.Font, FontStyle.Bold);
            cellStyleNew.ForeColor = Color.Green;            
        }

        /// <summary>
        /// Loads the appropriate database and sets the correct labels.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmManageAuthors_Load(object sender, EventArgs e)
        {
            switch (DBType)
            {
                case EditType.Author:
                    gridviewDB.DataSource = db.AuthorsTable;
                    Text = "Manage Authors";
                    grpDataGridView.Text = "Authors Database:";                    
                    break;
                case EditType.NoteType:
                    gridviewDB.DataSource = db.NoteTypeTable;                    
                    Text = "Manage Note Types";
                    grpDataGridView.Text = "Note Type Database:";
                    break;
            }            
            InvalidateGridview();
            chkID.Checked = Properties.Settings.Default.ShowID;
            chkSort.Checked = Properties.Settings.Default.ShowSort;
        }

        #region Interface

        #region Buttons
        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            MoveRows(true);
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            MoveRows(false);
        }

        /// <summary>
        /// Revert the DB to the last saved version.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevert_Click(object sender, EventArgs e)
        {
            DialogResult yesno = MessageBox.Show(this, "This will undo all your changes!\nAre you sure?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (yesno == DialogResult.Yes)
            {
                db.ReloadAll();
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        #endregion

        #region GridView

        /// <summary>
        /// Enables or Disables the "up/down" buttons based on how many items are selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridviewDB_SelectionChanged(object sender, EventArgs e)
        {
            if (gridviewDB.SelectedRows.Count > 0)
            {
                btnMoveDown.Enabled = gridviewDB.SelectedRows[gridviewDB.SelectedRows.Count - 1].Index < gridviewDB.RowCount - 2;
                btnMoveUp.Enabled = gridviewDB.SelectedRows[gridviewDB.SelectedRows.Count - 1].Index > 0;
            }
            else
            {
                btnMoveDown.Enabled = false;
                btnMoveUp.Enabled = false;
            }
        }

        /// <summary>
        /// Formats the DB table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridviewDB_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            gridviewDB.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            gridviewDB.Columns[0].ReadOnly = true;
            gridviewDB.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        /// <summary>
        /// Fills the ID and Sort field automatically when creating a new row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridviewDB_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == gridviewDB.NewRowIndex)
            {                
                int nextID = DBType == EditType.Author ? db.NextAuthor : db.NextNoteType;
                if (nextID == -1)
                    nextID = gridviewDB.Rows.Count;
                gridviewDB.Rows[e.RowIndex].Cells[0].Value = nextID;
                if (DBType == EditType.NoteType)
                {
                    gridviewDB.Rows[e.RowIndex].Cells["Sort"].Value = nextID;
                }
            }
        }

        private void gridviewDB_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.DefaultCellStyle = cellStyleModified;
        }

        private void gridviewDB_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            gridviewDB.Rows[e.RowIndex].DefaultCellStyle = cellStyleModified;
        }


        #endregion

        #region Checkboxes
        /// <summary>
        /// Save checkbox states to the settings for cross-session consistency.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkSort_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowSort = chkSort.Checked;
            Properties.Settings.Default.Save();
            InvalidateGridview();
        }

        /// <summary>
        /// Save checkbox states to the settings for cross-session consistency.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkID_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowID = chkID.Checked;
            Properties.Settings.Default.Save();
            InvalidateGridview();
        }


        #endregion

        /// <summary>
        /// Loads the Sort and ID fields based on the settings.
        /// </summary>
        private void InvalidateGridview()
        {
            gridviewDB.Columns["Id"].Visible = Properties.Settings.Default.ShowID;
            gridviewDB.Columns["Sort"].Visible = Properties.Settings.Default.ShowSort;            
        }

        #endregion
        private void SaveChanges()
        {
            DialogResult yesno = MessageBox.Show(this, "Once saved, changes can not be undone!\nAre you sure?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (yesno == DialogResult.Yes)
            {
                db.SaveAll();
                db.ReloadAll();
            }
            ((frmMain)Owner).InvalidateDatabase();
        }

        /// <summary>
        /// Moves rows (changes the Sort values)
        /// </summary>
        /// <param name="up">True if up was clicked, false if down.</param>
        private void MoveRows(bool up)
        {
            if (gridviewDB.SelectedRows.Count > 0)
            {
                List<int> indices = new List<int>();
                List<int> IDs = new List<int>();
                foreach (DataGridViewRow row in gridviewDB.SelectedRows)
                {
                    indices.Add(row.Index);
                }
                indices.Sort();
                if (!up)
                    indices.Reverse();
                for (int i = 0; i < indices.Count; i++)
                {
                    int first = Convert.ToInt32(gridviewDB.Rows[indices[i]].Cells["ID"].Value);
                    IDs.Add(first);
                    int second = Convert.ToInt32(gridviewDB.Rows[indices[i] + (up ? -1 : 1)].Cells["ID"].Value);
                    if (DBType == EditType.NoteType)
                        db.SwapNoteTyes(first, second);
                    else
                        db.SwapAuthorType(first, second);
                }
                db.SaveAll();
                db.ReloadAll();
                gridviewDB.ClearSelection();
                gridviewDB.CurrentCell = null;
                foreach (int id in IDs)
                {
                    DataGridViewRow row = gridviewDB.Rows.Cast<DataGridViewRow>().Where(r => Convert.ToInt32(r.Cells["ID"].Value) == id).FirstOrDefault();
                    if (row != null)
                    {
                        row.Selected = true;
                    }
                }
            }
        }
    }
}
