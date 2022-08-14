using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Youtube_Playlist_Naukar_Windows.Models;

namespace Youtube_Playlist_Naukar_Windows.Utilities
{
    public class SessionStorageUtilities
    {
        private const string UserSessionsBackupFileName = "user_sessions.json";

        private const string UserMetadataBackupFileName = "user_metadata.json";

        public static List<UserSession> TryLoadDataFromUserSessionsFile(
            string userSessionsFileDirectory)
        {
            string userMetadataFilePath =
                Path.Combine(userSessionsFileDirectory, UserSessionsBackupFileName);

            if (File.Exists(userMetadataFilePath))
            {
                try
                {
                    return JsonConvert.DeserializeObject<List<UserSession>>(
                        File.ReadAllText(userMetadataFilePath));
                }
                catch
                {
                    //
                }
            }

            return null;
        }

        public static UserData TryLoadDataFromUserMetadataFile(
            string userMetadataFileDirectory)
        {
            string userMetadataFilePath =
                Path.Combine(
                    userMetadataFileDirectory,
                    UserMetadataBackupFileName
                );

            if (File.Exists(userMetadataFilePath))
            {
                try
                {
                    return JsonConvert.DeserializeObject<UserData>(
                        File.ReadAllText(userMetadataFilePath));
                }
                catch
                {
                    //
                }

            }

            return null;
        }

        public static void SaveSessionsData(
            List<UserSession> userSessions,
            string applicationDirectory)
        {
            if (userSessions == null ||
                userSessions.Count <= 0)
            {
                return;
            }

            try
            {
                //backup sessions
                string sessionsData =
                    JsonConvert.SerializeObject(
                        userSessions, Formatting.Indented);
                string sessionsFilePath =
                    Path.Combine(
                        applicationDirectory, UserSessionsBackupFileName);

                File.WriteAllText(sessionsFilePath, sessionsData);

                //backup user specific info

                foreach (var sessionData in userSessions)
                {
                    if (sessionData?.UserData == null)
                    {
                        continue;
                    }

                    string userSessionDirectoryPath =
                        Path.Combine(
                            applicationDirectory,
                            CommonUtilities.Base64EncodeString(
                                sessionData.EmailAddress));

                    string userSessionFilePath =
                        Path.Combine(
                            userSessionDirectoryPath, UserMetadataBackupFileName);

                    File.WriteAllText(userSessionFilePath,
                        JsonConvert.SerializeObject(
                            sessionData.UserData, Formatting.Indented));
                }
            }
            catch
            {
                //
            }
        }

        public static string SetupUserMetadataSessionDirectory(
            string applicationDirectory,
            string userEmailAddress)
        {
            string activeUserSessionDirectoryPath =
                Path.Combine(
                    applicationDirectory,
                    CommonUtilities.Base64EncodeString(userEmailAddress));

            CommonUtilities.CreateDirectoryIfRequired(
                activeUserSessionDirectoryPath);

            CommonUtilities.CreateDirectoryIfRequired(
                activeUserSessionDirectoryPath + "/" + 
                Constants.VideoThumbnailsFolder);

            CommonUtilities.CreateDirectoryIfRequired(
                activeUserSessionDirectoryPath + "/" +
                Constants.PlaylistThumbnailsFolder);

            return activeUserSessionDirectoryPath;
        }

        public static void DeleteUserMetadataSessionDirectory(
            string applicationDirectory,
            string userEmailAddress)
        {
            string activeUserSessionDirectoryPath =
                Path.Combine(
                    applicationDirectory,
                    CommonUtilities.Base64EncodeString(userEmailAddress));

            CommonUtilities.DeleteDirectoryIfExists(
                activeUserSessionDirectoryPath);
        }
    }
}
