﻿using LoveCCA.ViewModels;
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
    public partial class EditKidPage : ContentPage
    {
        EditKidViewModel _viewModel;
        public EditKidPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new EditKidViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing();
        }
    }
}