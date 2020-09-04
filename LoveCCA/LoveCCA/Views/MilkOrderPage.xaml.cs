using LoveCCA.Services;
using LoveCCA.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            //Send to vm
        }
    }
}