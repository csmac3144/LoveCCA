using LoveCCA.Services;
using LoveCCA.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        IAuth auth;
        public Command UpdatePasswordCommand { get; }
        public Command GoBackCommand { get; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public string Message { get; set; }

        public ChangePasswordViewModel()
        {
            UpdatePasswordCommand = new Command(OnUpdatePasswordClicked);
            GoBackCommand = new Command(OnGoBackClicked);
            auth = DependencyService.Get<IAuth>();
        }

        private async void OnGoBackClicked(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private async void OnUpdatePasswordClicked(object obj)
        {
            var currentCredentials = await StorageVault.GetCredentials();

            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            if (string.IsNullOrEmpty(OldPassword) ||
                string.IsNullOrEmpty(NewPassword) ||
                string.IsNullOrEmpty(ConfirmNewPassword))
            {
                Message = "Please fill in all password fields";
                OnPropertyChanged("Message");
                return;
            }

            if (currentCredentials.Item2 != OldPassword)
            {
                Message = "Old password incorrect";
                OnPropertyChanged("Message");
                return;
            }

            if (NewPassword != ConfirmNewPassword)
            {
                Message = "New password must match confirmed new password";
                OnPropertyChanged("Message");
                return;
            }


            try
            {
                await auth.UpdatePassword(NewPassword);
                await StorageVault.SetCredentials(currentCredentials.Item1, NewPassword);
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            }
            catch (WeakPasswordException)
            {
                Message = "Password too weak";
                OnPropertyChanged("Message");
            }
            catch (Exception)
            {
                Message = "Error updating password";
                OnPropertyChanged("Message");
            }
        }
    }
}
