using LoveCCA.Models;
using LoveCCA.Services;
using LoveCCA.Views;
using System.Collections.Generic;
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
                    break;
                case 1:
                    await Shell.Current.GoToAsync($"{nameof(MilkOrderPage)}");
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                default:
                    break;
            }

        }

    }
}