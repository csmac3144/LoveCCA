using LoveCCA.Models;
using LoveCCA.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class StaffReportsViewModel : BaseViewModel
    {
        //public Command DoneCommand { get; }
        public Command SelectCommand { get; }

        public StaffReportsViewModel()
        {
            Title = "CCA Staff Reports";
            Reports = (new AvailableServices().Reports);
            SelectCommand = new Command(ServiceSelected);
        }

        private void ServiceSelected(object obj)
        {
            throw new NotImplementedException();
        }

        public List<ReportsModel> Reports { get; }

    }
}
