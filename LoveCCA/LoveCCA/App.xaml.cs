using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LoveCCA.Services;
using LoveCCA.Views;

namespace LoveCCA
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();

            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    var credentials = await StorageVault.GetCredentials();
                    var auth = DependencyService.Get<IAuth>();
                    var token = await auth.LoginWithEmailPassword(credentials.Item1, credentials.Item2);
                    if (!string.IsNullOrEmpty(token))
                    {
                        await StorageVault.SetToken(token);
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    }
                }
                catch (Exception)
                {
                    MainPage = new LoginPage();
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
