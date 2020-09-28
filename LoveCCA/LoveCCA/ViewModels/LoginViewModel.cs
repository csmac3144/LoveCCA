using LoveCCA.Services;
using LoveCCA.Views;
using System;
using System.Reflection;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        
        public Command LoginCommand { get; }
        public Command SignUpCommand { get; }
        public Command ForgotPasswordCommand { get; }
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginViewModel() 
        {
            Title = "Love CCA Login";
            LoginCommand = new Command(OnLoginClicked);
            SignUpCommand = new Command(OnSignUpClicked);
            ForgotPasswordCommand = new Command(OnForgotPasswordClicked);
            
            
        }

        private async void OnForgotPasswordClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(ForgotPasswordPage)}");
        }
        private async void OnSignUpClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(SignUpPage)}");
        }

        private void ClearFields()
        {
            Email = string.Empty;
            Password = string.Empty;
            Message = string.Empty;
            OnPropertyChanged("Email");
            OnPropertyChanged("Password");
        }

        private async void OnLoginClicked(object obj)
        {
            try
            {
                if (await LoginService.Instance.LoginWithEmailPassword(Email, Password))
                {
                    ClearFields();
                    if (await LoginService.Instance.IsCurrentUserVerified(refresh: false))
                    {
                        await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(AccountVerificationPage)}");
                    }
                }
                else
                {
                    Message = "There was a system problem. Please try later.";
                    OnPropertyChanged("Message");
                }
            }
            catch (InvalidLoginException)
            {
                Message = "Invalid login attempt";
                OnPropertyChanged("Message");
            }
            catch (Exception)
            {
                Message = "Invalid login attempt";
                OnPropertyChanged("Message");
            }
        }
    }
}
