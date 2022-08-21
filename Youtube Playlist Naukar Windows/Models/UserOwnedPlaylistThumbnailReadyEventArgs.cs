namespace Youtube_Playlist_Naukar_Windows.Models
{
    public class UserOwnedPlaylistThumbnailReadyEventArgs
    {
        public string PlaylistId { get; set; }

        public string PlaylistImagePathFromCustomerDirectory { get; set; }
    }
}