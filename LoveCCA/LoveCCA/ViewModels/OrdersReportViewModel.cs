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
    public class OrdersReportViewModel : BaseViewModel
    {
        public Command DoneCommand { get; }
        public Command RunCommand { get; }
        private IOrderHistoryService _orderHistoryService;
        public OrdersReportViewModel()
        {
            Title = "Orders Report";
            DoneCommand = new Command(OnDoneCommand);
            RunCommand = new Command(OnRunCommand);
            _orderHistoryService = DependencyService.Resolve<IOrderHistoryService>();
        }

        private async void OnRunCommand(object obj)
        {
            var orders = await _orderHistoryService.OrderQuery(SelectedDate, Products.ToArray()[SelectedProductIndex], Grades.ToArray()[SelectedGradeIndex]);
        }

        private async void OnDoneCommand(object obj)
        {
            await Shell.Current.GoToAsync($"..");
        }

        private int _selectedGradeIndex;
        public int SelectedGradeIndex
        {
            get
            {
                return _selectedGradeIndex;
            }
            set
            {
                _selectedGradeIndex = value;
                OnPropertyChanged(nameof(SelectedGradeIndex));
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await StorageVault.SetValue("selectedGrade", value.ToString());
                });
            }
        }
        private int _selectedProductIndex;
        public int SelectedProductIndex
        {
            get
            {
                return _selectedProductIndex;
            }
            set
            {
                _selectedProductIndex = value;
                OnPropertyChanged(nameof(SelectedProductIndex));
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await StorageVault.SetValue("selectedProduct", value.ToString());
                });
            }
        }
        public List<string> Products { get; private set; } // TODO
        public List<Grade> Grades { get; private set; }
        public List<Order> Orders { get; }
        public DateTime SelectedDate { get; set; }
        public async Task OnAppearing()
        {
            var config = await SchoolConfigurationService.Instance.GetSchoolConfiguration();
            Grades = config.Grades;
            OnPropertyChanged(nameof(Grades));

            var result = await StorageVault.GetValue("selectedGrade");
            if (result != null)
            {
                _selectedGradeIndex = int.Parse(result);
            }
            else
            {
                _selectedGradeIndex = 0;
            }

            OnPropertyChanged(nameof(SelectedGradeIndex));

            Products = await ProductService.GetProductClasses();
            OnPropertyChanged(nameof(Products));

            result = await StorageVault.GetValue("selectedProduct");
            if (result != null)
            {
                _selectedProductIndex = int.Parse(result);
            }
            else
            {
                _selectedProductIndex = 0;
            }

            OnPropertyChanged(nameof(SelectedProductIndex));

            SelectedDate = DateTime.Now.Date;

            OnPropertyChanged(nameof(SelectedDate));

        }

    }
}
