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
    /// A simple progress dialog
    /// </summary>
    public partial class frmProgress : Form
    {
        public frmProgress()
        {
            InitializeComponent();
        }

        public int Max
        {
            set { progressMain.Maximum = value; }
            get { return progressMain.Maximum; }
        }

        public int Progress
        {
            set { progressMain.Value = value; }
        }

        public string Message
        {
            set { lblStatus.Text = value;
                lblStatus.Update();
            }
        }
    }
}
