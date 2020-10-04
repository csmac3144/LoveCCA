using System;
using System.Threading.Tasks;
using Plugin.FirebasePushNotification;

namespace LoveCCA.Services
{
    public class PushNotificationService
    {

        private static PushNotificationService _instance { get; set; } 
        public static PushNotificationService Instance { 
            get {
                if (_instance == null)
                    _instance = new PushNotificationService();
                return _instance;
            } 
        }

        public PushNotificationService()
        {
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Received");

            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }
            };

            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Action");

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }

                }

            };

            CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            {

                System.Diagnostics.Debug.WriteLine("Deleted");

            };

        }

        public void Subscribe(string topic)
        {
            CrossFirebasePushNotification.Current.Subscribe(topic);
        }

        public void Subscribe(string[] topics)
        {
            CrossFirebasePushNotification.Current.Subscribe(topics);
        }

        public void Unsubscribe(string topic)
        {
            CrossFirebasePushNotification.Current.Unsubscribe(topic);
        }
        public void Unsubscribe(string[] topics)
        {
            CrossFirebasePushNotification.Current.Unsubscribe(topics);
        }
        public void UnsubscribeAll()
        {
            CrossFirebasePushNotification.Current.UnsubscribeAll();
        }
    }
}
