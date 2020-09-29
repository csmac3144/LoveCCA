﻿using LoveCCA.Models;
using LoveCCA.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoveCCA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MealOrderPage : ContentPage
    {
        private MealOrderViewModel _viewModel;
        public MealOrderPage()
        {
            InitializeComponent();
            _viewModel = (MealOrderViewModel)BindingContext;
            var picker = (Picker)FindByName("ChildPicker");
            picker.SelectedItem = _viewModel.Kids.FirstOrDefault();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (App.IsCheckout)
                return;
            _viewModel.OnAppearing();
        }


        private async void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var day = ((Xamarin.Forms.Switch)sender).BindingContext as Day;
            if (day != null)
            {
                if (day.OrderStatus == OrderStatus.Pending && e.Value)
                {
                    Debug.WriteLine("No change");
                    return;
                }
                if (day.OrderStatus == OrderStatus.None && !e.Value)
                {
                    Debug.WriteLine("No change");
                    return;
                }
                Debug.WriteLine($"{day.DateLabel} {day.OrderId} {e.Value}");
                await _viewModel.UpdateOrder(day, e.Value);
            }

        }
    }
}