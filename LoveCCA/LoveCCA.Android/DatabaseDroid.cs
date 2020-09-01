using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using LoveCCA.Models;

namespace LoveCCA.Droid
{
    class DatabaseDroid
    {
        private FirebaseDatabase firebase;
        public DatabaseDroid()
        {
            firebase = FirebaseDatabase.Instance;
        }


        public object GetProfile(string email)
        {

        }

        public async Task<List<Person>> GetAllPersons()
        {
            try
            {
                var dbref = firebase.GetReferenceFromUrl("https://love-cca.firebaseio.com");
                dbref.Child("user_profiles").Child(Guid.NewGuid().ToString()).Child("email").SetValue("steve.macdonald1@gmail.com");
                //dbref.Child("user_profiles").Child("bob@formatec.ca").Child("children").Child("0").SetValue("Randy");
                //dbref.Child("user_profiles").Child("steve@formatec.ca").Child("children").Child("0").SetValue("Katie");
                //dbref.Child("user_profiles").Child("steve@formatec.ca").Child("children").Child("1").SetValue("Ruby");
                //Guid.NewGuid().ToString()
                var listener = new MyValueEventListener();
                dbref.Child("user_profiles").AddListenerForSingleValueEvent(listener);
                

            }
            catch (Exception ex)
            {

                throw;
            }


            return null;
        }

        public class MyValueEventListener : Java.Lang.Object, IValueEventListener
        {

            public void OnCancelled(DatabaseError error)
            {
                throw new NotImplementedException();
            }

            public void OnDataChange(DataSnapshot snapshot)
            {
                System.Diagnostics.Debug.WriteLine(snapshot);
            }

        }
        public class ProfileEventListener : Java.Lang.Object, IValueEventListener
        {
            private string _email;
            public ProfileEventListener(string email)
            {
                _email = email;
            }

            public void OnCancelled(DatabaseError error)
            {
                throw new NotImplementedException();
            }

            public void OnDataChange(DataSnapshot snapshot)
            {
                System.Diagnostics.Debug.WriteLine(snapshot);
            }

        }
    }
}