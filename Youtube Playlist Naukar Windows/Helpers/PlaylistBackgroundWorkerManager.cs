using System.Collections.Generic;
using Youtube_Playlist_Naukar_Windows.Models;

namespace Youtube_Playlist_Naukar_Windows.Helpers
{
    public class PlaylistBackgroundWorkerManager
    {
        private static readonly PlaylistBackgroundWorkerManager
            BackgroundWorkerInstance =
                new PlaylistBackgroundWorkerManager();

        private Dictionary<string, PlaylistBackgroundWorkerData> 
            ActiveBackgroundWorkers { get; set; } =
                new Dictionary<string, PlaylistBackgroundWorkerData>();

        private Dictionary<string, PlaylistBackgroundWorkerData>
            PendingBackgroundWorkers { get; set; } =
            new Dictionary<string, PlaylistBackgroundWorkerData>();

        static PlaylistBackgroundWorkerManager()
        {
        }

        public void AddAndStartBackgroundWorker(string playlistId,
            PlaylistBackgroundWorkerData backgroundWorkerData)
        {
            if (PendingBackgroundWorkers.ContainsKey(playlistId))
            {
                //replace
                PendingBackgroundWorkers[playlistId] =
                    backgroundWorkerData;
            }
            else
            {
                PendingBackgroundWorkers.Add(
                    playlistId,
                    backgroundWorkerData);
            }

            StartBackgroundWorkerForPlaylist(playlistId);
        }

        public PlaylistBackgroundWorkerData GetActiveBackgroundWorker(
            string playlistId)
        {
            if (ActiveBackgroundWorkers.ContainsKey(playlistId))
            {
                return ActiveBackgroundWorkers[playlistId];
            }

            return null;
        }

        public void CancelBackgroundWorkerForPlaylistId(
            string playlistId)
        {
            if (ActiveBackgroundWorkers.ContainsKey(playlistId))
            {
                ActiveBackgroundWorkers[playlistId].BackgroundWorker
                    .CancelAsync();
            }
        }

        public void RemoveActiveBackgroundWorkerForPlaylistId(
            string playlistId)
        {
            if (ActiveBackgroundWorkers.ContainsKey(playlistId))
            {
                ActiveBackgroundWorkers.Remove(playlistId);
            }

            StartBackgroundWorkerForPlaylist(playlistId);
        }

        public static PlaylistBackgroundWorkerManager GetBackgroundWorkerManager
        {
            get
            {
                return BackgroundWorkerInstance;
            }
        }

        private void StartBackgroundWorkerForPlaylist(
            string playlistId)
        {
            if (ActiveBackgroundWorkers.ContainsKey(playlistId))
            {
                return;
            }

            if (!PendingBackgroundWorkers.ContainsKey(playlistId))
            {
                return;
            }

            var backgroundWorkerData =
                PendingBackgroundWorkers[playlistId];

            if (!ActiveBackgroundWorkers.ContainsKey(playlistId))
            {
                ActiveBackgroundWorkers.Add(playlistId,
                    backgroundWorkerData);
            }

            backgroundWorkerData.BackgroundWorker.RunWorkerAsync(
                (backgroundWorkerData.PlaylistId,
                    backgroundWorkerData.UserPlaylistVideos));

            PendingBackgroundWorkers.Remove(playlistId);
        }
    }
}
