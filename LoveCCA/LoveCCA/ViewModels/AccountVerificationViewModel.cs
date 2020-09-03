using LoveCCA.Services;
using LoveCCA.Views;
using System;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class AccountVerificationViewModel : BaseViewModel
    {
        public Command VerifyCommand { get; }
        public Command ResendCommand { get; }
        public Command GoBackCommand { get; }
        public string ConfirmationMessage { get; set; }

        public AccountVerificationViewModel()
        {
            VerifyCommand = new Command(OnVerifyClicked);
            ResendCommand = new Command(OnResendClicked);
            GoBackCommand = new Command(OnGoBackToLoginClicked);
            
        }

        private async void OnGoBackToLoginClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private void ClearMessages()
        {
            Message = string.Empty;
            ConfirmationMessage = string.Empty;
            OnPropertyChanged("ConfirmationMessage");

        }

        private async void OnResendClicked(object obj)
        {
            if (await LoginService.Instance.SendAccountVerificationLink())
            { 
                ClearMessages();
                ConfirmationMessage = "Verification email sent";
                OnPropertyChanged("ConfirmationMessage");
            }
            else
            {
                ClearMessages();
                Message = "Error sending verification email";
            }
        }

        private async void OnVerifyClicked(object obj)
        {
            if (await LoginService.Instance.IsCurrentUserVerified(refresh: true))
            {
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
            else
            {
                ClearMessages();
                Message = "Could not verify your account";
            }
        }
    }
}
