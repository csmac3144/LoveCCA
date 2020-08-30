using LoveCCA.Services;
using LoveCCA.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        IAuth auth;
        public Command LoginCommand { get; }
        public Command SignUpCommand { get; }
        public Command ForgotPasswordCommand { get; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            SignUpCommand = new Command(OnSignUpClicked);
            ForgotPasswordCommand = new Command(OnForgotPasswordClicked);
            auth = DependencyService.Get<IAuth>();
        }

        private async void OnForgotPasswordClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(ForgotPasswordPage)}");
        }
        private async void OnSignUpClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(SignUpPage)}");
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            string token = await auth.LoginWithEmailPassword(Email, Password);
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    await StorageVault.SetCredentials(Email, Password);
                    await StorageVault.SetToken(token);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Can't save credentials");
                }
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
            else
            {
                Message = "Invalid login attempt";
                OnPropertyChanged("Message");
            }
        }
    }
}
