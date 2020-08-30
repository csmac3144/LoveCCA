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

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                StorageVault.ClearCredentials();
            }
            catch (Exception)
            {
                Debug.WriteLine("Could not clear stored credentials"); 
            }
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
