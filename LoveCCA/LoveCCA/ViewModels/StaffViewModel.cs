using LoveCCA.Models;
using LoveCCA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class StaffViewModel : BaseViewModel
    {
        public Command CancelCommand { get; }
        public Command SubmitCommand { get; }
        public StaffViewModel()
        {
            Title = "CCA Staff";

            CancelCommand = new Command(OnCancelCommand);
            //SubmitCommand = new Command(OnSubmitCommand);
        }

        public List<UserProfile> Users { get; set; }

        private async void OnCancelCommand(object obj)
        {
            await Shell.Current.GoToAsync($"..");
        }

        public async Task OnAppearing()
        {
            var users = await UserProfileService.Instance.GetUserProfiles();
            foreach (var user in users)
            {
                user.IsDirty = false;
            }
            Users = users;
            OnPropertyChanged(nameof(Users));
        }

        public async Task OnDisappearing()
        {
            var changed = Users.Where(u => u.IsDirty).ToList();
            foreach (var user in changed)
            {
                await UserProfileService.Instance.UpdateStaffStatus(user);
            }
        }

    }
}
