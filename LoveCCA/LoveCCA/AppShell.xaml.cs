using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using LoveCCA.Services;
using LoveCCA.ViewModels;
using LoveCCA.Views;
using Xamarin.Forms;

namespace LoveCCA
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();


            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(MilkOrderPage), typeof(MilkOrderPage));
            Routing.RegisterRoute(nameof(MealOrderPage), typeof(MealOrderPage));
            Routing.RegisterRoute(nameof(MyKidsPage), typeof(MyKidsPage));
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(EditKidPage), typeof(EditKidPage));

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (LoginService.Instance.IsAuthenticated || await LoginService.Instance.TrySilentLogin())
                {
                    if (!(await LoginService.Instance.IsCurrentUserVerified(false)))
                    {
                        await Navigation.PushModalAsync(new AccountVerificationPage());
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    await Navigation.PushModalAsync(new LoginPage());
                }
            });
        }


        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            LoginService.Instance.SignOut();
            Shell.Current.FlyoutIsPresented = false;
            await Shell.Current.Navigation.PushModalAsync(new LoginPage());
        }

        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = false;
            await Shell.Current.Navigation.PushModalAsync(new ChangePasswordPage());
        }

        private async void Settings_Clicked(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = false;
            await Shell.Current.GoToAsync($"{nameof(SettingsPage)}");
        }

        private async void MyKids_Clicked(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = false;
            await Shell.Current.GoToAsync($"{nameof(MyKidsPage)}");
        }
    }
}
