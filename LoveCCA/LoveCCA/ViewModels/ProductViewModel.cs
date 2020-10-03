using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using LoveCCA.Models;
using LoveCCA.Services;
using System.Threading.Tasks;

namespace LoveCCA.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        protected IOrderHistoryService _orderHistoryService;

        public ProductViewModel()
        {
            _orderHistoryService = new OrderHistoryService();

        }

        private Decimal _subTotal = 0M;
        public Decimal Subtotal
        {
            get
            {
                return _subTotal;
            }
            set
            {
                _subTotal = value;
                OnPropertyChanged(nameof(SubtotalLabel));
            }
        }
        public string SubtotalLabel => Subtotal.ToString("C");

        public async Task UpdateOrder(Day day)
        {
            string id = await _orderHistoryService.SaveMealOrder(day);
            day.OrderId = id;
        }
    }
}
