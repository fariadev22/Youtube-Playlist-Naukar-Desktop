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
                notifyUIForPlaylistThumbnailChange;
        }

        public static PlaylistHelper GetPlaylistHelper
        {
            get
            {
                return PlaylistHelperInstance;
            }
        }

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

            var playlistsResult =
                await ApiClient.GetApiClient
                    .GetUserPlayListsPartialData(channelId,
                        cancellationToken);

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
                            null);
                    }
                }

                //load new playlists
                List<Playlist> newPlaylists = null;

                if (idsOfplaylistsToLoad.Count > 0)
                {
                    playlistsResult =
                        await ApiClient.GetApiClient
                            .GetPlayListsData(
                                cancellationToken,
                                channelId,
                                allPlaylistIds: idsOfplaylistsToLoad);

                    newPlaylists =
                        playlistsResult.Item1;

                    eTag = playlistsResult.Item2;
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
                        userOwnedPlaylists
                            .Values.ToList(), true);
            }
        }

        public Dictionary<string, UserPlayList>
            GetStoredUserOwnedPlaylists()
        {
            return
                SessionStorageManager.GetSessionManager.
                    GetUserOwnedPlaylistsFromSession();
        }

        public async Task<bool> AddContributorPlaylist(
            string playListId,
            CancellationToken cancellationToken)
        {
            try
            {
                var playlistResult =
                    await ApiClient.GetApiClient
                        .GetPlayListsData(
                            cancellationToken,
                            allPlaylistIds:
                            new List<string> { playListId });

                var playlists =
                    playlistResult.Item1;

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

            var partialPlaylistsData =
                await ApiClient.GetApiClient
                    .GetUserPlayListsPartialData(
                        alreadyLoadedPlaylists.Keys.ToList(),
                        cancellationToken);

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

            List<string> idsOfplaylistsToLoad =
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
                    idsOfplaylistsToLoad.Add(partialPlaylist.Id);
                    newPlaylistsData.Add(
                        partialPlaylist.Id,
                        null);
                }
            }

            //load new playlists
            if (idsOfplaylistsToLoad.Count > 0)
            {
                var playlistsResult =
                    await ApiClient.GetApiClient
                        .GetPlayListsData(
                            cancellationToken,
                            allPlaylistIds: idsOfplaylistsToLoad);

                var playlists =
                    playlistsResult.Item1;

                SessionStorageManager.GetSessionManager
                    .SaveUserContributorPlaylistsToUserSession(
                        newPlaylistsData,
                        playlists);
            }
        }

        public void RemoveContributorPlaylistEntry(
            string playlistId)
        {
            var userContributorPlaylists =
                SessionStorageManager.GetSessionManager.
                    GetUserContributorPlaylistsFromSession();

            if (userContributorPlaylists?.ContainsKey(playlistId) == true)
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
                        userContributorPlaylists
                            .Values.ToList(), false);
            }
        }

        public Dictionary<string, UserPlayList> 
            GetStoredUserContributorPlaylists()
        {
            return
                SessionStorageManager.GetSessionManager.
                    GetUserContributorPlaylistsFromSession();
        }

        private static void notifyUIForPlaylistThumbnailChange(
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
    }
}
