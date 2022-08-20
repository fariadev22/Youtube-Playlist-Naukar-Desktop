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

        static PlaylistHelper()
        {

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

                    SessionManager.GetSessionManager.
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
                SessionManager.GetSessionManager.
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

                    SessionManager.GetSessionManager.
                        SaveUserOwnedPlaylistsToUserSession(
                            newPlaylistsData, playlists, eTag);
                }
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
                    SessionManager.GetSessionManager
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

                SessionManager.GetSessionManager
                    .SaveUserContributorPlaylistsToUserSession(
                        newPlaylistsData,
                        playlists);
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

                    SessionManager.GetSessionManager.
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

        public void RemoveContributorPlaylistEntry(
            string playlistId)
        {
            Console.WriteLine("INFO: Removing playlist entry...");

            var userContributorPlaylists =
                SessionManager.GetSessionManager.GetUserSessionPlaylists().Item2;

            if (userContributorPlaylists?.ContainsKey(playlistId) == true)
            {
                SessionManager.GetSessionManager.DeleteUserContributorPlaylistFromUserSession(
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
    }
}
