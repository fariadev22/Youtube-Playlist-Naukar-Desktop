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
    public class PlaylistVideosHelper
    {
        private static readonly PlaylistVideosHelper 
            PlaylistVideosHelperInstance =
                new PlaylistVideosHelper();

        static PlaylistVideosHelper()
        {

        }

        public static PlaylistVideosHelper GetPlaylistVideosHelper
        {
            get
            {
                return PlaylistVideosHelperInstance;
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
                        SessionStorageManager.GetSessionManager.
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
                SessionStorageManager.GetSessionManager.
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

                    SessionStorageManager.GetSessionManager.
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

                            SessionStorageManager.GetSessionManager.
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

                SessionStorageManager.GetSessionManager.DeleteVideoFromUserSessionPlaylist(
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
    }
}
