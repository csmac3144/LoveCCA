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


            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            try
            {
                var auth = DependencyService.Get<IAuth>();
                auth.SignOut();
                StorageVault.ClearCredentials();
            }
            catch (Exception)
            {
                Debug.WriteLine("Could not sign out"); 
            }
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(ChangePasswordPage)}");

        }
    }
}
