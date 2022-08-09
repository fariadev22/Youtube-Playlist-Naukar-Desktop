using System;
using System.Collections.Generic;
using System.Linq;
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
                    new YouTubeService(new BaseClientService.Initializer
                    {
                        HttpClientInitializer = activeUserSession.UserCredential,
                        ApplicationName = Constants.ApplicationName
                    });
            }
            else
            {
                _youtubeService = new YouTubeService();
            }
        }

        /// <summary>
        /// This returns only user-owned playlists.
        /// </summary>
        public async Task<List<Playlist>> GetUserPlayListsData(
            string channelId,
            string pageToken = null)
        {
            var playListsRequest = _youtubeService.Playlists.List("snippet");
            playListsRequest.ChannelId = channelId;
            playListsRequest.MaxResults = 50;
            playListsRequest.Fields = "items/id,items/kind,items/snippet/title," +
                                      "items/snippet/thumbnails/default";

            if (!string.IsNullOrWhiteSpace(pageToken))
            {
                playListsRequest.PageToken = pageToken;
            }

            var playListsResponse = await playListsRequest.ExecuteAsync();
            var playLists = playListsResponse.Items?.ToList() 
                            ?? new List<Playlist>();

            if (!string.IsNullOrWhiteSpace(playListsResponse.NextPageToken))
            {
                var nextPlayLists =
                    await GetUserPlayListsData(
                        playListsResponse.NextPageToken);

                if (nextPlayLists != null &&
                   nextPlayLists.Count > 0)
                {
                    playLists.AddRange(nextPlayLists);
                }
            }

            return playLists;
        }

        public async Task<Playlist> GetUserPlaylistMetaData(
            string playlistId)
        {
            var playListRequest = _youtubeService.Playlists.List("snippet");
            playListRequest.Fields = "items/id,items/kind,items/snippet/title," +
                                      "items/snippet/thumbnails/default";
            playListRequest.Id = playlistId;
            var playListResponse = 
                await playListRequest.ExecuteAsync();
            var playLists = playListResponse.Items?.ToList() 
                            ?? new List<Playlist>();

            if (playLists.Count > 0)
            {
                return playLists[0];
            }

            return null;
        }

        public async Task<List<PlaylistItem>> GetPlaylistVideos(
            UserPlayList userPlayList,
            string pageToken = null)
        {
            List<PlaylistItem> playlistItems = 
                new List<PlaylistItem>();

            var playListItemsRequest = 
                _youtubeService.PlaylistItems.List("snippet");
            playListItemsRequest.MaxResults = 500;
            playListItemsRequest.PlaylistId = userPlayList.Id;
            playListItemsRequest.Fields =
                "nextPageToken, items(id)" +
                "items/snippet(title, thumbnails/default, resourceId, " +
                "videoOwnerChannelTitle, videoOwnerChannelId, position)";

            if (!string.IsNullOrWhiteSpace(pageToken))
            {
                playListItemsRequest.PageToken = pageToken;
            }

            var playListItemsResponse = 
                await playListItemsRequest.ExecuteAsync();

            if (playListItemsResponse != null)
            {
                if (playListItemsResponse.Items != null &&
                    playListItemsResponse.Items.Count > 0)
                {
                    playlistItems.AddRange(playListItemsResponse.Items);
                }

                if (!string.IsNullOrWhiteSpace(
                    playListItemsResponse.NextPageToken))
                {
                    var nextItems = await GetPlaylistVideos(userPlayList,
                        playListItemsResponse.NextPageToken);

                    if (nextItems != null &&
                        nextItems.Count > 0)
                    {
                        playlistItems.AddRange(nextItems);
                    }
                }
            }

            return playlistItems;
        }

        public async Task<PlaylistItem> AddVideoToPlayList(
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
                "snippet(title, thumbnails/default, resourceId, videoOwnerChannelTitle, " +
                "videoOwnerChannelId, position)";

            return await addVideoRequest.ExecuteAsync();
        }

        public async Task DeletePlaylistVideo(
            string uniqueVideoIdOfPlaylist)
        {
            if (!string.IsNullOrWhiteSpace(uniqueVideoIdOfPlaylist))
            {
                await _youtubeService.PlaylistItems.Delete(
                    uniqueVideoIdOfPlaylist).ExecuteAsync();
            }
        }
        
        public async Task<string> GetUserChannelId()
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
    }
}
