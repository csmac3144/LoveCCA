
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Firebase;
using System.Threading.Tasks;
using Android.Gms.Tasks;
using Firebase.Firestore;
using System;
using Plugin.CloudFirestore;
using Xamarin.Forms;
using LoveCCA.Services;

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

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }


}