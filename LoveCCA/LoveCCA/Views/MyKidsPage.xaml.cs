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
            ((LoveCCA.ViewModels.MyKidsViewModel)this.BindingContext).DeleteCommand.Execute(((SwipeItem)sender).BindingContext as string);
        }

        private async void ToolbarItemAdd_Clicked(object sender, EventArgs e)
        {
            var name = await DisplayPromptAsync("Add Child", "Enter the name of your child", "Save", "Cancel", "Child's name", 20);
            if (!string.IsNullOrEmpty(name))
            {
                await ((LoveCCA.ViewModels.MyKidsViewModel)this.BindingContext).AddKid(name);
            }
        }
    }
}