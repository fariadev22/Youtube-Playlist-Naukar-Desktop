using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Youtube_Playlist_Naukar_Windows.Helpers;
using Youtube_Playlist_Naukar_Windows.Models;
using Youtube_Playlist_Naukar_Windows.Utilities;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class HomePageForm : Form
    {
        public bool ReturnToLoginScreen { get; set; }

        private UserSession _activeUserSession;

        private string _youtubeChannelId;

        private Bitmap _defaultThumbnail;

        private Dictionary<string, UserPlayList> _userOwnedPlaylists;

        private Dictionary<string, UserPlayList> _userContributorPlaylists;

        private PlaylistHomePageForm _playlistForm;

        private CancellationTokenSource _cancellationTokenSource;

        public HomePageForm(
            UserSession activeUserSession)
        {
            InitializeComponent();

            _activeUserSession = activeUserSession;

            descriptionToolTip = new ToolTip();

            PlaylistHelper.UserOwnedPlaylistThumbnailReady +=
                UpdateUserOwnedPlaylistThumbnail;

            PlaylistHelper.UserContributorPlaylistThumbnailReady +=
                UpdateUserContributorPlaylistThumbnail;

            ConvertDefaultThumbnailToBitmap();
        }

        protected override async void OnLoad(EventArgs e)
        {
            _cancellationTokenSource =
                new CancellationTokenSource();

            //load user data
            LoadActiveUsersAccountInfo();

            //load playlists data when form starts loading
            await LoadUserPlaylists();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }

        #region Helpers

        private void LoadActiveUsersAccountInfo()
        {
            _youtubeChannelId =
                _activeUserSession.UserData?.ChannelId;

            email.Text = 
                _activeUserSession.EmailAddress ?? "";

            switchAccountMenuItem.DropDownItems.Clear();

            AccountHelper.GetAccountHelper
                .GetActiveAccountEmails()?
                .ForEach(
                    s => switchAccountMenuItem.DropDownItems.Add(s));
        }

        private async Task LoadUserPlaylists()
        {
            LoggerLabel.Text = @"Loading playlists....";

            await LoadUserOwnedPlaylists();

            await LoadUserContributorPlaylists();

            LoggerLabel.Text = "";
        }

        private async Task LoadUserOwnedPlaylists()
        {
            _userOwnedPlaylists =
                PlaylistHelper.GetPlaylistHelper.
                    GetStoredUserOwnedPlaylists() ?? 
                new Dictionary<string, UserPlayList>();

            var playlistsETagResult =
                await PlaylistHelper.GetPlaylistHelper.GetPlaylistsMetadata(
                    _activeUserSession.UserData?.UserOwnedPlaylistsETag,
                    _youtubeChannelId,
                    _cancellationTokenSource.Token);

            int? playlistsCount =
                playlistsETagResult.Item3;

            //if etag is not equal or does not exist we need to 
            //reload from scratch
            var reloadPlaylists = !playlistsETagResult.Item2;

            string newEtag = playlistsETagResult.Item1;

            if (reloadPlaylists)
            {
                ownerPlaylistsList.Items.Clear();
                ownerPlaylistsList.LargeImageList =
                    new ImageList
                    {
                        ImageSize = new Size(320, 180),
                        Images =
                        {
                            {"default", _defaultThumbnail}
                        }
                    };

                // Display the ProgressBar control.
                playlistsProgressBar.Visible = true;
                playlistsProgressBar.Minimum = 1;
                // Set Maximum to the total number of videos to load
                playlistsProgressBar.Maximum =
                    playlistsCount ?? 1;
                // Set the initial value of the ProgressBar.
                playlistsProgressBar.Value = 1;
                // Set the Step property to a value of 50 to represent
                // each chunk of videos being loaded
                playlistsProgressBar.Step = 1;

                var existingPlaylistsData = _userOwnedPlaylists;

                _userOwnedPlaylists =
                    new Dictionary<string, UserPlayList>();

                string nextPageToken = null;

                do
                {
                    var userPlaylistsVideosResult =
                        await PlaylistHelper.GetPlaylistHelper
                            .LoadUserOwnedPlaylists(_youtubeChannelId,
                                existingPlaylistsData,
                                _cancellationTokenSource.Token,
                                nextPageToken);

                    Dictionary<string, UserPlayList>
                        userPlaylists =
                            userPlaylistsVideosResult.Item1;

                    nextPageToken =
                        userPlaylistsVideosResult.Item2;

                    if (userPlaylists != null &&
                        userPlaylists.Count > 0)
                    {
                        foreach (var userPlaylist in userPlaylists)
                        {
                            _userOwnedPlaylists.Add(
                                userPlaylist.Key,
                                userPlaylist.Value);
                        }

                        if (!_cancellationTokenSource.IsCancellationRequested)
                        {
                            LoadOwnerPlaylistsUi(
                                userPlaylists.Values.ToList());
                        }
                    }

                    playlistsProgressBar.PerformStep();
                } while (!string.IsNullOrWhiteSpace(nextPageToken));

                playlistsProgressBar.Visible = false;

                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    PlaylistHelper.GetPlaylistHelper.SaveUserOwnedPlaylists(
                        _userOwnedPlaylists, newEtag);
                }
            }
            else if (!_cancellationTokenSource.IsCancellationRequested)
            {
                ownerPlaylistsList.Items.Clear();
                ownerPlaylistsList.LargeImageList =
                    new ImageList
                    {
                        ImageSize = new Size(320, 180),
                        Images =
                        {
                            {"default", _defaultThumbnail}
                        }
                    };
                
                playlistsProgressBar.Visible = false;
                LoadOwnerPlaylistsUi(
                    _userOwnedPlaylists.Values.ToList());
            }

            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                PlaylistHelper.GetPlaylistHelper
                    .DownloadUserOwnedPlaylistsThumbnails(
                        _userOwnedPlaylists);
            }
        }

        private async Task LoadUserContributorPlaylists()
        {
            //fetch data loaded from api and saved into session

            _userContributorPlaylists =
                PlaylistHelper.GetPlaylistHelper.GetStoredUserContributorPlaylists() ??
                new Dictionary<string, UserPlayList>();

            if (_userContributorPlaylists.Count > 0)
            {
                //load latest data for contributor playlists too

                await PlaylistHelper.GetPlaylistHelper.RefreshUserContributorPlaylists(
                    _userContributorPlaylists,
                    _cancellationTokenSource.Token);

                _userContributorPlaylists =
                    PlaylistHelper.GetPlaylistHelper.GetStoredUserContributorPlaylists();
            }

            if (_cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            LoadContributorPlaylistsUi();
        }

        private async Task RefreshPlaylists()
        {
            LoggerLabel.Text = @"Refreshing....";

            await LoadUserPlaylists();

            LoggerLabel.Text = "";

            MessageBox.Show(@"Playlists data has been refreshed.");
        }

        private void LoadOwnerPlaylistsUi(
            List<UserPlayList> currentUserOwnedPlaylists)
        {
            if (currentUserOwnedPlaylists == null)
            {
                return;
            }

            foreach (var userOwnedPlaylist in
                currentUserOwnedPlaylists)
            {
                ownerPlaylistsList.Items.Add(
                    userOwnedPlaylist.Id,
                    userOwnedPlaylist.Title,
                    "default");
            }
        }

        private void LoadContributorPlaylistsUi()
        {
            contributorPlaylistsList.Items.Clear();

            contributorPlaylistsList.LargeImageList
                = new ImageList
                {
                    ImageSize = new Size(320, 180)
                };
            
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

                PlaylistHelper.GetPlaylistHelper
                    .DownloadUserContributorPlaylistsThumbnails(
                        _userContributorPlaylists);
            }
        }

        private void ClearPlaylistPreviewDetails()
        {
            LoadPlaylistPreviewDetails(null);
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

            var description =
                selectedPlaylist?.Description ?? "-";

            var sanitizedDescription =
                Regex.Replace(description,
                    @"\t|\n|\r", "");

            descriptionValue.Text =
                sanitizedDescription.Length > 100
                    ? sanitizedDescription.Substring(0, 100) +
                      "..."
                    : sanitizedDescription;
            if (!string.IsNullOrWhiteSpace(description))
            {
                descriptionToolTip.SetToolTip(
                    descriptionValue, description);
            }

            if (selectedPlaylist?.Thumbnail == null ||
                selectedPlaylist.Thumbnail?.IsDownloaded == false)
            {
                playlistThumbnailPreview.ImageLocation =
                    @"default_image.png";
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

        private void OwnerPlaylistsList_SelectedIndexChanged(
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

        private void OwnerPlaylistsList_DoubleClicked(
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

                _playlistForm =
                    new PlaylistHomePageForm(
                        _userOwnedPlaylists[item.Name],
                        _activeUserSession);

                _playlistForm.Closed +=
                    ClosePlaylistForm;

                _playlistForm.Show();
            }
        }

        private void UpdateUserOwnedPlaylistThumbnail(
            object sender,
            UserOwnedPlaylistThumbnailReadyEventArgs eventArgs)
        {
            if (!ownerPlaylistsList.InvokeRequired)
            {
                return;
            }

            ownerPlaylistsList.Invoke(
                new MethodInvoker(delegate
                {
                    CommonUtilities.ConvertLocalImageToBitmapAndStoreInImageList(
                        _activeUserSession.UserDirectory + "/" +
                            eventArgs.PlaylistImagePathFromCustomerDirectory,
                        ownerPlaylistsList.LargeImageList,
                        eventArgs.PlaylistId);

                    int indexOfKey =
                        ownerPlaylistsList.Items.IndexOfKey(
                            eventArgs.PlaylistId);

                    if (indexOfKey >= 0)
                    {
                        ownerPlaylistsList.Items[indexOfKey].ImageKey =
                            eventArgs.PlaylistId;
                    }
                }));
        }

        private void ContributorPlaylistsList_SelectedIndexChanged(
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

        private void ContributorPlaylistsList_DoubleClicked(
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

                _playlistForm =
                    new PlaylistHomePageForm(
                        _userContributorPlaylists[item.Name],
                        _activeUserSession);

                _playlistForm.Closed +=
                    ClosePlaylistForm;

                _playlistForm.Show();
            }
        }

        private void UpdateUserContributorPlaylistThumbnail(
            object sender,
            UserContributorPlaylistThumbnailReadyEventArgs eventArgs)
        {
            if (!contributorPlaylistsList.InvokeRequired)
            {
                return;
            }

            contributorPlaylistsList.Invoke(
                new MethodInvoker(delegate
                {
                    CommonUtilities.ConvertLocalImageToBitmapAndStoreInImageList(
                        _activeUserSession.UserDirectory + "/" +
                            eventArgs.PlaylistImagePathFromCustomerDirectory,
                        contributorPlaylistsList.LargeImageList,
                        eventArgs.PlaylistId);

                    int indexOfKey =
                        contributorPlaylistsList.Items.IndexOfKey(
                            eventArgs.PlaylistId);

                    if (indexOfKey >= 0)
                    {
                        contributorPlaylistsList.Items[
                                indexOfKey].ImageKey =
                            eventArgs.PlaylistId;
                    }
                }));
        }

        private async void AddContributorPlaylistButton_Click(
            object sender, EventArgs e)
        {
            var urlInputForm = new AddPlaylistForm(
                _userContributorPlaylists);
            var dialogResult =
                urlInputForm.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                string urlInput =
                    urlInputForm.PlaylistUrlFromUser;

                CommonUtilities.TryGetPlaylistIdFromYoutubeUrl(
                        urlInput, out string playListId);

                var added =
                    await PlaylistHelper.
                        GetPlaylistHelper.AddContributorPlaylist(
                            playListId,
                            _cancellationTokenSource.Token);

                if (added)
                {
                    LoadContributorPlaylistsUi();

                    MessageBox.Show(@"Playlist added successfully.");
                }
                else if (!_cancellationTokenSource.
                    IsCancellationRequested)
                {
                    MessageBox.Show(@"An error occurred while trying to " +
                                    @"add the new playlist entry.");
                }
                
                urlInputForm.Dispose();
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                urlInputForm.Dispose();
            }
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
                        @"Are you sure you want to delete the selected " +
                        @"playlist entry from this application? ",
                        @"Confirm Deletion",
                        MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        PlaylistHelper.GetPlaylistHelper.
                            RemoveContributorPlaylistEntry(
                            item.Name);

                        contributorPlaylistsList.Items.RemoveByKey(
                            item.Name);

                        MessageBox.Show(@"Entry successfully removed.");
                    }
                    else
                    {
                        MessageBox.Show(@"Entry not removed.");
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    @"Select a contributor playlist to remove.");
            }
        }

        private async void RefreshPlaylistsMenuItem_Click(
            object sender, EventArgs e)
        {
            await RefreshPlaylists();
        }

        private async void RefreshPlaylistsButton_Click(
            object sender, EventArgs e)
        {
            await RefreshPlaylists();
        }

        private void ClosePlaylistForm(
            object sender, EventArgs eventArgs)
        {
            Show();
        }

        private async void ForgetCurrentAccountMenuItem_Click(
            object sender, EventArgs e)
        {
            var forgetAccountResult = 
                await AccountHelper.GetAccountHelper.
                    ForgetAndCloseAccount();

            if (forgetAccountResult.Item1)
            {
                MessageBox.Show(
                    @"Data associated with current " +
                    @"active account has been removed.",
                    @"Message",
                    MessageBoxButtons.OK);
                
                _activeUserSession = null;
                _youtubeChannelId = null;
                _userContributorPlaylists.Clear();
                _userOwnedPlaylists.Clear();

                ReturnToLoginScreen = true;
                Close();
            }
            else
            {
                MessageBox.Show(forgetAccountResult.Item2);
            }
        }

        private async void SwitchAccountWithSelectedEmail(
            object sender, ToolStripItemClickedEventArgs e)
        {
            string selectedEmail =
                e.ClickedItem.Text;

            if (selectedEmail == _activeUserSession.EmailAddress)
            {
                MessageBox.Show(@"Account already active.");
                return;
            }

            ClearPlaylistPreviewDetails();

            var changeAccountResult = 
                await AccountHelper.GetAccountHelper.ChangeAccount(
                    selectedEmail,
                    _cancellationTokenSource.Token);

            if (changeAccountResult.Item1)
            {
                _activeUserSession =
                    AccountHelper.GetAccountHelper.
                        GetActiveUserSession();

                LoadActiveUsersAccountInfo();

                await LoadUserPlaylists();

                MessageBox.Show(@"Account switched to " + selectedEmail);
            }
            else if(!_cancellationTokenSource.
                IsCancellationRequested)
            {
                MessageBox.Show(@"Unable to switch account to: " + 
                                selectedEmail);
            }
        }

        private async void AddNewAccountMenuItem_Click(
            object sender, EventArgs e)
        {
            var addAccountResult =
                await AccountHelper.GetAccountHelper.
                    ChangeAccount(
                        null,
                        _cancellationTokenSource.Token);

            if (addAccountResult.Item1)
            {
                _activeUserSession =
                    AccountHelper.GetAccountHelper.
                        GetActiveUserSession();

                LoadActiveUsersAccountInfo();

                await LoadUserPlaylists();

                MessageBox.Show(
                    @"Account with email " + email.Text +
                    @" added.");
            }
            else if (
                !_cancellationTokenSource.
                    IsCancellationRequested)
            {
                MessageBox.Show(
                    @"Unable to add account");
            }
        }

        private void UrlValue_LinkClicked(
            object sender, LinkLabelLinkClickedEventArgs e)
        {
            CommonUtilities.OpenLinkInBrowser(urlValue.Text);
        }

        private void OwnerValue_LinkClicked(
            object sender, LinkLabelLinkClickedEventArgs e)
        {
            CommonUtilities.OpenLinkInBrowser(
                ownerValue.Links[0].LinkData.ToString());
        }

        private void ViewDetailsMenuItem_Click(
            object sender, EventArgs e)
        {
            var detailsForm = new DetailsForm();
            detailsForm.Show(this);
        }

        #endregion
    }
}
