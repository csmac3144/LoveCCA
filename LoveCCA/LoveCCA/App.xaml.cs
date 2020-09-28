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

            MainPage = new AppShell();
         

            Device.BeginInvokeOnMainThread(async () =>
            {
                //await OrderCalendarService.Instance.Initialize(DateTime.Now);
                if (!(await LoginService.Instance.TrySilentLogin()))
                {
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
            });
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
