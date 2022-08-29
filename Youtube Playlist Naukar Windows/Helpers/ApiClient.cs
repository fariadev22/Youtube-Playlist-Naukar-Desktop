using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Youtube_Playlist_Naukar_Windows.Models;

namespace Youtube_Playlist_Naukar_Windows.Helpers
{
    /// <summary>
    /// Responsible for talking to the Youtube API
    /// on behalf of the logged in user
    /// </summary>
    public class ApiClient
    {
        private static readonly ApiClient ApiClientInstance =
            new ApiClient();

        static ApiClient()
        {

        }

        public static ApiClient GetApiClient
        {
            get
            {
                return ApiClientInstance;
            }
        }

        private YouTubeService _youtubeService;

        public void Initialize(
            UserSession activeUserSession)
        {
            if (activeUserSession?.UserCredential != null)
            {
                _youtubeService =
                    new YouTubeService(
                        new BaseClientService.Initializer
                    {
                        HttpClientInitializer = 
                            activeUserSession.UserCredential,
                        ApplicationName = 
                            Constants.ApplicationName
                    });
            }
            else
            {
                _youtubeService = new YouTubeService();
            }
        }

        public async Task<string>
            GetUserChannelId(
                CancellationToken cancellationToken)
        {
            //get user channel id
            var channelRequest = 
                _youtubeService.Channels.List("snippet");
            channelRequest.Mine = true;
            channelRequest.Fields = "items/id";
            var channelInfo = await channelRequest.ExecuteAsync(
                cancellationToken);
            string channelId = null;

            if (channelInfo?.Items != null &&
                channelInfo.Items.Count > 0)
            {
                channelId = channelInfo.Items[0].Id;
            }

            if (string.IsNullOrWhiteSpace(channelId) && 
                !cancellationToken.IsCancellationRequested)
            {
                throw new AggregateException("No YouTube channel found.");
            }

            return channelId;
        }

        #region Playlists

        /// <summary>
        /// Returns playlists associated with a channel
        /// </summary>
        public async Task<(List<Playlist>, string)> 
            GetPlayListsData(
                CancellationToken cancellationToken,
                string channelId,
                string pageToken = null)
        {
            if (string.IsNullOrWhiteSpace(channelId))
            {
                return (new List<Playlist>(), "");
            }

            var playListsRequest =
                _youtubeService.Playlists.List(
                    GetPlaylistsRequestPartString());

            playListsRequest.ChannelId = channelId;

            playListsRequest.MaxResults = 50;

            playListsRequest.Fields =
                GetPlaylistRequestFields();

            if (!string.IsNullOrWhiteSpace(pageToken))
            {
                playListsRequest.PageToken = pageToken;
            }

            //get all channel playlists

            var playListsResponse =
                await playListsRequest.ExecuteAsync(
                    cancellationToken);

            if (playListsResponse == null)
            {
                return ((new List<Playlist>(), ""));
            }

            string eTag = playListsResponse.ETag;

            var playLists =
                playListsResponse.Items?.ToList()
                ?? new List<Playlist>();

            if (!string.IsNullOrWhiteSpace(
                playListsResponse.NextPageToken))
            {
                var nextPlayLists =
                    (await GetPlayListsData(
                        cancellationToken,
                        channelId,
                        playListsResponse.NextPageToken))
                    .Item1;

                if (nextPlayLists != null &&
                    nextPlayLists.Count > 0)
                {
                    playLists.AddRange(nextPlayLists);
                }
            }

            return (playLists, eTag);
        }
        
        /// <summary>
        /// Get data of playlists whose ids are provided
        /// </summary>
        public async Task<List<Playlist>>
            GetPlayListsData(
                CancellationToken cancellationToken,
                List<string> allPlaylistIds,
                string pageToken = null)
        {
            if (allPlaylistIds == null ||
                allPlaylistIds.Count <= 0)
            {
                return new List<Playlist>();
            }

            //API does not support more than 50 ids at a 
            //time.
            List<List<string>> playlistIdsChunks =
                new List<List<string>>();

            if (allPlaylistIds.Count < 50)
            {
                playlistIdsChunks.Add(allPlaylistIds);
            }
            else
            {
                int chunkSize = 50;
                playlistIdsChunks =
                    allPlaylistIds
                        .Select((x, i) =>
                            new { Index = i, Value = x })
                        .GroupBy(x => x.Index / chunkSize)
                        .Select(x => x.Select(v => v.Value).ToList())
                        .ToList();
            }

            var playListsRequest =
                _youtubeService.Playlists.List(
                    GetPlaylistsRequestPartString());

            playListsRequest.MaxResults = 50;

            playListsRequest.Fields =
                GetPlaylistRequestFields();

            if (!string.IsNullOrWhiteSpace(pageToken))
            {
                playListsRequest.PageToken = pageToken;
            }

            List<Playlist> finalPlaylists =
                new List<Playlist>();

            foreach (var playlistIds in playlistIdsChunks)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                playListsRequest.Id =
                    string.Join(",", playlistIds);

                var playListsResponse =
                    await playListsRequest.ExecuteAsync(
                        cancellationToken);

                if (playListsResponse == null)
                {
                    continue;
                }

                var playLists = playListsResponse.Items?.ToList()
                                ?? new List<Playlist>();

                if (!string.IsNullOrWhiteSpace(
                    playListsResponse.NextPageToken))
                {
                    var nextPlayLists =
                        await GetPlayListsData(
                            cancellationToken,
                            playlistIds,
                            playListsResponse.NextPageToken);

                    if (nextPlayLists != null &&
                        nextPlayLists.Count > 0)
                    {
                        playLists.AddRange(nextPlayLists);
                    }
                }

                finalPlaylists.AddRange(playLists);
            }

            return finalPlaylists;
        }

        public async Task<(List<Playlist>, string)> 
            GetPlayListsPartialData(
                string channelId,
                CancellationToken cancellationToken,
                string pageToken = null)
        {
            //part string needs to be same as original
            //request in order to get same etags
            var playListsRequest =
                _youtubeService.Playlists.List(
                    GetPlaylistsRequestPartString());
            playListsRequest.ChannelId = channelId;
            playListsRequest.MaxResults = 50;
            playListsRequest.Fields =
                "nextPageToken, etag, items(id, etag)";

            if (!string.IsNullOrWhiteSpace(pageToken))
            {
                playListsRequest.PageToken = pageToken;
            }

            var playListsResponse = 
                await playListsRequest.ExecuteAsync(
                    cancellationToken);

            if (playListsResponse == null)
            {
                return ((new List<Playlist>(), ""));
            }

            string eTag = 
                playListsResponse.ETag;

            var playLists = 
                playListsResponse.Items?.ToList()
                    ?? new List<Playlist>();

            if (!string.IsNullOrWhiteSpace(
                playListsResponse.NextPageToken))
            {
                var nextPlayLists =
                    (await GetPlayListsPartialData(
                        channelId,
                        cancellationToken,
                        playListsResponse.NextPageToken)).Item1;

                if (nextPlayLists != null &&
                   nextPlayLists.Count > 0)
                {
                    playLists.AddRange(nextPlayLists);
                }
            }

            return (playLists, eTag);
        }

        public async Task<List<Playlist>>
            GetPlayListsPartialData(
                List<string> allPlaylistIds,
                CancellationToken cancellationToken,
                string pageToken = null)
        {
            //API does not support more than 50 ids at a 
            //time.
            List<List<string>> playlistIdsChunks =
                new List<List<string>>();

            if (allPlaylistIds != null &&
                allPlaylistIds.Count > 0)
            {
                if (allPlaylistIds.Count < 50)
                {
                    playlistIdsChunks.Add(allPlaylistIds);
                }
                else
                {
                    int chunkSize = 50;
                    playlistIdsChunks =
                        allPlaylistIds
                            .Select((x, i) =>
                                new { Index = i, Value = x })
                            .GroupBy(x => x.Index / chunkSize)
                            .Select(x => x.Select(v => v.Value).ToList())
                            .ToList();
                }
            }
            else
            {
                return new List<Playlist>();
            }

            //part string needs to be same as original
            //request in order to get same etags
            var playListsRequest =
                _youtubeService.Playlists.List(
                    GetPlaylistsRequestPartString());
            playListsRequest.MaxResults = 50;
            playListsRequest.Fields =
                "nextPageToken, etag, items(id, etag)";

            if (!string.IsNullOrWhiteSpace(pageToken))
            {
                playListsRequest.PageToken = pageToken;
            }

            List<Playlist> finalPlaylists = 
                new List<Playlist>();

            foreach (var playlistIds in playlistIdsChunks)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                playListsRequest.Id =
                    string.Join(",", playlistIds);

                var playListsResponse =
                    await playListsRequest.ExecuteAsync(
                        cancellationToken);

                if (playListsResponse == null)
                {
                    continue;
                }

                var playLists = 
                    playListsResponse.Items?.ToList()
                    ?? new List<Playlist>();

                if (!string.IsNullOrWhiteSpace(
                    playListsResponse.NextPageToken))
                {
                    var nextPlayLists =
                        (await GetPlayListsPartialData(
                            playlistIds,
                            cancellationToken,
                            playListsResponse.NextPageToken));

                    if (nextPlayLists != null &&
                        nextPlayLists.Count > 0)
                    {
                        playLists.AddRange(nextPlayLists);
                    }
                }

                finalPlaylists.AddRange(playLists);
            }

            return finalPlaylists;

        }

        #endregion

        #region Videos

        /// <summary>
        /// Returns playlist videos.
        /// </summary>
        public async Task<(List<PlaylistItem>, string)> 
            GetPlaylistVideos(
            UserPlayList userPlayList,
            CancellationToken cancellationToken,
            string pageToken = null)
        {
            if (string.IsNullOrWhiteSpace(
                userPlayList?.Id))
            {
                return (new List<PlaylistItem>(), 
                    string.Empty);
            }

            string playlistId = userPlayList.Id;

            var playListItemsRequest = 
                _youtubeService.PlaylistItems.List(
                    GetVideoRequestPartString());

            playListItemsRequest.MaxResults = 500;
            
            playListItemsRequest.Fields =
                GetPlaylistItemRequestFields();

            playListItemsRequest.PlaylistId =
                playlistId;

            if (!string.IsNullOrWhiteSpace(pageToken))
            {
                playListItemsRequest.PageToken = pageToken;
            }

            var playListItemsResponse =
                await playListItemsRequest.ExecuteAsync(
                    cancellationToken);

            if (playListItemsResponse == null)
            {
                return (new List<PlaylistItem>(), string.Empty);
            }

            List<PlaylistItem> playlistItems =
                new List<PlaylistItem>();

            string eTag = playListItemsResponse.ETag;

            if (playListItemsResponse.Items != null &&
                playListItemsResponse.Items.Count > 0)
            {
                playlistItems.AddRange(
                    playListItemsResponse.Items);
            }

            if (!string.IsNullOrWhiteSpace(
                playListItemsResponse.NextPageToken))
            {
                var nextItemsResult =
                    await GetPlaylistVideos(
                        userPlayList,
                        cancellationToken,
                        playListItemsResponse.NextPageToken);

                var nextItems = nextItemsResult.Item1;

                if (nextItems != null &&
                    nextItems.Count > 0)
                {
                    playlistItems.AddRange(nextItems);
                }
            }

            return (playlistItems, eTag);
        }

        public async Task<List<PlaylistItem>>
            GetPlaylistVideos(
                List<string> allVideoIds,
                CancellationToken cancellationToken,
                string pageToken = null)
        {
            if (allVideoIds == null ||
                allVideoIds.Count <= 0)
            {
                return new List<PlaylistItem>();
            }

            //API does not support more than 50 ids at a 
            //time.
            List<List<string>> videoIdsChunks =
                new List<List<string>>();

            if (allVideoIds.Count < 50)
            {
                videoIdsChunks.Add(allVideoIds);
            }
            else
            {
                int chunkSize = 50;
                videoIdsChunks =
                    allVideoIds
                        .Select((x, i) =>
                            new { Index = i, Value = x })
                        .GroupBy(x => x.Index / chunkSize)
                        .Select(x => x.Select(v => v.Value).ToList())
                        .ToList();
            }

            var playListItemsRequest =
                _youtubeService.PlaylistItems.List(
                    GetVideoRequestPartString());
            
            playListItemsRequest.MaxResults = 500;

            playListItemsRequest.Fields =
                GetPlaylistItemRequestFields();

            if (!string.IsNullOrWhiteSpace(pageToken))
            {
                playListItemsRequest.PageToken = pageToken;
            }

            List<PlaylistItem> playlistItems =
                new List<PlaylistItem>();

            foreach (var videoIds in videoIdsChunks)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                playListItemsRequest.Id =
                    string.Join(",", videoIds);

                var playListItemsResponse =
                    await playListItemsRequest.ExecuteAsync(
                        cancellationToken);

                if (playListItemsResponse != null)
                {
                    if (playListItemsResponse.Items != null &&
                        playListItemsResponse.Items.Count > 0)
                    {
                        playlistItems.AddRange(
                            playListItemsResponse.Items);
                    }

                    if (!string.IsNullOrWhiteSpace(
                        playListItemsResponse.NextPageToken))
                    {
                        var nextItems =
                            await GetPlaylistVideos(
                                videoIds,
                                cancellationToken,
                                playListItemsResponse.NextPageToken);

                        if (nextItems != null &&
                            nextItems.Count > 0)
                        {
                            playlistItems.AddRange(nextItems);
                        }
                    }
                }
            }

            return playlistItems;
        }

        public async Task<(List<PlaylistItem>, string)>
            GetPlaylistVideosPartialData(
            UserPlayList userPlayList,
            CancellationToken cancellationToken,
            string pageToken = null)
        {
            List<PlaylistItem> playlistItems =
                new List<PlaylistItem>();

            //part string needs to be same as in 
            //original request to get same etags
            var playListItemsRequest =
                _youtubeService.PlaylistItems.List(
                    GetVideoRequestPartString());
            playListItemsRequest.MaxResults = 500;
            playListItemsRequest.PlaylistId = 
                userPlayList.Id;
            playListItemsRequest.Fields =
                "nextPageToken, etag, items(id, etag)";

            if (!string.IsNullOrWhiteSpace(pageToken))
            {
                playListItemsRequest.PageToken = pageToken;
            }

            var playListItemsResponse =
                await playListItemsRequest.ExecuteAsync(
                    cancellationToken);

            string eTag = string.Empty;

            if (playListItemsResponse != null)
            {
                eTag = playListItemsResponse.ETag;

                if (playListItemsResponse.Items != null &&
                    playListItemsResponse.Items.Count > 0)
                {
                    playlistItems.AddRange(
                        playListItemsResponse.Items);
                }

                if (!string.IsNullOrWhiteSpace(
                    playListItemsResponse.NextPageToken))
                {
                    var nextItemsResult =
                        await GetPlaylistVideosPartialData(
                            userPlayList,
                            cancellationToken,
                            playListItemsResponse.NextPageToken);

                    var nextItems = nextItemsResult.Item1;

                    if (nextItems != null &&
                        nextItems.Count > 0)
                    {
                        playlistItems.AddRange(nextItems);
                    }
                }
            }

            return (playlistItems, eTag);
        }

        public async Task<Dictionary<string, string>>
            GetVideosDuration(
            List<string> videoIds,
            CancellationToken cancellationToken,
            string pageToken = null)
        {
            //key = video id
            //value = video duration
            Dictionary<string, string> videosDuration =
                new Dictionary<string, string>();

            //videoIds split into chunks of 50 because 
            //API does not support more than 50 ids at a 
            //time.
            List<List<string>> videoIdsChunks =
                new List<List<string>>();

            if (videoIds == null ||
                videoIds.Count <= 0)
            {
                return videosDuration;
            }

            if (videoIds.Count < 50)
            {
                videoIdsChunks.Add(videoIds);
            }
            else
            {
                int chunkSize = 50;
                videoIdsChunks =
                    videoIds
                        .Select((x, i) => 
                            new { Index = i, Value = x })
                        .GroupBy(x => x.Index / chunkSize)
                        .Select(x => x.Select(v => v.Value).ToList())
                        .ToList();
            }

            foreach (var currentVideoIds in videoIdsChunks)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var videoRequest =
                   _youtubeService.Videos.List(
                       "contentDetails");
                videoRequest.MaxResults = 50;
                videoRequest.Id =
                    string.Join(",", currentVideoIds);
                videoRequest.Fields =
                    "items(id, contentDetails(duration))";

                if (!string.IsNullOrWhiteSpace(pageToken))
                {
                    videoRequest.PageToken = pageToken;
                }

                var videoResponse =
                    await videoRequest.ExecuteAsync(
                        cancellationToken);

                if (videoResponse?.Items != null &&
                    videoResponse.Items.Count > 0)
                {
                    foreach (var videoItem in videoResponse.Items)
                    {
                        if (!string.IsNullOrWhiteSpace(videoItem.Id) &&
                            !videosDuration.ContainsKey(videoItem.Id))
                        {
                            videosDuration.Add(videoItem.Id,
                                videoItem.ContentDetails?.Duration);
                        }
                    }
                }
            }
            
            return videosDuration;
        }

        public async Task<PlaylistItem> 
            AddVideoToPlayList(
            string videoId,
            UserPlayList userPlayList,
            CancellationToken cancellationToken)
        {
            var playListItemFromVideoId = new PlaylistItem
            {
                Snippet = new PlaylistItemSnippet
                {
                    PlaylistId = userPlayList.Id,
                    ResourceId = new ResourceId
                    {
                        VideoId = videoId,
                        Kind = "youtube#video"
                    }
                }
            };
            var addVideoRequest = 
                _youtubeService.PlaylistItems.Insert(
                    playListItemFromVideoId, "snippet");

            addVideoRequest.Fields =
                "id, " +
                "snippet(title, thumbnails/medium/url, " +
                "resourceId, videoOwnerChannelTitle, " +
                "videoOwnerChannelId, position)";

            return await addVideoRequest.ExecuteAsync(
                cancellationToken);
        }

        public async Task 
            DeletePlaylistVideo(
            string uniqueVideoIdOfPlaylist,
            CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(uniqueVideoIdOfPlaylist))
            {
                await _youtubeService.PlaylistItems.Delete(
                    uniqueVideoIdOfPlaylist).ExecuteAsync(
                    cancellationToken);
            }
        }

        #endregion

        #region Utilities

        private static string GetPlaylistsRequestPartString()
        {
            return "snippet, contentDetails, status";
        }

        private static string GetPlaylistRequestFields()
        {
            return
                "nextPageToken," +
                "etag," +
                "items" +
                "(" +
                    "id," +
                    "etag," +
                    "kind," +
                    "status(privacyStatus)," +
                    "contentDetails(itemCount)," +
                    "snippet(" +
                        "publishedAt," +
                        "description," +
                        "title," +
                        "thumbnails/medium/url," +
                        "channelId," +
                        "channelTitle" +
                    ")" +
                ")";
        }

        private static string GetVideoRequestPartString()
        {
            return "snippet, status";
        }

        private static string GetPlaylistItemRequestFields()
        {
            return "nextPageToken, etag, items(id, etag, " +
                   "snippet(publishedAt, channelId, title, " +
                   "description, resourceId/videoId, " +
                   "thumbnails/medium/url, channelTitle, " +
                   "videoOwnerChannelId, videoOwnerChannelTitle, " +
                   "position), status(privacyStatus))";
        }

        #endregion
    }
}
