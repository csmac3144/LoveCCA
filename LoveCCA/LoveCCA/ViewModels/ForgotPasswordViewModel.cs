using LoveCCA.Services;
using LoveCCA.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        

        public Command SendLinkCommand { get; }
        public Command GoBackCommand { get; }
        public string Email { get; set; }

        public ForgotPasswordViewModel()
        {
            SendLinkCommand = new Command(OnSendLinkClicked);
            GoBackCommand = new Command(OnGoBackClicked);
            
        }

        private async void OnGoBackClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async void OnSendLinkClicked(object obj)
        {
            if (await LoginService.Instance.SendResetPasswordLink(Email)) 
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                Message = "Cannot send reset link - please check email address";
            }
        }
    }
}
