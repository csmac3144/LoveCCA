using LoveCCA.Models;
using LoveCCA.Services;
using LoveCCA.Services.PayPalService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.ViewModels
{
    public class ShoppingCartViewModel : BaseViewModel
    {
        private string _transactionID;
        private Item _selectedItem;
        private IOrderHistoryService _orderHistoryService;
        private IShoppingCartService _shoppingCartService;
        private IPayPalService _payPalService;

        public ObservableCollection<CartItem> Items { get; }

        public Command LoadItemsCommand { get; }
        public Command PayPalCommand { get; }

        public ShoppingCartViewModel()
        {
            Title = "Shopping Cart";
            CartTotal = "$0.00";
            _orderHistoryService = DependencyService.Get<IOrderHistoryService>();
            _shoppingCartService = DependencyService.Get<IShoppingCartService>();
            _payPalService = DependencyService.Get<IPayPalService>();
            Items = new ObservableCollection<CartItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            PayPalCommand = new Command(() => CheckoutWithPayPal());

            if (_payPalService != null)
                _payPalService.OnPayPalResult += OnPayPalResult;

        }

        private async void OnPayPalResult(object sender, PayPalResult e)
        {
            using (var client = new HttpClient())
            {
                var nvc = new List<KeyValuePair<string, string>>();
                nvc.Add(new KeyValuePair<string, string>("payment_method_nonce", e.Nonce));
                nvc.Add(new KeyValuePair<string, string>("amount", e.Amount));
                var req = new HttpRequestMessage(HttpMethod.Post, "https://us-central1-love-cca.cloudfunctions.net/ccapay/checkout") { Content = new FormUrlEncodedContent(nvc) };
                var res = await client.SendAsync(req);
                if (res.IsSuccessStatusCode)
                {
                    _transactionID = await res.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new Exception("Payment failed");
                }
            }
        }

        public void CheckoutWithPayPal()
        {
            string amount = _shoppingCartService.GrandTotal.ToString();
            _payPalService.StartCheckout(amount, "CCA Purchases");
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
                CartTotal = _shoppingCartService.GrandTotal.ToString("C");
                OnPropertyChanged("CartTotal");
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