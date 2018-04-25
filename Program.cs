using System;
using System.Windows.Forms;

namespace Note_Profiler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frmMain mainForm = new frmMain();
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(mainForm.UnhandledThreadExceptionHandler); //Global error handler.
            Application.Run(mainForm);
        }
    }
}
