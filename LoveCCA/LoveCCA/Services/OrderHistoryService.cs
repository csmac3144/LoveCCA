using LoveCCA.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LoveCCA.Services
{
    public interface IOrderHistoryService
    {
        List<Order> Orders { get; }
        Task LoadOrders(bool forceRefresh = false);
        Task<string> SaveOrder(Day day);
        Task CompletePendingOrders();
    }

    public class OrderHistoryService : IOrderHistoryService
    {
        bool _loaded = false;
        public OrderHistoryService()
        {
            Orders = new List<Order>();
        }
        public List<Order> Orders { get; private set; }

        public async Task<string> SaveOrder(Day day)
        {
            switch (day.OrderStatus)
            {
                case OrderStatus.None:
                    return await HandleNoneDayOrder(day);
                case OrderStatus.Pending:
                    return await HandlePendingDayOrder(day);
                case OrderStatus.Completed:
                    throw new NotImplementedException();
            }
            return null;
        }

        private async Task<string> HandlePendingDayOrder(Day day)
        {
            if (!string.IsNullOrEmpty(day.OrderId))
            {
                //Existing order
                var oldrecord = Orders.FirstOrDefault(o => o.Id == day.OrderId);
                if (oldrecord != null)
                {
                    if (oldrecord.Status == (int)day.OrderStatus)
                        return day.OrderId;
                    oldrecord.Status = (int)OrderStatus.Pending;
                    await UpdateOrderStatus(oldrecord.Id, (int)OrderStatus.Pending);
                }
                return day.OrderId;
            }
            else
            {
                var order = new Order
                {
                    Email = UserProfileService.Instance.CurrentUserProfile.Email,
                    Child = day.OrderChild,
                    ProductType = day.OrderProductType,
                    DeliveryDate = day.Date.Date,
                    OrderDate = DateTime.Now.Date,
                    Status = (int)day.OrderStatus,
                    Quantity = 1
                };
                order = await AppendOrder(order);
                Orders.Add(order);
                return order.Id;
            }
        }

        private async Task<string> UpdateOrderStatus(string id, int status)
        {
            try
            {
                await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("orders")
                         .GetDocument(id)
                         .UpdateDataAsync(new { Status = status });
            }
            catch (Exception)
            {
                Debug.WriteLine("Error updating order status");
            }
            return id;
        }

        private async Task<string> HandleNoneDayOrder(Day day)
        {
            if (!string.IsNullOrEmpty(day.OrderId)) 
            {
                var oldrecord = Orders.FirstOrDefault(o => o.Id == day.OrderId);
                if (oldrecord != null)
                {
                    await RemoveOrder(oldrecord.Id);
                    Orders.Remove(oldrecord);
                    return null;
                }
            }
            return day.OrderId;
        }

        private async Task RemoveOrder(string id)
        {
            try
            {
                await CrossCloudFirestore.Current
                                         .Instance
                                         .GetCollection("orders")
                                         .GetDocument(id)
                                         .DeleteDocumentAsync();
            }
            catch (Exception)
            {
                Debug.WriteLine("Error deleting order");
            }
        }

        private async Task<Order> AppendOrder(Order order)
        {
            try
            {
                order.Id = Guid.NewGuid().ToString();
                await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("orders")
                         .GetDocument(order.Id)
                         .SetDataAsync(order);
                return order;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task LoadOrders(bool forceRefresh = false)
        {
            //TODO: Only this year's orders

            try
            {
                if (forceRefresh || !_loaded)
                {
                    Orders.Clear();
                    string email = UserProfileService.Instance.CurrentUserProfile.Email;
                    var query = await CrossCloudFirestore.Current
                             .Instance
                             .GetCollection("orders")
                             .WhereEqualsTo("Email", email.ToLower())
                             .OrderBy("OrderDate", false)
                             .GetDocumentsAsync();

                    Orders = query.ToObjects<Order>().ToList();
                    _loaded = true;
                }
            }
            catch (System.Exception)
            {

                System.Diagnostics.Debug.WriteLine("Error loading orders");
            }


        }

        public async Task CompletePendingOrders()
        {
            foreach (var order in Orders.Where(o => o.Status == (int)OrderStatus.Pending))
            {
                order.Status = (int)OrderStatus.Completed;
                await UpdateOrderStatus(order.Id, (int)OrderStatus.Completed);
            }
        }
    }
}
