using LoveCCA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoveCCA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AbsenceReportPage : ContentPage
    {
        AbsenceReportViewModel _viewModel;
        public AbsenceReportPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as AbsenceReportViewModel;
        }

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {
            _viewModel.DeleteCommand.Execute(((SwipeItem)sender).BindingContext);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.RefreshReports();
        }
    }
}