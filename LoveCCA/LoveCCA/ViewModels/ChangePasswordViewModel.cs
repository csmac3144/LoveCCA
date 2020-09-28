using LoveCCA.Services;
using LoveCCA.Views;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class ChangePasswordViewModel : BaseViewModel
    {
        

        public Command UpdatePasswordCommand { get; }
        public Command GoBackCommand { get; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        public ChangePasswordViewModel()
        {
            Title = "Change Password";
            UpdatePasswordCommand = new Command(OnUpdatePasswordClicked);
            GoBackCommand = new Command(OnGoBackClicked);
            
        }

        private async void OnGoBackClicked(object obj)
        {
            await Shell.Current.Navigation.PopModalAsync();
            //await Shell.Current.GoToAsync("..");
            //await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
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
                return;
            }

            if (currentCredentials.Item2 != OldPassword)
            {
                Message = "Old password incorrect";
                return;
            }

            if (NewPassword != ConfirmNewPassword)
            {
                Message = "New password must match confirmed new password";
                return;
            }


            try
            {
                if (await LoginService.Instance.UpdatePassword(NewPassword))
                {
                    await StorageVault.SetCredentials(currentCredentials.Item1, NewPassword);
                    await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                } 
                else
                {
                    Message = "Error updating password";
                }
            }
            catch (WeakPasswordException)
            {
                Message = "Password too weak";
            }
        }
    }
}
