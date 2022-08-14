using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis;
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
                        ApplicationName = Constants.ApplicationName
                    });
            }
            else
            {
                _youtubeService = new YouTubeService();
            }
        }

        /// <summary>
        /// Returns playlists data.
        /// </summary>
        /// <param name="channelId">
        /// If this is set, only the playlists owned 
        /// by the current channel are returned
        /// </param>
        /// <param name="pageToken">
        /// If this is set the playlists are returned 
        /// for this current page.
        /// </param>
        /// <param name="playlistIds">
        /// If this is set only data of the playlists 
        /// with these ids are returned.
        /// </param>
        public async Task<(List<Playlist>, string)> 
            GetPlayListsData(
            string channelId = null,
            string pageToken = null,
            List<string> playlistIds = null)
        {
            var playListsRequest = 
                _youtubeService.Playlists.List(
                    GetPlaylistsRequestPartString());

            if(!string.IsNullOrWhiteSpace(channelId))
            {
                playListsRequest.ChannelId = channelId;
            }
            
            playListsRequest.MaxResults = 50;
            playListsRequest.Fields = 
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

            if (!string.IsNullOrWhiteSpace(pageToken))
            {
                playListsRequest.PageToken = pageToken;
            }

            if(playlistIds != null &&
                playlistIds.Count > 0)
            {
                playListsRequest.Id =
                    string.Join(",", playlistIds);
                //channel Id not allowed in such cases then
                playListsRequest.ChannelId = null;
            }

            var playListsResponse = 
                await playListsRequest.ExecuteAsync();
            string eTag = playListsResponse.ETag;
            var playLists = playListsResponse.Items?.ToList() 
                            ?? new List<Playlist>();

            if (!string.IsNullOrWhiteSpace(
                playListsResponse.NextPageToken))
            {
                var nextPlayLists =
                    (await GetPlayListsData(
                        playListsResponse.NextPageToken,
                        playlistIds: playlistIds)).Item1;

                if (nextPlayLists != null &&
                   nextPlayLists.Count > 0)
                {
                    playLists.AddRange(nextPlayLists);
                }
            }

            return (playLists, eTag);
        }

        public async Task<(List<Playlist>, string)> 
            GetUserPlayListsPartialData(
            string channelId,
            string pageToken = null)
        {
            //part string needs to be same as original request
            //in order to get same etags
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
                await playListsRequest.ExecuteAsync();

            string eTag = playListsResponse.ETag;

            var playLists = playListsResponse.Items?.ToList()
                            ?? new List<Playlist>();

            if (!string.IsNullOrWhiteSpace(
                playListsResponse.NextPageToken))
            {
                var nextPlayLists =
                    (await GetUserPlayListsPartialData(
                        playListsResponse.NextPageToken)).Item1;

                if (nextPlayLists != null &&
                   nextPlayLists.Count > 0)
                {
                    playLists.AddRange(nextPlayLists);
                }
            }

            return (playLists, eTag);
        }

        public async Task<(List<PlaylistItem>, string)> 
            GetPlaylistVideos(
            UserPlayList userPlayList,
            string pageToken = null,
            List<string> videoIds = null)
        {
            List<PlaylistItem> playlistItems = 
                new List<PlaylistItem>();
            
            var playListItemsRequest = 
                _youtubeService.PlaylistItems.List(
                    GetVideoRequestPartString());
            playListItemsRequest.MaxResults = 500;
            playListItemsRequest.PlaylistId = userPlayList.Id;
            playListItemsRequest.Fields =
                "nextPageToken, etag, items(id, etag, " +
                "snippet(publishedAt, channelId, title, " +
                "description, resourceId/videoId, " +
                "thumbnails/medium/url, channelTitle, " +
                "videoOwnerChannelId, videoOwnerChannelTitle, " +
                "position), status(privacyStatus))";

            if(videoIds != null &&
                videoIds.Count > 0)
            {
                playListItemsRequest.Id =
                    string.Join(",", videoIds);
            }

            if (!string.IsNullOrWhiteSpace(pageToken))
            {
                playListItemsRequest.PageToken = pageToken;
            }

            var playListItemsResponse = 
                await playListItemsRequest.ExecuteAsync();

            string eTag = string.Empty;

            if (playListItemsResponse != null)
            {
                eTag = playListItemsResponse.ETag;

                if (playListItemsResponse.Items != null &&
                    playListItemsResponse.Items.Count > 0)
                {
                    playlistItems.AddRange(playListItemsResponse.Items);
                }

                if (!string.IsNullOrWhiteSpace(
                    playListItemsResponse.NextPageToken))
                {
                    var nextItemsResult = 
                        await GetPlaylistVideos(
                            userPlayList,
                            playListItemsResponse.NextPageToken,
                            videoIds: videoIds);

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

        public async Task<(List<PlaylistItem>, string)>
            GetPlaylistVideosPartialData(
            UserPlayList userPlayList,
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
                await playListItemsRequest.ExecuteAsync();

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
            string pageToken = null)
        {
            //key = video id
            //value = video duration
            Dictionary<string, string> videosDuration =
                new Dictionary<string, string>();

            if (videoIds == null ||
                videoIds.Count <= 0)
            {
                return videosDuration;
            }

            var videoRequest =
                _youtubeService.Videos.List(
                    "contentDetails");
            videoRequest.MaxResults = 500;
            videoRequest.Id =
                string.Join(",", videoIds);
            videoRequest.Fields =
                "items(id, contentDetails(duration))";

            if (!string.IsNullOrWhiteSpace(pageToken))
            {
                videoRequest.PageToken = pageToken;
            }

            var videoResponse =
                await videoRequest.ExecuteAsync();

            if (videoResponse != null)
            {
                if (videoResponse.Items != null &&
                    videoResponse.Items.Count > 0)
                {
                    foreach(var videoItem in videoResponse.Items)
                    {
                        if(!string.IsNullOrWhiteSpace(videoItem.Id) &&
                            !videosDuration.ContainsKey(videoItem.Id))
                        {
                            videosDuration.Add(videoItem.Id,
                                videoItem.ContentDetails?.Duration);
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(
                    videoResponse.NextPageToken))
                {
                    var nextVideosDuration =
                        await GetVideosDuration(
                            videoIds,
                            videoResponse.NextPageToken);

                    if (nextVideosDuration != null &&
                        nextVideosDuration.Count > 0)
                    {
                        foreach(var nextVideoDuration in 
                            nextVideosDuration)
                        {
                            if (!videosDuration.ContainsKey(
                                    nextVideoDuration.Key))
                            {
                                videosDuration.Add(
                                    nextVideoDuration.Key,
                                    nextVideoDuration.Value);
                            }
                        }
                    }
                }
            }

            return videosDuration;
        }

        public async Task<PlaylistItem> 
            AddVideoToPlayList(
            string videoId,
            UserPlayList userPlayList)
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
            var addVideoRequest = _youtubeService.PlaylistItems.Insert(
                playListItemFromVideoId, "snippet");

            addVideoRequest.Fields =
                "id, " +
                "snippet(title, thumbnails/medium/url, " +
                "resourceId, videoOwnerChannelTitle, " +
                "videoOwnerChannelId, position)";

            return await addVideoRequest.ExecuteAsync();
        }

        public async Task 
            DeletePlaylistVideo(
            string uniqueVideoIdOfPlaylist)
        {
            if (!string.IsNullOrWhiteSpace(uniqueVideoIdOfPlaylist))
            {
                await _youtubeService.PlaylistItems.Delete(
                    uniqueVideoIdOfPlaylist).ExecuteAsync();
            }
        }
        
        public async Task<string>
            GetUserChannelId()
        {
            //get user channel id
            var channelRequest = _youtubeService.Channels.List("snippet");
            channelRequest.Mine = true;
            channelRequest.Fields = "items/id";
            var channelInfo = await channelRequest.ExecuteAsync();
            string channelId = null;

            if (channelInfo?.Items != null && 
                channelInfo.Items.Count > 0)
            {
                channelId = channelInfo.Items[0].Id;
            }

            if (string.IsNullOrWhiteSpace(channelId))
            {
                throw new AggregateException("No YouTube channel found.");
            }

            return channelId;
        }

        private static string GetPlaylistsRequestPartString()
        {
            return "snippet, contentDetails, status";
        }

        private static string GetVideoRequestPartString()
        {
            return "snippet, status";
        }
    }
}
