using System;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.FirebasePushNotification;
using Xamarin.Forms;

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
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        string title = "Message From CCA";
                        string message = "No content was found";
                        switch (p.Data.Values.Count)
                        {
                            case 0:
                                break;
                            case 1:
                                message = p.Data.Values.ToArray()[0] as string;
                                break;
                            case 2:
                                title = p.Data.Values.ToArray()[1] as string;
                                message = p.Data.Values.ToArray()[0] as string;
                                break;
                        }
                        await UserDialogs.Instance.AlertAsync(message, title);
                    }
                    catch (Exception)
                    {
                    }
                });
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
