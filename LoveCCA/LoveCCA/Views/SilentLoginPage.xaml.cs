using LoveCCA.Services;
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
    public partial class SilentLoginPage : ContentPage
    {
        public SilentLoginPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (LoginService.Instance.IsAuthenticated || await LoginService.Instance.TrySilentLogin())
            {
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                await Navigation.PushModalAsync(new LoginPage());
            }
        }
    }
}