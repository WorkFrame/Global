using System;
using System.Windows.Forms;

namespace NetEti.DemoApplications
{
    static class Program
    {
        /// <summary>
        /// <para xml:lang="de">
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </para>
        /// <para xml:lang="en">
        /// Main application-entrypoint.
        /// </para>
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
