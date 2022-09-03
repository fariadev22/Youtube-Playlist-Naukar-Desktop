using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Youtube_Playlist_Naukar_Windows.Models;
using Youtube_Playlist_Naukar_Windows.Utilities;

namespace Youtube_Playlist_Naukar_Windows.Helpers
{
    public class LoginHelper
    {
        private readonly string _tokenStorageDirectory;
        private readonly string _applicationName;
        
        public LoginHelper(
            string tokenStorageDirectory,
            string applicationName)
        {
            _tokenStorageDirectory = tokenStorageDirectory;
            _applicationName = applicationName;
        }

        public async Task<UserSession> LoginUser(
            List<UserSession> availableUserSessions,
            CancellationToken cancellationToken)
        {
            bool loginSuccessful = false;

            var activeUserSession = 
                UpdateActiveUserSession(
                    availableUserSessions);

            while (!loginSuccessful &&
                   !cancellationToken.IsCancellationRequested)
            {
                if (activeUserSession != null)
                {
                    activeUserSession.UserCredential ??=
                        await AuthenticateUser(
                            activeUserSession.UserIdForTokenStorage,
                            cancellationToken);

                    if (!IsUserCredentialTokenValid(
                            activeUserSession.UserCredential))
                    {
                        //authenticate again

                        activeUserSession.UserCredential =
                            await AuthenticateUser(
                                activeUserSession.UserIdForTokenStorage,
                                cancellationToken);

                        string emailAddress =
                            await CredentialUtilities.
                                GetEmailAddressAssociatedWithCredential(
                                    activeUserSession.UserCredential,
                                    _applicationName,
                                    cancellationToken);

                        if (!string.IsNullOrWhiteSpace(emailAddress))
                        {
                            //Associated email address has changed.
                            if (activeUserSession.EmailAddress !=
                                emailAddress)
                            {
                                //the current active user data is no longer relevant
                                availableUserSessions.Remove(activeUserSession);

                                //let's see if we already have a session with same email
                                var existingSessionWithEmail =
                                    availableUserSessions.Find(
                                        s => s.EmailAddress == emailAddress);

                                //session with this email already exists. Let's update that
                                //and remove the current active session
                                if (existingSessionWithEmail != null)
                                {
                                    activeUserSession = 
                                        await UpdateExistingSessionWithNewCredential(
                                            existingSessionWithEmail,
                                            activeUserSession);
                                }
                                else //it's a new account
                                {
                                    activeUserSession = CreateUserSession(emailAddress, 
                                        activeUserSession.UserCredential,
                                        Guid.NewGuid().ToString());
                                    availableUserSessions.Add(activeUserSession);
                                }
                            }
                        }
                        else //authentication failed. Need to try another account
                        {
                            availableUserSessions.Remove(activeUserSession);
                            activeUserSession = 
                                UpdateActiveUserSession(availableUserSessions);
                        }
                    }
                    else
                    {
                        loginSuccessful = true;
                    }
                }
                else //no user session exists
                {
                    activeUserSession = 
                        await LoginNewAccount(availableUserSessions,
                            cancellationToken);
                }
            }

            return activeUserSession;
        }

        public async Task<UserSession> LoginNewAccount(
            List<UserSession> availableUserSessions,
            CancellationToken cancellationToken)
        {
            UserSession activeUserSession;
            string userId = GenerateUserIdForTokenStorage();
            var credential = await AuthenticateUser(userId, cancellationToken);
            var emailAddress =
                await CredentialUtilities.
                    GetEmailAddressAssociatedWithCredential(
                        credential,
                        _applicationName,
                        cancellationToken);

            var existingSession =
                availableUserSessions.Find(
                    s => s.EmailAddress == emailAddress);

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                Console.WriteLine("ERROR: Login for new account failed.");
                return null;
            }

            var newSession =
                CreateUserSession(emailAddress, credential, userId);

            availableUserSessions.ForEach(s => s.IsDefaultUser = false);

            if (existingSession != null)
            {
                activeUserSession =
                    await UpdateExistingSessionWithNewCredential(
                        existingSession, newSession);
            }
            else
            {
                availableUserSessions.Add(newSession);
                activeUserSession = newSession;
                activeUserSession.IsDefaultUser = true;
            }

            return activeUserSession;
        }

        private async Task<UserSession> 
            UpdateExistingSessionWithNewCredential(
            UserSession existingSessionWithEmail,
            UserSession newUserSession)
        {
            //revoke the existing session's credential
            //as it will now use the new one
            await CredentialUtilities.RevokeCredential(
                existingSessionWithEmail.UserCredential);

            existingSessionWithEmail.UserIdForTokenStorage =
                newUserSession.UserIdForTokenStorage;
            existingSessionWithEmail.UserCredential =
                newUserSession.UserCredential;
            existingSessionWithEmail.IsDefaultUser = true;

            return existingSessionWithEmail;
        }

        private UserSession UpdateActiveUserSession(
            List<UserSession> availableUserSessions)
        {
            if (availableUserSessions.Count <= 0)
            {
                return null;
            }

            var activeUserSession =
                availableUserSessions.FirstOrDefault(u => 
                    u.IsDefaultUser);

            if (activeUserSession == null)
            {
                activeUserSession = 
                    availableUserSessions.First();
                activeUserSession.IsDefaultUser = true;
            }

            return activeUserSession;
        }

        private async Task<UserCredential> AuthenticateUser(
            string userIdForTokenStorage,
            CancellationToken cancellationToken)
        {
            return await CredentialUtilities.GenerateCredentialForUser(
                userIdForTokenStorage, _tokenStorageDirectory,
                cancellationToken);
        }

        private UserSession CreateUserSession(
            string emailAddress,
            UserCredential credential,
            string userId)
        {
            return new UserSession
            {
                EmailAddress = emailAddress,
                IsDefaultUser = true,
                UserCredential = credential,
                UserData = new UserData
                {
                    EmailAddress = emailAddress
                },
                UserIdForTokenStorage = userId
            };
        }

        private static bool IsUserCredentialTokenValid(
            UserCredential userCredential)
        {
            return userCredential?.Token?.IsExpired(
                Google.Apis.Util.SystemClock.Default) == false;
        }

        private static string GenerateUserIdForTokenStorage()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
