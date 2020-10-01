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
            MenuOptions = new ObservableCollection<MenuOption>();
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

        public async Task SelectOption(MenuOption option)
        {
            SelectedMenuOption = null;
            SelectedMenuOptionID = null;

            foreach (var opt in MenuOptions)
            {
                if (opt == option)
                    continue;
                if (opt.Glyph == "⚫")
                {
                    Parent.Subtotal -= opt.Price;
                    break;
                }
            }


            if (option.Glyph == "⚪")
            {
                foreach (var o in MenuOptions)
                {
                    o.Glyph = "⚪";
                    o.Notify();
                }
                option.Glyph = "⚫";
                SelectedMenuOption = option;
                SelectedMenuOptionID = option.Id;
                Parent.Subtotal += option.Price;
                option.Notify();
                OrderStatus = OrderStatus.Pending;
            }
            else
            {
                Parent.Subtotal -= option.Price;
                option.Glyph = "⚪";
                option.Notify();
                OrderStatus = OrderStatus.None;
            }
            await Parent.UpdateOrder(this);
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
        public ObservableCollection<MenuOption> MenuOptions { get; set; }
        public MenuOption SelectedMenuOption { get; set; }
        public string SelectedMenuOptionID { get; set; }

        public MealOrderViewModel Parent { get; internal set; }
    }
}
