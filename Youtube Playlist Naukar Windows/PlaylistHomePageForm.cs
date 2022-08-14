using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Youtube_Playlist_Naukar_Windows.Helpers;
using Youtube_Playlist_Naukar_Windows.Models;
using Youtube_Playlist_Naukar_Windows.Utilities;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class PlaylistHomePageForm : Form
    {
        private UserPlayList _playlist;

        private UserSession _activeUserSession;

        private HomePageForm _mainMenu;

        public PlaylistHomePageForm(
            UserPlayList playlist,
            UserSession activeUserSession,
            HomePageForm mainMenu)
        {
            InitializeComponent();
            _playlist = playlist;
            _activeUserSession = activeUserSession;
            _mainMenu = mainMenu;
            Text = _playlist.Title;
        }

        public async Task LoadPlaylistVideos()
        {
            await PlaylistHelper.GetPlaylistHelper
                .LoadPlaylist(_playlist);

            LoadPlaylistVideosUI(
                _playlist.PlayListVideos.Values.ToList());
        }

        private void searchBar_TextChanged(
            object sender, EventArgs e)
        {
            //playlistVideoList.Items.Clear();

            var filteredVideos = 
                PlaylistHelper.GetPlaylistHelper
                .SearchVideoInPlayList(
                    searchBar.Text, _playlist);

            LoadPlaylistVideosUI(filteredVideos,
                false);
        }

        private void LoadPlaylistVideosUI(
            List<UserPlayListVideo> playlistVideos,
            bool downloadVideoImages = true)
        {
            //playlistVideoList.LargeImageList ??=
            //    new ImageList();

            //if (downloadVideoImages)
            //{
            //    CommonUtilities.DownloadImagesToUserDirectory(
            //        _activeUserSession.UserDirectory,
            //        playlistVideos
            //            .Where(v =>
            //                !playlistVideoList.Items.ContainsKey(
            //                    v.UniqueVideoIdInPlaylist))
            //            .Select(v =>
            //                new KeyValuePair<string, string>(
            //                    v.UniqueVideoIdInPlaylist,
            //                    v.ThumbnailUrl)
            //            ).ToList(),
            //        playlistVideoList.LargeImageList);

            //    playlistVideoList.LargeImageList.ImageSize
            //        = new Size(128, 128);
            //}

            //foreach (var userOwnedPlaylist in
            //    playlistVideos)
            //{
            //    playlistVideoList.Items.Add(
            //        userOwnedPlaylist.UniqueVideoIdInPlaylist,
            //        userOwnedPlaylist.Title,
            //        userOwnedPlaylist.UniqueVideoIdInPlaylist);
            //}

            //playlistVideoList.Anchor =
            //    AnchorStyles.Bottom;
        }

        private void returnToMainMenu_Click(object sender, EventArgs e)
        {
            Hide();
            _mainMenu.Show();
        }

        private async void addVideos_Click(object sender, EventArgs e)
        {
            var urlInputForm = new AddVideoForm();
            var dialogResult =
                urlInputForm.ShowDialog(this);

            if (dialogResult == DialogResult.OK)
            {
                string urlInput =
                    urlInputForm.VideoUrlOrUrls;

                if (string.IsNullOrWhiteSpace(urlInput))
                {
                    MessageBox.Show("No video URL provided.");
                }
                else
                {
                    var messages = 
                        await PlaylistHelper.GetPlaylistHelper
                            .AddVideoOrVideosToPlayList(
                                urlInput, _playlist);

                    new LogsForm(messages).Show(this);
                }

                urlInputForm.Dispose();
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                urlInputForm.Dispose();
            }

            LoadPlaylistVideosUI(
                _playlist.PlayListVideos.Values.ToList());
        }

        private async void refreshVideos_Click(object sender, EventArgs e)
        {
            await LoadPlaylistVideos();
        }

        private void findDuplicates_Click(object sender, EventArgs e)
        {
            var duplicates = 
                PlaylistHelper.GetPlaylistHelper.GetPlaylistDuplicates(
                    _playlist);

            if (duplicates.Count > 0)
            {
                MessageBox.Show(duplicates.Count + 
                                " duplicates found in playlist.");
                //playlistVideoList.Items.Clear();
                LoadPlaylistVideosUI(
                    duplicates.SelectMany(d => d).ToList(),
                    false);
            }
            else
            {
                MessageBox.Show("No duplicates in playlist found.");
            }
        }

        private async void deleteVideo_Click(object sender, EventArgs e)
        {
            //if (playlistVideoList.SelectedItems.Count > 0)
            //{
            //    ListViewItem item =
            //        playlistVideoList.SelectedItems[0];

            //    if (_playlist.PlayListVideos.ContainsKey(item.Name))
            //    {
            //        var result = MessageBox.Show(
            //            "Are you sure you want to delete the selected " +
            //                "playlist video from your YouTube account? " +
            //                "This action cannot be undone.",
            //            "Confirm Deletion",
            //            MessageBoxButtons.YesNo);

            //        if (result == DialogResult.Yes)
            //        {
            //            MessageLogger.Text = "Deleting video...";
            //            await PlaylistHelper.GetPlaylistHelper
            //                .DeleteVideoFromPlaylist(_playlist,
            //                    _playlist.PlayListVideos[item.Name]);

            //            playlistVideoList.Items.RemoveByKey(
            //                item.Name);

            //            MessageLogger.Text = "";
                            
            //            MessageBox.Show("Video successfully " +
            //                            "removed from your YouTube account.");
            //        }
            //        else
            //        {
            //            MessageBox.Show("Video not removed.");
            //        }
            //    }
            //}
            //else
            //{
            //    MessageBox.Show(
            //        "Select a video to remove.");
            //}
        }
    }
}
