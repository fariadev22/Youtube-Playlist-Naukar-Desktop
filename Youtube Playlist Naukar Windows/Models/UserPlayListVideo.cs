namespace Youtube_Playlist_Naukar_Windows.Models
{
    public class UserPlayListVideo
    {
        public string UniqueVideoIdInPlaylist { get; set; }

        public string VideoId { get; set; }

        public string Title { get; set; }

        public string ThumbnailUrl { get; set; }

        public string VideoOwnerChannelTitle { get; set; }

        public string VideoOwnerChannelId { get; set; }

        public long? PositionInPlayList { get; set; }
    }
}
