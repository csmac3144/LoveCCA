using LoveCCA.Models;
using LoveCCA.Services;
using LoveCCA.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoveCCA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MilkOrderPage : ContentPage
    {
        private MilkOrderViewModel _viewModel;
        public MilkOrderPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new MilkOrderViewModel();
            var picker = (Picker)FindByName("ChildPicker");
            picker.SelectedItem = picker.Items.FirstOrDefault();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private async void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var day = ((Xamarin.Forms.Switch)sender).BindingContext as Day;
            if (day != null)
            {
                Debug.WriteLine($"{day.DateLabel} {day.OrderId} {e.Value}");
                await _viewModel.UpdateOrder(day);
            }

        }
    }
}