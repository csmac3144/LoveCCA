using LoveCCA.Services;
using LoveCCA.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class SignUpViewModel : BaseViewModel
    {
        IAuth auth;
        public Command SignUpCommand { get; }
        public Command GoBackCommand { get; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Message { get; set; }

        public SignUpViewModel()
        {
            SignUpCommand = new Command(OnSignUpClicked);
            GoBackCommand = new Command(OnGoBackClicked);
            auth = DependencyService.Get<IAuth>();
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
            OnPropertyChanged("Message");
        }
        private async void OnSignUpClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            if (Password != ConfirmPassword)
            {
                Message = "Passwords must match";
                OnPropertyChanged("Message");
                return;
            }
            string token = string.Empty;

            try
            {
                token = await auth.CreateUserWithEmailPassword(Email, Password);
            }
            catch (EmailInUseException)
            {
                Message = "An account already exists for that email address";
                OnPropertyChanged("Message");
                return;
            }
            catch (BadEmailFormatException)
            {
                Message = "Invalid email address format";
                OnPropertyChanged("Message");
                return;
            }
            catch (WeakPasswordException)
            {
                Message = "Password too weak";
                OnPropertyChanged("Message");
                return;
            }
            catch (Exception)
            {
                Message = "Could not create account";
                OnPropertyChanged("Message");
                return;
            }
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
                try
                {
                    await auth.SendAccountVerificationLink();
                }
                catch (Exception)
                {
                    Debug.WriteLine("Could not send verification email");
                }
                ClearFields();
                await Shell.Current.GoToAsync($"//{nameof(AccountVerificationPage)}");
            }
            else
            {
                Message = "Could not create account";
                OnPropertyChanged("Message");
                return;
            }
        }
    }
}
