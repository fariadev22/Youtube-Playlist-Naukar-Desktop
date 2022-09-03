using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Youtube_Playlist_Naukar_Windows.Models;
using Youtube_Playlist_Naukar_Windows.Utilities;

namespace Youtube_Playlist_Naukar_Windows.Helpers.BackgroundWorkers
{
    public class SessionStorageBackgroundWorker: BackgroundWorkerBase
    {
        private static readonly SessionStorageBackgroundWorker
            BackgroundWorkerInstance =
                new SessionStorageBackgroundWorker();

        private string _applicationDirectory;

        private List<UserSession> _userSessions;

        private BackgroundWorker _backgroundWorker;

        static SessionStorageBackgroundWorker()
        {
        }
        
        public static SessionStorageBackgroundWorker
            GetSessionStorageBackgroundWorker
        {
            get
            {
                return BackgroundWorkerInstance;
            }
        }

        public SessionStorageBackgroundWorker()
        {
            _backgroundWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };

            _backgroundWorker.DoWork +=
                SaveUserSessions;

            _backgroundWorker.RunWorkerAsync();
        }

        public void SetApplicationDirectory(
            string applicationDirectory)
        {
            _applicationDirectory = applicationDirectory;
        }

        public void UpdateUserSessions(
            List<UserSession> newUserSessions)
        {
            _userSessions = newUserSessions;
        }

        public async void SaveUserSessions(
            object sender, DoWorkEventArgs e)
        {
            while (!_backgroundWorker.CancellationPending)
            {
                if (!string.IsNullOrWhiteSpace(_applicationDirectory) &&
                    _userSessions != null)
                {
                    SessionStorageUtilities.SaveSessionsData(
                        _userSessions,
                        _applicationDirectory);
                }

                // asynchronously wait 
                await Task.Delay(10000);
            }
        }

        public override void CancelBackgroundWork()
        {
            _backgroundWorker.CancelAsync();
        }

        public override bool IsBusy()
        {
            return _backgroundWorker.IsBusy;
        }
    }
}
