
using Android.App;
using Android.Content.PM;
using Android.OS;
using Firebase;
using LoveCCA.Services;
using Plugin.FirebasePushNotification;

namespace LoveCCA.Droid
{
    [Activity(Label = "Love CCA", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            FirebaseApp.InitializeApp(Android.App.Application.Context);


            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);


            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental", "SwipeView_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.DependencyService.RegisterSingleton<IPushNotificationService>(new PushNotificationService());
            global::Xamarin.Forms.DependencyService.RegisterSingleton<IHolidayService>(new HolidayService());
            global::Xamarin.Forms.DependencyService.RegisterSingleton<IOrderHistoryService>(new OrderHistoryService());
            global::Xamarin.Forms.DependencyService.RegisterSingleton<IShoppingCartService>(new ShoppingCartService());
            global::Xamarin.Forms.DependencyService.RegisterSingleton<IOrderCalendarService>(new OrderCalendarService());
            LoadApplication(new App());

            FirebasePushNotificationManager.ProcessIntent(this, Intent);

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            };

        }
    }


}