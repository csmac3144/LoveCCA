using LoveCCA.Services;
using LoveCCA.Views;
using System;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class AccountVerificationViewModel : BaseViewModel
    {
        IAuth auth;
        public Command VerifyCommand { get; }
        public Command ResendCommand { get; }
        public Command GoBackCommand { get; }
        public string Message { get; set; }
        public string ConfirmationMessage { get; set; }

        public AccountVerificationViewModel()
        {
            VerifyCommand = new Command(OnVerifyClicked);
            ResendCommand = new Command(OnResendClicked);
            GoBackCommand = new Command(OnGoBackToLoginClicked);
            auth = DependencyService.Get<IAuth>();
        }

        private async void OnGoBackToLoginClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private void ClearMessages()
        {
            Message = string.Empty;
            OnPropertyChanged("Message");
            ConfirmationMessage = string.Empty;
            OnPropertyChanged("ConfirmationMessage");

        }

        private async void OnResendClicked(object obj)
        {
            try
            {
                await auth.SendAccountVerificationLink();
                ClearMessages();
                ConfirmationMessage = "Verification email sent";
                OnPropertyChanged("ConfirmationMessage");
            }
            catch (Exception)
            {
                ClearMessages();
                Message = "Error sending verification email";
                OnPropertyChanged("Message");
            }
        }

        private async void OnVerifyClicked(object obj)
        {
            try
            {
                bool verified = await auth.IsCurrentUserVerified(refresh: true);
                if (verified)
                {
                    await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                }
                else
                {
                    ClearMessages();
                    Message = "Could not verify your account";
                    OnPropertyChanged("Message");
                }
            }
            catch (Exception)
            {
                ClearMessages();
                Message = "Could not verify your account";
                OnPropertyChanged("Message");
            }
        }
    }
}
