using Acr.UserDialogs;
using LoveCCA.Models;
using LoveCCA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class EditKidViewModel : BaseViewModel
    {
        public Command SaveCommand { get; }
        public EditKidViewModel()
        {
            Title = "Add CCA Student";
            SaveCommand = new Command(OnSaveClicked);
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Grade { get; set; }

        private async void OnSaveClicked(object obj)
        {
            if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
                return;
            var kid = new Student
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Grade = int.Parse(this.Grade)
            };

            if (UserProfileService.Instance.CurrentUserProfile.Kids.Any(k => k.Id == kid.Id))
            {
                await UserDialogs.Instance.AlertAsync($"Student already exists");
                return;
            }

            await UserProfileService.Instance.AddKid(kid);
            await UserProfileService.Instance.UpdateCurrentProfile();
            await Shell.Current.GoToAsync($"..");
        }
    }
}
