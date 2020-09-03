using LoveCCA.Models;
using LoveCCA.Views;
using Plugin.CloudFirestore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LoveCCA.Services
{
    //public interface IUserProfileService
    //{
    //    UserProfile CurrentUserProfile { get; }
    //    Task LoadUserProfile(string email);
    //    Task<UserProfile> GetUserProfile(string email);
    //    Task RemoveKid(string name);
    //    Task AddKid(string name);
    //}

    public class UserProfileService 
    {

        private static UserProfileService _instance;

        public static UserProfileService Instance
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
                        Email = email,
                        AllowNotifications = true
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

        public async Task UpdateCurrentProfile()
        {
            await CrossCloudFirestore.Current
             .Instance
             .GetCollection("user_profiles")
             .GetDocument(CurrentUserProfile.Id)
             .UpdateDataAsync(new { Email = CurrentUserProfile.Email, 
                Name = CurrentUserProfile.Name,
                CellPhone = CurrentUserProfile.CellPhone,
                AllowNotifications = CurrentUserProfile.AllowNotifications,
                Kids =  CurrentUserProfile.Kids});
        }

        public async Task AddKid(string name)
        {
            if (CurrentUserProfile != null)
            {
                if (CurrentUserProfile.Kids != null)
                {
                    if (!CurrentUserProfile.Kids.Any(i => i == name))
                    {
                        CurrentUserProfile.Kids.Add(name);
                        await UpdateCurrentProfile();
                    }
                }
            }
        }


        public async Task RemoveKid(string name)
        {
            if (CurrentUserProfile != null)
            {
                if (CurrentUserProfile.Kids != null)
                {
                    var item = CurrentUserProfile.Kids.FirstOrDefault(i => i == name);
                    if (item != null)
                    {
                        CurrentUserProfile.Kids.Remove(item);
                        await UpdateCurrentProfile();
                    }
                }
            }
        }
    }
}
