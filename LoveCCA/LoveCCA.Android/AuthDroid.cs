
using System;
using System.Threading.Tasks;
using Firebase.Auth;
using LoveCCA.Droid;
using LoveCCA.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthDroid))]
namespace LoveCCA.Droid
{
    public class AuthDroid : IAuth
    {
        public async Task SendResetPasswordLink(string email)
        {
            try
            {
                await FirebaseAuth.Instance.SendPasswordResetEmailAsync(email);
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
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = await user.User.GetIdTokenAsync(false);
                return token.Token;
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                throw new InvalidLoginException();
            }
        }
        public async Task<string> CreateUserWithEmailPassword(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                var token = await user.User.GetIdTokenAsync(false);
                return token.Token;
            }
            catch (FirebaseAuthWeakPasswordException)
            {
                throw new WeakPasswordException();
            }
            catch (FirebaseAuthInvalidCredentialsException)
            {
                throw new BadEmailFormatException();
            }
            catch (FirebaseAuthUserCollisionException)
            {
                throw new EmailInUseException();
            }
            catch (Exception e)
            {
                throw new SignUpErrorException();
            }
        }
    }
}