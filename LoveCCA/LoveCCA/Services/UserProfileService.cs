﻿using LoveCCA.Models;
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
                FCMTokens = CurrentUserProfile.FCMTokens,
                Kids =  CurrentUserProfile.Kids});
        }

        public async Task AddKid(Student kid)
        {
            if (string.IsNullOrEmpty(kid.Id))
                kid.Id = $"{kid.LastName.ToLower()}_{kid.FirstName.ToLower()}_{kid.Grade}";
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
