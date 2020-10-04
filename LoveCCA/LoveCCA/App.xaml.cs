using LoveCCA.Services;
using LoveCCA.Views;
using System;
using Xamarin.Forms;

namespace LoveCCA
{
    public partial class App : Application
    {
        public static string FCMToken { get; set; }
        public static bool IsCheckout { get; internal set; }

        public App()
        {
            InitializeComponent();
            Xamarin.Essentials.Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        private async void Connectivity_ConnectivityChanged(object sender, Xamarin.Essentials.ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet || 
                e.NetworkAccess == Xamarin.Essentials.NetworkAccess.ConstrainedInternet)
            {
                return;
            }
            await MainPage.Navigation.PushModalAsync(new ConnectivityPage());
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
