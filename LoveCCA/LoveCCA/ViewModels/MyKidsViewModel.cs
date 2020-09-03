using LoveCCA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class MyKidsViewModel : BaseViewModel
    {
        public Command DeleteCommand;
        public MyKidsViewModel()
        {
            Title = "My Kids";
            DeleteCommand = new Command(OnDelete);
            RefreshKids();
        }



        private async void OnDelete(object obj)
        {
            string name = obj as string;
            if (string.IsNullOrEmpty(name))
                return;
            var item = Kids.FirstOrDefault(i => i == name);
            if (item != null)
            {
                await UserProfileService.Instance.RemoveKid(name);
                Kids.Remove(item);
                OnPropertyChanged("Kids");
            }
        }

        public void RefreshKids()
        {
            if (Kids == null)
                Kids = new ObservableCollection<string>();
            Kids.Clear();
            foreach (var kid in UserProfileService.Instance.CurrentUserProfile.Kids)
            {
                Kids.Add(kid);
            }
        }

        internal async Task AddKid(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;
            var item = Kids.FirstOrDefault(i => i == name);
            if (item != null)
            {
                return;
            }
            await UserProfileService.Instance.AddKid(name);
            Kids.Add(name);
            OnPropertyChanged("Kids");

        }

        public ObservableCollection<string> Kids { get; set; }
    }
}