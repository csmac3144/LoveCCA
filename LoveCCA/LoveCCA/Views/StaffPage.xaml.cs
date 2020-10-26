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
    public partial class StaffPage : ContentPage
    {
        StaffViewModel _viewModel;
        public StaffPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as StaffViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await _viewModel.OnDisappearing();
        }
    }
}