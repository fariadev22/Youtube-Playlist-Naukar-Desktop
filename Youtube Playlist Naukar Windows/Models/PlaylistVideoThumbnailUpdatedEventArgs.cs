using System;

namespace Youtube_Playlist_Naukar_Windows.Models
{
    public class PlaylistVideoThumbnailUpdatedEventArgs: EventArgs
    {
        public string PlaylistId { get; set; }

        public string VideoId { get; set; }

        public string PlaylistVideoImagePathFromCustomerDirectory { get; set; }
    }
}