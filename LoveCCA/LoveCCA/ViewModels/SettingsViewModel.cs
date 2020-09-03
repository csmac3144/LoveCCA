using LoveCCA.Services;
using LoveCCA.Views;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public Command MyKidsCommand { get; }

        public SettingsViewModel()
        {
            Title = "Settings";
            AllowNotificaitons = UserProfileService.Instance.CurrentUserProfile.AllowNotifications;
            MyKidsCommand = new Command(OnMyKidsTapped);
        }

        public string Name { get; set; }
        public string CellPhone { get; set; }

        public bool AllowNotificaitons { get; set; }

        private async void OnMyKidsTapped(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(MyKidsPage)}");
        }
    }
}