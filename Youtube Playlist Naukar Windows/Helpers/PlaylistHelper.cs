using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3.Data;
using Youtube_Playlist_Naukar_Windows.Models;

namespace Youtube_Playlist_Naukar_Windows.Helpers
{
    public class PlaylistHelper
    {
        private static readonly PlaylistHelper 
            PlaylistHelperInstance =
                new PlaylistHelper();

        public static event EventHandler<
                UserOwnedPlaylistThumbnailReadyEventArgs>
            UserOwnedPlaylistThumbnailReady;

        public static event EventHandler<
                UserContributorPlaylistThumbnailReadyEventArgs>
            UserContributorPlaylistThumbnailReady;

        static PlaylistHelper()
        {
            SessionStorageManager.GetSessionManager.
                    PlaylistThumbnailUpdated +=
                NotifyUiForPlaylistThumbnailChange;
        }

        public static PlaylistHelper GetPlaylistHelper
        {
            get
            {
                return PlaylistHelperInstance;
            }
        }

        #region User Owned Playlists

        public async Task LoadUserOwnedPlaylists(
            string channelId,
            Dictionary<string, UserPlayList> alreadyLoadedPlaylists,
            string alreadyLoadedPlaylistsEtag,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(channelId))
            {
                return;
            }

            try
            {
                //playlists not loaded already
                if(alreadyLoadedPlaylists == null ||
                    alreadyLoadedPlaylists.Count <= 0 ||
                    string.IsNullOrWhiteSpace(
                        alreadyLoadedPlaylistsEtag))
                {
                    var playlistsResult =
                        await ApiClient.GetApiClient.GetPlayListsData(
                            cancellationToken,
                            channelId);

                    var playlists =
                        playlistsResult.Item1;

                    string eTag = playlistsResult.Item2;

                    if (playlists == null ||
                        playlists.Count <= 0)
                    {
                        return;
                    }

                    SessionStorageManager.GetSessionManager.
                        SaveUserOwnedPlaylistsToUserSession(
                            playlists, eTag);
                }
                else //playlists data already exists
                {
                    await RefreshUserOwnedPlaylists(channelId,
                        alreadyLoadedPlaylists, 
                        alreadyLoadedPlaylistsEtag,
                        cancellationToken);
                }                
            }
            catch
            {
                //
            }
        }

        public async Task RefreshUserOwnedPlaylists(
            string channelId, 
            Dictionary<string, UserPlayList> alreadyLoadedPlaylists,
            string alreadyLoadedPlaylistsEtag,
            CancellationToken cancellationToken)
        {
            alreadyLoadedPlaylists ??= 
                new Dictionary<string, UserPlayList>();

            (List<Playlist>, string) playlistsResult = (null, null);

            try
            {
                playlistsResult =
                    await ApiClient.GetApiClient
                        .GetPlayListsPartialData(channelId,
                            cancellationToken);
            }
            catch
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }

            var partialPlaylistsData =
                playlistsResult.Item1;

            string eTag = playlistsResult.Item2;

            //playlists do not exist anymore
            //so need to update data
            if (partialPlaylistsData == null ||
                partialPlaylistsData.Count <= 0)
            {
                alreadyLoadedPlaylists.Clear();
                SessionStorageManager.GetSessionManager.
                    SaveUserOwnedPlaylistsToUserSession(
                        partialPlaylistsData, eTag);
                return;
            }

            //need to compare playlists data
            if (eTag != alreadyLoadedPlaylistsEtag) 
            {
                List<string> idsOfplaylistsToLoad =
                    new List<string>();

                Dictionary<string, UserPlayList>
                    newPlaylistsData =
                        new Dictionary<string, UserPlayList>();

                foreach (var partialPlaylist in
                    partialPlaylistsData)
                {
                    if (alreadyLoadedPlaylists
                            .ContainsKey(partialPlaylist.Id) &&
                        partialPlaylist.ETag ==
                        alreadyLoadedPlaylists[
                                partialPlaylist.Id]
                            ?.PlaylistETag)
                    {
                        //same playlist item so can be 
                        //added directly
                        newPlaylistsData.Add(partialPlaylist.Id,
                            alreadyLoadedPlaylists[partialPlaylist.Id]);
                    }
                    else
                    {
                        idsOfplaylistsToLoad.Add(partialPlaylist.Id);
                        newPlaylistsData.Add(
                            partialPlaylist.Id,
                            new UserPlayList
                            {
                                Id = partialPlaylist.Id
                            });
                    }
                }

                //load new playlists
                List<Playlist> newPlaylists = null;

                if (idsOfplaylistsToLoad.Count > 0)
                {
                    try
                    {
                        newPlaylists =
                            await ApiClient.GetApiClient
                                .GetPlayListsData(
                                    cancellationToken,
                                    idsOfplaylistsToLoad);
                    }
                    catch
                    {
                        if(cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }
                    }
                }

                SessionStorageManager.GetSessionManager.
                    SaveUserOwnedPlaylistsToUserSession(
                        newPlaylistsData, 
                        newPlaylists ?? new List<Playlist>(),
                        eTag);
            }
        }

        public void DownloadUserOwnedPlaylistsThumbnails(
            Dictionary<string, UserPlayList> userOwnedPlaylists)
        {
            if (userOwnedPlaylists.Count > 0)
            {
                SessionStorageManager.GetSessionManager.
                    DownloadPlaylistThumbnailsInBackgroundAndNotifyMainThread(
                        userOwnedPlaylists.Values.ToList(), true);
            }
        }

        public Dictionary<string, UserPlayList>
            GetStoredUserOwnedPlaylists()
        {
            return
                SessionStorageManager.GetSessionManager.
                    GetUserOwnedPlaylistsFromSession();
        }

        #endregion

        #region Contributor Playlists

        public async Task<bool> AddContributorPlaylist(
            string playListId,
            CancellationToken cancellationToken)
        {
            try
            {
                var playlists =
                    await ApiClient.GetApiClient
                        .GetPlayListsData(
                            cancellationToken,
                            new List<string> { playListId });

                if (playlists != null &&
                    playlists.Count > 0 &&
                    playlists[0] != null)
                {
                    var contributorPlaylist =
                        playlists[0];

                    SessionStorageManager.GetSessionManager.
                        SaveUserContributorPlaylistToUserSession(
                            contributorPlaylist);

                    return true;
                }
            }
            catch
            {
                //
            }

            return false;
        }

        public async Task RefreshUserContributorPlaylists(
            Dictionary<string, UserPlayList> alreadyLoadedPlaylists,
            CancellationToken cancellationToken)
        {
            alreadyLoadedPlaylists ??=
                new Dictionary<string, UserPlayList>();

            List<Playlist> partialPlaylistsData = null;

            try
            {
                partialPlaylistsData =
                    await ApiClient.GetApiClient
                        .GetPlayListsPartialData(
                            alreadyLoadedPlaylists.Keys.ToList(),
                            cancellationToken);
            }
            catch
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }
            
            //playlists do not exist anymore
            //so need to update data
            if (partialPlaylistsData == null ||
                partialPlaylistsData.Count <= 0)
            {
                foreach (var alreadyLoadedPlaylist in
                    alreadyLoadedPlaylists)
                {
                    SessionStorageManager.GetSessionManager
                        .DeleteUserContributorPlaylistFromUserSession(
                            alreadyLoadedPlaylist.Value);
                }

                alreadyLoadedPlaylists.Clear();
                return;
            }

            //need to compare playlists data

            List<string> idsOfPlaylistsToLoad =
                new List<string>();

            Dictionary<string, UserPlayList>
                newPlaylistsData =
                    new Dictionary<string, UserPlayList>();

            foreach (var partialPlaylist in
                partialPlaylistsData)
            {
                if (!alreadyLoadedPlaylists.ContainsKey(
                    partialPlaylist.Id))
                {
                    continue;
                }

                if (partialPlaylist.ETag ==
                    alreadyLoadedPlaylists[
                            partialPlaylist.Id]
                        ?.PlaylistETag)
                {
                    //same playlist item so can be 
                    //added directly
                    newPlaylistsData.Add(partialPlaylist.Id,
                        alreadyLoadedPlaylists[partialPlaylist.Id]);
                }
                else
                {
                    idsOfPlaylistsToLoad.Add(partialPlaylist.Id);
                    newPlaylistsData.Add(
                        partialPlaylist.Id,
                        new UserPlayList
                        {
                            Id = partialPlaylist.Id
                        });
                }
            }

            //load new playlists
            List<Playlist> playlists = null;

            if (idsOfPlaylistsToLoad.Count > 0)
            {
                try
                {
                    playlists =
                        await ApiClient.GetApiClient
                            .GetPlayListsData(
                                cancellationToken,
                                idsOfPlaylistsToLoad);
                }
                catch
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                }
            }

            SessionStorageManager.GetSessionManager
                .SaveUserContributorPlaylistsToUserSession(
                    newPlaylistsData,
                    playlists ?? new List<Playlist>());
        }

        public void RemoveContributorPlaylistEntry(
            string playlistId)
        {
            var userContributorPlaylists =
                SessionStorageManager.GetSessionManager.
                    GetUserContributorPlaylistsFromSession();

            if (userContributorPlaylists?.ContainsKey(
                playlistId) == true)
            {
                SessionStorageManager.GetSessionManager.
                    DeleteUserContributorPlaylistFromUserSession(
                        userContributorPlaylists[playlistId]);
            }
        }

        public void DownloadUserContributorPlaylistsThumbnails(
            Dictionary<string, UserPlayList> userContributorPlaylists)
        {
            if (userContributorPlaylists.Count > 0)
            {
                SessionStorageManager.GetSessionManager.
                    DownloadPlaylistThumbnailsInBackgroundAndNotifyMainThread(
                        userContributorPlaylists.Values.ToList(), false);
            }
        }

        public Dictionary<string, UserPlayList> 
            GetStoredUserContributorPlaylists()
        {
            return
                SessionStorageManager.GetSessionManager.
                    GetUserContributorPlaylistsFromSession();
        }

        #endregion

        #region  Utilities

        private static void NotifyUiForPlaylistThumbnailChange(
            object sender,
            PlaylistThumbnailUpdatedEventArgs eventArgs)
        {
            var isUserOwnedPlaylist =
                eventArgs.IsOwnerPlaylist;

            if (isUserOwnedPlaylist)
            {
                if (UserOwnedPlaylistThumbnailReady != null)
                {
                    UserOwnedPlaylistThumbnailReady(null,
                        new UserOwnedPlaylistThumbnailReadyEventArgs
                        {
                            PlaylistId =
                                eventArgs.PlaylistId,
                            PlaylistImagePathFromCustomerDirectory =
                                eventArgs.PlaylistImagePathFromCustomerDirectory
                        });
                }
            }
            else
            {
                if (UserContributorPlaylistThumbnailReady != null)
                {
                    UserContributorPlaylistThumbnailReady(null,
                        new UserContributorPlaylistThumbnailReadyEventArgs
                        {
                            PlaylistId =
                                eventArgs.PlaylistId,
                            PlaylistImagePathFromCustomerDirectory =
                                eventArgs.PlaylistImagePathFromCustomerDirectory
                        });
                }
            }
        }

        #endregion
    }
}
