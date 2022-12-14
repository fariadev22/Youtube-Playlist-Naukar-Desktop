using System;
using System.Collections.Generic;
using Google.Apis.YouTube.v3.Data;

namespace Youtube_Playlist_Naukar_Windows.Models
{
    public class UserPlayList
    {
        public string PlaylistETag { get; set; }

        public string PlaylistVideosETag { get; set; }

        public string Id { get; set; }

        public string Description { get; set; }

        public PrivacyStatusEnum PrivacyStatus { get; set; }

        public string Title { get; set; }

        public Thumbnail Thumbnail { get; set; }

        public long TotalVideosInPlaylist { get; set; }

        public DateTime? PublishedOn { get; set; }

        public string PlaylistOwnerChannelId { get; set; }

        public string PlaylistOwnerChannelTitle { get; set; }

        public Dictionary<string, UserPlayListVideo> PlayListVideos { get; set; } 
            = new Dictionary<string, UserPlayListVideo>();

        public static UserPlayList ConvertYoutubePlaylistToUserPlaylist(
            Playlist playList)
        {
            Enum.TryParse(
                playList.Status?.PrivacyStatus,
                true,
                out PrivacyStatusEnum privacyStatus);

            var userPlaylist = new UserPlayList
            {
                Id = playList.Id,
                PlaylistETag = playList.ETag,
                TotalVideosInPlaylist =
                    playList.ContentDetails?.ItemCount ?? 0,
                PrivacyStatus = privacyStatus
            };

            if (playList.Snippet != null)
            {
                userPlaylist.Title =
                    playList.Snippet.Title;
                userPlaylist.Description =
                    playList.Snippet.Description;

                if (!string.IsNullOrWhiteSpace(
                    playList.Snippet.Thumbnails?.Medium?.Url))
                {
                    userPlaylist.Thumbnail =
                        new Thumbnail
                        {
                            LocalPathFromUserDirectory =
                                "/" + Constants.PlaylistThumbnailsFolder +
                                "/" + playList.Id + ".jpg",
                            Url = playList.Snippet.Thumbnails.Medium.Url
                        };
                }
                
                userPlaylist.PublishedOn =
                    playList.Snippet.PublishedAt;
                userPlaylist.PlaylistOwnerChannelId =
                    playList.Snippet.ChannelId;
                userPlaylist.PlaylistOwnerChannelTitle =
                    playList.Snippet.ChannelTitle;
            }

            return userPlaylist;
        }
    }
}
