using LoveCCA.Services;
using LoveCCA.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        public Command SignUpCommand { get; }
        public Command GoBackCommand { get; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public SignUpViewModel()
        {
            Title = "Sign Up for this App";

            SignUpCommand = new Command(OnSignUpClicked);
            GoBackCommand = new Command(OnGoBackClicked);
        }
        private async void OnGoBackClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private void ClearFields()
        {
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            Message = string.Empty;
            OnPropertyChanged("Email");
            OnPropertyChanged("Password");
            OnPropertyChanged("ConfirmPassword");
        }
        private async void OnSignUpClicked(object obj)
        {
            if (Password != ConfirmPassword)
            {
                Message = "Passwords must match";
                return;
            }

            try
            {
                if (await LoginService.Instance.CreateUserWithEmailPassword(Email, Password))
                {
                    if (await LoginService.Instance.SendAccountVerificationLink())
                    {
                        ClearFields();
                        await Shell.Current.GoToAsync($"//{nameof(AccountVerificationPage)}");
                    } 
                    else
                    {
                        Message = "Could not send verification email";
                        return;
                    }
                }
            }
            catch (EmailInUseException)
            {
                Message = "An account already exists for that email address";
                return;
            }
            catch (BadEmailFormatException)
            {
                Message = "Invalid email address format";
                return;
            }
            catch (WeakPasswordException)
            {
                Message = "Password too weak";
                return;
            }
            catch (Exception)
            {
                Message = "Could not create account";
                return;
            }
        }
    }
}
