using LoveCCA.ViewModels;
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
    public partial class AbsencePage : ContentPage
    {
        private AbsenceViewModel _viewModel;

        public AbsencePage()
        {
            InitializeComponent();
            _viewModel = (AbsenceViewModel)BindingContext;
            var picker = (Picker)FindByName("ChildPicker");
            picker.SelectedItem = _viewModel.Kids.FirstOrDefault();
        }
    }
}