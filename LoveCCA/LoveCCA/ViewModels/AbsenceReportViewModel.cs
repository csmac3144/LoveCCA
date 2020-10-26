using LoveCCA.Models;
using LoveCCA.Services;
using LoveCCA.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class AbsenceReportViewModel : BaseViewModel
    {
        AbsenceService _service;
        public Command<AbsenceReport> DeleteCommand { get; }
        public AbsenceReportViewModel()
        {
            Title = "Absence Reports";
            DeleteCommand = new Command<AbsenceReport>(OnDelete);
            _service = new AbsenceService();
        }

        private async void OnDelete(AbsenceReport report)
        {
            await _service.DeleteReport(report);
            Reports.Remove(report);
            OnPropertyChanged(nameof(Reports));
        }

        public async Task RefreshReports()
        {
            if (Reports == null)
                Reports = new ObservableCollection<AbsenceReport>();
            Reports.Clear();
            foreach (var report in await _service.GetReports())
            {
                Reports.Add(report);
            }
            OnPropertyChanged(nameof(Reports));
        }

        public ObservableCollection<AbsenceReport> Reports { get; set; }
    }
}