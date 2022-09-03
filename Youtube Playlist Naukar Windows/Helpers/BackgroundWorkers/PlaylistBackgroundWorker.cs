using System.Collections.Generic;
using System.ComponentModel;
using Youtube_Playlist_Naukar_Windows.Models;

namespace Youtube_Playlist_Naukar_Windows.Helpers.BackgroundWorkers
{
    public class PlaylistBackgroundWorker: BackgroundWorkerBase
    {
        private static readonly PlaylistBackgroundWorker
            BackgroundWorkerInstance =
                new PlaylistBackgroundWorker();

        private Dictionary<string, PlaylistBackgroundWorkerData> 
            _activeBackgroundWorkers =
                new Dictionary<string, PlaylistBackgroundWorkerData>();

        private Dictionary<string, PlaylistBackgroundWorkerData>
            _pendingBackgroundWorkers =
            new Dictionary<string, PlaylistBackgroundWorkerData>();

        static PlaylistBackgroundWorker()
        {
        }

        public static PlaylistBackgroundWorker 
            GetPlaylistBackgroundWorker
        {
            get
            {
                return BackgroundWorkerInstance;
            }
        }

        public void AddAndStartBackgroundWorker(
            string playlistId,
            PlaylistBackgroundWorkerData backgroundWorkerData)
        {
            if (_pendingBackgroundWorkers.ContainsKey(playlistId))
            {
                //replace
                _pendingBackgroundWorkers[playlistId] =
                    backgroundWorkerData;
            }
            else
            {
                _pendingBackgroundWorkers.Add(
                    playlistId,
                    backgroundWorkerData);
            }

            StartBackgroundWorkerForPlaylist(playlistId);
        }

        public void CancelBackgroundWorkerForPlaylistId(
            string playlistId)
        {
            if (!_activeBackgroundWorkers.ContainsKey(playlistId))
            {
                return;
            }

            _activeBackgroundWorkers[playlistId].BackgroundWorker?
                .CancelAsync();
        }

        public bool IsBackgroundWorkForPlaylistCancelled(
            string playlistId)
        {
            var backgroundWorkerState =
                GetActiveBackgroundWorker(playlistId);

            if (backgroundWorkerState?.BackgroundWorker != null &&
                backgroundWorkerState.BackgroundWorker.
                    CancellationPending)
            {
                return true;
            }

            return false;
        }

        public override void CancelBackgroundWork()
        {
            _pendingBackgroundWorkers.Clear();

            if (_activeBackgroundWorkers.Count > 0)
            {
                foreach (var activeBackgroundWorker in
                    _activeBackgroundWorkers)
                {
                    activeBackgroundWorker.Value.
                        BackgroundWorker?.CancelAsync();
                }
            }
        }

        public override bool IsBusy()
        {
            if (_pendingBackgroundWorkers.Count <= 0 &&
                _activeBackgroundWorkers.Count <= 0)
            {
                return false;
            }

            return true;
        }

        private void StartBackgroundWorkerForPlaylist(
            string playlistId)
        {
            if (_activeBackgroundWorkers.ContainsKey(playlistId))
            {
                return;
            }

            if (!_pendingBackgroundWorkers.ContainsKey(playlistId))
            {
                return;
            }

            var backgroundWorkerData =
                _pendingBackgroundWorkers[playlistId];

            _activeBackgroundWorkers.Add(playlistId,
                backgroundWorkerData);

            backgroundWorkerData.BackgroundWorker =
                new BackgroundWorker
                {
                    WorkerSupportsCancellation = true
                };

            backgroundWorkerData.BackgroundWorker.DoWork +=
                backgroundWorkerData.PlaylistWorkHander;

            backgroundWorkerData.BackgroundWorker.RunWorkerCompleted +=
                RemoveActiveBackgroundWorkerForPlaylistId;

            backgroundWorkerData.BackgroundWorker.RunWorkerAsync(
                (backgroundWorkerData.PlaylistId,
                    backgroundWorkerData.UserPlaylistVideos));

            _pendingBackgroundWorkers.Remove(playlistId);
        }

        private PlaylistBackgroundWorkerData GetActiveBackgroundWorker(
            string playlistId)
        {
            if (_activeBackgroundWorkers.ContainsKey(playlistId))
            {
                return _activeBackgroundWorkers[playlistId];
            }

            return null;
        }

        private void RemoveActiveBackgroundWorkerForPlaylistId(
            object sender, RunWorkerCompletedEventArgs e)
        {
            string playlistId = e.Result.ToString();

            if (string.IsNullOrWhiteSpace(playlistId))
            {
                return;
            }

            if (_activeBackgroundWorkers.ContainsKey(playlistId))
            {
                _activeBackgroundWorkers.Remove(playlistId);
            }

            StartBackgroundWorkerForPlaylist(playlistId);
        }
    }
}
