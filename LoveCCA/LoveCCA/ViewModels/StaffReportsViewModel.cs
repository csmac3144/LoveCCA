using LoveCCA.Models;
using LoveCCA.Services;
using LoveCCA.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class StaffReportsViewModel : BaseViewModel
    {
        public Command<ReportsModel> ItemTappedCommand { get; }

        public StaffReportsViewModel()
        {
            Title = "CCA Staff Reports";
            Reports = (new AvailableServices().Reports);
            ItemTappedCommand = new Command<ReportsModel>(ServiceSelected);
        }

        private async void ServiceSelected(ReportsModel report)
        {
            switch (report.Id)
            {
                case 0: // Weekly Orders
                    await Shell.Current.GoToAsync($"{nameof(OrdersReportPage)}");

                    break;
            }
        }

        public List<ReportsModel> Reports { get; }

    }
}
