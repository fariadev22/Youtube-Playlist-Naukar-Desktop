using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Forms;

namespace Youtube_Playlist_Naukar_Windows.Utilities
{
    public static class CommonUtilities
    {
        public static void DownloadImagesToUserDirectory(
            string userDirectoryPath,
            List<KeyValuePair<string, string>> imageUrls,
            ImageList imageList)
        {
            foreach (var imageUrl in imageUrls)
            {
                try
                {
                    System.Net.WebRequest request =
                        System.Net.WebRequest.Create(imageUrl.Value);
                    System.Net.WebResponse response = request.GetResponse();
                    Stream respStream = response.GetResponseStream();
                    Bitmap bitmap = new Bitmap(respStream);
                    respStream?.Dispose();

                    imageList.Images.Add(imageUrl.Key, bitmap);
                }
                catch
                {
                    System.Net.WebRequest request =
                        System.Net.WebRequest.Create(
                            "https://www.contentviewspro.com/wp-content/uploads/2017/07/default_image.png");
                    System.Net.WebResponse response = request.GetResponse();
                    Stream respStream = response.GetResponseStream();
                    Bitmap bitmap = new Bitmap(respStream);
                    respStream?.Dispose();

                    imageList.Images.Add(imageUrl.Key, bitmap);
                }
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
