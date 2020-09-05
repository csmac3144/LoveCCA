using LoveCCA.Models;
using LoveCCA.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class ShoppingCartViewModel : BaseViewModel
    {
        private Item _selectedItem;
        private IOrderHistoryService _orderHistoryService;
        private IShoppingCartService _shoppingCartService;

        public ObservableCollection<CartItem> Items { get; }

        public Command LoadItemsCommand { get; }

        public ShoppingCartViewModel()
        {
            Title = "Shopping Cart";
            CartTotal = "$0.00";
            _orderHistoryService = DependencyService.Get<IOrderHistoryService>();
            _shoppingCartService = DependencyService.Get<IShoppingCartService>();
            Items = new ObservableCollection<CartItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        public string CartTotal { get; set; }


        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }


        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                await _shoppingCartService.Initialize();
                foreach (var item in _shoppingCartService.CartItems)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}