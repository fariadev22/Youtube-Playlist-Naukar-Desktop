using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;
using Youtube_Playlist_Naukar_Windows.Helpers.BackgroundWorkers;
using Youtube_Playlist_Naukar_Windows.Models;
using Youtube_Playlist_Naukar_Windows.Utilities;

namespace Youtube_Playlist_Naukar_Windows.Helpers
{
    public class SessionStorageManager
    {
        private static readonly SessionStorageManager 
            SessionManagerInstance =
            new SessionStorageManager();

        public event EventHandler<PlaylistThumbnailUpdatedEventArgs> 
            PlaylistThumbnailUpdated;

        public event EventHandler<PlaylistVideoThumbnailUpdatedEventArgs>
            PlaylistVideoThumbnailUpdated;

        static SessionStorageManager()
        {

        }

        public static SessionStorageManager GetSessionManager
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

        public SessionStorageManager()
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

            SessionStorageBackgroundWorker.
                GetSessionStorageBackgroundWorker.
                SetApplicationDirectory(_applicationDirectory);
        }

        #region Users

        public async Task<UserSession> StartSession(
            CancellationToken cancellationToken)
        {
            _activeUserSession = 
                await _loginHelper.LoginUser(
                    _userSessions, cancellationToken);

            //no session activated
            if (_activeUserSession == null)
            {
                return null;
            }

            string activeUserSessionDirectoryPath =
                SessionStorageUtilities.
                    SetupUserMetadataSessionDirectory(
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

            SaveSession();

            return _activeUserSession;
        }

        public async Task<UserSession> ChangeSession(
            CancellationToken cancellationToken,
            string emailAddress = null)
        {
            var userSessionForProvidedEmail =
                string.IsNullOrWhiteSpace(emailAddress)
                    ? null
                    : _userSessions.Find(u => 
                        u.EmailAddress == emailAddress);

            if (userSessionForProvidedEmail != null)
            {
                _userSessions.ForEach(u => u.IsDefaultUser = false);
                userSessionForProvidedEmail.IsDefaultUser = true;
                return await StartSession(cancellationToken);
            }

            await _loginHelper.LoginNewAccount(_userSessions,
                cancellationToken);

            return await StartSession(cancellationToken);
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
                _userSessions.Find(
                    u => u.EmailAddress == 
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

        public void SaveChannelIdInUserSession(
            string channelId)
        {
            _activeUserSession.UserData.ChannelId = channelId;

            SaveSession();
        }

        public List<string> GetUserSessionEmails()
        {
            return
                _userSessions?.Select(s => s.EmailAddress ?? "")
                    .Distinct().ToList()
                ?? new List<string>();
        }

        #endregion

        #region Playlists

        public void SaveUserOwnedPlaylistsToUserSession(
            Dictionary<string, UserPlayList> playLists,
            string etag)
        {
            _activeUserSession.UserData.UserOwnedPlayLists =
                playLists;

            _activeUserSession.UserData.UserOwnedPlaylistsETag =
                etag;

            SaveSession();
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
            }

            SaveSession();
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
                }
                else
                {
                    Console.WriteLine(@"Playlist already added.");
                }
            }

            SaveSession();
        }

        public void SaveUserContributorPlaylistsToUserSession(
            Dictionary<string, UserPlayList> alreadyLoadedPlaylists,
            List<Playlist> newPlayListsData)
        {
            alreadyLoadedPlaylists ??=
                new Dictionary<string, UserPlayList>();

            _activeUserSession.UserData.UserContributorPlayLists
                = alreadyLoadedPlaylists;

            if (newPlayListsData != null &&
                newPlayListsData.Count > 0)
            {
                foreach (var playList in newPlayListsData)
                {
                    if (!_activeUserSession.UserData.UserContributorPlayLists.
                        ContainsKey(playList.Id))
                    {
                        _activeUserSession.UserData.UserContributorPlayLists.Add(
                            playList.Id,
                            UserPlayList.ConvertYoutubePlaylistToUserPlaylist(playList)
                        );
                    }
                    else
                    {
                        _activeUserSession.UserData.UserContributorPlayLists[playList.Id] =
                            UserPlayList.ConvertYoutubePlaylistToUserPlaylist(playList);
                    }
                }
            }

            SaveSession();
        }

        public void DeleteUserContributorPlaylistFromUserSession(
            UserPlayList playList)
        {
            if (playList != null &&
                _activeUserSession.UserData.
                    UserContributorPlayLists.ContainsKey(playList.Id))
            {
                _activeUserSession.UserData.
                    UserContributorPlayLists.Remove(
                        playList.Id);
            }

            SaveSession();
        }

        public Dictionary<string, UserPlayList> 
            GetUserOwnedPlaylistsFromSession()
        {
            return _activeUserSession.UserData?.UserOwnedPlayLists;
        }

        public Dictionary<string, UserPlayList>
            GetUserContributorPlaylistsFromSession()
        {
            return _activeUserSession.UserData?.UserContributorPlayLists;
        }

        public void DownloadPlaylistThumbnailsInBackgroundAndNotifyMainThread(
            List<UserPlayList> userPlaylists,
            bool isUserOwnedPlaylists)
        {
            if (isUserOwnedPlaylists)
            {
                PlaylistsBackgroundWorker.GetPlaylistsBackgroundWorker.
                    RunOwnedPlaylistsBackgroundWorker(
                        DownloadPlaylistsThumbnailsToUserDirectory,
                        userPlaylists);
            }
            else
            {
                PlaylistsBackgroundWorker.GetPlaylistsBackgroundWorker.
                    RunContributorPlaylistsBackgroundWorker(
                        DownloadPlaylistsThumbnailsToUserDirectory,
                        userPlaylists);
            }
        }

        #endregion

        #region Playlist Videos

        public void SavePlaylist(
            UserPlayList userPlaylist,
            string playlistVideosETag)
        {
            userPlaylist.PlaylistVideosETag = playlistVideosETag;
            SaveSession();
        }

        /// <summary>
        /// Depending on the sort mechanism of the playlist, a
        /// new video can either get added at the start of
        /// playlist or end or any random user specified position.
        /// This can result in changes to positions of other videos.
        /// So we need to be vary of that.
        /// </summary>
        public void AddNewVideoToUserSessionPlaylist(
            UserPlayList userPlayList,
            PlaylistItem playListItem,
            Dictionary<string, string> playlistItemsVideoDuration)
        {
            SavePlaylistVideoToUserSessionPlaylist(
                userPlayList,
                playListItem,
                playlistItemsVideoDuration);

            var newVideo =
                userPlayList.PlayListVideos[playListItem.Id];

            //update positions of videos
            if (newVideo.PositionInPlayList != null)
            {
                //new video not added at the end so
                //we need to update positions
                if (newVideo.PositionInPlayList !=
                    userPlayList.PlayListVideos.Count - 1)
                {
                    foreach (var userPlayListVideo in
                        userPlayList.PlayListVideos.Values
                            .Where(v =>
                                v.UniqueVideoIdInPlaylist !=
                                newVideo.UniqueVideoIdInPlaylist &&
                                v.PositionInPlayList >=
                                newVideo.PositionInPlayList))
                    {
                        userPlayListVideo.PositionInPlayList =
                            userPlayListVideo.PositionInPlayList + 1;
                    }
                }
            }

            SaveSession();
        }

        public void DeleteVideoFromUserSessionPlaylist(
            UserPlayList userPlaylist,
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
                            .Where(v => 
                                v.PositionInPlayList > videoToDeletePosition))
                    {
                        userPlayListVideo.PositionInPlayList =
                            userPlayListVideo.PositionInPlayList - 1;
                    }
                }
            }

            SaveSession();
        }

        public void DownloadPlaylistVideoThumbnailsInBackgroundAndNotifyMainThread(
            string playlistId,
            List<UserPlayListVideo> playlistVideos)
        {
            PlaylistBackgroundWorker.GetPlaylistBackgroundWorker.
                AddAndStartBackgroundWorker(
                    playlistId,
                    new PlaylistBackgroundWorkerData
                    {
                        PlaylistId = playlistId,
                        UserPlaylistVideos = playlistVideos,
                        PlaylistWorkHander = 
                            DownloadPlaylistVideosThumbnailsToUserDirectory
                    });
        }

        #endregion

        #region Helpers
        private void SavePlaylistVideoToUserSessionPlaylist(
            UserPlayList userPlayList,
            PlaylistItem playListItem,
            Dictionary<string, string> playlistItemsVideoDuration)
        {
            if (userPlayList == null ||
                playListItem?.Id == null)
            {
                return;
            }

            var videoId =
                playListItem.Snippet?.ResourceId?.VideoId;

            var duration =
                !string.IsNullOrWhiteSpace(videoId) &&
                playlistItemsVideoDuration?.ContainsKey(videoId) == true
                    ? playlistItemsVideoDuration[videoId]
                    : null;

            var userPlaylistVideo =
                UserPlayListVideo.ConvertPlayListItemToUserPlayListVideo(
                    playListItem, duration);

            if (!userPlayList.PlayListVideos.ContainsKey(
                playListItem.Id))
            {
                userPlayList.PlayListVideos.Add(
                    playListItem.Id,
                    userPlaylistVideo);
            }
            else
            {
                userPlayList.PlayListVideos[playListItem.Id]
                    = userPlaylistVideo;
            }

            //it is null for deleted/private videos
            if (userPlaylistVideo.PositionInPlayList == null)
            {
                var userPlaylistVideos =
                    userPlayList.PlayListVideos.Values.ToList();

                int currentVideoIndex = 
                    userPlaylistVideos.IndexOf(userPlaylistVideo);

                if (currentVideoIndex == 0)
                {
                    userPlaylistVideo.PositionInPlayList = 0;
                }
                else if (currentVideoIndex > 0 &&
                         (currentVideoIndex - 1) > 0)
                {
                    userPlaylistVideo.PositionInPlayList =
                        userPlaylistVideos.
                            ElementAt(currentVideoIndex - 1)?.
                            PositionInPlayList;
                }
                    
            }

            SaveSession();
        }

        private void DownloadPlaylistsThumbnailsToUserDirectory(
            object sender, DoWorkEventArgs e)
        {
            (List<UserPlayList>, bool) userPlaylistThumbnailsData =
                ((List<UserPlayList>, bool))e.Argument;

            if (userPlaylistThumbnailsData.Item1 == null)
            {
                return;
            }

            var userPlaylists =
                userPlaylistThumbnailsData.Item1;

            var areUserOwnedPlaylists =
                userPlaylistThumbnailsData.Item2;

            string directoryPath =
                _activeUserSession.UserDirectory;

            foreach (var userPlaylist in
                userPlaylists)
            {
                if (userPlaylist.Thumbnail == null)
                {
                    continue;
                }

                if (PlaylistsBackgroundWorker.
                    GetPlaylistsBackgroundWorker.
                        IsBackgroundWorkForPlaylistsCancelled(
                            areUserOwnedPlaylists))
                {
                    break;
                }

                if (!userPlaylist.Thumbnail.IsDownloaded ||
                    !File.Exists(
                        directoryPath + "/" + 
                        userPlaylist.Thumbnail.LocalPathFromUserDirectory))
                {
                    CommonUtilities.DownloadImageToUserDirectory(
                        directoryPath,
                        userPlaylist.Thumbnail);

                    userPlaylist.Thumbnail.IsDownloaded = true;

                    SaveSession();
                }
                
                if (PlaylistThumbnailUpdated != null)
                {
                    PlaylistThumbnailUpdated(null,
                        new PlaylistThumbnailUpdatedEventArgs
                        {
                            PlaylistId = userPlaylist.Id,
                            PlaylistImagePathFromCustomerDirectory =
                                userPlaylist.Thumbnail.
                                    LocalPathFromUserDirectory,
                            IsOwnerPlaylist =
                                areUserOwnedPlaylists
                        });
                }
            }
        }

        private void DownloadPlaylistVideosThumbnailsToUserDirectory(
            object sender, DoWorkEventArgs e)
        {
            (string, List<UserPlayListVideo>) userPlaylistVideosRequest =
                ((string, List<UserPlayListVideo>)) e.Argument;

            string playlistId =
                userPlaylistVideosRequest.Item1;

            List<UserPlayListVideo> userPlaylistVideos =
                userPlaylistVideosRequest.Item2;

            if (userPlaylistVideos == null)
            {
                return;
            }

            string directoryPath = 
                _activeUserSession.UserDirectory;

            foreach (var userPlaylistVideo in
                userPlaylistVideos)
            {
                if (userPlaylistVideo.Thumbnail == null)
                {
                    continue;
                }
                
                if (PlaylistBackgroundWorker.GetPlaylistBackgroundWorker.
                    IsBackgroundWorkForPlaylistCancelled(
                        playlistId))
                {
                    break;
                }

                if (!userPlaylistVideo.Thumbnail.IsDownloaded ||
                    !File.Exists(
                        directoryPath + "/" +
                        userPlaylistVideo.Thumbnail.LocalPathFromUserDirectory))
                {
                    CommonUtilities.DownloadImageToUserDirectory(
                        directoryPath,
                        userPlaylistVideo.Thumbnail);

                    userPlaylistVideo.Thumbnail.IsDownloaded = true;

                    SaveSession();
                }

                try
                {
                    if (PlaylistVideoThumbnailUpdated != null)
                    {
                        PlaylistVideoThumbnailUpdated(null,
                            new PlaylistVideoThumbnailUpdatedEventArgs
                            {
                                PlaylistId = playlistId,
                                VideoId = userPlaylistVideo.UniqueVideoIdInPlaylist,
                                PlaylistVideoImagePathFromCustomerDirectory = 
                                    userPlaylistVideo.Thumbnail.
                                        LocalPathFromUserDirectory
                            });
                    }
                }
                catch
                {
                    //
                }
            }

            e.Result = playlistId;
        }

        private void SaveSession()
        {
            SessionStorageBackgroundWorker.
                GetSessionStorageBackgroundWorker.UpdateUserSessions(
                    _userSessions);
        }

        #endregion
    }
}
