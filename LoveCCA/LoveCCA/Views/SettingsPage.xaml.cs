using LoveCCA.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoveCCA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }


        private async void toolbarItemSave_Clicked(object sender, EventArgs e)
        {
            await ((SettingsViewModel)BindingContext).SaveChanges();
        }
    }
}