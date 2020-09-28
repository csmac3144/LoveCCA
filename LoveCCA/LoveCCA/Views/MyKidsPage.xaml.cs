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
    public partial class MyKidsPage : ContentPage
    {
        public MyKidsPage()
        {
            InitializeComponent();
        }

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {
            ((LoveCCA.ViewModels.MyKidsViewModel)this.BindingContext).DeleteCommand.Execute(((SwipeItem)sender).BindingContext);
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((LoveCCA.ViewModels.MyKidsViewModel)this.BindingContext).RefreshKids();
        }
    }
}