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
            try
            {
                var activeUserSession =
                    await SessionManager.GetSessionManager
                        .StartSession();

                if (activeUserSession == null)
                {
                    LoginSuccessful = false;
                    MessageBox.Show(@"Login failed.");
                }
                else
                {
                    ApiClient.GetApiClient
                        .Initialize(activeUserSession);

                    try
                    {
                        ActiveUserSession =
                            activeUserSession;

                        LoginSuccessful = true;
                        
                        Close();
                    }
                    catch
                    {
                        LoginSuccessful = false;
                        MessageBox.Show(
                            @"You do not have a YouTube Channel. " +
                            @"Please create a channel against your " +
                            @"YouTube account and then try again.");
                    }
                }
            }
            catch
            {
                LoginSuccessful = false;
                MessageBox.Show(@"Login failed.");
            }
            
        }
    }
}
