using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            string alreadyLoadedPlaylistsEtag)
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
                        alreadyLoadedPlaylistsEtag);
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
            string alreadyLoadedPlaylistsEtag)
        {
            alreadyLoadedPlaylists ??= 
                new Dictionary<string, UserPlayList>();

            var playlistsResult =
                await ApiClient.GetApiClient
                    .GetUserPlayListsPartialData(channelId);

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
                if (idsOfplaylistsToLoad.Count > 0)
                {
                    playlistsResult =
                        await ApiClient.GetApiClient
                            .GetPlayListsData(
                                channelId,
                                playlistIds: idsOfplaylistsToLoad);

                    var playlists =
                        playlistsResult.Item1;

                    eTag = playlistsResult.Item2;

                    SessionStorageManager.GetSessionManager.
                        SaveUserOwnedPlaylistsToUserSession(
                            newPlaylistsData, playlists, eTag);
                }
            }
        }

        public async Task AddContributorPlaylist(
            string playListId)
        {
            try
            {
                var playlistResult =
                    await ApiClient.GetApiClient
                        .GetPlayListsData(playlistIds:
                            new List<string> { playListId });

                var playlists =
                    playlistResult.Item1;

                if (playlists != null &&
                    playlists.Count > 0 &&
                    playlists[0] != null)
                {
                    var contributorPlaylist =
                        playlists[0];

                    Console.WriteLine(
                        "INFO: Playlist with title '" +
                        contributorPlaylist.Snippet?.Title +
                        "' added successfully.");

                    SessionStorageManager.GetSessionManager.
                        SaveUserContributorPlaylistToUserSession(
                            contributorPlaylist);
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Unable to add " +
                                  "selected contributor playlist.");
            }
        }

        public async Task RefreshUserContributorPlaylists(
            Dictionary<string, UserPlayList> alreadyLoadedPlaylists)
        {
            alreadyLoadedPlaylists ??=
                new Dictionary<string, UserPlayList>();

            var playlistsResult =
                await ApiClient.GetApiClient
                    .GetUserPlayListsPartialData(
                        alreadyLoadedPlaylists.Keys.ToList());

            var partialPlaylistsData =
                playlistsResult.Item1;

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
                playlistsResult =
                    await ApiClient.GetApiClient
                        .GetPlayListsData(
                            playlistIds: idsOfplaylistsToLoad);

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
            Console.WriteLine("INFO: Removing playlist entry...");

            var userContributorPlaylists =
                SessionStorageManager.GetSessionManager.GetUserSessionPlaylists().Item2;

            if (userContributorPlaylists?.ContainsKey(playlistId) == true)
            {
                SessionStorageManager.GetSessionManager.
                    DeleteUserContributorPlaylistFromUserSession(
                        userContributorPlaylists[playlistId]);

                Console.WriteLine("INFO: Selected contributor playlist " +
                                  "has been successfully removed.");
            }
            else
            {
                Console.WriteLine(
                    "WARN: Selected playlist for removal " +
                    "does not exist in the currently loaded " +
                    "contributor playlists already.");
            }
        }

        public void DownloadUserOwnedPlaylistsThumbnails(
            Dictionary<string, UserPlayList> userOwnedPlaylists)
        {
            if (userOwnedPlaylists.Count > 0)
            {
                SessionStorageManager.GetSessionManager.
                    DownloadPlaylistThumbnailsInBackgroundAndInformUI(
                        userOwnedPlaylists
                            .Values.ToList(), true);
            }
        }

        public void DownloadUserContributorPlaylistsThumbnails(
            Dictionary<string, UserPlayList> userContributorPlaylists)
        {
            if (userContributorPlaylists.Count > 0)
            {
                SessionStorageManager.GetSessionManager.
                    DownloadPlaylistThumbnailsInBackgroundAndInformUI(
                        userContributorPlaylists
                            .Values.ToList(), false);
            }
        }

        public Dictionary<string, UserPlayList> GetStoredUserOwnedPlaylists()
        {
            return 
                SessionStorageManager.GetSessionManager.
                    GetUserSessionPlaylists().Item1;
        }

        public Dictionary<string, UserPlayList> GetStoredUserContributorPlaylists()
        {
            return
                SessionStorageManager.GetSessionManager.
                    GetUserSessionPlaylists().Item2;
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
