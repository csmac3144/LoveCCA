using LoveCCA.Services;
using System.Collections.Generic;

namespace LoveCCA.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel()
        {
            Title = "Settings";
            AllowNotificaitons = UserProfileService.Instance.CurrentUserProfile.AllowNotifications;
            Kids = UserProfileService.Instance.CurrentUserProfile.Kids;
        }

        public bool AllowNotificaitons { get; set; }

        public List<string> Kids { get; set; }
    }
}