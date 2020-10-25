using Acr.UserDialogs;
using LoveCCA.Models;
using LoveCCA.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class EditKidViewModel : BaseViewModel
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public EditKidViewModel()
        {
            Title = "Add CCA Student";
            SaveCommand = new Command(OnSaveClicked);
            CancelCommand = new Command(OnCancelClicked);
        }

        public string FirstName { get; set; }

        public async Task OnAppearing()
        {
            if (Grades == null)
            {
                var config = await SchoolConfigurationService.Instance.GetSchoolConfiguration();
                Grades = config.Grades;
                OnPropertyChanged(nameof(Grades));
            }
        }

        public string LastName { get; set; }
        public List<Grade> Grades { get; private set; }
        public Grade SelectedItem { get; set; }

        private async void OnCancelClicked(object obj)
        {
            await Shell.Current.GoToAsync($"..");
        }

        private async void OnSaveClicked(object obj)
        {
            if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
                return;
            if (SelectedItem == null)
                return;
            var kid = new Student
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Grade = SelectedItem
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
