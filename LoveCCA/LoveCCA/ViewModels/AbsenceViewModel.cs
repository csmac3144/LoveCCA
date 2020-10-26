using Acr.UserDialogs;
using LoveCCA.Models;
using LoveCCA.Services;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class AbsenceViewModel : BaseViewModel
    {
        public Command CancelCommand { get; }
        public Command SubmitCommand { get; }
        public AbsenceViewModel()
        {
            Title = "Report Student Absence";
            SelectedDate = DateTime.Now.Date;
            Kids = UserProfileService.Instance.CurrentUserProfile.Kids;

            CancelCommand = new Command(OnCancelCommand);
            SubmitCommand = new Command(OnSubmitCommand);
        }

        private async void OnSubmitCommand(object obj)
        {
            var report = new AbsenceReport
            {
                Id = Guid.NewGuid().ToString(),
                Date = SelectedDate.Date,
                ReportedDate = DateTime.Now,
                StudentName = SelectedStudent.Name,
                Grade = SelectedStudent.Grade.ToString(),
                Comments = Text,
                ParentName = UserProfileService.Instance.CurrentUserProfile.Name,
                ParentEmail = UserProfileService.Instance.CurrentUserProfile.Email
            };
            var service = new AbsenceService();
            try
            {
                await service.SubmitReport(report);
                await Shell.Current.GoToAsync($"..");

                UserDialogs.Instance.Toast(new ToastConfig("Absence Report Submitted to CCA")
                    .SetDuration(TimeSpan.FromSeconds(3))
                    .SetPosition(ToastPosition.Bottom));

            }
            catch (Exception)
            {
                UserDialogs.Instance.Alert("Could not submit abscence report.", "Error", "OK");
            }

        }

        private async void OnCancelCommand(object obj)
        {
            await Shell.Current.GoToAsync($"..");
        }

        public string Text { get; set; }
        public List<Student> Kids { get; set; }

        public DateTime SelectedDate { get; set; }
        public Student SelectedStudent { get; set; }

    }
}
