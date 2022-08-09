using System.Collections.Generic;
using Newtonsoft.Json;

namespace Youtube_Playlist_Naukar_Windows.Models
{
    public class UserPlayList
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ThumbnailUrl { get; set; }

        [JsonIgnore]
        public Dictionary<string, UserPlayListVideo> PlayListVideos { get; set; } 
            = new Dictionary<string, UserPlayListVideo>();

        [JsonIgnore]
        public bool PlayListVideosDataLoaded { get; set; } 
    }
}
