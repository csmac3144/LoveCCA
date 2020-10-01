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
        Task LoadOrders();
        Task<string> SaveMilkOrder(Day day, bool value);
        Task CompletePendingOrders();
        Task<string> SaveMealOrder(Day day);
    }

    public class OrderHistoryService : IOrderHistoryService
    {
        public OrderHistoryService()
        {
            Orders = new List<Order>();
        }
        public List<Order> Orders { get; private set; }

        public async Task<string> SaveMilkOrder(Day day, bool value)
        {
            if (value) day.OrderStatus = OrderStatus.Pending;
            else day.OrderStatus = OrderStatus.None;

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
                await RemoveOrder(day.OrderId);
            }

            var order = new Order
            {
                Email = UserProfileService.Instance.CurrentUserProfile.Email,
                Kid = day.OrderKid,
                ProductType = day.OrderProductType,
                DeliveryDate = day.Date.Date,
                OrderDate = DateTime.Now.Date,
                Status = (int)day.OrderStatus,
                Quantity = 1,
                SelectedOptionID = day.SelectedMenuOption?.Id
            };
            order = await AppendOrder(order);
            return order.Id;
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
            catch (Exception ex)
            {
                Debug.WriteLine("Error updating order status " + ex.Message);
                throw;
            }
            return id;
        }


        private async Task<string> HandleNoneDayOrder(Day day)
        {
            if (!string.IsNullOrEmpty(day.OrderId)) 
            {
                await RemoveOrder(day.OrderId);
            }
            return null;
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


        public async Task LoadOrders()
        {
            //TODO: Only this year's orders

            try
            {
                Orders.Clear();
                string email = UserProfileService.Instance.CurrentUserProfile.Email;
                var query = await CrossCloudFirestore.Current
                            .Instance
                            .GetCollection("orders")
                            .WhereEqualsTo("Email", email.ToLower())
                            .WhereGreaterThan("OrderDate", HolidayService.GetStartOfCurrentSchoolYear())
                            .OrderBy("OrderDate", false)
                            .GetDocumentsAsync();

                Orders = query.ToObjects<Order>().ToList();
                Debug.WriteLine("LOADED ORDERS");
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading orders {ex.Message}");
            }


        }

        public async Task CompletePendingOrders()
        {
            var ids = Orders.Where(o => o.Status == (int)OrderStatus.Pending).Select(o => o.Id).ToList();
            Orders.Clear();
            foreach (var id in ids)
            {
                await UpdateOrderStatus(id, (int)OrderStatus.Completed);
            }
        }

        public async Task<string> SaveMealOrder(Day day)
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
    }
}
