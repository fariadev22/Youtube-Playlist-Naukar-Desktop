using Google.Apis.YouTube.v3.Data;
using System;

namespace Youtube_Playlist_Naukar_Windows.Models
{
    public class UserPlayListVideo
    {
        public string ETag { get; set; }

        public string UniqueVideoIdInPlaylist { get; set; }

        public string VideoId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Thumbnail Thumbnail { get; set; }

        public string VideoOwnerChannelTitle { get; set; }

        public string VideoOwnerChannelId { get; set; }

        public string VideoAddedToPlaylistByChannelId { get; set; }

        public string VideoAddedToPlaylistByChannelTitle { get; set; }

        public long? PositionInPlayList { get; set; }

        public DateTime? VideoAddedToPlaylistOn { get; set; }

        public PrivacyStatusEnum PrivacyStatus { get; set; }

        public static UserPlayListVideo
            ConvertPlayListItemToUserPlayListVideo(
                PlaylistItem playListItem)
        {
            Enum.TryParse(
                playListItem.Status?.PrivacyStatus,
                true,
                out PrivacyStatusEnum privacyStatus);

            var userPlaylistVideo =
                new UserPlayListVideo
                {
                    ETag = playListItem.ETag,
                    UniqueVideoIdInPlaylist = playListItem.Id,
                    PrivacyStatus = privacyStatus
                };

            if (playListItem.Snippet != null)
            {
                userPlaylistVideo.VideoId =
                    playListItem.Snippet.ResourceId.VideoId;

                if (!string.IsNullOrWhiteSpace(
                    playListItem.Snippet.Thumbnails?.Medium?.Url))
                {
                    userPlaylistVideo.Thumbnail = new Thumbnail
                    {
                        Url =
                            playListItem.Snippet.Thumbnails.Medium.Url,
                        LocalPathFromUserDirectory =
                            "/" + Constants.VideoThumbnailsFolder +
                            "/" + userPlaylistVideo.UniqueVideoIdInPlaylist + 
                            ".jpg"
                    };
                }
                
                userPlaylistVideo.Title =
                    playListItem.Snippet.Title;
                userPlaylistVideo.Description =
                    playListItem.Snippet.Description;
                userPlaylistVideo.VideoOwnerChannelId =
                    playListItem.Snippet.VideoOwnerChannelId;
                userPlaylistVideo.VideoOwnerChannelTitle =
                    playListItem.Snippet.VideoOwnerChannelTitle;
                userPlaylistVideo.PositionInPlayList =
                    playListItem.Snippet.Position;
                userPlaylistVideo.VideoAddedToPlaylistOn =
                    playListItem.Snippet.PublishedAt;
                userPlaylistVideo.VideoAddedToPlaylistByChannelId =
                    playListItem.Snippet.ChannelId;
                userPlaylistVideo.VideoAddedToPlaylistByChannelTitle =
                    playListItem.Snippet.ChannelTitle;
                userPlaylistVideo.VideoOwnerChannelId =
                    playListItem.Snippet.VideoOwnerChannelId;
                userPlaylistVideo.VideoOwnerChannelTitle =
                    playListItem.Snippet.VideoOwnerChannelTitle;
            }

            return userPlaylistVideo;
        }
    }
}
