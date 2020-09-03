using LoveCCA.Services;
using LoveCCA.Views;
using Xamarin.Forms;

namespace LoveCCA
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
         

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!(await LoginService.Instance.TrySilentLogin()))
                {
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
            });
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
