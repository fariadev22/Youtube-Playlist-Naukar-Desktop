using System.Collections.Generic;
using System.ComponentModel;

namespace Youtube_Playlist_Naukar_Windows.Models
{
    public class PlaylistBackgroundWorkerData
    {
        public string PlaylistId { get; set; }

        public List<UserPlayListVideo> UserPlaylistVideos { get; set; }

        public BackgroundWorker BackgroundWorker { get; set; }
    }
}