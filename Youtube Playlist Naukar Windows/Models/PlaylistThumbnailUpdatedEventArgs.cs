using System;

namespace Youtube_Playlist_Naukar_Windows.Models
{
    public class PlaylistThumbnailUpdatedEventArgs : EventArgs
    {
        public string PlaylistId { get; set; }

        public string PlaylistImagePathFromCustomerDirectory { get; set; }

        public bool IsOwnerPlaylist { get; set; }
    }
}