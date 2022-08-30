using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Youtube_Playlist_Naukar_Windows.Models;

namespace Youtube_Playlist_Naukar_Windows.Helpers
{
    public class AccountHelper
    {
        private static readonly AccountHelper
            AccountHelperInstance =
                new AccountHelper();

        private static UserSession _activeUserSession;

        static AccountHelper()
        {
        }

        public static AccountHelper GetAccountHelper
        {
            get
            {
                return AccountHelperInstance;
            }
        }

        public async Task<(bool, string)> TryOpenUserAccount(
            CancellationToken cancellationToken,
            UserSession userSession = null)
        {
            _activeUserSession =
                userSession ?? 
                await SessionStorageManager.GetSessionManager
                    .StartSession(cancellationToken);

            if (_activeUserSession == null)
            {
                return (false, "Login failed.");
            }

            ApiClient.GetApiClient
                .Initialize(_activeUserSession);

            try
            {
                var channelId =
                    await ApiClient.GetApiClient.GetUserChannelId(
                        cancellationToken);

                SessionStorageManager.GetSessionManager.SaveChannelIdInUserSession(
                    channelId);

                return (true, string.Empty);
            }
            catch (AggregateException)
            {
                return (false,
                    "You do not have a YouTube Channel. " +
                    "Please create a channel against your " +
                    "YouTube account and then try again.");
            }
            catch (HttpRequestException)
            {
                return (false,
                    "Unable to connect to YouTube server. " +
                    "Please check your internet connection.");
            }
            catch
            {
                return (false,
                    "An unknown error occurred. " +
                    "Please try again later.");
            }
        }

        public async Task<(bool, string)> ChangeAccount(
            string emailAddress,
            CancellationToken cancellationToken)
        {
            _activeUserSession =
                await SessionStorageManager.GetSessionManager
                    .ChangeSession(emailAddress, cancellationToken);

            return await TryOpenUserAccount(
                cancellationToken, 
                _activeUserSession);
        }

        public List<string> GetActiveAccountEmails()
        {
            return SessionStorageManager.GetSessionManager
                .GetUserSessionEmails();
        }

        public async Task<(bool, string)> ForgetAndCloseAccount()
        {
            try
            {
                await SessionStorageManager.GetSessionManager
                    .ForgetCurrentUser();

                _activeUserSession = null;

                return (true, "");
            }
            catch
            {
                return (false, "Unable to remove account details. " +
                               "Please try again.");
            }
        }

        public UserSession GetActiveUserSession()
        {
            return _activeUserSession;
        }
    }
}
