using System.Collections.Generic;
using System.ComponentModel;
using Youtube_Playlist_Naukar_Windows.Models;

namespace Youtube_Playlist_Naukar_Windows.Helpers.BackgroundWorkers
{
    public class PlaylistsBackgroundWorker: BackgroundWorkerBase
    {
        private static readonly PlaylistsBackgroundWorker
            BackgroundWorkerInstance =
                new PlaylistsBackgroundWorker();

        private BackgroundWorker _backgroundWorker;

        static PlaylistsBackgroundWorker()
        {
        }
        
        public static PlaylistsBackgroundWorker
            GetPlaylistsBackgroundWorker
        {
            get
            {
                return BackgroundWorkerInstance;
            }
        }

        public void RunPlaylistsBackgroundWorker(
            DoWorkEventHandler workHandler,
            List<UserPlayList> userPlaylists,
            bool isUserOwnedPlaylists)
        {
            _backgroundWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };

            _backgroundWorker.DoWork +=
                workHandler;

            _backgroundWorker.RunWorkerAsync(
                (userPlaylists, isUserOwnedPlaylists));
        }

        public override void CancelBackgroundWork()
        {
            _backgroundWorker?.CancelAsync();
        }

        public bool IsBackgroundWorkCancelled()
        {
            if (_backgroundWorker == null ||
                _backgroundWorker.CancellationPending)
            {
                return true;
            }

            return false;
        }

        public override bool IsBusy()
        {
            return _backgroundWorker?.IsBusy == true;
        }
    }
}
