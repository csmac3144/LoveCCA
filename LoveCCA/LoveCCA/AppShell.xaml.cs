using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Routing.RegisterRoute(nameof(MyKidsPage), typeof(MyKidsPage));
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            LoginService.Instance.SignOut();
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(ChangePasswordPage)}");

        }

        private async void Settings_Clicked(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = false;
            await Shell.Current.GoToAsync($"{nameof(SettingsPage)}");
        }
    }
}
