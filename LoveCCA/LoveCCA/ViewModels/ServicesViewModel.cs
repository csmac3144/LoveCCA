using Acr.UserDialogs;
using LoveCCA.Models;
using LoveCCA.Services;
using LoveCCA.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class ServicesViewModel : BaseViewModel
    {
        public ServicesViewModel()
        {
            Title = "Services";
            var services = new AvailableServices();
            Items = services.Services;
            ItemTapped = new Command<ServicesModel>(OnItemSelected);

        }

        public Command<ServicesModel> ItemTapped { get; }


        public List<ServicesModel> Items { get; private set; }

        async void OnItemSelected(ServicesModel item)
        {
            if (item == null)
                return;

            switch (item.Id)
            {
                case 0:
                    if (await SettingsOK())
                    {
                        await Shell.Current.GoToAsync($"{nameof(MealOrderPage)}");
                    }
                    break;
                case 1:
                    if (await SettingsOK())
                    {
                        await Shell.Current.GoToAsync($"{nameof(MilkOrderPage)}");
                    }
                    break;
                case 2:
                    await Shell.Current.GoToAsync($"{nameof(SchoolCalendarPage)}");
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 7:
                    await Shell.Current.GoToAsync($"{nameof(StaffReportsPage)}");
                    break;
                default:
                    break;
            }

        }

        private async Task<bool> SettingsOK()
        {
            if (UserProfileService.Instance.CurrentUserProfile.Kids.Count == 0)
            {
                await UserDialogs.Instance.AlertAsync("Please go to 'My Kids' in the menu to add a student to the app.", "No CCA Student Found", "OK");
                return false;
            }
            return true;
        }
    }
}