using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;

namespace Youtube_Playlist_Naukar_Windows.Utilities
{
    public static class CredentialUtilities
    {
        public static async Task RevokeCredential(
            UserCredential userCredential)
        {
            if (userCredential != null)
            {
                await userCredential.RevokeTokenAsync(CancellationToken.None);
            }
        }

        public static async Task<UserCredential> GenerateCredentialForUser(
            string userIdForTokenStorage,
            string tokenStorageDirectory)
        {
            UserCredential credential;

            using (var stream = new FileStream(
                "client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[]
                    {
                        YouTubeService.Scope.Youtube,
                        GmailService.Scope.GmailMetadata
                    },
                    userIdForTokenStorage,
                    CancellationToken.None,
                    new FileDataStore(tokenStorageDirectory));
            }

            return credential;
        }

        public static async Task<string> GetEmailAddressAssociatedWithCredential(
            UserCredential userCredential,
            string applicationName)
        {
            var service = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = userCredential,
                ApplicationName = applicationName,
            });

            try
            {
                var gmailProfileRequest = service.Users.GetProfile("me");
                gmailProfileRequest.Fields = "emailAddress";
                var gmailProfileResponse = await gmailProfileRequest.ExecuteAsync();
                return gmailProfileResponse.EmailAddress;
            }
            catch
            {
                //
            }

            return null;
        }
    }
}
