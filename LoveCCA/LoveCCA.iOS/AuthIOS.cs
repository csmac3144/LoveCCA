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