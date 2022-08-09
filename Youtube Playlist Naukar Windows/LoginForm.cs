using System;
using System.Windows.Forms;
using Youtube_Playlist_Naukar_Windows.Helpers;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class LoginForm : Form
    {
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
                    MessageBox.Show("Login failed");
                }
                else
                {
                    ApiClient.GetApiClient
                        .Initialize(activeUserSession);

                    try
                    {
                        string channelId =
                            await ApiClient.GetApiClient.
                                GetUserChannelId();
                        
                        SessionManager.GetSessionManager.
                            SaveChannelIdInUserSession(
                                channelId);

                        Hide();

                        var loadingPage =
                            new LoadingForm();

                        loadingPage.Show();

                        var homePage = new HomePageForm(
                            channelId,
                            activeUserSession);

                        await homePage.LoadUserPlaylists();

                        loadingPage.Dispose();

                        homePage.Show();
                    }
                    catch
                    {
                        MessageBox.Show(
                            "You do not have a YouTube Channel. " +
                            "Please create a channel against your " +
                            "YouTube account and then try again.");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Login failed");
            }
            
        }
    }
}
