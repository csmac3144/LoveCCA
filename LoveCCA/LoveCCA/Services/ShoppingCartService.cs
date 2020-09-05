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
            string email = UserProfileService.Instance.CurrentUserProfile.Email;

            await LoadProducts();
            await _orderHistoryService.LoadOrders(forceRefresh: true);
            LoadCart();
        }

        private void LoadCart()
        {
            CartItems.Clear();
            GrandTotal = 0;
            var kids = UserProfileService.Instance.CurrentUserProfile.Kids;
            if (kids == null || kids.Count == 0)
            {
                kids = new List<string>() { "My Child" };
            }
            foreach (var product in Products)
            {
                var productOrders = _orderHistoryService.Orders.Where(o => o.ProductType == product.Name && o.Status == (int)OrderStatus.Pending);
                if (productOrders.Any())
                {
                    foreach (var kid in kids)
                    {
                        var ordersByKid = productOrders.Where(o => o.Child == kid);
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
                                Glyph = product.Glyph,
                                Price = product.Price.ToString("C"),
                                ProductType = product.Name,
                                Child = kid,
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

        private async Task LoadProducts()
        {
            try
            {
                    _products.Clear();
                    var query = await CrossCloudFirestore.Current
                                .Instance
                                .GetCollection("products")
                                .GetDocumentsAsync();

                    _products = query.ToObjects<Product>().ToList();
            }
            catch (System.Exception)
            {

                System.Diagnostics.Debug.WriteLine("Error loading products");
            }
        }
    }
}
