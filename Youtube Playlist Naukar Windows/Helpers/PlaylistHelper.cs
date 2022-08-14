using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FuzzySharp;
using Google.Apis.YouTube.v3.Data;
using Youtube_Playlist_Naukar_Windows.Models;
using Youtube_Playlist_Naukar_Windows.Utilities;

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

        public async Task LoadPlaylist(
            UserPlayList userPlaylist)
        {
            if (userPlaylist == null)
            {
                Console.WriteLine("ERROR: No playlist found.");
                return;
            }

            if (userPlaylist.PlayListVideosDataLoaded)
            {
                Console.WriteLine("INFO: Playlist opened.");
                return;
            }

            try
            {
                //no videos loaded before
                if (string.IsNullOrWhiteSpace(
                        userPlaylist.PlaylistVideosETag) ||
                    userPlaylist.PlayListVideos == null ||
                    userPlaylist.PlayListVideos.Count <= 0)
                {
                    var playlistVideosResult =
                        await ApiClient.GetApiClient.GetPlaylistVideos(
                            userPlaylist);

                    var playlistVideos = playlistVideosResult.Item1;

                    var etag =
                        playlistVideosResult.Item2;

                    if (playlistVideos != null)
                    {
                        SessionManager.GetSessionManager.
                            SavePlaylistVideosToUserSessionPlaylist(
                                userPlaylist, playlistVideos, etag);
                    }
                }
                else
                {
                    await RefreshPlaylistVideos(userPlaylist);
                }
            }
            catch
            {
                Console.WriteLine(
                    "ERROR: Unable to open selected playlist.");
            }
        }

        public async Task RefreshPlaylistVideos(
            UserPlayList userPlaylist)
        {
            var alreadyLoadedVideos =
                userPlaylist.PlayListVideos ??
                new Dictionary<string, UserPlayListVideo>();

            var playlistVideosResult =
                await ApiClient.GetApiClient.GetPlaylistVideosPartialData(
                    userPlaylist);

            var partialVideosData =
                playlistVideosResult.Item1;

            var eTag =
                playlistVideosResult.Item2;

            //videos do not exist anymore
            //so need to update data
            if (partialVideosData == null ||
                partialVideosData.Count <= 0)
            {
                userPlaylist.PlayListVideos?.Clear();
                SessionManager.GetSessionManager.
                    SavePlaylistVideosToUserSessionPlaylist(
                        userPlaylist, partialVideosData, eTag);
                return;
            }

            //need to compare playlists data
            if (eTag != userPlaylist.PlaylistVideosETag)
            {
                List<string> idsOfVideosToLoad =
                    new List<string>();

                Dictionary<string, UserPlayListVideo>
                    newVideosData =
                        new Dictionary<string, UserPlayListVideo>();

                foreach (var partialVideo in
                    partialVideosData)
                {
                    if (alreadyLoadedVideos
                            .ContainsKey(partialVideo.Id) &&
                        partialVideo.ETag ==
                        alreadyLoadedVideos[
                                partialVideo.Id]
                            ?.ETag)
                    {
                        //same video item so can be 
                        //added directly
                        newVideosData.Add(
                            partialVideo.Id,
                            alreadyLoadedVideos[partialVideo.Id]);
                    }
                    else
                    {
                        idsOfVideosToLoad.Add(partialVideo.Id);
                        newVideosData.Add(
                            partialVideo.Id,
                            null);
                    }
                }

                //load new videos
                if (idsOfVideosToLoad.Count > 0)
                {
                    playlistVideosResult =
                        await ApiClient.GetApiClient
                            .GetPlaylistVideos(
                                userPlaylist,
                                videoIds: idsOfVideosToLoad);

                    var videos =
                        playlistVideosResult.Item1;

                    eTag = playlistVideosResult.Item2;

                    SessionManager.GetSessionManager.
                        SavePlaylistVideosToUserSessionPlaylist(
                            userPlaylist, newVideosData, videos, eTag);
                }
            }
        }

        public List<UserPlayListVideo> SearchVideoInPlayList(
            string searchQuery,
            UserPlayList userPlayList)
        {
            if (userPlayList == null ||
                string.IsNullOrWhiteSpace(searchQuery))
            {
                return new List<UserPlayListVideo>();
            }

            Dictionary<UserPlayListVideo, int> videoAndSearchScores =
                new Dictionary<UserPlayListVideo, int>();

            if (userPlayList.PlayListVideos?.Values != null)
            {
                foreach (var playlistVideo in userPlayList.PlayListVideos.Values)
                {
                    int score = Fuzz.PartialRatio(
                        searchQuery.ToLower(), playlistVideo.Title.ToLower());

                    if (score >= 80)
                    {
                        videoAndSearchScores.Add(playlistVideo, score);
                    }
                }
            }

            //most relevant stuff on top
            var results =
                videoAndSearchScores.OrderByDescending(v =>
                    v.Value).Select(v => v.Key).ToList();

            return results;
        }

        public async Task<List<string>> AddVideoOrVideosToPlayList(
            string youTubeUrlsString,
            UserPlayList userPlayList)
        {
            List<string> urls =
                youTubeUrlsString.Split('\n').
                    Select(url => url.Trim()).ToList();

            List<string> messages = new List<string>();

            foreach (var url in urls)
            {
                bool isValidUrl =
                    CommonUtilities.TryGetVideoIdFromYoutubeUrl(
                        url, out string videoId);

                if (!isValidUrl)
                {
                    messages.Add(
                        "Unable to add video with URL " +
                        "[" + url + "] -> " +
                        "Invalid URL");

                    continue;
                }

                if (userPlayList.PlayListVideos?.Values.ToList()
                    .Any(v => v.VideoId == videoId) == true)
                {
                    messages.Add(
                        "Video with URL [" + url + "] " +
                        "already exists in the playlist.");
                }
                else
                {
                    try
                    {
                        var playListItem =
                            await ApiClient.GetApiClient.AddVideoToPlayList(
                                videoId, userPlayList);

                        if (playListItem == null)
                        {
                            messages.Add(
                                "Unable to add video with URL [" + url + "]");
                        }
                        else
                        {
                            messages.Add(
                                "Successfully added video with URL [" + url + "]");

                            SessionManager.GetSessionManager.
                                SavePlaylistVideoToUserSessionPlaylist(
                                    userPlayList, playListItem);
                        }
                    }
                    catch
                    {
                        messages.Add(
                            "Unable to add video with URL [" + url + "]");
                    }
                }
            }

            return messages;
        }

        public List<List<UserPlayListVideo>> GetPlaylistDuplicates(
            UserPlayList playlist)
        {
            if (playlist != null &&
                playlist.PlayListVideosDataLoaded &&
                playlist.PlayListVideos != null)
            {
                return 
                    playlist.PlayListVideos.Values.GroupBy(v => v.VideoId)
                        .Select(grp => grp.ToList())
                        .Where(grp => grp.Count > 1)
                        .ToList();
            }

            return new List<List<UserPlayListVideo>>();
        }

        public async Task DeleteVideoFromPlaylist(
            UserPlayList userPlaylist,
            UserPlayListVideo videoToDelete)
        {
            if (userPlaylist == null ||
                videoToDelete == null)
            {
                return;
            }

            Console.WriteLine("INFO: Deleting video...");

            try
            {
                long? videoToDeleteIndex = 
                    videoToDelete.PositionInPlayList;

                await ApiClient.GetApiClient.DeletePlaylistVideo(
                    videoToDelete.UniqueVideoIdInPlaylist);

                SessionManager.GetSessionManager.DeleteVideoFromUserSessionPlaylist(
                    userPlaylist,
                    videoToDelete);

                Console.WriteLine("INFO: Video successfully " +
                                  "removed from playlist.");
            }
            catch
            {
                Console.WriteLine("ERROR: Unable to remove video " +
                                  "from playlist successfully.");
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
