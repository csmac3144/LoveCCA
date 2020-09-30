using System;
using System.Collections.ObjectModel;

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

        public void SelectOption(MenuOption option)
        {
            if (option.Glyph == "⚪")
            {
                foreach (var o in MenuOptions)
                {
                    o.Glyph = "⚪";
                    o.Notify();
                }
                option.Glyph = "⚫";
                option.Notify();
            }
            else
            {
                option.Glyph = "⚪";
                option.Notify();
            }

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
        public OrderStatus OrderStatus { get; internal set; }
        public Student OrderKid { get; set; }
        public string OrderProductType { get; set; }
        public bool IsPending => OrderStatus == OrderStatus.Pending;
        public ObservableCollection<MenuOption> MenuOptions { get; set; }
        public int SelectedMealOption { get; set; }
    }
}
