using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Youtube_Playlist_Naukar_Windows.Helpers;
using Youtube_Playlist_Naukar_Windows.Models;
using Youtube_Playlist_Naukar_Windows.Utilities;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class HomePageForm : Form
    {
        private UserSession _activeUserSession;

        private string _youtubeChannelId;

        private Dictionary<string, UserPlayList> _userOwnedPlaylists;

        private Dictionary<string, UserPlayList> _userContributorPlaylists;

        public HomePageForm(
            string youtubeChannelId,
            UserSession activeUserSession)
        {
            InitializeComponent();

            _activeUserSession = activeUserSession;

            _youtubeChannelId = youtubeChannelId;
            
            if (_activeUserSession.EmailAddress != null)
            {
                email.Text = _activeUserSession.EmailAddress;
            }

            SessionManager.GetSessionManager
                .GetUserSessionEmails()?
                .ForEach(
                        s => 
                        switchAccountMenuItem.DropDownItems.Add(s));

            var playlists = 
                SessionManager.GetSessionManager.
                    GetUserSessionPlaylists();

            _userOwnedPlaylists =
                playlists.Item1 ?? 
                new Dictionary<string, UserPlayList>();

            _userContributorPlaylists =
                playlists.Item2 ?? 
                new Dictionary<string, UserPlayList>();
        }

        public async Task LoadUserPlaylists()
        {
            LoggerLabel.Text = "Loading playlists....";
            LoggerLabel.ForeColor = Color.Orange;

            await PlaylistHelper.GetPlaylistHelper
                .LoadUserOwnedPlaylists(
                _youtubeChannelId,
                _userOwnedPlaylists,
                _activeUserSession.UserData?.UserOwnedPlaylistsETag);

            ownerPlaylistsList.Items.Clear();

            contributorPlaylistsList.Items.Clear();

            LoadOwnerPlaylistsUI();

            LoadContributorPlaylistsUI();

            LoggerLabel.Text = "";

            SessionManager.GetSessionManager.SaveSession();

            //playlistsTabs.Anchor =
            //    AnchorStyles.Bottom;
        }

        private void ownerPlaylistsList_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            playlistDetailsPanel.Visible = true;

            if (ownerPlaylistsList.SelectedItems.Count <= 0)
            {
                return;
            }

            ListViewItem selectedItem =
                ownerPlaylistsList.SelectedItems[0];

            if (_userOwnedPlaylists.ContainsKey(
                selectedItem.Name))
            {
                var selectedPlaylist =
                    _userOwnedPlaylists[selectedItem.Name];

                LoadPlaylistPreviewDetails(selectedPlaylist);
            }
        }

        private async void ownerPlaylistsList_DoubleClicked(
            object sender, EventArgs e)
        {
            if (ownerPlaylistsList.SelectedItems.Count <= 0)
            {
                return;
            }

            ListViewItem item =
                ownerPlaylistsList.SelectedItems[0];

            if (_userOwnedPlaylists.ContainsKey(item.Name))
            {
                Hide();

                var loadingPage =
                    new LoadingForm();

                loadingPage.Show();

                var playlistForm =
                    new PlaylistHomePageForm(
                        _userOwnedPlaylists[item.Name],
                        _activeUserSession,
                        this);

                await playlistForm.LoadPlaylistVideos();

                loadingPage.Dispose();
                
                playlistForm.Show();
            }
        }

        private void contributorPlaylistsList_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            playlistDetailsPanel.Visible = true;

            if (contributorPlaylistsList.SelectedItems.Count <= 0)
            {
                return;
            }

            ListViewItem selectedItem =
                contributorPlaylistsList.SelectedItems[0];

            if (_userContributorPlaylists.ContainsKey(
                selectedItem.Name))
            {
                var selectedPlaylist =
                    _userContributorPlaylists[selectedItem.Name];

                LoadPlaylistPreviewDetails(selectedPlaylist);
            }
        }

        private async void contributorPlaylistsList_DoubleClicked(
            object sender, EventArgs e)
        {
            if (contributorPlaylistsList.SelectedItems.Count <= 0)
            {
                return;
            }

            ListViewItem item =
                contributorPlaylistsList.SelectedItems[0];

            if (_userContributorPlaylists.ContainsKey(item.Name))
            {
                Hide();

                var loadingPage =
                    new LoadingForm();

                loadingPage.Show();

                var playlistForm =
                    new PlaylistHomePageForm(
                        _userContributorPlaylists[item.Name],
                        _activeUserSession,
                        this);

                await playlistForm.LoadPlaylistVideos();

                loadingPage.Dispose();

                playlistForm.Show();
            }
        }

        private void LoadPlaylistPreviewDetails(UserPlayList selectedPlaylist)
        {
            titleValue.Text =
                GetPreviewItemValue(selectedPlaylist.Title);
            var playlistUrl =
                CommonUtilities.GetYoutubePlaylistUrlFromPlaylistId(
                    selectedPlaylist.Id);
            urlValue.Text = GetPreviewItemValue(playlistUrl);
            urlValue.Links.Clear();
            urlValue.Links.Add(0, urlValue.Text.Length,
                playlistUrl);

            totalVideosValue.Text =
                GetPreviewItemValue(
                    selectedPlaylist.TotalVideosInPlaylist.ToString());
            ownerValue.Text =
                GetPreviewItemValue(
                    selectedPlaylist.PlaylistOwnerChannelTitle);
            ownerValue.Links.Clear();
            ownerValue.Links.Add(0, ownerValue.Text.Length,
                CommonUtilities.GetYoutubeChannelUrlFromChannelId(
                    selectedPlaylist.PlaylistOwnerChannelId));

            if (selectedPlaylist.PublishedOn != null)
            {
                createdOnValue.Text =
                    GetPreviewItemValue(
                        selectedPlaylist.PublishedOn.Value
                            .ToString("dd/MM/yyyy HH:mm"));
            }

            privacyStatusValue.Text =
                GetPreviewItemValue(
                    selectedPlaylist.PrivacyStatus.ToString());

            descriptionValue.Text =
                GetPreviewItemValue(selectedPlaylist.Description);

            if (string.IsNullOrWhiteSpace(
                selectedPlaylist.ThumbnailLocalPathFromUserDirectory))
            {
                playlistThumbnailPreview.ImageLocation =
                    "default_image.png";
            }
            else
            {
                playlistThumbnailPreview.ImageLocation =
                    _activeUserSession.UserDirectory +
                        selectedPlaylist.ThumbnailLocalPathFromUserDirectory;
            }
        }

        private string GetPreviewItemValue(
            string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "-";
            }

            return value;
        }

        private void RemoveContributorPlaylistButton_Click(
            object sender, EventArgs e)
        {
            if (contributorPlaylistsList.SelectedItems.Count > 0)
            {
                ListViewItem item =
                    contributorPlaylistsList.SelectedItems[0];

                if (_userContributorPlaylists.ContainsKey(item.Name))
                {
                    var result = MessageBox.Show(
                        "Are you sure you want to delete the selected " +
                        "playlist entry from this application? ",
                        "Confirm Deletion",
                        MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        PlaylistHelper.GetPlaylistHelper.
                            RemoveContributorPlaylistEntry(
                            item.Name);

                        contributorPlaylistsList.Items.RemoveByKey(
                            item.Name);

                        MessageBox.Show("Entry successfully removed.");

                        SessionManager.GetSessionManager
                            .SaveSession();
                    }
                    else
                    {
                        MessageBox.Show("Entry not removed.");
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    "Select a contributor playlist to remove.");
            }
        }

        private async void AddContributorPlaylistButton_Click(
            object sender, EventArgs e)
        {
            var urlInputForm = new AddPlaylistForm();
            var dialogResult = 
                urlInputForm.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                string urlInput = 
                    urlInputForm.PlaylistUrlFromUser;

                if (string.IsNullOrWhiteSpace(urlInput))
                {
                    MessageBox.Show("No URL provided.");
                }
                else
                {
                    bool isValidUrl =
                        CommonUtilities.TryGetPlaylistIdFromYoutubeUrl(
                            urlInput, out string playListId);

                    if (!isValidUrl)
                    {
                        MessageBox.Show("URL is invalid.");
                    }
                    else if (_userContributorPlaylists.ContainsKey(
                        playListId))
                    {
                        MessageBox.Show("Playlist already exists.");
                    }
                    else
                    {
                        await PlaylistHelper.
                            GetPlaylistHelper.AddContributorPlaylist(
                                playListId);

                        LoadContributorPlaylistsUI();

                        SessionManager.GetSessionManager
                            .SaveSession();

                        MessageBox.Show("Entry successfully added.");
                    }
                }

                urlInputForm.Dispose();
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                urlInputForm.Dispose();
            }
        }

        private void LoadOwnerPlaylistsUI()
        {
            ownerPlaylistsList.LargeImageList ??=
                new ImageList();

            CommonUtilities.ConvertLocalImagesToBitmapImageList(
                _activeUserSession.UserDirectory,
                _userOwnedPlaylists.Values.Select(v =>
                    new KeyValuePair<string, string>(
                        v.Id,
                        v.ThumbnailLocalPathFromUserDirectory)
                ).ToList(),
                ownerPlaylistsList.LargeImageList);

            ownerPlaylistsList.LargeImageList.ImageSize
                = new Size(320, 180);

            foreach (var userOwnedPlaylist in
                _userOwnedPlaylists)
            {
                ownerPlaylistsList.Items.Add(
                    userOwnedPlaylist.Value.Id,
                    userOwnedPlaylist.Value.Title,
                    userOwnedPlaylist.Value.Id);
            }
        }

        private void LoadContributorPlaylistsUI()
        {
            contributorPlaylistsList.LargeImageList
                ??= new ImageList();

            CommonUtilities.ConvertLocalImagesToBitmapImageList(
                _activeUserSession.UserDirectory,
                _userContributorPlaylists.Values.Select(v =>
                    new KeyValuePair<string, string>(
                        v.Id,
                        v.ThumbnailLocalPathFromUserDirectory)
                ).ToList(),
                contributorPlaylistsList.LargeImageList);

            contributorPlaylistsList.LargeImageList.ImageSize
                = new Size(320, 180);

            foreach (var userContributorPlaylist in
                _userContributorPlaylists)
            {
                contributorPlaylistsList.Items.Add(
                    userContributorPlaylist.Value.Id,
                    userContributorPlaylist.Value.Title,
                    userContributorPlaylist.Value.Id);
            }
        }

        private async void RefreshPlaylists_Click(
            object sender, EventArgs e)
        {
            await RefreshPlaylists();
        }

        private async void RefreshPlaylistsButton_Click(
            object sender, EventArgs e)
        {
            await RefreshPlaylists();
        }

        private async Task RefreshPlaylists()
        {
            LoggerLabel.Text = "Refreshing....";
            LoggerLabel.ForeColor = Color.Orange;

            await PlaylistHelper.GetPlaylistHelper
                .RefreshUserOwnedPlaylists(
                    _youtubeChannelId,
                    _userOwnedPlaylists,
                    _activeUserSession.UserData?.UserOwnedPlaylistsETag);

            _userOwnedPlaylists =
                SessionManager.GetSessionManager
                    .GetUserSessionPlaylists().Item1;

            ownerPlaylistsList.Items.Clear();
            LoadOwnerPlaylistsUI();

            LoggerLabel.Text = "";

            MessageBox.Show("Playlists data has been refreshed.");

            SessionManager.GetSessionManager
                .SaveSession();
        }

        private async void ForgetCurrentAccount_Click(
            object sender, EventArgs e)
        {
            try
            {
                await SessionManager.GetSessionManager
                    .ForgetCurrentUser();

                MessageBox.Show(
                    "Data associated with current " +
                    "active account has been removed.",
                    "Message",
                    MessageBoxButtons.OK);

                Hide();
                SessionManager.GetSessionManager
                    .SaveSession();

                var loginForm = new LoginForm();
                loginForm.Show();
            }
            catch
            {
                MessageBox.Show(
                    "Unable to remove account details. " +
                    "Please try again.");
            }
        }

        private async void SwitchToAccountWithSelectedEmail(
            object sender, ToolStripItemClickedEventArgs e)
        {
            string selectedEmail =
                e.ClickedItem.Text;

            _activeUserSession =
                await SessionManager.GetSessionManager
                    .ChangeSession(selectedEmail);

            await ChangeAccount();

            SessionManager.GetSessionManager
                .SaveSession();

            _userOwnedPlaylists =
                _activeUserSession.UserData?.UserOwnedPlayLists;

            _userContributorPlaylists =
                _activeUserSession.UserData?.UserContributorPlayLists;

            await LoadUserPlaylists();

            MessageBox.Show("Account switched to " + selectedEmail);
        }

        private async void AddNewAccountMenuItem_Click(
            object sender, EventArgs e)
        {
            var emailInputForm = new AddAccountForm();
            var dialogResult =
                emailInputForm.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                string emailInput =
                    emailInputForm.UserEmailAddress;

                if (string.IsNullOrWhiteSpace(emailInput))
                {
                    MessageBox.Show("No email provided.");
                }
                else if (switchAccountMenuItem.DropDownItems.ContainsKey(emailInput))
                {
                    MessageBox.Show("Account already added.");
                }
                else
                {
                    _activeUserSession =
                        await SessionManager.GetSessionManager
                            .ChangeSession(emailInput);

                    await ChangeAccount();

                    _userOwnedPlaylists =
                        _activeUserSession.UserData?.UserOwnedPlayLists;

                    _userContributorPlaylists =
                        _activeUserSession.UserData?.UserContributorPlayLists;

                    await LoadUserPlaylists();

                    MessageBox.Show(
                        "Account with email " +
                        emailInput + " added.");

                    SessionManager.GetSessionManager
                        .SaveSession();
                }

                emailInputForm.Dispose();
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                emailInputForm.Dispose();
            }
        }

        private async Task ChangeAccount()
        {
            ApiClient.GetApiClient.Initialize(
                _activeUserSession);

            _youtubeChannelId =
                await ApiClient.GetApiClient.GetUserChannelId();

            if (_activeUserSession.EmailAddress != null)
            {
                email.Text = _activeUserSession.EmailAddress;
            }

            switchAccountMenuItem.DropDownItems.Clear();
            SessionManager.GetSessionManager
                .GetUserSessionEmails()?
                .ForEach(
                    s => switchAccountMenuItem.DropDownItems.Add(s));

            await LoadUserPlaylists();
        }

        private void ViewDetails_Click(object sender, EventArgs e)
        {
            var detailsForm = new DetailsForm();
            detailsForm.Show(this);
        }

        private void urlValue_LinkClicked(
            object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(urlValue.Text)
            {
                UseShellExecute = true
            });
        }

        private void ownerValue_LinkClicked(
            object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(
                ownerValue.Links[0].LinkData.ToString())
            {
                UseShellExecute = true
            });
        }
    }
}
