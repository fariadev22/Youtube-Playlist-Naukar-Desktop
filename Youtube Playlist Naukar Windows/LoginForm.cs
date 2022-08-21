using System;
using System.Windows.Forms;
using Youtube_Playlist_Naukar_Windows.Helpers;
using Youtube_Playlist_Naukar_Windows.Models;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class LoginForm : Form
    {
        public bool LoginSuccessful { get; set; }

        public UserSession ActiveUserSession { get; set; } 

        public LoginForm()
        {
            InitializeComponent();
        }

        private async void loginButton_Click(
            object sender, EventArgs e)
        {
            var loginResult = 
                await AccountHelper.GetAccountHelper
                    .TryOpenUserAccount();

            if (loginResult.Item1)
            {
                LoginSuccessful = true;

                ActiveUserSession =
                    AccountHelper.GetAccountHelper.
                        GetActiveUserSession();

                Close();
            }
            else
            {
                LoginSuccessful = false;
                MessageBox.Show(loginResult.Item2);
            }
        }
    }
}
