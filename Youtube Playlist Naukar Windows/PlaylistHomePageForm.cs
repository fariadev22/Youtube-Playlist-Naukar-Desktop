using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
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

        private CancellationTokenSource _cancellationTokenSource;

        private int timerTicks;
        
        private int waitUntil = 10;

        private const int videoThumbnailWidth = 150;

        private const int videoThumbnailHeight = 84;

        private List<DataRow> rows = 
            new List<DataRow>();

        public PlaylistHomePageForm(
            UserPlayList playlist,
            UserSession activeUserSession)
        {
            InitializeComponent();
            _playlist = playlist;
            _activeUserSession = activeUserSession;
            Text = _playlist.Title;

            PlaylistVideosHelper.VideoThumbnailReady +=
                UpdatePlaylistVideoThumbnail;
        }

        public async Task LoadPlaylistVideos(
            CancellationToken token)
        {
            await PlaylistVideosHelper.GetPlaylistVideosHelper
                .LoadPlaylist(_playlist, token);

            if (!token.IsCancellationRequested)
            {
                LoadPlaylistVideosUI(
                    _playlist.PlayListVideos.Values.ToList());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timerTicks++;

            if (timerTicks > waitUntil)
            {
                //Stop the timer and begin the search in a background thread.
                timer1.Stop();
                DisplaySearchResults();
            }
        }

        protected override async void OnLoad(EventArgs e)
        {
            if (_playlist != null)
            {
                MessageLogger.Text = "Loading videos...";
                _cancellationTokenSource =
                    new CancellationTokenSource();

                await LoadPlaylistVideos(
                    _cancellationTokenSource.Token);

                MessageLogger.Text = "";
            }
            else
            {
                Close();
            }
        }

        private void searchBar_TextChanged(
            object sender, EventArgs e)
        {
            if (!timer1.Enabled)
                timer1.Start();
            //Reset the timer when a character is entered
            timerTicks = 0;
        }

        private void DisplaySearchResults()
        {
            if (playlistVideosDataView.DataSource == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(
                searchBar.Text))
            {
                playlistVideosDataView.DataSource =
                    rows
                    .OrderBy(s =>
                        int.Parse(Convert.ToString(s["Position"]))
                    )
                    .CopyToDataTable();

                return;
            }

            var filteredVideos =
                PlaylistVideosHelper.GetPlaylistVideosHelper
                    .SearchVideoInPlayList(
                        searchBar.Text, _playlist)
                    .Select(f => f.UniqueVideoIdInPlaylist)
                    .ToList();

            var rowsFiltered =
                rows
                    .Where(r => filteredVideos.Contains(
                        Convert.ToString(r["VideoId"])));

            if (rowsFiltered.Any())
            {
                playlistVideosDataView.DataSource =
                    rowsFiltered
                        .OrderBy(s =>
                        filteredVideos.IndexOf(
                            Convert.ToString(s["VideoId"]))
                    )
                    .CopyToDataTable();
            }
            else
            {
                playlistVideosDataView.DataSource =
                    (playlistVideosDataView.DataSource as DataTable)?.Clone();
            }
        }

        private void LoadPlaylistVideosUI(
            List<UserPlayListVideo> playlistVideos,
            bool downloadVideoImages = true)
        {
            DataTable table = new DataTable();

            table.Columns.Add(
                new DataColumn("VideoId", typeof(string)));
            table.Columns.Add(
                new DataColumn("Position", typeof(string)));
            table.Columns.Add(
                new DataColumn("Preview", typeof(Image)));
            table.Columns.Add(
                new DataColumn("Title", typeof(string)));
            table.Columns.Add(
                new DataColumn("Duration", typeof(string)));
            table.Columns.Add(
                new DataColumn("Owner Name", typeof(string)));

            Bitmap defaultImage =
                new Bitmap(Image.FromFile("default_image.png"), 
                    videoThumbnailWidth, videoThumbnailHeight);

            if (playlistVideos != null &&
                playlistVideos.Count > 0)
            {
                foreach (var playlistVideo in 
                    playlistVideos)
                {
                    table.Rows.Add(
                        playlistVideo.UniqueVideoIdInPlaylist,
                        ((playlistVideo.PositionInPlayList ?? 0) + 1)
                            .ToString(),
                        defaultImage,
                        playlistVideo.Title,
                        playlistVideo.Duration,
                        playlistVideo.VideoOwnerChannelTitle);
                }
            }

            playlistVideosDataView.DataSource = table;
            playlistVideosDataView.Columns[0].Visible = false;
            playlistVideosDataView.Columns[1].AutoSizeMode =
                DataGridViewAutoSizeColumnMode.AllCells;
            playlistVideosDataView.Columns[2].AutoSizeMode =
                DataGridViewAutoSizeColumnMode.AllCells;
            playlistVideosDataView.Columns[3].AutoSizeMode =
                DataGridViewAutoSizeColumnMode.Fill;
            playlistVideosDataView.Columns[4].AutoSizeMode =
                DataGridViewAutoSizeColumnMode.AllCells;
            playlistVideosDataView.Columns[5].AutoSizeMode =
                DataGridViewAutoSizeColumnMode.AllCells;

            rows.Clear();
            rows.AddRange(table.Rows.Cast<DataRow>());

            if (downloadVideoImages)
            {
                PlaylistVideosHelper.GetPlaylistVideosHelper
                    .DownloadPlaylistVideoThumbnails(
                        _playlist.Id, playlistVideos);
            }
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
                        await PlaylistVideosHelper.
                            GetPlaylistVideosHelper
                                .AddVideoOrVideosToPlayList(
                                    urlInput, _playlist, 
                                    _cancellationTokenSource.Token);

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

        private async void refreshVideos_Click(
            object sender, EventArgs e)
        {
            await LoadPlaylistVideos(_cancellationTokenSource.Token);
        }

        private void findDuplicates_Click(object sender, EventArgs e)
        {
            var duplicates = 
                PlaylistVideosHelper.GetPlaylistVideosHelper
                    .GetPlaylistDuplicates(_playlist);

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
            if (playlistVideosDataView.SelectedRows.Count > 0)
            {
                var item =
                    playlistVideosDataView.SelectedRows[0];

                var videoPlaylistId =
                    item.Cells[0].Value.ToString() ?? "";

                if (_playlist.PlayListVideos.ContainsKey(
                        videoPlaylistId))
                {
                    var result = MessageBox.Show(
                        "Are you sure you want to delete the selected " +
                            "playlist video from your YouTube account? " +
                            "This action cannot be undone.",
                        "Confirm Deletion",
                        MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        MessageLogger.Text = "Deleting video...";
                        await PlaylistVideosHelper.GetPlaylistVideosHelper
                            .DeleteVideoFromPlaylist(_playlist,
                                _playlist.PlayListVideos[videoPlaylistId],
                                _cancellationTokenSource.Token);

                        playlistVideosDataView.Rows.Remove(item);

                        MessageLogger.Text = "";

                        MessageBox.Show("Video successfully " +
                                        "removed from your YouTube account.");
                    }
                    else
                    {
                        MessageBox.Show("Video not removed.");
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    "Select a video to remove.");
            }
        }

        private void UpdatePlaylistVideoThumbnail(
            object sender, VideoThumbnailReadyEventArgs eventArgs)
        {
            if (!playlistVideosDataView.InvokeRequired)
            {
                UpdatePlaylistThumbnailInDataView(
                    eventArgs);
                return;
            }

            playlistVideosDataView.Invoke(
                new MethodInvoker(delegate
                    {
                        UpdatePlaylistThumbnailInDataView(
                            eventArgs);
                    }));
        }

        private void UpdatePlaylistThumbnailInDataView(
            VideoThumbnailReadyEventArgs eventArgs)
        {
            //disposed
            if (_playlist == null ||
                _activeUserSession == null ||
                _playlist.Id != eventArgs.PlaylistId)
            {
                return;
            }

            var bitmap =
                CommonUtilities.ConvertLocalImageToBitmap(
                    _activeUserSession.UserDirectory,
                    eventArgs.PlaylistVideoImagePathFromCustomerDirectory,
                    videoThumbnailWidth,
                    videoThumbnailHeight
                );

            var filteredRow =
                rows.First(r => r["VideoId"].ToString() == 
                                eventArgs.VideoId);
            filteredRow["Preview"] = bitmap;

            DataGridViewRow row = playlistVideosDataView.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(r => r.Cells["VideoId"].Value.ToString() ==
                            eventArgs.VideoId);

            if (row != null)
            {
                row.SetValues(filteredRow.ItemArray);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var privateVideos = 
                PlaylistVideosHelper.GetPlaylistVideosHelper.GetPrivateVideos(
                    _playlist.PlayListVideos.Values.ToList());
        }

        private void returnHomeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            PlaylistBackgroundWorkerManager.GetBackgroundWorkerManager.
                CancelBackgroundWorkerForPlaylistId(_playlist.Id);
            _playlist = null;
            _activeUserSession = null;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = null;
        }
    }
}
