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

        public async Task LoadPlaylistVideos(
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

            (List<PlaylistItem>, string) playlistVideosResult =
                (null, null);

            try
            {
                playlistVideosResult =
                    await ApiClient.GetApiClient.
                        GetPlaylistVideosPartialData(
                            userPlaylist, cancellationToken);
            }
            catch
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }
            
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
            if (eTag != userPlaylist.PlaylistVideosETag &&
                !cancellationToken.IsCancellationRequested)
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
                            alreadyLoadedVideos[partialVideo.Id]
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
                            new UserPlayListVideo
                            {
                                UniqueVideoIdInPlaylist = partialVideo.Id
                            });
                    }
                }

                //load new videos
                List<PlaylistItem> newVideos = null;
                Dictionary<string, string> videoDurations = null;

                if (idsOfVideosToLoad.Count > 0)
                {
                    try
                    {
                        newVideos =
                            await ApiClient.GetApiClient
                                .GetPlaylistVideos(
                                    idsOfVideosToLoad,
                                    cancellationToken);

                        videoDurations =
                            await ApiClient.GetApiClient.GetVideosDuration(
                                newVideos
                                    .Select(v =>
                                        v.Snippet?.ResourceId?.VideoId)
                                    .ToList(),
                                cancellationToken);
                    }
                    catch
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return;
                        }
                    }
                }

                SessionStorageManager.GetSessionManager.
                    SavePlaylistVideosToUserSessionPlaylist(
                        userPlaylist, 
                        newVideosData, 
                        newVideos ?? new List<PlaylistItem>(), 
                        videoDurations ?? new Dictionary<string, string>(),
                        eTag);
            }
        }

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
