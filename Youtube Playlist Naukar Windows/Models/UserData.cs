using System.Collections.Generic;
using Newtonsoft.Json;

namespace Youtube_Playlist_Naukar_Windows.Models
{
    public class UserData
    {
        /// <summary>
        /// Google email address associated with the user
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Youtube channel Id of the user
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// List of playlists that the user owns
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, UserPlayList> UserOwnedPlayLists { get; set; } 
            = new Dictionary<string, UserPlayList>();

        /// <summary>
        /// List of playlists that the user does not own but he is a contributor of them
        /// </summary>
        public Dictionary<string, UserPlayList> UserContributorPlayLists { get; set; }
            = new Dictionary<string, UserPlayList>();
    }
}
