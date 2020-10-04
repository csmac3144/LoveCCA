using LoveCCA.Models;
using LoveCCA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    class SchoolCalendarViewModel : BaseViewModel
    {
        public ObservableCollection<SpecialDay> Items { get; private set; }
        public Command LoadItemsCommand { get; }
        public Command DoneCommand { get; }

        public SchoolCalendarViewModel()
        {
            Items = new ObservableCollection<SpecialDay>();
            Title = "School Calendar";

            DoneCommand = new Command(async () => await Done());

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

        }
        public async Task Done()
        {
            await Shell.Current.GoToAsync($"..");
        }

        public void OnDisappearing()
        {
            Items?.Clear();
            Items = null;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var config = await SchoolConfigurationService.Instance.GetSchoolConfiguration();
                foreach (var item in config.SpecialDays)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

    }
}
