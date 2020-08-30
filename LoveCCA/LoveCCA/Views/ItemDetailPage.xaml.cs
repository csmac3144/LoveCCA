using System.ComponentModel;
using Xamarin.Forms;
using LoveCCA.ViewModels;

namespace LoveCCA.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}