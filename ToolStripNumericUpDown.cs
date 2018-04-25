using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Note_Profiler
{

    /// <summary>
    /// A custom class that allows placing UpDown controllers in the toolstrip
    /// </summary>

    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]    
    public partial class ToolStripNumericUpDown : ToolStripControlHost
    {
        public ToolStripNumericUpDown() : base(new NumericUpDown())
        {
            InitializeComponent();
        }

        public NumericUpDown NumericUpDownControl
        {         
            get
            {
                return Control as NumericUpDown;
            }
        }

        public decimal Value
        {
            get
            {
                return NumericUpDownControl.Value;
            }
            set
            {
                NumericUpDownControl.Value = value;
            }
        }

        public decimal Maximum
        {
            get
            {
                return NumericUpDownControl.Maximum;
            }
            set
            {
                NumericUpDownControl.Maximum = value;
            }
        }

        public decimal Minimum
        {
            get
            {
                return NumericUpDownControl.Minimum;
            }
            set
            {
                NumericUpDownControl.Minimum = value;
            }
        }

        public decimal Increment
        {
            get
            {
                return NumericUpDownControl.Increment;
            }
            set
            {
                NumericUpDownControl.Increment = value;
            }
        }
    }
}
