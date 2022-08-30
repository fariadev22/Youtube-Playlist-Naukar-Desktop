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

        private SortedDictionary<long, DataRow> rows =
            new SortedDictionary<long, DataRow>();

        private Bitmap _defaultImage;

        private DataTable _dataTable;

        public PlaylistHomePageForm(
            UserPlayList playlist,
            UserSession activeUserSession)
        {
            InitializeComponent();
            _playlist = playlist;
            _activeUserSession = activeUserSession;
            Text = _playlist.Title;
            toolTip1 = new ToolTip();
            MessageLogger.ForeColor = Color.DodgerBlue;
            _defaultImage =
                new Bitmap(Image.FromFile("default_image.png"),
                    videoThumbnailWidth, videoThumbnailHeight);

            PlaylistVideosHelper.VideoThumbnailReady +=
                UpdatePlaylistVideoThumbnail;
        }

        public async Task LoadPlaylistVideos(
            CancellationToken token)
        {
            MessageLogger.Text = "Loading videos...";

            addVideosButton.Enabled = false;
            refreshVideosButton.Enabled = false;
            deleteVideoButton.Enabled = false;
            filterBox.Enabled = false;
            searchBar.Enabled = false;

            await PlaylistVideosHelper.GetPlaylistVideosHelper
                .LoadPlaylistVideos(_playlist, token);

            if (!token.IsCancellationRequested)
            {
                LoadPlaylistVideosUI(
                    _playlist.PlayListVideos.Values.ToList());
            }

            UpdateTotalVideosCount();

            addVideosButton.Enabled = true;
            refreshVideosButton.Enabled = true;
            deleteVideoButton.Enabled = true;
            filterBox.Enabled = true;
            searchBar.Enabled = true;

            MessageLogger.Text = "";
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
               
                _cancellationTokenSource =
                    new CancellationTokenSource();

                await LoadPlaylistVideos(
                    _cancellationTokenSource.Token);

                playlistNameValue.Text =
                    _playlist?.Title ?? "-";
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
                    rows.Values.CopyToDataTable();

                return;
            }

            var filteredVideos =
                PlaylistVideosHelper.GetPlaylistVideosHelper
                    .SearchVideoInPlayList(
                        searchBar.Text, _playlist)
                    .Select(f => f.UniqueVideoIdInPlaylist)
                    .ToList();

            var rowsFiltered =
                rows.Values
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

            _dataTable = table;

            if (playlistVideos != null &&
                playlistVideos.Count > 0)
            {
                foreach (var playlistVideo in 
                    playlistVideos)
                {
                    table.Rows.Add(
                        GetRowFromRowValues(playlistVideo));
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
            var tableRows =
                table.Rows.Cast<DataRow>();

            foreach (var tableRow in tableRows)
            {
                var position =
                    long.Parse(tableRow["Position"].ToString());
                rows.Add(position, tableRow);
            }

            if (downloadVideoImages)
            {
                PlaylistVideosHelper.GetPlaylistVideosHelper
                    .DownloadPlaylistVideoThumbnails(
                        _playlist.Id, playlistVideos);
            }
        }

        private object[] GetRowFromRowValues(
            UserPlayListVideo playlistVideo)
        {
            object[] rowValues =
            {
                playlistVideo.UniqueVideoIdInPlaylist,
                ((playlistVideo.PositionInPlayList ?? 0) + 1)
                .ToString(),
                _defaultImage,
                playlistVideo.Title,
                playlistVideo.Duration,
                playlistVideo.VideoOwnerChannelTitle
            };
            return rowValues;
        }

        private async void addVideos_Click(object sender, EventArgs e)
        {
            var urlInputForm = new AddVideoForm();
            var dialogResult =
                urlInputForm.ShowDialog(this);
            List<UserPlayListVideo> newVideos
                = new List<UserPlayListVideo>();

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
                    var logsForm = new LogsForm();
                    logsForm.Show();

                    List<string> urls =
                        urlInput.Split('\n').
                            Select(url => url.Trim()).ToList();

                    foreach (var url in urls)
                    {
                        logsForm.AddRow(url, "In Progress");

                        var videoAdditionResult =
                            await PlaylistVideosHelper.GetPlaylistVideosHelper.
                                AddVideoToPlayList(
                                    url, _playlist, _cancellationTokenSource.Token);

                        var message = videoAdditionResult.Item1;
                        var videoAdded = videoAdditionResult.Item2;

                        if (videoAdded != null)
                        {
                            newVideos.Add(videoAdded);
                        }

                        logsForm.UpdateRow(url, message);
                    }
                }

                urlInputForm.Dispose();
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                urlInputForm.Dispose();
            }

            if(!_cancellationTokenSource.IsCancellationRequested)
            {
                var newRows =
                    new SortedDictionary<long, DataRow>();

                foreach (DataRow row in rows.Values)
                {
                    var id = row["VideoId"].ToString();
                    if (_playlist.PlayListVideos.ContainsKey(id))
                    {
                        row["Position"] =
                            (_playlist.PlayListVideos[id].
                                PositionInPlayList + 1).ToString();
                    }
                }

                foreach (DataRow row in rows.Values)
                {
                    newRows.Add(long.Parse(row["Position"].ToString()),
                        row);
                }

                rows = newRows;

                foreach (var playlistVideo in newVideos)
                {
                    var newRow = _dataTable.NewRow();
                    newRow.ItemArray = GetRowFromRowValues(
                        playlistVideo);
                    rows.Add((playlistVideo.PositionInPlayList ?? 0) + 1, 
                        newRow);
                }

                playlistVideosDataView.DataSource =
                    rows.Values.CopyToDataTable();

                PlaylistVideosHelper.GetPlaylistVideosHelper
                    .DownloadPlaylistVideoThumbnails(
                        _playlist.Id, newVideos);
            }

            UpdateTotalVideosCount();
        }

        private async void refreshVideos_Click(
            object sender, EventArgs e)
        {
            await LoadPlaylistVideos(_cancellationTokenSource.Token);
        }

        private void UpdateTotalVideosCount()
        {
            totalVideosValue.Text =
                (_playlist?.PlayListVideos?.Count ?? 0).ToString();
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

                        var videoToRemove =
                            _playlist.PlayListVideos[videoPlaylistId];

                        await PlaylistVideosHelper.GetPlaylistVideosHelper
                            .DeleteVideoFromPlaylist(_playlist,
                                videoToRemove,
                                _cancellationTokenSource.Token);

                        if (rows.ContainsKey(
                            videoToRemove.PositionInPlayList.Value + 1))
                        {
                            rows.Remove(
                                videoToRemove.PositionInPlayList.Value + 1);
                        }

                        MessageLogger.Text = "";

                        foreach (DataRow row in rows.Values)
                        {
                            var id = row["VideoId"].ToString();
                            if (_playlist.PlayListVideos.ContainsKey(id))
                            {
                                row["Position"] =
                                    (_playlist.PlayListVideos[id].
                                        PositionInPlayList + 1).ToString();
                            }
                        }

                        playlistVideosDataView.DataSource =
                            rows.Values.CopyToDataTable();

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

            UpdateTotalVideosCount();
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
                rows.Values.FirstOrDefault(r => r["VideoId"].ToString() == 
                                eventArgs.VideoId);
            if (filteredRow != null)
            {
                filteredRow["Preview"] = bitmap;
            }
            
            DataGridViewRow row = playlistVideosDataView.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(r => r.Cells["VideoId"].Value.ToString() ==
                            eventArgs.VideoId);

            if (row != null &&
                filteredRow != null)
            {
                row.SetValues(filteredRow.ItemArray);
            }
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

        private void noFilterButton_CheckedChanged(object sender, EventArgs e)
        {
            if (noFilterButton.Checked &&
                rows.Count > 0)
            {
                playlistVideosDataView.DataSource =
                    rows.Values.CopyToDataTable();
            }
        }

        private void showDuplicatesButton_CheckedChanged(
            object sender, EventArgs e)
        {
            if (!showDuplicatesButton.Checked &&
                rows.Count > 0)
            {
                return;
            }

            var duplicates =
                PlaylistVideosHelper.GetPlaylistVideosHelper
                    .GetPlaylistDuplicates(_playlist);

            if (duplicates.Count > 0)
            {
                MessageBox.Show(
                    duplicates.Count +
                    " duplicates found in playlist.");
            }

            var filteredVideos =
                duplicates
                    .SelectMany(d => d)
                    .Select(f => f.UniqueVideoIdInPlaylist)
                    .ToList();

            var rowsFiltered =
                rows.Values
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

                MessageBox.Show("No duplicates in playlist found.");
            }
        }

        private void showPrivate_CheckedChanged(object sender, EventArgs e)
        {
            if (!showPrivate.Checked &&
                rows.Count > 0)
            {
                return;
            }

            var filteredVideos =
                PlaylistVideosHelper.GetPlaylistVideosHelper
                    .GetPrivateVideos(
                        _playlist.PlayListVideos.Values.ToList())
                    .Select(f => f.UniqueVideoIdInPlaylist)
                    .ToList();

            var rowsFiltered =
                rows.Values
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
                MessageBox.Show("No private videos found in playlist.");

                playlistVideosDataView.DataSource =
                    (playlistVideosDataView.DataSource as DataTable)?.Clone();
            }
        }

        private void LoadPlaylistPreviewDetails(
            UserPlayListVideo selectedVideo)
        {
            titleValue.Text =
                GetPreviewItemValue(selectedVideo?.Title);

            durationValue.Text =
                GetPreviewItemValue(selectedVideo?.Duration);

            playlistPositionValue.Text =
                GetPreviewItemValue(
                    (selectedVideo?.PositionInPlayList + 1)?.ToString());

            var videoUrl =
                selectedVideo?.VideoId == null
                ? ""
                : CommonUtilities.GetYoutubeVideoUrlFromVideoId(
                    selectedVideo.VideoId);

            urlValue.Text = GetPreviewItemValue(videoUrl);

            urlValue.Links.Clear();

            urlValue.Links.Add(0, urlValue.Text.Length,
                videoUrl);

            videoOwnerValue.Text =
                GetPreviewItemValue(
                    selectedVideo?.VideoOwnerChannelTitle);

            videoOwnerValue.Links.Clear();

            videoOwnerValue.Links.Add(0, videoOwnerValue.Text.Length,
                selectedVideo == null
                    ? string.Empty
                    : CommonUtilities.GetYoutubeChannelUrlFromChannelId(
                    selectedVideo.VideoOwnerChannelId));

            if (selectedVideo?.VideoAddedToPlaylistOn != null)
            {
                addedOnValue.Text =
                    GetPreviewItemValue(
                        selectedVideo.VideoAddedToPlaylistOn.Value
                            .ToString("dd/MM/yyyy HH:mm"));
            }
            else
            {
                addedOnValue.Text = @"-";
            }

            addedByValue.Text =
                GetPreviewItemValue(
                    selectedVideo?.VideoAddedToPlaylistByChannelTitle);

            addedByValue.Links.Clear();

            addedByValue.Links.Add(0, addedByValue.Text.Length,
                selectedVideo == null
                    ? string.Empty
                    : CommonUtilities.GetYoutubeChannelUrlFromChannelId(
                        selectedVideo.VideoAddedToPlaylistByChannelId));

            privacyStatusValue.Text =
                GetPreviewItemValue(
                    selectedVideo?.PrivacyStatus.ToString());

            var description =
                selectedVideo?.Description ?? "-";

            descriptionValue.Text =
                description.Length > 100 
                    ? description.Substring(0, 100) + "..."
                    : description;

            if (!string.IsNullOrWhiteSpace(description))
            {
                toolTip1.SetToolTip(
                    descriptionValue, description);
            }

            if (selectedVideo?.Thumbnail == null ||
                selectedVideo.Thumbnail?.IsDownloaded == false)
            {
                videoThumbnailPreview.ImageLocation =
                    "default_image.png";
            }
            else
            {
                videoThumbnailPreview.ImageLocation =
                    _activeUserSession.UserDirectory +
                        selectedVideo.
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

        private void playlistVideosDataView__SelectedIndexChanged(
            object sender, EventArgs e)
        {
            UserPlayListVideo selectedPlaylistVideo = null;

            if (playlistVideosDataView.SelectedRows.Count > 0)
            {
                var selectedItem =
                    playlistVideosDataView.SelectedRows[0].Cells["VideoId"];

                var selectedItemId =
                    selectedItem.Value.ToString();

                if (_playlist.PlayListVideos.ContainsKey(
                    selectedItemId))
                {
                    selectedPlaylistVideo =
                        _playlist.PlayListVideos[selectedItemId];
                }
            }

            LoadPlaylistPreviewDetails(selectedPlaylistVideo);
        }

        private void urlValue_LinkClicked(
            object sender, LinkLabelLinkClickedEventArgs e)
        {
            CommonUtilities.OpenLinkInBrowser(urlValue.Text);
        }

        private void addedByValue_LinkClicked(
            object sender, LinkLabelLinkClickedEventArgs e)
        {
            CommonUtilities.OpenLinkInBrowser(
                addedByValue.Links[0].LinkData.ToString());
        }

        private void videoOwnerValue_LinkClicked(
            object sender, LinkLabelLinkClickedEventArgs e)
        {
            CommonUtilities.OpenLinkInBrowser(
                videoOwnerValue.Links[0].LinkData.ToString());
        }
    }
}
