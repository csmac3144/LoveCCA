using LoveCCA.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LoveCCA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchoolCalendarPage : ContentPage
    {
        private SchoolCalendarViewModel _viewModel;
        public SchoolCalendarPage()
        {
            InitializeComponent();
            _viewModel = (SchoolCalendarViewModel)BindingContext;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}