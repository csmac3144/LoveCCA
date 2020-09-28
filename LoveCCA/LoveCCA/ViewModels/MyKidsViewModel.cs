using LoveCCA.Models;
using LoveCCA.Services;
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
            Student kid = obj as Student;
            if (kid == null)
                return;
            var item = Kids.FirstOrDefault(i => i.FirstName.ToLower() == kid.FirstName.ToLower() &&
                i.LastName.ToLower() == kid.LastName.ToLower() &&
                i.Grade == kid.Grade);
            if (item != null)
            {
                await UserProfileService.Instance.RemoveKid(kid);
                Kids.Remove(item);
                OnPropertyChanged("Kids");
            }
        }

        public void RefreshKids()
        {
            if (Kids == null)
                Kids = new ObservableCollection<Student>();
            Kids.Clear();
            foreach (var kid in UserProfileService.Instance.CurrentUserProfile.Kids)
            {
                Kids.Add(kid);
            }
        }

        internal async Task AddKid(Student kid)
        {
            if (string.IsNullOrEmpty(kid.FirstName) &&
                string.IsNullOrEmpty(kid.LastName)) {
                return;

            }
            var item = Kids.FirstOrDefault(i => i.FirstName.ToLower() == kid.FirstName.ToLower() &&
                i.LastName.ToLower() == kid.LastName.ToLower() &&
                i.Grade == kid.Grade);
            if (item != null)
            {
                return;
            }
            await UserProfileService.Instance.AddKid(kid);
            Kids.Add(kid);
            OnPropertyChanged("Kids");

        }

        public ObservableCollection<Student> Kids { get; set; }
    }
}