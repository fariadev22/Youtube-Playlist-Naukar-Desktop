using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Youtube_Playlist_Naukar_Windows.Helpers;
using Youtube_Playlist_Naukar_Windows.Helpers.BackgroundWorkers;
using Youtube_Playlist_Naukar_Windows.Models;
using Youtube_Playlist_Naukar_Windows.Utilities;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class PlaylistHomePageForm : Form
    {
        private UserPlayList _playlist;

        private UserSession _activeUserSession;

        private CancellationTokenSource _cancellationTokenSource;

        private int _timerTicks;
        
        private int waitUntil = 10;

        private const int VideoThumbnailWidth = 150;

        private const int VideoThumbnailHeight = 84;

        private SortedDictionary<long, DataRow> _rows =
            new SortedDictionary<long, DataRow>();

        private Bitmap _defaultImage;

        private DataTable _dataTable;

        private bool _dataTableLoaded;

        private VideoFilter _activeVideoFilter =
            VideoFilter.None;

        public PlaylistHomePageForm(
            UserPlayList playlist,
            UserSession activeUserSession)
        {
            InitializeComponent();
            
            _playlist = playlist;
            Text = _playlist.Title;
            _activeUserSession = activeUserSession;
            descriptionToolTip = new ToolTip();
            _defaultImage =
                new Bitmap(Image.FromFile("default_image.png"),
                    VideoThumbnailWidth, VideoThumbnailHeight);
            _dataTable = InitializeVideoTable();

            PlaylistVideosHelper.VideoThumbnailReady +=
                UpdatePlaylistVideoThumbnail;
        }

        protected override async void OnLoad(
            EventArgs e)
        {
            if (_playlist != null)
            {
                _cancellationTokenSource =
                    new CancellationTokenSource();

                await LoadPlaylistVideos();

                if (!_cancellationTokenSource.
                    IsCancellationRequested)
                {
                    playlistNameValue.Text =
                        _playlist.Title ?? "-";
                }
            }
            else
            {
                Close();
            }
        }

        protected override void OnFormClosing(
            FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            PlaylistBackgroundWorker.GetPlaylistBackgroundWorker.
                CancelBackgroundWorkerForPlaylistId(_playlist.Id);
            _playlist = null;
            _activeUserSession = null;
            _cancellationTokenSource.Cancel();
        }

        #region Helpers

        private async Task LoadPlaylistVideos()
        {
            MessageLogger.Text = @"Loading...";

            //disable user controls
            addVideosButton.Enabled = false;
            refreshVideosButton.Enabled = false;
            deleteVideoButton.Enabled = false;
            filterBox.Enabled = false;
            searchBar.Enabled = false;

           var playlistETagResult =
                await PlaylistVideosHelper.GetPlaylistVideosHelper.
                    GetPlaylistVideosEtag(
                        _playlist.PlaylistVideosETag, _playlist.Id,
                        _cancellationTokenSource.Token);

            //if etag is not equal or does not exist we need to 
            //reload from scratch
            var reloadPlaylists = !playlistETagResult.Item2;

            if (reloadPlaylists)
            {
                _dataTable.Clear();
                _rows.Clear();

                // Display the ProgressBar control.
                videoLoadProgressBar.Visible = true;
                videoLoadProgressBar.Minimum = 1;
                // Set Maximum to the total number of videos to load
                videoLoadProgressBar.Maximum =
                    int.Parse(_playlist.TotalVideosInPlaylist.ToString());
                // Set the initial value of the ProgressBar.
                videoLoadProgressBar.Value = 1;
                // Set the Step property to a value of 50 to represent
                // each chunk of videos being loaded
                videoLoadProgressBar.Step = 50;

                var existingVideoData =
                    _playlist.PlayListVideos;

                string newEtag = playlistETagResult.Item1;

                _playlist.PlayListVideos = 
                    new Dictionary<string, UserPlayListVideo>();

                long? lastVideoPosition = null;
                string nextPageToken = null;

                do
                {
                    var userPlaylistsVideosResult =
                        await PlaylistVideosHelper.GetPlaylistVideosHelper
                            .LoadPlaylistVideos(_playlist.Id,
                                _cancellationTokenSource.Token,
                                existingVideoData,
                                nextPageToken,
                                lastVideoPosition);

                    Dictionary<string, UserPlayListVideo> 
                        userPlaylistVideos =
                            userPlaylistsVideosResult.Item1;

                    nextPageToken =
                        userPlaylistsVideosResult.Item2;

                    if (userPlaylistVideos != null &&
                        userPlaylistVideos.Count > 0)
                    {
                        foreach (var userPlaylistVideo in userPlaylistVideos)
                        {
                            _playlist.PlayListVideos.Add(
                                userPlaylistVideo.Key,
                                userPlaylistVideo.Value);

                            lastVideoPosition =
                                userPlaylistVideo.Value.PositionInPlayList;
                        }

                        if (!_cancellationTokenSource.
                            IsCancellationRequested)
                        {
                            LoadPlaylistVideosUi(
                                userPlaylistVideos.Values.ToList());
                            UpdateTotalVideosCount();
                        }
                    }

                    videoLoadProgressBar.PerformStep();

                } while (!string.IsNullOrWhiteSpace(nextPageToken));

                videoLoadProgressBar.Visible = false;

                if(!_cancellationTokenSource.IsCancellationRequested)
                {
                    PlaylistVideosHelper.GetPlaylistVideosHelper.
                        SavePlaylist(_playlist, newEtag);
                }
            }
            else if(!_dataTableLoaded && 
                    !_cancellationTokenSource.
                        IsCancellationRequested)
            {
                _dataTable.Clear();
                _rows.Clear();
                videoLoadProgressBar.Visible = false;
                LoadPlaylistVideosUi(
                    _playlist.PlayListVideos.Values.ToList());
                UpdateTotalVideosCount();
            }

            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                PlaylistVideosHelper.GetPlaylistVideosHelper
                    .DownloadPlaylistVideoThumbnails(
                        _playlist.Id,
                        _playlist.PlayListVideos.Values.ToList());
            }

            addVideosButton.Enabled = true;
            refreshVideosButton.Enabled = true;
            deleteVideoButton.Enabled = true;
            filterBox.Enabled = true;
            searchBar.Enabled = true;

            MessageLogger.Text = @"";
            _dataTableLoaded = true;
        }

        private void LoadPlaylistVideosUi(
            List<UserPlayListVideo> playlistVideos)
        {
            List<DataRow> newRows = new List<DataRow>();

            if (playlistVideos != null &&
                playlistVideos.Count > 0)
            {
                foreach (var playlistVideo in
                    playlistVideos)
                {
                    var newRow = _dataTable.NewRow();
                    newRow.ItemArray = 
                        GetRowFromRowValues(playlistVideo);

                    newRows.Add(newRow);

                    _dataTable.Rows.Add(newRow);
                }
            }

            playlistVideosDataView.DataSource = _dataTable;
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

            if (newRows.Count > 0)
            {
                foreach (var newTableRow in newRows)
                {
                    if (newTableRow?["Position"] != null &&
                        long.TryParse(newTableRow["Position"].ToString(),
                            out var position))
                    {
                        _rows.Add(position, newTableRow);
                    }
                }
            }
        }

        private static DataTable InitializeVideoTable()
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
            return table;
        }

        private object[] GetRowFromRowValues(
            UserPlayListVideo playlistVideo)
        {
            if (playlistVideo == null)
            {
                return new object[] { };
            }

            return new object[]
            {
                playlistVideo.UniqueVideoIdInPlaylist,
                ((playlistVideo.PositionInPlayList ?? 0) + 1)
                    .ToString(),
                _defaultImage,
                playlistVideo.Title,
                playlistVideo.Duration,
                playlistVideo.VideoOwnerChannelTitle
            };
        }

        private void UpdateTotalVideosCount()
        {
            totalVideosValue.Text =
                (_playlist?.PlayListVideos?.Count ?? 0).ToString();
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
                    (selectedVideo?.PositionInPlayList + 1)?
                    .ToString());

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
            videoOwnerValue.Links.Add(
                0, videoOwnerValue.Text.Length,
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
            addedByValue.Links.Add(0, 
                addedByValue.Text.Length,
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
                    ? Regex.Replace(description,
                          @"\t|\n|\r", "").Substring(0, 100) +
                      "..."
                    : description;
            if (!string.IsNullOrWhiteSpace(description))
            {
                descriptionToolTip.SetToolTip(
                    descriptionValue, description);
            }

            if (selectedVideo?.Thumbnail == null ||
                selectedVideo.Thumbnail?.IsDownloaded == false)
            {
                videoThumbnailPreview.ImageLocation =
                    @"default_image.png";
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

        private void DisplaySearchResults()
        {
            if (playlistVideosDataView.DataSource == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(
                searchBar.Text))
            {
                if (_rows.Count > 0)
                {
                    playlistVideosDataView.DataSource =
                        _rows.Values.CopyToDataTable();
                }

                return;
            }

            var filteredVideos =
                PlaylistVideosHelper.GetPlaylistVideosHelper
                    .SearchVideoInPlayList(
                        searchBar.Text, _playlist)
                    .Select(f => f.UniqueVideoIdInPlaylist)
                    .ToList();

            var rowsFiltered =
                _rows.Values
                    .Where(r => filteredVideos.Contains(
                        Convert.ToString(r["VideoId"])))
                    .ToList();

            if (rowsFiltered.Any())
            {
                playlistVideosDataView.DataSource =
                    rowsFiltered
                        .OrderBy(s =>
                            filteredVideos.IndexOf(
                                Convert.ToString(s["VideoId"]))
                        )
                        .CopyToDataTable();

                ApplyFilterToRows();
            }
            else
            {
                // a way to empty the table without affecting references
                playlistVideosDataView.DataSource =
                    (playlistVideosDataView.DataSource as DataTable)?.Clone();
            }
        }

        private void ApplyFilterToRows()
        {
            if (_activeVideoFilter ==
                VideoFilter.None ||
                _rows.Count <= 0)
            {
                return;
            }

            VideoFilter activeFilterStore =
                _activeVideoFilter;

            RadioButton radioButton =
                filterBox.Controls
                    .OfType<RadioButton>()
                    .FirstOrDefault(x =>
                        x.Name == "noFilterButton");

            if (radioButton != null)
            {
                radioButton.Checked = true;
            }


            if (activeFilterStore ==
                VideoFilter.Duplicate)
            {
                radioButton =
                    filterBox.Controls
                        .OfType<RadioButton>()
                        .FirstOrDefault(x =>
                            x.Name == "showDuplicatesButton");

                if (radioButton != null)
                {
                    radioButton.Checked = true;
                }
            }
            else if (activeFilterStore ==
                     VideoFilter.Private)
            {
                radioButton =
                    filterBox.Controls
                        .OfType<RadioButton>()
                        .FirstOrDefault(x =>
                            x.Name == "showPrivate");

                if (radioButton != null)
                {
                    radioButton.Checked = true;
                }
            }
        }

        #endregion

        #region Events

        private void ReturnHomeButton_Click(
            object sender, EventArgs e)
        {
            Close();
        }

        private void Timer_Tick(
            object sender, EventArgs e)
        {
            _timerTicks++;

            if (_timerTicks > waitUntil)
            {
                //Stop the timer and begin the
                //search in a background thread.
                timer.Stop();
                DisplaySearchResults();
            }
        }

        private void SearchBar_TextChanged(
            object sender, EventArgs e)
        {
            if (!timer.Enabled)
                timer.Start();
            //Reset the timer when a character is entered
            _timerTicks = 0;
        }
        
        private async void AddVideos_Click(
            object sender, EventArgs e)
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

                var logsForm = new LogsForm();

                logsForm.Show(this);

                List<string> urls =
                    urlInput.Split('\n').
                        Select(url => url.Trim()).ToList();

                foreach (var url in urls)
                {
                    string urlId = Guid.NewGuid().ToString();

                    logsForm.AddRow(urlId, url, "In Progress");

                    var videoAdditionResult =
                        await PlaylistVideosHelper.GetPlaylistVideosHelper.
                            AddVideoToPlayList(
                                url, _playlist,
                                _cancellationTokenSource.Token);

                    var message = videoAdditionResult.Item1;
                    var videoAdded = videoAdditionResult.Item2;

                    if (videoAdded != null)
                    {
                        newVideos.Add(videoAdded);
                    }

                    logsForm.UpdateRow(urlId, url, message,
                        videoAdded != null);
                }

                urlInputForm.Dispose();
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                urlInputForm.Dispose();
            }

            if(!_cancellationTokenSource.
                IsCancellationRequested)
            {
                var newRows =
                    new SortedDictionary<long, DataRow>();

                //update positions of existing video rows
                foreach (DataRow row in _rows.Values)
                {
                    var id = row["VideoId"].ToString();
                    if (id != null &&
                        _playlist.PlayListVideos.ContainsKey(id))
                    {
                        row["Position"] =
                            (_playlist.PlayListVideos[id].
                                PositionInPlayList + 1).ToString();
                    }
                }

                //build a new dictionary to hold rows against 
                //new positions
                foreach (DataRow row in _rows.Values)
                {
                    if (row?["Position"] != null &&
                        long.TryParse(row["Position"].ToString(), 
                            out var position))
                    {
                        newRows.Add(position, row);
                    }
                }

                _rows = newRows;

                //add new videos to rows
                foreach (var playlistVideo in newVideos)
                {
                    var newRow = _dataTable.NewRow();
                    newRow.ItemArray = GetRowFromRowValues(
                        playlistVideo);
                    _rows.Add((playlistVideo.PositionInPlayList ?? 0) + 1, 
                        newRow);
                }

                if (_rows.Any())
                {
                    playlistVideosDataView.DataSource =
                        _rows.Values.CopyToDataTable();

                    ApplyFilterToRows();
                }

                //download thumbnails of new videos
                PlaylistVideosHelper.GetPlaylistVideosHelper
                    .DownloadPlaylistVideoThumbnails(
                        _playlist.Id, newVideos);

                UpdateTotalVideosCount();
            }
        }

        private async void RefreshVideos_Click(
            object sender, EventArgs e)
        {
            if (_playlist == null ||
                _cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            PlaylistBackgroundWorker.GetPlaylistBackgroundWorker.
                CancelBackgroundWorkerForPlaylistId(
                    _playlist.Id);
            await LoadPlaylistVideos();
            ApplyFilterToRows();
            MessageBox.Show(@"Playlist videos have been refreshed.");
        }
        
        private async void DeleteVideo_Click(
            object sender, EventArgs e)
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
                        @"Are you sure you want to delete the selected " +
                        @"playlist video from your YouTube account? " +
                        @"This action cannot be undone.",
                        @"Confirm Deletion",
                        MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        MessageLogger.Text = @"Deleting...";
                        videoLoadProgressBar.Visible = false;

                        var videoToRemove =
                            _playlist.PlayListVideos[videoPlaylistId];

                        bool videoDeleted = 
                            await PlaylistVideosHelper.GetPlaylistVideosHelper
                                .DeleteVideoFromPlaylist(_playlist,
                                    videoToRemove,
                                    _cancellationTokenSource.Token);

                        if (_cancellationTokenSource.IsCancellationRequested)
                        {
                            return;
                        }

                        if (!videoDeleted)
                        {
                            MessageBox.Show(@"Video not removed.");
                            return;
                        }

                        //delete row tied to video
                        if (videoToRemove.PositionInPlayList != null &&
                            _rows.ContainsKey(
                                videoToRemove.PositionInPlayList.Value + 1))
                        {
                            _rows.Remove(
                                videoToRemove.PositionInPlayList.Value + 1);
                        }

                        MessageLogger.Text = "";
                        var newRows =
                            new SortedDictionary<long, DataRow>();

                        //update existing video positions
                        foreach (DataRow row in _rows.Values)
                        {
                            var id = row["VideoId"].ToString();
                            if (id != null &&
                                _playlist.PlayListVideos.ContainsKey(id))
                            {
                                row["Position"] =
                                    (_playlist.PlayListVideos[id].
                                        PositionInPlayList + 1).ToString();
                            }
                        }

                        //build a new dictionary to hold rows against 
                        //new positions
                        foreach (DataRow row in _rows.Values)
                        {
                            if (row?["Position"] != null &&
                                long.TryParse(row["Position"].ToString(),
                                    out var position))
                            {
                                newRows.Add(position, row);
                            }
                        }

                        _rows = newRows;

                        if (_rows.Any())
                        {
                            playlistVideosDataView.DataSource =
                                _rows.Values.CopyToDataTable();

                            ApplyFilterToRows();
                        }

                        MessageBox.Show(@"Video successfully " +
                                        @"removed from your YouTube account.");
                    }
                    else
                    {
                        MessageBox.Show(@"Video not removed.");
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    @"Select a video to remove.");
            }

            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                UpdateTotalVideosCount();
            }
        }

        private void UpdatePlaylistVideoThumbnail(
            object sender, 
            VideoThumbnailReadyEventArgs eventArgs)
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
            if (_cancellationTokenSource.
                    IsCancellationRequested ||
                _playlist == null ||
                _activeUserSession == null ||
                _playlist.Id != eventArgs.PlaylistId)
            {
                return;
            }

            var bitmap =
                CommonUtilities.ConvertLocalImageToBitmap(
                    _activeUserSession.UserDirectory + "/" + 
                        eventArgs.
                        PlaylistVideoImagePathFromCustomerDirectory,
                    VideoThumbnailWidth,
                    VideoThumbnailHeight
                );

            //filtered row is row of the DataTable not DataGridView
            var dataTableRow =
                _rows.Values.FirstOrDefault(
                    r => r["VideoId"].ToString() == 
                        eventArgs.VideoId);

            if (dataTableRow != null)
            {
                //this alone won't update the Datagridview
                dataTableRow["Preview"] = bitmap;
            }
            
            DataGridViewRow dataGridViewRow = 
                playlistVideosDataView.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(r => 
                        r.Cells["VideoId"].Value.ToString() ==
                            eventArgs.VideoId);

            //this helps trigger the datagridview that a row has changed
            if (dataGridViewRow != null &&
                dataTableRow != null)
            {
                dataGridViewRow.SetValues(dataTableRow.ItemArray);
            }
        }

        private void NoFilterButton_CheckedChanged(
            object sender, EventArgs e)
        {
            if (noFilterButton.Checked)
            {
                _activeVideoFilter =
                    VideoFilter.None;

                if(_rows.Count > 0)
                {
                    playlistVideosDataView.DataSource =
                        _rows.Values.CopyToDataTable();
                }
            }
        }

        private void ShowDuplicatesButton_CheckedChanged(
            object sender, EventArgs e)
        {
            if (!showDuplicatesButton.Checked)
            {
                return;
            }

            _activeVideoFilter =
                VideoFilter.Duplicate;

            if (_rows.Count > 0)
            {
                var duplicates =
                    PlaylistVideosHelper.GetPlaylistVideosHelper
                        .GetPlaylistDuplicates(_playlist);

                if (duplicates.Count > 0)
                {
                    MessageBox.Show(
                        duplicates.Count +
                        @" duplicates found in playlist.");
                }

                var filteredVideos =
                    duplicates
                        .SelectMany(d => d)
                        .Select(f => f.UniqueVideoIdInPlaylist)
                        .ToList();

                var rowsFiltered =
                    _rows.Values
                        .Where(r => filteredVideos.Contains(
                            Convert.ToString(r["VideoId"])))
                        .ToList();

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

                    MessageBox.Show(@"No duplicates in playlist found.");
                }
            }
        }

        private void ShowPrivate_CheckedChanged(
            object sender, EventArgs e)
        {
            if (!showPrivate.Checked)
            {
                return;
            }

            _activeVideoFilter =
                VideoFilter.Private;

            if (_rows.Count > 0)
            {
                var filteredVideos =
                    PlaylistVideosHelper.GetPlaylistVideosHelper
                        .GetPrivateVideos(
                            _playlist.PlayListVideos.Values.ToList())
                        .Select(f => f.UniqueVideoIdInPlaylist)
                        .ToList();

                var rowsFiltered =
                    _rows.Values
                        .Where(r => filteredVideos.Contains(
                            Convert.ToString(r["VideoId"])))
                        .ToList();

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
                    MessageBox.Show(@"No private videos found in playlist.");

                    playlistVideosDataView.DataSource =
                        (playlistVideosDataView.DataSource as DataTable)?.Clone();
                }
            }
        }

        private void PlaylistVideosDataView__SelectedIndexChanged(
            object sender, EventArgs e)
        {
            UserPlayListVideo selectedPlaylistVideo = null;

            if (playlistVideosDataView.SelectedRows.Count > 0)
            {
                var selectedItem =
                    playlistVideosDataView.SelectedRows[0].Cells["VideoId"];

                var selectedItemId =
                    selectedItem.Value.ToString();

                if (selectedItemId != null &&
                    _playlist.PlayListVideos.ContainsKey(
                        selectedItemId))
                {
                    selectedPlaylistVideo =
                        _playlist.PlayListVideos[selectedItemId];
                }
            }

            LoadPlaylistPreviewDetails(selectedPlaylistVideo);
        }

        private void UrlValue_LinkClicked(
            object sender, LinkLabelLinkClickedEventArgs e)
        {
            CommonUtilities.OpenLinkInBrowser(urlValue.Text);
        }

        private void AddedByValue_LinkClicked(
            object sender, LinkLabelLinkClickedEventArgs e)
        {
            CommonUtilities.OpenLinkInBrowser(
                addedByValue.Links[0].LinkData.ToString());
        }

        private void VideoOwnerValue_LinkClicked(
            object sender, LinkLabelLinkClickedEventArgs e)
        {
            CommonUtilities.OpenLinkInBrowser(
                videoOwnerValue.Links[0].LinkData.ToString());
        }

        #endregion
    }
}
