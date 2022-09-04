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

        private BackgroundWorker _ownedPlaylistsBackgroundWorker;

        private BackgroundWorker _contributorPlaylistsBackgroundWorker;

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

        public void RunOwnedPlaylistsBackgroundWorker(
            DoWorkEventHandler workHandler,
            List<UserPlayList> userPlaylists)
        {
            _ownedPlaylistsBackgroundWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };

            _ownedPlaylistsBackgroundWorker.DoWork +=
                workHandler;

            _ownedPlaylistsBackgroundWorker.RunWorkerAsync(
                (userPlaylists, true));

            _ownedPlaylistsBackgroundWorker.RunWorkerCompleted +=
                RemoveOwnedBackgroundWorkerForPlaylistId;
        }

        public void RunContributorPlaylistsBackgroundWorker(
            DoWorkEventHandler workHandler,
            List<UserPlayList> userPlaylists)
        {
            _contributorPlaylistsBackgroundWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };

            _contributorPlaylistsBackgroundWorker.DoWork +=
                workHandler;

            _contributorPlaylistsBackgroundWorker.RunWorkerAsync(
                (userPlaylists, false));

            _contributorPlaylistsBackgroundWorker.RunWorkerCompleted +=
                RemoveContributorBackgroundWorkerForPlaylistId;
        }

        public bool IsBackgroundWorkForPlaylistsCancelled(
            bool areUserOwnedPlaylists)
        {
            if (areUserOwnedPlaylists &&
                (_ownedPlaylistsBackgroundWorker == null ||
                 _ownedPlaylistsBackgroundWorker.CancellationPending))
            {
                return true;
            }

            if (!areUserOwnedPlaylists &&
                (_contributorPlaylistsBackgroundWorker == null ||
                _contributorPlaylistsBackgroundWorker.CancellationPending))
            {
                return true;
            }

            return false;
        }

        public override void CancelBackgroundWork()
        {
            _ownedPlaylistsBackgroundWorker?.CancelAsync();
            _contributorPlaylistsBackgroundWorker?.CancelAsync();
        }

        public override bool IsBusy()
        {
            return 
                _ownedPlaylistsBackgroundWorker?.IsBusy == true ||
                _contributorPlaylistsBackgroundWorker?.IsBusy == true;
        }

        private void RemoveOwnedBackgroundWorkerForPlaylistId(
            object sender, RunWorkerCompletedEventArgs e)
        {
            _ownedPlaylistsBackgroundWorker = null;
        }

        private void RemoveContributorBackgroundWorkerForPlaylistId(
            object sender, RunWorkerCompletedEventArgs e)
        {
            _contributorPlaylistsBackgroundWorker = null;
        }
    }
}
