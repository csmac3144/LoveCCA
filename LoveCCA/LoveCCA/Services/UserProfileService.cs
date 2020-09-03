using LoveCCA.Models;
using Plugin.CloudFirestore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LoveCCA.Services
{
    public interface IUserProfileService
    {
        UserProfile CurrentUserProfile { get; }
        Task LoadUserProfile(string email);
        Task<UserProfile> GetUserProfile(string email);
    }

    public class UserProfileService : IUserProfileService
    {

        private static IUserProfileService _instance;

        public static IUserProfileService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UserProfileService();

                return _instance;
            }
        }

        public UserProfile CurrentUserProfile { get; private set; }

        public async Task LoadUserProfile(string email)
        {
            try
            {
                var profile = await GetUserProfile(email);

                if (profile == null)
                {
                    profile = new UserProfile
                    {
                        Id = email,
                        Email = email
                    };
                    await CrossCloudFirestore.Current
                                 .Instance
                                 .GetCollection("user_profiles")
                                 .AddDocumentAsync(profile);
                    profile = await GetUserProfile(email);
                }
                CurrentUserProfile = profile;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserProfile> GetUserProfile(string email)
        {
            try
            {
                var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .GetCollection("user_profiles")
                                     .WhereEqualsTo("Email", email.ToLower())
                                     .LimitTo(1)
                                     .GetDocumentsAsync();

                var documents = query.ToObjects<UserProfile>();

                return documents.FirstOrDefault();

            }
            catch (Exception)
            {
                Debug.WriteLine("Error fetching user profile");
            }
            return null;
        }
    }
}
