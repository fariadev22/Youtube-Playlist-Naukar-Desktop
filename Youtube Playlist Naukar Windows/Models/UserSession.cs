using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;

namespace Youtube_Playlist_Naukar_Windows.Models
{
    public class UserSession
    {
        /// <summary>
        /// Unique id assigned to the logged in user by the app
        /// </summary>
        public string UserIdForTokenStorage { get; set; }

        /// <summary>
        /// Should the user account be selected when app is opened?
        /// </summary>
        public bool IsDefaultUser { get; set; }

        /// <summary>
        /// Google email address associated with the user
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Credentials object that helps authenticate API requests for the user
        /// </summary>
        [JsonIgnore]
        public UserCredential UserCredential { get; set; }

        /// <summary>
        /// Data related to playlists associated with this session
        /// </summary>
        public UserData UserData { get; set; }

        [JsonIgnore]
        public string UserDirectory { get; set; }
    }
}
