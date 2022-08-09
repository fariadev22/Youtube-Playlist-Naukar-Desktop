using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Youtube_Playlist_Naukar_Windows
{
    static class Program
    {
        private const string ApplicationName = "PlaylistManager";

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
            Application.Exit();
        }
    }
}
