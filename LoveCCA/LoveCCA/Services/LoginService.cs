using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.Services
{
    public interface ILoginService
    {
        bool IsAuthenticated { get; }
        Task<bool> TrySilentLogin();
        Task<bool> SendAccountVerificationLink();
        Task<bool> IsCurrentUserVerified(bool refresh);
        Task<bool> UpdatePassword(string newPassword); 
        Task<bool> SendResetPasswordLink(string email);
        Task<bool> LoginWithEmailPassword(string email, string password);
        Task<bool> CreateUserWithEmailPassword(string email, string password);
        bool SignOut();

    }

    public class LoginService : ILoginService
    {
        private IAuth _auth;

        private static ILoginService _instance;
        public static ILoginService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LoginService();

                return _instance;
            }
        }

        public bool IsAuthenticated { get; private set; }

        public LoginService()
        {
            _auth = DependencyService.Get<IAuth>();
        }

        public async Task<bool> CreateUserWithEmailPassword(string email, string password)
        {
            try
            {
                string token = await _auth.CreateUserWithEmailPassword(email, password);
                if (!string.IsNullOrEmpty(token))
                {
                    await StorageVault.SetCredentials(email, password);
                    await StorageVault.SetToken(token);
                    await UserProfileService.Instance.LoadUserProfile(email);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<bool> IsCurrentUserVerified(bool refresh)
        {
            return _auth.IsCurrentUserVerified(refresh);
        }

        public async Task<bool> LoginWithEmailPassword(string email, string password)
        {
            try
            {
                IsAuthenticated = false;
                string token = await _auth.LoginWithEmailPassword(email, password);
                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        await StorageVault.SetCredentials(email, password);
                        await StorageVault.SetToken(token);
                        await UserProfileService.Instance.LoadUserProfile(email);
                        IsAuthenticated = true;
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("Either can't save credentials or load profile");
                    }
                }
            }
            catch (InvalidLoginException)
            {
                Debug.WriteLine("Invalid login exception");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return IsAuthenticated;
        }

        public async Task<bool> SendAccountVerificationLink()
        {
            try
            {
                await _auth.SendAccountVerificationLink();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendResetPasswordLink(string email)
        {
            try
            {
                await _auth.SendResetPasswordLink(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SignOut()
        {
            try
            {
                _auth.SignOut();
                StorageVault.ClearCredentials();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> TrySilentLogin()
        {
            try
            {
                var credentials = await StorageVault.GetCredentials();
                return await LoginWithEmailPassword(credentials.Item1, credentials.Item2); 
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdatePassword(string newPassword)
        {
            try
            {
                await _auth.UpdatePassword(newPassword);
                var currentCredentials = await StorageVault.GetCredentials();
                await StorageVault.SetCredentials(currentCredentials.Item1, newPassword);
                return true;
            }
            catch (WeakPasswordException)
            {
                throw;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
