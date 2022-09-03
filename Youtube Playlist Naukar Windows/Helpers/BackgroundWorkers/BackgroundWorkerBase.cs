namespace Youtube_Playlist_Naukar_Windows.Helpers.BackgroundWorkers
{
    public abstract class BackgroundWorkerBase
    {
        public abstract void CancelBackgroundWork();

        public abstract bool IsBusy();
    }
}
