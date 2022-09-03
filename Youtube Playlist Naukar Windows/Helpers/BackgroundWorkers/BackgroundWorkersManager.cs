using System.Collections.Generic;
using System.Linq;

namespace Youtube_Playlist_Naukar_Windows.Helpers.BackgroundWorkers
{
    public class BackgroundWorkersManager
    {
        private static readonly BackgroundWorkersManager
            BackgroundWorkerManagerInstance =
                new BackgroundWorkersManager();

        private List<BackgroundWorkerBase> _backgroundWorkers;

        static BackgroundWorkersManager()
        {
        }
        
        public static BackgroundWorkersManager
            GetBackgroundWorkerManager
        {
            get
            {
                return BackgroundWorkerManagerInstance;
            }
        }

        public BackgroundWorkersManager()
        {
            _backgroundWorkers = new List<BackgroundWorkerBase>
            {
                PlaylistBackgroundWorker.GetPlaylistBackgroundWorker,
                SessionStorageBackgroundWorker.GetSessionStorageBackgroundWorker,
                PlaylistsBackgroundWorker.GetPlaylistsBackgroundWorker
            };
        }

        public void CancelAllBackgroundWork()
        {
            foreach (var worker in _backgroundWorkers)
            {
                worker.CancelBackgroundWork();
            }

            bool hasAllWorkersFinished = false;

            while (!hasAllWorkersFinished)
            {
                hasAllWorkersFinished =
                    _backgroundWorkers.All(w => !w.IsBusy());

                System.Threading.Thread.Sleep(1000);
            }

            _backgroundWorkers.Clear();
        }

    }
}
