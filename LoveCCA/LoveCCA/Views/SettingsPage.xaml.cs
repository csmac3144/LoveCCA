using LoveCCA.Services;
using LoveCCA.Services.MealService;
using LoveCCA.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoveCCA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel _viewModel;
        public SettingsPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as SettingsViewModel;

#if DEBUG
            var b = new Button { Text = "Run Data Tasks" };
            Grid grid = this.FindByName("SettingsGrid") as Grid;
            grid.Children.Add(b, 0, 4);
            b.Clicked += async (object sender, System.EventArgs e) => {
                var service = new SchoolConfigurationService();
                await service.GenerateConfig();
            };
#endif

        }



        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            await _viewModel.SaveChanges();
        }

    }
}