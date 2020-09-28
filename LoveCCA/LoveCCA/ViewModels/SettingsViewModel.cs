using LoveCCA.Services;
using LoveCCA.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public Command DoneCommand { get; }

        public SettingsViewModel()
        {
            Title = "Settings";
            AllowNotificaitons = UserProfileService.Instance.CurrentUserProfile.AllowNotifications;
            Name = UserProfileService.Instance.CurrentUserProfile.Name;
            CellPhone = UserProfileService.Instance.CurrentUserProfile.CellPhone;
            DoneCommand = new Command(async () => await Done());
        }

        public async Task Done()
        {
            await Shell.Current.GoToAsync($"..");
        }

        public string Name { get; set; }
        public string CellPhone { get; set; }

        internal async Task SaveChanges()
        {
            UserProfileService.Instance.CurrentUserProfile.AllowNotifications = AllowNotificaitons;
            UserProfileService.Instance.CurrentUserProfile.Name = Name;
            UserProfileService.Instance.CurrentUserProfile.CellPhone = CellPhone;
            await UserProfileService.Instance.UpdateCurrentProfile();
        }

        public bool AllowNotificaitons { get; set; }

    }
}