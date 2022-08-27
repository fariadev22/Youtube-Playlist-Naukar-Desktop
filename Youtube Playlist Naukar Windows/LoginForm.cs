using System;
using System.Threading;
using System.Windows.Forms;
using Youtube_Playlist_Naukar_Windows.Helpers;
using Youtube_Playlist_Naukar_Windows.Models;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class LoginForm : Form
    {
        public bool LoginSuccessful { get; set; }

        public UserSession ActiveUserSession { get; set; }

        private CancellationTokenSource _cancellationTokenSource;

        public LoginForm()
        {
            InitializeComponent();
            _cancellationTokenSource =
                new CancellationTokenSource();
        }

        private async void loginButton_Click(
            object sender, EventArgs e)
        {
            var loginResult = 
                await AccountHelper.GetAccountHelper
                    .TryOpenUserAccount(
                        _cancellationTokenSource.Token);

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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
        }
    }
}
