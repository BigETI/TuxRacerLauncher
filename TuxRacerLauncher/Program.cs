using System;
using System.IO;
using System.Windows.Forms;

namespace TuxRacerLauncher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (File.Exists("./tuxracer.exe"))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormLauncher());
            }
            else
            {
                MessageBox.Show("Tux Racer is not installed in the current working directory.", "Missing Tux Racer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
