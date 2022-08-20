using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

        private Bitmap _defaultThumbnail { get; set; }

        private Dictionary<string, UserPlayList> _userOwnedPlaylists;

        private Dictionary<string, UserPlayList> _userContributorPlaylists;

        public bool ReturnToLoginScreen { get; set; }

        public HomePageForm(
            UserSession activeUserSession)
        {
            InitializeComponent();

            _activeUserSession = activeUserSession;

            SessionManager.GetSessionManager.
                    PlaylistThumbnailUpdated +=
                updatePlaylistThumbnail;

            ConvertDefaultThumbnailToBitmap();
        }

        protected override async void OnLoad(EventArgs e)
        {
            //load user data
            await OpenUserAccount();

            //load playlists data when form starts loading
            await LoadUserPlaylists();
        }

        #region Helpers

        private async Task OpenUserAccount()
        {
            ApiClient.GetApiClient.Initialize(
                _activeUserSession);

            _youtubeChannelId =
                await ApiClient.GetApiClient.GetUserChannelId();

            SessionManager.GetSessionManager.
                SaveChannelIdInUserSession(
                    _youtubeChannelId);

            if (_activeUserSession.EmailAddress != null)
            {
                email.Text = _activeUserSession.EmailAddress;
            }

            switchAccountMenuItem.DropDownItems.Clear();

            SessionManager.GetSessionManager
                .GetUserSessionEmails()?
                .ForEach(
                    s => switchAccountMenuItem.DropDownItems.Add(s));
        }

        private async Task LoadUserPlaylists()
        {
            LoggerLabel.Text = "Loading playlists....";

            LoggerLabel.ForeColor = Color.DodgerBlue;

            //load user owned playlists data from api
            await PlaylistHelper.GetPlaylistHelper
                .LoadUserOwnedPlaylists(
                    _youtubeChannelId,
                    _userOwnedPlaylists,
                    _activeUserSession.UserData?.
                        UserOwnedPlaylistsETag);

            //fetch data loaded from api and saved into session
            var playlists =
                SessionManager.GetSessionManager.
                    GetUserSessionPlaylists();

            _userOwnedPlaylists =
                playlists.Item1 ??
                new Dictionary<string, UserPlayList>();

            _userContributorPlaylists =
                playlists.Item2 ??
                new Dictionary<string, UserPlayList>();

            if (_userContributorPlaylists.Count > 0)
            {
                //load latest data for contributor playlists too

                await PlaylistHelper.GetPlaylistHelper.
                    RefreshUserContributorPlaylists(
                        _userContributorPlaylists);

                _userContributorPlaylists =
                    SessionManager.GetSessionManager.
                        GetUserSessionPlaylists().Item2;
            }
            
            LoadOwnerPlaylistsUI();

            LoadContributorPlaylistsUI();

            DownloadPlaylistThumbnails();

            LoggerLabel.Text = "";
        }

        private void LoadOwnerPlaylistsUI()
        {
            ownerPlaylistsList.Items.Clear();

            ownerPlaylistsList.LargeImageList ??=
                new ImageList();

            ownerPlaylistsList.LargeImageList.ImageSize
                = new Size(320, 180);

            if (_userOwnedPlaylists != null)
            {
                ownerPlaylistsList.LargeImageList.Images
                    .Add("default",
                        _defaultThumbnail);

                foreach (var userOwnedPlaylist in
                    _userOwnedPlaylists)
                {
                   ownerPlaylistsList.Items.Add(
                        userOwnedPlaylist.Value.Id,
                        userOwnedPlaylist.Value.Title,
                        "default");
                }
            }
        }

        private void LoadContributorPlaylistsUI()
        {
            contributorPlaylistsList.Items.Clear();

            contributorPlaylistsList.LargeImageList
                ??= new ImageList();

            contributorPlaylistsList.LargeImageList.ImageSize
                = new Size(320, 180);

            if (_userContributorPlaylists != null)
            {
                contributorPlaylistsList.LargeImageList.Images
                    .Add("default",
                        _defaultThumbnail);

                foreach (var userContributorPlaylist in
                    _userContributorPlaylists)
                {
                    contributorPlaylistsList.Items.Add(
                        userContributorPlaylist.Value.Id,
                        userContributorPlaylist.Value.Title,
                        "default");
                }
            }
        }

        private void DownloadPlaylistThumbnails()
        {
            if (_userOwnedPlaylists.Count > 0)
            {
                SessionManager.GetSessionManager.
                    DownloadPlaylistThumbnailsInBackgroundAndInformUI(
                        _userOwnedPlaylists
                            .Values.ToList(), true);
            }

            if (_userContributorPlaylists.Count > 0)
            {
                SessionManager.GetSessionManager.
                    DownloadPlaylistThumbnailsInBackgroundAndInformUI(
                        _userContributorPlaylists
                            .Values.ToList(), false);
            }
        }

        private void LoadPlaylistPreviewDetails(
            UserPlayList selectedPlaylist)
        {
            titleValue.Text =
                GetPreviewItemValue(selectedPlaylist?.Title);

            var playlistUrl =
                selectedPlaylist?.Id == null
                ? ""
                : CommonUtilities.GetYoutubePlaylistUrlFromPlaylistId(
                    selectedPlaylist.Id);

            urlValue.Text = GetPreviewItemValue(playlistUrl);

            urlValue.Links.Clear();
            
            urlValue.Links.Add(0, urlValue.Text.Length,
                playlistUrl);

            totalVideosValue.Text =
                GetPreviewItemValue(
                    (selectedPlaylist?.TotalVideosInPlaylist)
                    .ToString());

            ownerValue.Text =
                GetPreviewItemValue(
                    selectedPlaylist?.PlaylistOwnerChannelTitle);

            ownerValue.Links.Clear();
            
            ownerValue.Links.Add(0, ownerValue.Text.Length,
                selectedPlaylist == null
                    ? string.Empty
                    : CommonUtilities.GetYoutubeChannelUrlFromChannelId(
                    selectedPlaylist.PlaylistOwnerChannelId));

            if (selectedPlaylist?.PublishedOn != null)
            {
                createdOnValue.Text =
                    GetPreviewItemValue(
                        selectedPlaylist.PublishedOn.Value
                            .ToString("dd/MM/yyyy HH:mm"));
            }
            else
            {
                createdOnValue.Text = @"-";
            }

            privacyStatusValue.Text =
                GetPreviewItemValue(
                    selectedPlaylist?.PrivacyStatus.ToString());

            descriptionValue.Text =
                GetPreviewItemValue(selectedPlaylist?.Description);

            if (selectedPlaylist?.Thumbnail == null ||
                selectedPlaylist.Thumbnail?.IsDownloaded == false)
            {
                playlistThumbnailPreview.ImageLocation =
                    "default_image.png";
            }
            else
            {
                playlistThumbnailPreview.ImageLocation =
                    _activeUserSession.UserDirectory +
                        selectedPlaylist.
                        Thumbnail.LocalPathFromUserDirectory;
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

        private async Task RefreshPlaylists()
        {
            LoggerLabel.Text = "Refreshing....";
            LoggerLabel.ForeColor = Color.DodgerBlue;

            await PlaylistHelper.GetPlaylistHelper
                .RefreshUserOwnedPlaylists(
                    _youtubeChannelId,
                    _userOwnedPlaylists,
                    _activeUserSession.UserData?.
                        UserOwnedPlaylistsETag);

            await PlaylistHelper.GetPlaylistHelper.
                RefreshUserContributorPlaylists(
                    _userContributorPlaylists);

            var playlistsResult =
                SessionManager.GetSessionManager.
                    GetUserSessionPlaylists();

            _userOwnedPlaylists =
                playlistsResult.Item1;

            _userContributorPlaylists =
                playlistsResult.Item2;

            LoadOwnerPlaylistsUI();

            LoadContributorPlaylistsUI();

            DownloadPlaylistThumbnails();

            LoggerLabel.Text = "";

            MessageBox.Show("Playlists data has been refreshed.");
        }

        private void OpenLinkInBrowser(
            string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(
                    url)
                {
                    UseShellExecute = true
                });
            }
            catch
            {
                //
            }
        }

        private void ConvertDefaultThumbnailToBitmap()
        {
            using (FileStream imageStream = new FileStream(
                "default_image.png", FileMode.Open))
            {
                _defaultThumbnail = new Bitmap(imageStream);
            }
        }

        #endregion

        #region Events

        private void updatePlaylistThumbnail(
            object sender,
            PlaylistThumbnailUpdatedEventArgs eventArgs)
        {
            var isUserOwnedPlaylist =
                eventArgs.IsOwnerPlaylist;

            if (isUserOwnedPlaylist)
            {
                if (ownerPlaylistsList.InvokeRequired)
                {
                    ownerPlaylistsList.Invoke(
                        new MethodInvoker(delegate
                        {
                            CommonUtilities.ConvertLocalImageToBitmap(
                                _activeUserSession.UserDirectory,
                                ownerPlaylistsList.LargeImageList,
                                eventArgs.PlaylistId,
                                eventArgs.PlaylistImagePathFromCustomerDirectory);

                            int indexOfKey =
                                ownerPlaylistsList.Items.IndexOfKey(
                                    eventArgs.PlaylistId);

                            ownerPlaylistsList.Items[indexOfKey].ImageKey =
                                eventArgs.PlaylistId;
                        }));
                }
            }
            else
            {
                if (contributorPlaylistsList.InvokeRequired)
                {
                    contributorPlaylistsList.Invoke(
                        new MethodInvoker(delegate
                        {
                            CommonUtilities.ConvertLocalImageToBitmap(
                                _activeUserSession.UserDirectory,
                                contributorPlaylistsList.LargeImageList,
                                eventArgs.PlaylistId,
                                eventArgs.PlaylistImagePathFromCustomerDirectory);

                            int indexOfKey =
                                contributorPlaylistsList.Items.IndexOfKey(
                                    eventArgs.PlaylistId);

                            contributorPlaylistsList.Items[
                                    indexOfKey].ImageKey =
                                eventArgs.PlaylistId;
                        }));
                }
            }
        }

        private void ownerPlaylistsList_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            UserPlayList selectedPlaylist = null;

            if (ownerPlaylistsList.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem =
                    ownerPlaylistsList.SelectedItems[0];

                if (_userOwnedPlaylists.ContainsKey(
                    selectedItem.Name))
                {
                    selectedPlaylist =
                        _userOwnedPlaylists[selectedItem.Name];
                }
            }

            LoadPlaylistPreviewDetails(selectedPlaylist);
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
                var loadingPage =
                    new LoadingForm();

                loadingPage.Show();

                var playlistForm =
                    new PlaylistHomePageForm(
                        _userOwnedPlaylists[item.Name],
                        _activeUserSession);

                await playlistForm.LoadPlaylistVideos();

                loadingPage.Dispose();
                
                playlistForm.Show();
            }
        }

        private void contributorPlaylistsList_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            UserPlayList selectedPlaylist = null;

            if (contributorPlaylistsList.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem =
                    contributorPlaylistsList.SelectedItems[0];

                if (_userContributorPlaylists.ContainsKey(
                    selectedItem.Name))
                {
                    selectedPlaylist =
                        _userContributorPlaylists[selectedItem.Name];
                }
            }

            LoadPlaylistPreviewDetails(selectedPlaylist);
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
                var loadingPage =
                    new LoadingForm();

                loadingPage.Show();

                var playlistForm =
                    new PlaylistHomePageForm(
                        _userContributorPlaylists[item.Name],
                        _activeUserSession);

                await playlistForm.LoadPlaylistVideos();

                loadingPage.Dispose();

                playlistForm.Show();
            }
        }

        private async void addContributorPlaylistButton_Click(
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

        private void removeContributorPlaylistButton_Click(
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

        private async void refreshPlaylistsMenuItem_Click(
            object sender, EventArgs e)
        {
            await RefreshPlaylists();
        }

        private async void refreshPlaylistsButton_Click(
            object sender, EventArgs e)
        {
            await RefreshPlaylists();
        }

        private async void forgetCurrentAccountMenuItem_Click(
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

                ReturnToLoginScreen = true;
                Close();
            }
            catch
            {
                MessageBox.Show(
                    "Unable to remove account details. " +
                    "Please try again.");
            }
        }

        private async void switchAccountWithSelectedEmail(
            object sender, ToolStripItemClickedEventArgs e)
        {
            string selectedEmail =
                e.ClickedItem.Text;

            _activeUserSession =
                await SessionManager.GetSessionManager
                    .ChangeSession(selectedEmail);

            await OpenUserAccount();

            await LoadUserPlaylists();

            MessageBox.Show("Account switched to " + selectedEmail);
        }

        private async void addNewAccountMenuItem_Click(
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
                else if (switchAccountMenuItem.DropDownItems.
                    ContainsKey(emailInput))
                {
                    MessageBox.Show("Account already added.");
                }
                else
                {
                    _activeUserSession =
                        await SessionManager.GetSessionManager
                            .ChangeSession(emailInput);

                    await OpenUserAccount();

                    await LoadUserPlaylists();

                    MessageBox.Show(
                        "Account with email " + 
                        email.Text + " added.");
                }

                emailInputForm.Dispose();
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                emailInputForm.Dispose();
            }
        }

        private void viewDetailsMenuItem_Click(
            object sender, EventArgs e)
        {
            var detailsForm = new DetailsForm();
            detailsForm.Show(this);
        }

        private void urlValue_LinkClicked(
            object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLinkInBrowser(urlValue.Text);
        }

        private void ownerValue_LinkClicked(
            object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLinkInBrowser(
                ownerValue.Links[0].LinkData.ToString());
        }

        #endregion
    }
}
