using LoveCCA.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LoveCCA.Models
{
    public enum OrderStatus
    {
        None, Pending, Completed
    }
    public class Day
    {
        public Day()
        {
            OrderStatus = OrderStatus.None;
            Products = new ObservableCollection<Product>();
        }

        public DateTime Date { get; set; }
        public string DateLabel {  
            get
            {
                return Date.ToString("M");
            }
        }

        public string OrderDateLabel {  
            get
            {
                return OrderDate.ToString("M");
            }
        }

        public async Task SelectOption(Product option)
        {
            SelectedProduct = null;
            SelectedProductID = null;

            foreach (var opt in Products)
            {
                if (opt == option)
                    continue;
                if (opt.SelectionGlyph == "⚫")
                {
                    ParentViewModel.Subtotal -= opt.Price;
                    break;
                }
            }


            if (option.SelectionGlyph == "⚪")
            {
                foreach (var o in Products)
                {
                    o.SelectionGlyph = "⚪";
                    o.Notify();
                }
                option.SelectionGlyph = "⚫";
                SelectedProduct = option;
                SelectedProductID = option.Id;
                ParentViewModel.Subtotal += option.Price;
                option.Notify();
                OrderStatus = OrderStatus.Pending;
            }
            else
            {
                ParentViewModel.Subtotal -= option.Price;
                option.SelectionGlyph = "⚪";
                option.Notify();
                OrderStatus = OrderStatus.None;
            }
            await ParentViewModel.UpdateOrder(this);
        }

        public string DayOfWeekLabel {  
            get
            {
                return Date.DayOfWeek.ToString();
            }
        }
        public string Description { get; set; }
        public bool IsNotSchoolDay { get; set; }
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; internal set; }
        public Student OrderKid { get; set; }
        public string OrderProductType { get; set; }
        public bool IsPending => OrderStatus == OrderStatus.Pending;
        public ObservableCollection<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }
        public string SelectedProductID { get; set; }

        public ProductViewModel ParentViewModel { get; internal set; }
    }
}
