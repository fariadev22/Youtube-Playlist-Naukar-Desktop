using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public static event EventHandler<
                VideoThumbnailReadyEventArgs> VideoThumbnailReady;

        static PlaylistVideosHelper()
        {
            SessionStorageManager.GetSessionManager.
                    PlaylistVideoThumbnailUpdated +=
                NotifyUiForVideoThumbnailChange;
        }

        public static PlaylistVideosHelper GetPlaylistVideosHelper
        {
            get
            {
                return PlaylistVideosHelperInstance;
            }
        }

        public async Task<(string, bool)> GetPlaylistVideosEtag(
            string existingETag,
            string playlistId,
            CancellationToken cancellationToken)
        {
            try
            {
                string newEtag = 
                    await ApiClient.GetApiClient.GetPlaylistVideosETag(
                        playlistId, cancellationToken);

                if (!string.IsNullOrWhiteSpace(newEtag) &&
                    existingETag == newEtag)
                {
                    return (existingETag, true);
                }

                return (newEtag, false);
            }
            catch
            {
                //
            }

            return (existingETag, false);
        }

        /// <summary>
        /// Loads/refreshes playlist videos. Videos can
        /// become private or have their duration changed (if it
        /// was a live video) or have their description changed,
        /// have their position in playlist changed.
        /// </summary>
        public async Task<(Dictionary<string, UserPlayListVideo>, string)>
            LoadPlaylistVideos(
                string playlistId,
                CancellationToken cancellationToken,
                Dictionary<string, UserPlayListVideo> 
                    existingUserPlayListVideos,
                string pageToken = null,
                long? lastVideoPosition = null)
        {
            if (string.IsNullOrWhiteSpace(playlistId))
            {
                return (existingUserPlayListVideos, pageToken);
            }

            try
            {
                //get videos data
                var playlistVideosResult =
                    await ApiClient.GetApiClient.GetPlaylistVideos(
                        playlistId, cancellationToken, pageToken);

                var playlistVideos =
                    playlistVideosResult.Item1;

                var nextPageToken =
                    playlistVideosResult.Item2;

                if (playlistVideos == null)
                {
                    return (new Dictionary<string, UserPlayListVideo>(),
                        nextPageToken);
                }

                //get video durations data

                Dictionary<string, string> playlistVideosDurationResult;

                if (existingUserPlayListVideos != null)
                {
                    //get durations of only new or updated videos
                    playlistVideosDurationResult =
                        await ApiClient.GetApiClient.GetVideosDuration(
                            playlistVideos
                                .Where(v => !existingUserPlayListVideos.ContainsKey(v.Id) ||
                                            existingUserPlayListVideos[v.Id]?.ETag != v.ETag)
                                .Select(v =>
                                    v.Snippet?.ResourceId?.VideoId)
                                .ToList(),
                            cancellationToken);
                }
                else
                {
                    //get durations data for all videos
                    playlistVideosDurationResult =
                        await ApiClient.GetApiClient.GetVideosDuration(
                            playlistVideos
                                .Select(v =>
                                    v.Snippet?.ResourceId?.VideoId)
                                .ToList(),
                            cancellationToken);
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    return 
                        (new Dictionary<string, UserPlayListVideo>(), string.Empty);
                }

                playlistVideosDurationResult ??=
                    new Dictionary<string, string>();

                //process the videos data received and convert to 
                //user playlists videos

                Dictionary<string, UserPlayListVideo>
                    newUserPlaylistVideos =
                        new Dictionary<string, UserPlayListVideo>();

                foreach (var playlistVideo in playlistVideos)
                {
                    //existing video entry hasn't changed
                    if (existingUserPlayListVideos != null &&
                        existingUserPlayListVideos.ContainsKey(playlistVideo.Id) &&
                        existingUserPlayListVideos[playlistVideo.Id]?.ETag ==
                            playlistVideo.ETag)
                    {
                        newUserPlaylistVideos.Add(
                            playlistVideo.Id,
                            existingUserPlayListVideos[playlistVideo.Id]);
                    }
                    else
                    {
                        var userPlaylistVideo =
                            GetUserPlaylistVideoFromPlaylistItem(
                                playlistVideo,
                                playlistVideosDurationResult);

                        newUserPlaylistVideos.Add(
                            playlistVideo.Id,
                            userPlaylistVideo);

                        //it is null for deleted/private videos
                        if (userPlaylistVideo.PositionInPlayList == null)
                        {
                            var userPlaylistVideos =
                                newUserPlaylistVideos.Values.ToList();

                            int currentVideoIndex =
                                userPlaylistVideos.IndexOf(userPlaylistVideo);

                            if (currentVideoIndex == 0)
                            {
                                if (lastVideoPosition != null)
                                {
                                    userPlaylistVideo.PositionInPlayList =
                                        lastVideoPosition + 1;
                                }
                                else
                                {
                                    //no video before it
                                    userPlaylistVideo.PositionInPlayList = 0;
                                }
                            }
                            else if (currentVideoIndex > 0 &&
                                     (currentVideoIndex - 1) >= 0)
                            {
                                userPlaylistVideo.PositionInPlayList =
                                    userPlaylistVideos
                                        .ElementAt(currentVideoIndex - 1)?
                                        .PositionInPlayList + 1;
                            }
                        }
                    }
                }

                return (newUserPlaylistVideos, nextPageToken);
            }
            catch
            {
                //
            }

            return (new Dictionary<string, UserPlayListVideo>(),
                    string.Empty);
        }

        public void SavePlaylist(
            UserPlayList userPlaylist,
            string newEtag)
        {
            SessionStorageManager.GetSessionManager.SavePlaylist(
                userPlaylist, newEtag);
        }

        /// <summary>
        /// Depending on the sort mechanism of the playlist, a
        /// new video can either get added at the start of
        /// playlist or end or any random user specified position.
        /// This can result in changes to positions of other videos.
        /// So we need to be vary of that.
        /// </summary>
        public async Task<(string, UserPlayListVideo)> 
            AddVideoToPlayList(
            string youTubeUrl,
            UserPlayList userPlayList,
            CancellationToken cancellationToken)
        {
            bool isValidUrl =
                CommonUtilities.TryGetVideoIdFromYoutubeUrl(
                    youTubeUrl, out string videoId);

            if (!isValidUrl)
            {
                return ("Not added. Invalid URL.", null);
            }

            if (userPlayList.PlayListVideos?.Values.ToList()
                .Any(v => v.VideoId == videoId) == true)
            {
                return ("Not added. " +
                       "Video already exists in playlist.", null);
            }

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
                    return ("Not added.", null);
                }

                SessionStorageManager.GetSessionManager.
                    AddNewVideoToUserSessionPlaylist(
                        userPlayList, playListItem,
                        playlistVideosDurationResult);

                var addedVideo =
                    userPlayList.PlayListVideos?.ContainsKey(
                        playListItem.Id) == true
                        ? userPlayList.PlayListVideos[playListItem.Id]
                        : null;

                return ("Successfully added.", addedVideo);
            }
            catch
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return ("", null);
                }

                return ("Not added.", null);
            }
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

        public List<List<UserPlayListVideo>>
            GetPlaylistDuplicates(
                UserPlayList playlist)
        {
            if (playlist != null &&
                playlist.PlayListVideos != null)
            {
                return
                    playlist.PlayListVideos.Values
                        .GroupBy(v => v.VideoId)
                        .Select(grp => grp.ToList())
                        .Where(grp => grp.Count > 1)
                        .ToList();
            }

            return new List<List<UserPlayListVideo>>();
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

        private UserPlayListVideo GetUserPlaylistVideoFromPlaylistItem(
            PlaylistItem playListItem, 
            Dictionary<string, string> playlistItemsVideoDuration)
        {
            var videoId =
                playListItem.Snippet?.ResourceId?.VideoId;

            var duration =
                !string.IsNullOrWhiteSpace(videoId) &&
                playlistItemsVideoDuration?.ContainsKey(videoId) == true
                    ? playlistItemsVideoDuration[videoId]
                    : null;

            return UserPlayListVideo.
                ConvertPlayListItemToUserPlayListVideo(
                    playListItem, duration);
        }

        private static void NotifyUiForVideoThumbnailChange(
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
