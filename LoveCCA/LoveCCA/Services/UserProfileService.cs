using LoveCCA.Models;
using LoveCCA.Views;
using Plugin.CloudFirestore;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LoveCCA.Services
{

    public class UserProfileService 
    {
        public UserProfileService()
        {

        }

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

        public async Task<List<UserProfile>> GetUserProfiles()
        {
            var query = await CrossCloudFirestore.Current
                     .Instance
                     .GetCollection("user_profiles")
                     .GetDocumentsAsync();

            return query.ToObjects<UserProfile>().ToList();

        }

        public async Task LoadUserProfile(string email)
        {
            try
            {
                var profile = await GetUserProfile(email);

                if (profile == null)
                {
                    // First time use -- set up new profile
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

                if(!string.IsNullOrEmpty(App.FCMToken))
                {
                    if (!CurrentUserProfile.FCMTokens.Any(t => t == App.FCMToken))
                    {
                        CurrentUserProfile.FCMTokens.Add(App.FCMToken);
                        await UpdateCurrentProfile();

                    }
                }

                if (CurrentUserProfile.Kids == null)
                    CurrentUserProfile.Kids = new List<Student>();

                var result = await StorageVault.GetValue("notificationsRegistered");
                if (result == null || !bool.Parse(result))
                {
                    PushNotificationService.Instance.Subscribe(new string[] { "urgent", "information" });
                    await StorageVault.SetValue("notificationsRegistered", "true");
                    Console.WriteLine("Intial registration of notifications complete;");
                }

            }
            catch (Exception)
            {
                
            }
        }

        public async Task UpdateStaffStatus(UserProfile user)
        {
            try
            {
                await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("user_profiles")
                         .GetDocument(user.Id)
                         .UpdateDataAsync(new { IsStaffMember = user.IsStaffMember });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error updating profile status " + ex.Message);
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
            try
            {
                await CrossCloudFirestore.Current
                 .Instance
                 .GetCollection("user_profiles")
                 .GetDocument(CurrentUserProfile.Id)
                 .UpdateDataAsync(new
                 {
                     Email = CurrentUserProfile.Email,
                     Name = CurrentUserProfile.Name,
                     CellPhone = CurrentUserProfile.CellPhone,
                     AllowNotifications = CurrentUserProfile.AllowNotifications,
                     //UrgentNotificationsOnly = CurrentUserProfile.UrgentNotificationsOnly,
                     FCMTokens = CurrentUserProfile.FCMTokens,
                     Kids = CurrentUserProfile.Kids
                 });
                if (CurrentUserProfile.AllowNotifications)
                {
                    PushNotificationService.Instance.Subscribe(new string[] { "urgent", "information" });
                    //if (CurrentUserProfile.UrgentNotificationsOnly)
                    //{
                    //    PushNotificationService.Instance.Unsubscribe("information");
                    //    PushNotificationService.Instance.Subscribe("urgent");
                    //}
                    //else
                    //{
                    //    PushNotificationService.Instance.Subscribe(new string[] { "urgent", "information" });
                    //}
                }
                else
                {
                    PushNotificationService.Instance.UnsubscribeAll();
                }
            }
            catch (Exception)
            {

            }

        }

        public async Task AddKid(Student kid)
        {
            if (string.IsNullOrEmpty(kid.Id))
                kid.Id = $"{kid.LastName.ToLower()}_{kid.FirstName.ToLower()}_{kid.Grade.Name}";
            if (CurrentUserProfile != null)
            {
                if (CurrentUserProfile.Kids != null)
                {
                    var item = CurrentUserProfile.Kids.FirstOrDefault(i => i.FirstName.ToLower() == kid.FirstName.ToLower() &&
                        i.LastName.ToLower() == kid.LastName.ToLower() &&
                        i.Grade == kid.Grade);


                    if (item == null)
                    {
                        CurrentUserProfile.Kids.Add(kid);
                        await UpdateCurrentProfile();
                    }
                }
            }
        }


        public async Task RemoveKid(Student kid)
        {
            if (CurrentUserProfile != null)
            {
                if (CurrentUserProfile.Kids != null)
                {
                    var item = CurrentUserProfile.Kids.FirstOrDefault(i => i.FirstName.ToLower() == kid.FirstName.ToLower() &&
                        i.LastName.ToLower() == kid.LastName.ToLower() &&
                        i.Grade == kid.Grade);

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
