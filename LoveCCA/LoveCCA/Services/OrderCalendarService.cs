using LoveCCA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LoveCCA.Services
{
    public interface IOrderCalendarService
    {
        Task Initialize(DateTime initDate, Student kid, string productType);
        void CreatePendingOrder(Day schoolDay);
        void CompleteOrder(Day schoolDay);
        List<Day> WeekDays { get; }
        Student Kid { get; }
        string ProductType { get; }


    }

    public class OrderCalendarService : IOrderCalendarService
    {
        private DateTime _initWeekStart;
        private SchoolYearSettings _schoolYearSettings;
        private IHolidayService _holidayService;
        private IOrderHistoryService _orderHistoryService;
        private List<Order> _relevantOrders;

        public OrderCalendarService()
        {
            _orderHistoryService = new OrderHistoryService();
            _holidayService = new HolidayService();
            WeekDays = new List<Day>();
        }

        public SchoolYearSettings SchoolYearSettings => _schoolYearSettings;


        public List<Day> WeekDays { get; private set; }
        public int Index { get; set; }
        public Student Kid { get; private set; }
        public string ProductType { get; private set; }

        public async Task Initialize(DateTime initDate, Student kid, string productType)
        {
            Kid = kid;
            ProductType = productType;

            await _orderHistoryService.LoadOrders();
            _relevantOrders = _orderHistoryService.Orders.Where(o => o.Kid.Id == this.Kid.Id && o.ProductType == this.ProductType).ToList();

            if (_schoolYearSettings == null)
                _schoolYearSettings = await _holidayService.LoadSchoolSettings();
            _initWeekStart = initDate.StartOfWeek(DayOfWeek.Sunday);
            LoadWeeks();

        }


        private Day CheckIfHoliday(Day day)
        {
            day.IsNotSchoolDay = (day.Date < _schoolYearSettings.YearStart);
            day.IsNotSchoolDay |= (day.Date.Date.DayOfWeek == DayOfWeek.Saturday || day.Date.Date.DayOfWeek == DayOfWeek.Sunday);
            day = DateIsWithinRanges(day);
            day = DateIsHoliday(day);
            return day;

        }

        private Day DateIsHoliday(Day day)
        {
            foreach (var holiday in _schoolYearSettings.Holidays.Where(h => !h.IsRange))
            {
                if (day.Date.Date == holiday.Date.Date)
                {
                    day.IsNotSchoolDay = true;
                    day.Description = holiday.Description;
                }
            }
            return day;
        }

        private Day DateIsWithinRanges(Day day)
        {
            foreach (var holiday in _schoolYearSettings.Holidays.Where(h => h.IsRange)) {
                if (day.Date.Date >= holiday.Date.Date && day.Date.Date <= holiday.EndDate.Date)
                {
                    day.IsNotSchoolDay = true;
                    day.Description = holiday.Description;
                    break;
                }
            }
            return day;
        }

        public void CreatePendingOrder(Day schoolDay)
        {
            schoolDay.OrderStatus = OrderStatus.Pending;
            schoolDay.OrderKid = Kid;
            schoolDay.OrderProductType = ProductType;
        }

        private void LoadWeeks()
        {
            WeekDays.Clear();
            var startDate = _initWeekStart.AddDays(Index * 7);
            var endDate = _schoolYearSettings.YearEnd;
            var dateToAdd = startDate;

            while (dateToAdd <= endDate)
            {
                var day = new Day() { Date = dateToAdd, OrderProductType = ProductType, OrderKid = Kid };
                day = CheckIfHoliday(day);
                day = MarkOrderHistory(day);
                WeekDays.Add(day);
                dateToAdd = dateToAdd.AddDays(1);
            }
        }

        private Day MarkOrderHistory(Day day)
        {
            var order = _relevantOrders.FirstOrDefault(o => o.DeliveryDate.Date == day.Date.Date);
            if (order != null)
            {
                day.OrderId = order.Id;
                day.OrderKid = order.Kid;
                day.OrderProductType = order.ProductType;
                if (order.Status == (int)OrderStatus.Completed)
                {
                    day.OrderStatus = OrderStatus.Completed;
                }
                if (order.Status == (int)OrderStatus.Pending)
                {
                    day.OrderStatus = OrderStatus.Pending;
                }
            }
            return day;
        }

        public void CompleteOrder(Day schoolDay)
        {
            schoolDay.OrderStatus = OrderStatus.Completed;
            schoolDay.OrderKid = Kid;
            schoolDay.OrderProductType = ProductType;
        }
    }



    public class SchoolYearSettings
    {
        public DateTime YearStart { get; set; }
        public DateTime YearEnd { get; set; }
        public List<Holiday> Holidays { get; set; }
    }

    public class Holiday
    {
        public Holiday()
        {
            EndDate = DateTime.MinValue;
        }
        public bool IsRange
        {
            get
            {
                return EndDate > Date;
            }
        }
        public DateTime Date { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
    }



    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
