using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FuzzySharp;
using Youtube_Playlist_Naukar_Windows.Models;
using Youtube_Playlist_Naukar_Windows.Utilities;

namespace Youtube_Playlist_Naukar_Windows.Helpers
{
    public class PlaylistVideosHelper
    {
        private static readonly PlaylistVideosHelper 
            PlaylistVideosHelperInstance =
                new PlaylistVideosHelper();

        public static event EventHandler<
                VideoThumbnailReadyEventArgs> VideoThumbnailReady;

        static PlaylistVideosHelper()
        {
            SessionStorageManager.GetSessionManager.
                    PlaylistVideoThumbnailUpdated +=
                notifyUIForVideoThumbnailChange;
        }

        public static PlaylistVideosHelper GetPlaylistVideosHelper
        {
            get
            {
                return PlaylistVideosHelperInstance;
            }
        }

        public async Task LoadPlaylist(
            UserPlayList userPlaylist,
            CancellationToken cancellationToken)
        {
            if (userPlaylist == null)
            {
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
                            userPlaylist, cancellationToken);

                    var playlistVideos = playlistVideosResult.Item1;

                    var etag =
                        playlistVideosResult.Item2;

                    if (playlistVideos != null)
                    {
                        var playlistVideosDurationResult =
                            await ApiClient.GetApiClient.GetVideosDuration(
                                playlistVideos
                                    .Select(v => 
                                        v.Snippet?.ResourceId?.VideoId)
                                    .ToList(),
                                cancellationToken);

                        SessionStorageManager.GetSessionManager.
                            SavePlaylistVideosToUserSessionPlaylist(
                                userPlaylist, playlistVideos, 
                                playlistVideosDurationResult,
                                etag);
                    }
                }
                else
                {
                    await RefreshPlaylistVideos(userPlaylist,
                        cancellationToken);
                }
            }
            catch
            {
                //
            }
        }

        public async Task RefreshPlaylistVideos(
            UserPlayList userPlaylist,
            CancellationToken cancellationToken)
        {
            var alreadyLoadedVideos =
                userPlaylist.PlayListVideos ??
                new Dictionary<string, UserPlayListVideo>();

            var playlistVideosResult =
                await ApiClient.GetApiClient.
                    GetPlaylistVideosPartialData(
                        userPlaylist, cancellationToken);

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
                        userPlaylist, partialVideosData, 
                        new Dictionary<string, string>(), eTag);
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
                                cancellationToken,
                                videoIds: idsOfVideosToLoad);

                    var videos =
                        playlistVideosResult.Item1;

                    eTag = playlistVideosResult.Item2;

                    var playlistVideosDurationResult =
                        await ApiClient.GetApiClient.GetVideosDuration(
                            videos
                                .Select(v =>
                                    v.Snippet?.ResourceId?.VideoId)
                                .ToList(),
                            cancellationToken);

                    SessionStorageManager.GetSessionManager.
                        SavePlaylistVideosToUserSessionPlaylist(
                            userPlaylist, newVideosData, videos, 
                            playlistVideosDurationResult, eTag);
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
                foreach (var playlistVideo in userPlayList.
                    PlayListVideos.Values)
                {
                    int score = Fuzz.PartialRatio(
                        searchQuery.ToLower(), 
                        playlistVideo.Title.ToLower());

                    if (score >= 80)
                    {
                        videoAndSearchScores.Add(
                            playlistVideo, score);
                    }
                }
            }

            //most relevant stuff on top
            return
                videoAndSearchScores.OrderByDescending(v =>
                    v.Value).Select(v => v.Key).ToList();
        }

        public async Task<List<string>> AddVideoOrVideosToPlayList(
            string youTubeUrlsString,
            UserPlayList userPlayList,
            CancellationToken cancellationToken)
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
                                videoId, userPlayList, cancellationToken);

                        var playlistVideosDurationResult =
                            await ApiClient.GetApiClient.GetVideosDuration(
                                new List<string>
                                {
                                    playListItem?.Snippet?.ResourceId?.VideoId
                                },
                                cancellationToken);

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
                                    userPlayList, playListItem,
                                    playlistVideosDurationResult);
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

        public async Task<bool> DeleteVideoFromPlaylist(
            UserPlayList userPlaylist,
            UserPlayListVideo videoToDelete,
            CancellationToken cancellationToken)
        {
            if (userPlaylist == null ||
                videoToDelete == null)
            {
                return true;
            }

            try
            {
                await ApiClient.GetApiClient.DeletePlaylistVideo(
                    videoToDelete.UniqueVideoIdInPlaylist,
                    cancellationToken);

                SessionStorageManager.GetSessionManager.
                    DeleteVideoFromUserSessionPlaylist(
                        userPlaylist,
                        videoToDelete);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void DownloadPlaylistVideoThumbnails(
            string playlistId,
            List<UserPlayListVideo> playlistVideos)
        {
            if (playlistVideos.Count > 0)
            {
                SessionStorageManager.GetSessionManager.
                    DownloadPlaylistVideoThumbnailsInBackgroundAndNotifyMainThread(
                        playlistId, playlistVideos);
            }
        }

        public List<UserPlayListVideo> GetPrivateVideos(
            List<UserPlayListVideo> playlistVideos)
        {
            if (playlistVideos.Count > 0)
            {
                return playlistVideos.FindAll(
                    v => v.PrivacyStatus == PrivacyStatusEnum.Private);
            }

            return new List<UserPlayListVideo>();
        }

        private static void notifyUIForVideoThumbnailChange(
            object sender,
            PlaylistVideoThumbnailUpdatedEventArgs eventArgs)
        {
            if (VideoThumbnailReady != null)
            {
                VideoThumbnailReady(null,
                    new VideoThumbnailReadyEventArgs
                    {
                        PlaylistId = eventArgs.PlaylistId,
                        VideoId =
                            eventArgs.VideoId,
                        PlaylistVideoImagePathFromCustomerDirectory =
                            eventArgs.PlaylistVideoImagePathFromCustomerDirectory
                    });
            }
        }
    }
}
