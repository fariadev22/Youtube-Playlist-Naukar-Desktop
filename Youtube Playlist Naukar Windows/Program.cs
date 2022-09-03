using System;
using System.Windows.Forms;
using Youtube_Playlist_Naukar_Windows.Helpers.BackgroundWorkers;

namespace Youtube_Playlist_Naukar_Windows
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RunApplication();
        }

        private static void RunApplication()
        {
            var loginForm = new LoginForm();
            Application.Run(loginForm);

            if (loginForm.LoginSuccessful)
            {
                var homeForm = new HomePageForm(
                    loginForm.ActiveUserSession);

                Application.Run(homeForm);

                if (homeForm.ReturnToLoginScreen)
                {
                    RunApplication();
                }
            }

            //cancel background tasks and wait for them to end
            BackgroundWorkersManager.GetBackgroundWorkerManager
                .CancelAllBackgroundWork();

            Application.Exit();
        }
    }
}
