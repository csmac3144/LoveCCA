using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Auth;
using LoveCCA.iOS;
using LoveCCA.Services;
using Foundation;

[assembly: Dependency(typeof(AuthIOS))]
namespace LoveCCA.iOS
{
    public class AuthIOS : IAuth
    {

        public void SignOut()
        {
            try
            {
                NSError error;
                Auth.DefaultInstance.SignOut(out error);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> IsCurrentUserVerified(bool refresh)
        {
            try
            {
                if (refresh)
                    await Auth.DefaultInstance.CurrentUser.ReloadAsync();
                return Auth.DefaultInstance.CurrentUser.IsEmailVerified;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public async Task UpdatePassword(string password)
        {
            try
            {
                await Auth.DefaultInstance.CurrentUser.UpdatePasswordAsync(password);
            }
            catch (Exception)
            {
                throw new UpdatePasswordException();
            }
        }
        public async Task SendAccountVerificationLink()
        {
            try
            {
                await Auth.DefaultInstance.CurrentUser.SendEmailVerificationAsync();
            }
            catch (Exception)
            {
                throw new SendAccountVerificationLinkException();
            }
        }
        public async Task SendResetPasswordLink(string email)
        {
            try
            {
                await Auth.DefaultInstance.SendPasswordResetAsync(email);
            }
            catch (Exception)
            {
                throw new SendPasswordResetLinkException();
            }
        }
        public async Task<string> LoginWithEmailPassword(string email, string password)
        {
            try
            {
                var user = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
                return await user.User.GetIdTokenAsync();
            }
            catch (Exception)
            {
                throw new InvalidLoginException();
            }

        }
        public async Task<string> CreateUserWithEmailPassword(string email, string password)
        {
            try
            {
                var user = await Auth.DefaultInstance.CreateUserAsync(email, password);
                return await user.User.GetIdTokenAsync();
            }

            catch (NSErrorException e)
            {
                switch (e.Code)
                {
                    case 17007:
                        throw new EmailInUseException();
                    case 17008:
                        throw new BadEmailFormatException();
                    case 17026:
                        throw new WeakPasswordException();
                    default:
                        throw new SignUpErrorException();
                }
            }

        }
    }
}