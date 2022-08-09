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
            string channelId)
        {
            if (string.IsNullOrWhiteSpace(channelId))
            {
                Console.WriteLine("ERROR: No channel found to load playlists from.");
                return;
            }

            try
            {
                var playlists = await ApiClient.GetApiClient.GetUserPlayListsData(
                    channelId);

                if (playlists == null ||
                    playlists.Count <= 0)
                {
                    Console.WriteLine(
                        "ERROR: No playlist exists for the current user. " +
                        "Add a playlist to continue.");
                    return;
                }

                SessionManager.GetSessionManager.SaveUserOwnedPlaylistsToUserSession(
                    playlists);
            }
            catch
            {
                Console.WriteLine("ERROR: Unable to fetch playlists.");
            }
        }

        public async Task LoadPlaylist(
            UserPlayList userPlaylist)
        {
            Console.WriteLine("INFO: Opening playlist.....");

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
                var playlistVideos =
                    await ApiClient.GetApiClient.GetPlaylistVideos(userPlaylist);

                Console.WriteLine("INFO: Playlist opened. " +
                                  "Total " + 
                                  (playlistVideos?.Count ?? 0) + 
                                  " videos found.");

                if (playlistVideos != null)
                {
                    SessionManager.GetSessionManager.SavePlaylistVideosToUserSessionPlaylist(
                        userPlaylist, playlistVideos);
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Unable to open selected playlist.");
            }
        }

        public async Task RefreshPlaylist(
            UserPlayList userPlaylist)
        {
            Console.WriteLine("INFO: Refreshing playlist...");
            userPlaylist.PlayListVideosDataLoaded = false;
            try
            {
                await LoadPlaylist(userPlaylist);
                Console.WriteLine("INFO: Playlist refreshed.");
            }
            catch 
            {
                Console.WriteLine("ERROR: Unable to refresh playlist.");
            }
        }

        public void SearchVideoInPlayList(
            string searchQuery,
            UserPlayList userPlayList)
        {
            if (userPlayList == null ||
                string.IsNullOrWhiteSpace(searchQuery))
            {
                return;
            }

            Console.WriteLine("INFO: Searching in playlist....");

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

            if (results.Count <= 0)
            {
                Console.WriteLine("INFO: No search results found.");
            }
            else
            {
                Console.WriteLine("INFO: Search results returned following videos:");
                int index = 1;

                foreach (var video in results)
                {
                    Console.WriteLine(
                        index + 
                        ". Title: " + video.Title + 
                        ". Position " + (video.PositionInPlayList + 1) + 
                        ". Url: " + CommonUtilities.GetYoutubeVideoUrlFromVideoId(
                            video.VideoId));
                    index++;
                }
            }
        }

        public async Task AddVideoOrVideosToPlayList(
            string youTubeUrlsString,
            UserPlayList userPlayList)
        {
            if (string.IsNullOrWhiteSpace(youTubeUrlsString))
            {
                Console.WriteLine("WARN: No YouTube URL(s) found.");
                return;
            }

            List<string> urls =
                youTubeUrlsString.Split(',').
                    Select(url => url.Trim()).ToList();

            foreach (var url in urls)
            {
                bool isValidUrl =
                    CommonUtilities.TryGetVideoIdFromYoutubeUrl(
                        url, out string videoId);

                if (!isValidUrl)
                {
                    Console.WriteLine(
                        "ERROR: Unable to add video with URL [" + url + "] -> " +
                        "Invalid URL");

                    continue;
                }

                if (userPlayList.PlayListVideos?.Values.ToList()
                    .Any(v => v.VideoId == videoId) == true)
                {
                    Console.WriteLine(
                        "ERROR: Video with URL [" + url + "] " +
                        "already exists in the playlist.");
                }
                else
                {
                    try
                    {
                        var playListItem =
                            await ApiClient.GetApiClient.AddVideoToPlayList(videoId, userPlayList);

                        if (playListItem == null)
                        {
                            Console.WriteLine(
                                "ERROR: Unable to add video with URL [" + url + "]");
                        }
                        else
                        {
                            Console.WriteLine(
                                "INFO: Successfully added video with URL [" + url + "]");

                            SessionManager.GetSessionManager.SavePlaylistVideoToUserSessionPlaylist(
                                userPlayList, playListItem);
                        }
                    }
                    catch
                    {
                        Console.WriteLine(
                            "ERROR: Unable to add video with URL [" + url + "]");
                    }
                }
            }
        }

        public void PrintPlaylistDuplicates(
            UserPlayList playlist)
        {
            if (playlist != null &&
                playlist.PlayListVideosDataLoaded &&
                playlist.PlayListVideos != null)
            {
                var duplicateVideos = 
                    playlist.PlayListVideos.Values.GroupBy(v => v.VideoId)
                        .Select(grp => grp.ToList())
                        .Where(grp => grp.Count > 1)
                        .ToList();

                if (duplicateVideos.Count > 0)
                {
                    Console.WriteLine(
                        "INFO: Following duplicate videos detected in the playlist:");

                    int index = 1;
                    foreach (var duplicateVideo in
                        duplicateVideos)
                    {
                        Console.WriteLine(
                            index + ". " + 
                            "Title: " + duplicateVideo[0].Title + ", " +
                            "Positions: " +
                            string.Join(
                                ", ",
                                duplicateVideo.Select(
                                    v => v.PositionInPlayList + 1)
                            ) + ". URL: " + 
                            CommonUtilities.GetYoutubeVideoUrlFromVideoId(
                                duplicateVideo[0].VideoId)
                        );

                        index++;
                    }

                    return;
                }
            }

            Console.WriteLine("INFO: No duplicate videos found in selected playlist.");
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

        public async Task RefreshUserPlaylists(
            string channelId)
        {
            Console.WriteLine("INFO: Refreshing playlists...");

            var currentPlaylists =
                SessionManager.GetSessionManager.GetUserSessionPlaylists();

            List<Playlist> playlistsFromApi;
            try
            {
                playlistsFromApi =
                    await ApiClient.GetApiClient.GetUserPlayListsData(
                        channelId);
            }
            catch
            {
                Console.WriteLine("ERROR: Unable to fetch new playlists data.");
                return;
            }
            
            var currentUserOwnedPlaylists = 
                currentPlaylists.Item1;

            if (playlistsFromApi != null &&
                currentUserOwnedPlaylists != null)
            {
                var playlistIds = playlistsFromApi.Select(p => p.Id);

                //remove extra playlists
                var userOwnedPlaylistIdsToRemove =
                    currentUserOwnedPlaylists.Keys.Where(k => 
                        !playlistIds.Contains(k));

                foreach (var playlistToRemove in 
                    userOwnedPlaylistIdsToRemove)
                {
                    currentUserOwnedPlaylists.Remove(playlistToRemove);
                }

                List<Playlist> newPlaylists =
                    new List<Playlist>();

                foreach (var playlist in playlistsFromApi)
                {
                    if (currentUserOwnedPlaylists.ContainsKey(playlist.Id))
                    {
                        UpdatePlaylistWithNewInformation(
                            currentUserOwnedPlaylists[playlist.Id],
                            playlist);
                    }
                    else
                    {
                        newPlaylists.Add(playlist);
                    }
                }

                //save newly discovered playlists
                //that were not loaded before
                if (newPlaylists.Count > 0)
                {
                    SessionManager.GetSessionManager.SaveUserOwnedPlaylistsToUserSession(
                        newPlaylists);
                }
            }

            Console.WriteLine("INFO: Playlists information refreshed.");
        }

        public async Task AddContributorPlaylist(
            string playListId)
        {
            try
            {
                var contributorPlaylist =
                    await ApiClient.GetApiClient
                        .GetUserPlaylistMetaData(playListId);

                if (contributorPlaylist != null)
                {
                    Console.WriteLine(
                        "INFO: Playlist with title '" +
                        contributorPlaylist.Snippet?.Title +
                        "' added successfully.");
                }

                SessionManager.GetSessionManager.
                    SaveUserContributorPlaylistToUserSession(
                    contributorPlaylist);
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

        private void UpdatePlaylistWithNewInformation(
            UserPlayList userPlaylist,
            Playlist newPlaylist)
        {
            if (userPlaylist == null ||
                newPlaylist == null)
            {
                return;
            }

            if (newPlaylist.Snippet != null)
            {
                userPlaylist.ThumbnailUrl =
                    newPlaylist.Snippet?.Thumbnails?.Default__?.Url;

                userPlaylist.Title =
                    newPlaylist.Snippet?.Title;

                userPlaylist.PlayListVideosDataLoaded = false;
            }
        }
    }
}
