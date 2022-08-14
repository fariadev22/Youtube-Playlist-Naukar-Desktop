using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;
using Youtube_Playlist_Naukar_Windows.Models;
using Youtube_Playlist_Naukar_Windows.Utilities;

namespace Youtube_Playlist_Naukar_Windows.Helpers
{
    public class SessionManager
    {
        private static readonly SessionManager SessionManagerInstance =
            new SessionManager();

        static SessionManager()
        {

        }

        public static SessionManager GetSessionManager
        {
            get
            {
                return SessionManagerInstance;
            }
        }

        private LoginHelper _loginHelper;

        private List<UserSession> _userSessions;

        private UserSession _activeUserSession;

        private string _applicationDirectory;

        public SessionManager()
        {
            _applicationDirectory =
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData
                ) + "/YoutubePlayListNaukar";

            CommonUtilities.CreateDirectoryIfRequired(
                _applicationDirectory);

            _userSessions =
                SessionStorageUtilities.
                    TryLoadDataFromUserSessionsFile(
                        _applicationDirectory) ??
                new List<UserSession>();

            _loginHelper = new LoginHelper(
                _applicationDirectory,
                Constants.ApplicationName);
        }

        public async Task<UserSession> StartSession()
        {
            _activeUserSession = 
                await _loginHelper.LoginUser(_userSessions);

            //no session activated
            if (_activeUserSession == null)
            {
                throw new ArgumentException("Error starting a user session.");
            }

            string activeUserSessionDirectoryPath =
                SessionStorageUtilities.SetupUserMetadataSessionDirectory(
                    _applicationDirectory,
                    _activeUserSession.EmailAddress);

            _activeUserSession.UserDirectory =
                activeUserSessionDirectoryPath;

            var activeUserData =
                SessionStorageUtilities.TryLoadDataFromUserMetadataFile(
                    activeUserSessionDirectoryPath);

            if (activeUserData != null)
            {
                _activeUserSession.UserData = activeUserData;
            }

            _activeUserSession.UserData ??= new UserData
            {
                EmailAddress = _activeUserSession.EmailAddress
            };

            return _activeUserSession;
        }

        public async Task<UserSession> ChangeSession(string emailAddress)
        {
            var userSessionForProvidedEmail =
                _userSessions.Find(u => u.EmailAddress == emailAddress);

            if (userSessionForProvidedEmail != null)
            {
                _userSessions.ForEach(u => u.IsDefaultUser = false);
                userSessionForProvidedEmail.IsDefaultUser = true;
                return await StartSession();
            }

            await _loginHelper.LoginNewAccount(_userSessions);

            return await StartSession();
        }

        public async Task ForgetCurrentUser()
        {
            if (_activeUserSession == null)
            {
                return;
            }

            SessionStorageUtilities.DeleteUserMetadataSessionDirectory(
                _applicationDirectory,
                _activeUserSession.EmailAddress);

            var userSessionToDelete =
                _userSessions.Find(u => u.EmailAddress == 
                                        _activeUserSession.EmailAddress);

            if (userSessionToDelete != null)
            {
                await CredentialUtilities.RevokeCredential(
                    userSessionToDelete.UserCredential);

                _userSessions.Remove(userSessionToDelete);

                SaveSession();
            }

            _activeUserSession = null;
        }

        public void SaveSession()
        {
            SessionStorageUtilities.SaveSessionsData(_userSessions,
                _applicationDirectory);
        }

        public void SaveUserOwnedPlaylistsToUserSession(
            List<Playlist> playLists,
            string etag)
        {
            _activeUserSession.UserData.UserOwnedPlayLists
                ??= new Dictionary<string, UserPlayList>();
            _activeUserSession.UserData.UserOwnedPlaylistsETag =
                etag;

            if (playLists != null &&
                playLists.Count > 0)
            {
                foreach (var playList in playLists)
                {
                    if (!_activeUserSession.UserData.UserOwnedPlayLists.
                        ContainsKey(playList.Id))
                    {
                        _activeUserSession.UserData.UserOwnedPlayLists.Add(
                            playList.Id,
                            UserPlayList.ConvertYoutubePlaylistToUserPlaylist(playList)
                        );
                    }
                    else
                    {
                        Console.WriteLine("Playlist already added.");
                    }
                }
            }

            DownloadPlaylistsThumbnailsToUserDirectory(
                _activeUserSession.UserData.UserOwnedPlayLists
                    .Values.ToList());
        }

        public void SaveUserOwnedPlaylistsToUserSession(
            Dictionary<string, UserPlayList> alreadyLoadedPlaylists,
            List<Playlist> newPlayListsData,
            string etag)
        {
            alreadyLoadedPlaylists ??=
                new Dictionary<string, UserPlayList>();

            _activeUserSession.UserData.UserOwnedPlayLists
                = alreadyLoadedPlaylists;
            _activeUserSession.UserData.UserOwnedPlaylistsETag =
                etag;

            if (newPlayListsData != null &&
                newPlayListsData.Count > 0)
            {
                foreach (var playList in newPlayListsData)
                {
                    if (!_activeUserSession.UserData.UserOwnedPlayLists.
                        ContainsKey(playList.Id))
                    {
                        _activeUserSession.UserData.UserOwnedPlayLists.Add(
                            playList.Id,
                            UserPlayList.ConvertYoutubePlaylistToUserPlaylist(playList)
                        );
                    }
                    else
                    {
                        _activeUserSession.UserData.UserOwnedPlayLists[playList.Id] =
                            UserPlayList.ConvertYoutubePlaylistToUserPlaylist(playList);
                    }
                }

                DownloadPlaylistsThumbnailsToUserDirectory(
                    _activeUserSession.UserData.UserOwnedPlayLists
                        .Values
                        .Where(v => newPlayListsData.Any(
                            p => p.Id == v.Id))
                        .ToList());
            }
        }

        public void SaveUserContributorPlaylistToUserSession(
            Playlist playList)
        {
            _activeUserSession.UserData.UserContributorPlayLists
                ??= new Dictionary<string, UserPlayList>();

            if (playList != null)
            {
                if (!_activeUserSession.UserData.UserContributorPlayLists.
                    ContainsKey(playList.Id))
                {
                    var userPlaylist =
                        UserPlayList.ConvertYoutubePlaylistToUserPlaylist(
                            playList);

                    _activeUserSession.UserData.UserContributorPlayLists.Add(
                        playList.Id,
                        userPlaylist
                    );

                    DownloadPlaylistsThumbnailsToUserDirectory(
                        new List<UserPlayList>{userPlaylist});
                }
                else
                {
                    Console.WriteLine("Playlist already added.");
                }
            }
        }

        public void DeleteUserContributorPlaylistFromUserSession(
            UserPlayList playList)
        {
            if (playList != null)
            {
                _activeUserSession.UserData.UserContributorPlayLists.Remove(
                    playList.Id);
            }
        }

        public (Dictionary<string, UserPlayList>, Dictionary<string, UserPlayList>) 
            GetUserSessionPlaylists()
        {
            return (_activeUserSession.UserData?.UserOwnedPlayLists,
                _activeUserSession.UserData?.UserContributorPlayLists);
        }

        public void SavePlaylistVideosToUserSessionPlaylist(
            UserPlayList userPlayList,
            List<PlaylistItem> playlistItems,
            string eTag)
        {
            if (userPlayList == null ||
                playlistItems == null ||
                playlistItems.Count <= 0)
            {
                return;
            }

            foreach (var playListItem in playlistItems)
            {
                SavePlaylistVideoToUserSessionPlaylist(userPlayList,
                    playListItem);
            }

            DownloadPlaylistVideosThumbnailsToUserDirectory(
                userPlayList.PlayListVideos.Values.ToList());

            userPlayList.PlaylistVideosETag =
                eTag;
            userPlayList.PlayListVideosDataLoaded = true;
        }

        public void SavePlaylistVideosToUserSessionPlaylist(
            UserPlayList userPlaylist,
            Dictionary<string, UserPlayListVideo> alreadyLoadedVideos,
            List<PlaylistItem> newPlaylistItems,
            string etag)
        {
            alreadyLoadedVideos ??=
                new Dictionary<string, UserPlayListVideo>();

            userPlaylist.PlayListVideos = alreadyLoadedVideos;
            userPlaylist.PlaylistVideosETag = etag;

            if (newPlaylistItems != null &&
                newPlaylistItems.Count > 0)
            {
                foreach (var playListItem in newPlaylistItems)
                {
                    SavePlaylistVideoToUserSessionPlaylist(
                        userPlaylist,
                        playListItem);
                }

                DownloadPlaylistVideosThumbnailsToUserDirectory(
                    userPlaylist.PlayListVideos.Values
                        .Where(v => newPlaylistItems.Any(
                            p => p.Id == v.UniqueVideoIdInPlaylist))
                        .ToList());
            }

            userPlaylist.PlayListVideosDataLoaded = true;
        }

        public void SavePlaylistVideoToUserSessionPlaylist(
            UserPlayList userPlayList,
            PlaylistItem playListItem)
        {
            if (userPlayList == null ||
                playListItem?.Id == null)
            {
                return;
            }

            if (!userPlayList.PlayListVideos.ContainsKey(
                    playListItem.Id))
            {
                userPlayList.PlayListVideos.Add(
                    playListItem.Id,
                    UserPlayListVideo.ConvertPlayListItemToUserPlayListVideo(
                        playListItem));
            }
            else
            {
                userPlayList.PlayListVideos[playListItem.Id]
                    = UserPlayListVideo.ConvertPlayListItemToUserPlayListVideo(
                        playListItem);
            }
        }

        public void DeleteVideoFromUserSessionPlaylist(UserPlayList userPlaylist,
            UserPlayListVideo videoToDelete)
        {
            if (userPlaylist == null ||
                videoToDelete == null)
            {
                return;
            }

            long? videoToDeletePosition =
                videoToDelete.PositionInPlayList;

            if (videoToDelete.UniqueVideoIdInPlaylist != null &&
                userPlaylist.PlayListVideos?.ContainsKey(
                    videoToDelete.UniqueVideoIdInPlaylist) == true)
            {
                userPlaylist.PlayListVideos.Remove(
                    videoToDelete.UniqueVideoIdInPlaylist);

                //update positions of videos
                if (videoToDeletePosition != null)
                {
                    foreach (var userPlayListVideo in
                        userPlaylist.PlayListVideos.Values
                            .Where(v => v.PositionInPlayList > videoToDeletePosition))
                    {
                        userPlayListVideo.PositionInPlayList =
                            userPlayListVideo.PositionInPlayList - 1;
                    }
                }
            }
        }

        public void SaveChannelIdInUserSession(string channelId)
        {
            _activeUserSession.UserData.ChannelId = channelId;
        }

        public List<string> GetUserSessionEmails()
        {
            return 
                _userSessions?.Select(s => s.EmailAddress ?? "")
                .Distinct().ToList()
                   ?? new List<string>();
        }

        public string GetActiveUserEmail()
        {
            return _activeUserSession?.EmailAddress;
        }

        public void DownloadPlaylistsThumbnailsToUserDirectory(
            List<UserPlayList> userPlaylists)
        {
            string directoryPath =
                _activeUserSession.UserDirectory + "/" +
                Constants.PlaylistThumbnailsFolder;

            if (userPlaylists != null)
            {
                foreach (var userPlaylist in
                    userPlaylists)
                {
                    CommonUtilities.DownloadImageToUserDirectory(
                        directoryPath,
                        userPlaylist.Id,
                        userPlaylist.ThumbnailUrl,
                        out var imageFileName);

                    userPlaylist.
                            ThumbnailLocalPathFromUserDirectory =
                        "/" +
                        Constants.PlaylistThumbnailsFolder + "/" +
                        imageFileName;
                }
            }
        }

        public void DownloadPlaylistVideosThumbnailsToUserDirectory(
            List<UserPlayListVideo> userPlaylistVideos)
        {
            string directoryPath =
                _activeUserSession.UserDirectory + "/" +
                Constants.VideoThumbnailsFolder;

            if (userPlaylistVideos == null)
            {
                return;
            }

            foreach (var userPlaylistVideo in
                userPlaylistVideos)
            {
                if (!string.IsNullOrWhiteSpace(
                    userPlaylistVideo.ThumbnailLocalPathFromUserDirectory))
                {
                    //we don't want to download video image again 
                    //since video images don't change
                    continue;
                }

                try
                {
                    CommonUtilities.DownloadImageToUserDirectory(
                        directoryPath, 
                        userPlaylistVideo.ThumbnailUrl,
                        userPlaylistVideo.UniqueVideoIdInPlaylist,
                        out var imageFileName);

                    userPlaylistVideo.
                            ThumbnailLocalPathFromUserDirectory =
                        "/" +
                        Constants.VideoThumbnailsFolder + "/" +
                        imageFileName;
                }
                catch
                {
                    //
                }
            }
        }
    }
}
