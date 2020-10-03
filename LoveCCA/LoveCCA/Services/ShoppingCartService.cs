using LoveCCA.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.Services
{
    public interface IShoppingCartService
    {
        Task Initialize();
        List<Product> Products { get; }
        List<CartItem> CartItems { get;  }
        decimal GrandTotal { get; }

        Task PaymentComplete(string transactionID);
    }
    public class ShoppingCartService : IShoppingCartService
    {
        private List<Product> _products;
        private IOrderHistoryService _orderHistoryService;
        public ShoppingCartService() : this(DependencyService.Get<IOrderHistoryService>())
        {
        }
        public ShoppingCartService(IOrderHistoryService orderHistoryService)
        {
            _products = new List<Product>();
            CartItems = new List<CartItem>();
            _orderHistoryService = orderHistoryService;
        }

        public async Task Initialize()
        {
            _products = await ProductService.LoadProducts();
            await _orderHistoryService.LoadOrders();
            LoadCart();
        }

        private void LoadCart()
        {
            CartItems.Clear();
            GrandTotal = 0;
            var kids = UserProfileService.Instance.CurrentUserProfile.Kids;

            foreach (var product in Products)
            {
                var productOrders = _orderHistoryService.Orders.Where(o => 
                    o.SelectedProductID == product.Id  
                    && o.Status == (int)OrderStatus.Pending);
                if (productOrders.Any())
                {
                    foreach (var kid in kids)
                    {
                        var ordersByKid = productOrders.Where(o => o.Kid.Id == kid.Id);
                        if (ordersByKid.Any())
                        {
                            decimal total = 0;
                            int count = 0;
                            foreach (var o in ordersByKid)
                            {
                                count++;
                                total += product.Price;
                            }
                            var cartItem = new CartItem
                            {
                                Id = Guid.NewGuid().ToString(),
                                ShortDescription = product.ShortDescription,
                                ProductID = product.Id,
                                Glyph = product.Glyph,
                                Price = product.Price.ToString("C"),
                                ProductType = product.Name,
                                Kid = kid,
                                Quantity = count.ToString(),
                                Total = total.ToString("C")
                            };
                            CartItems.Add(cartItem);
                            GrandTotal += total;
                        }
                    }
                }
            }
        }

        public List<CartItem> CartItems { get; private set; }

        public List<Product> Products
        {
            get
            {
                return _products;
            }
        }

        public decimal GrandTotal { get; private set; }



        public async Task PaymentComplete(string transactionID)
        {
            await _orderHistoryService.CompletePendingOrders();
            await Initialize();
        }
    }
}
