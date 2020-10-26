using LoveCCA.Services;
using LoveCCA.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoveCCA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServicesPage : ContentPage
    {
        ServicesViewModel _viewModel;
        public ServicesPage()
        {
            InitializeComponent();
            _viewModel = this.BindingContext as ServicesViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (string.IsNullOrEmpty(UserProfileService.Instance.CurrentUserProfile.Name))
            {
                bool answer = await DisplayAlert("Complete Setup?", "Would you like to enter your name and phone number now?", "Yes", "No");
                Debug.WriteLine("Answer: " + answer);
                if (answer)
                {
                    await Shell.Current.GoToAsync($"{nameof(SettingsPage)}");
                }
            } 
            else
            {
                if (!UserProfileService.Instance.CurrentUserProfile.Kids.Any())
                {
                    bool answer = await DisplayAlert("No CCA Student", "You must enter at least one CCA student. Would you like to do that now?", "Yes", "No");
                    Debug.WriteLine("Answer: " + answer);
                    if (answer)
                    {
                        await Shell.Current.GoToAsync($"{nameof(MyKidsPage)}");
                    }
                }

            }
        }
    }

    
}