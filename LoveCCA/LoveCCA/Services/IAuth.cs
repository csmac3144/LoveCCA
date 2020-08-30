using System.Threading.Tasks;

namespace LoveCCA.Services
{
    public interface IAuth
    {
        Task<string> LoginWithEmailPassword(string email, string password);
        Task<string> CreateUserWithEmailPassword(string email, string password);
        Task SendResetPasswordLink(string email);
        Task<bool> IsCurrentUserVerified(bool refresh);
        Task SendAccountVerificationLink();
        void SignOut();
        Task UpdatePassword(string password);
    }
}
