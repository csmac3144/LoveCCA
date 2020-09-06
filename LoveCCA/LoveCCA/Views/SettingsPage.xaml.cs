using LoveCCA.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoveCCA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel _viewModel;
        public SettingsPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as SettingsViewModel;
        }



        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            await _viewModel.SaveChanges();
        }

    }
}