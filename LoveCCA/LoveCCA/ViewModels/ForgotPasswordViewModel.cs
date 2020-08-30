using LoveCCA.Services;
using LoveCCA.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        IAuth auth;
        public Command SendLinkCommand { get; }
        public Command GoBackCommand { get; }
        public string Email { get; set; }
        public string Message { get; set; }

        public ForgotPasswordViewModel()
        {
            SendLinkCommand = new Command(OnSendLinkClicked);
            GoBackCommand = new Command(OnGoBackClicked);
            auth = DependencyService.Get<IAuth>();
        }

        private async void OnGoBackClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async void OnSendLinkClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            try
            {
                await auth.SendResetPasswordLink(Email);
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            catch (Exception)
            {
                Message = "Cannot send reset link - please check email address";
                OnPropertyChanged("Message");
            }
        }
    }
}
