﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Windows.Forms;
using Youtube_Playlist_Naukar_Windows.Models;

namespace Youtube_Playlist_Naukar_Windows.Utilities
{
    public static class CommonUtilities
    {
        public static void DownloadImageToUserDirectory(
            string userDirectoryPath,
            string imageId,
            Thumbnail thumbnail)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(
                thumbnail.Url, 
                userDirectoryPath + "/" + 
                    thumbnail.LocalPathFromUserDirectory);
        }

        public static void ConvertLocalImageToBitmapAndStoreInImageList(
            string userDirectoryPath, 
            ImageList imageList, 
            string imageId,
            string imageFileName)
        {
            try
            {
                using (FileStream imageStream = new FileStream(
                    userDirectoryPath + "/" + imageFileName,
                    FileMode.Open))
                {
                    Bitmap bitmap = new Bitmap(imageStream);
                    imageList.Images.Add(imageId, bitmap);
                }
            }
            catch
            {
                using (FileStream imageStream = new FileStream(
                    "default_image.png", FileMode.Open))
                {
                    var defaultBitmap = new Bitmap(imageStream);
                    imageList.Images.Add(imageId, defaultBitmap);
                }
            }
        }

        public static Bitmap ConvertLocalImageToBitmap(
            string userDirectoryPath,
            string imageFileName,
            int width,
            int height)
        {
            try
            {
                return new Bitmap(
                    Image.FromFile(
                        userDirectoryPath + "/" + imageFileName),
                    width,
                    height);
            }
            catch
            {
                return new Bitmap(
                    Image.FromFile(
                        "default_image.png"),
                    width,
                    height);
            }
        }

        public static string Base64EncodeString(string toEncode)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(toEncode);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static void CreateDirectoryIfRequired(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public static void DeleteDirectoryIfExists(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                var files = Directory.EnumerateFiles(directoryPath).ToList();
                files.ForEach(File.Delete);

                var directories =
                    Directory.EnumerateDirectories(directoryPath).ToList();
                directories.ForEach(DeleteDirectoryIfExists);

                Directory.Delete(directoryPath);
            }
        }

        public static bool TryGetVideoIdFromYoutubeUrl(string youtubeUrl,
            out string videoId)
        {
            videoId = string.Empty;

            if (string.IsNullOrWhiteSpace(youtubeUrl))
            {
                return false;
            }

            var unfoldedUrl = 
                youtubeUrl.Replace("youtu.be/", "youtube.com/watch?v=");

            if (Uri.TryCreate(unfoldedUrl, UriKind.Absolute, out Uri uri))
            {
                videoId = HttpUtility.ParseQueryString(uri.Query).Get("v");
            }

            if (string.IsNullOrWhiteSpace(videoId))
            {
                return false;
            }

            return true;
        }

        public static string GetYoutubeVideoUrlFromVideoId(
            string videoId)
        {
            if (string.IsNullOrWhiteSpace(videoId))
            {
                return string.Empty;
            }

            return "https://youtube.com/watch?v=" + videoId;
        }

        public static string GetYoutubeChannelUrlFromChannelId(
            string channelId)
        {
            if (string.IsNullOrWhiteSpace(channelId))
            {
                return string.Empty;
            }

            return "https://www.youtube.com/channel/" + channelId;
        }

        public static string GetYoutubePlaylistUrlFromPlaylistId(
            string playlistId)
        {
            if (string.IsNullOrWhiteSpace(playlistId))
            {
                return string.Empty;
            }

            return "https://www.youtube.com/playlist?list=" + playlistId;
        }

        public static bool TryGetPlaylistIdFromYoutubeUrl(string youtubeUrl,
            out string playListId)
        {
            playListId = string.Empty;

            if (string.IsNullOrWhiteSpace(youtubeUrl))
            {
                return false;
            }

            if (Uri.TryCreate(youtubeUrl, UriKind.Absolute, out Uri uri))
            {
                playListId = HttpUtility.ParseQueryString(uri.Query).Get("list");
            }

            if (string.IsNullOrWhiteSpace(playListId))
            {
                return false;
            }

            return true;
        }
    }
}
