using LoveCCA.Models;
using Plugin.CloudFirestore.Attributes;
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
        private SchoolYearConfiguration _schoolYearConfiguration;
        private IHolidayService _holidayService;
        private IOrderHistoryService _orderHistoryService;
        private List<Order> _relevantOrders;

        public OrderCalendarService()
        {
            _orderHistoryService = new OrderHistoryService();
            _holidayService = new HolidayService();
            WeekDays = new List<Day>();
        }

        public SchoolYearConfiguration SchoolYearSettings => _schoolYearConfiguration;


        public List<Day> WeekDays { get; private set; }
        public int Index { get; set; }
        public Student Kid { get; private set; }
        public string ProductType { get; private set; }

        public virtual async Task Initialize(DateTime initDate, Student kid, string productType)
        {
            Kid = kid;
            ProductType = productType;

            await _orderHistoryService.LoadOrders();
            _relevantOrders = _orderHistoryService.Orders.Where(o => o.Kid.Id == this.Kid.Id && o.ProductType == this.ProductType).ToList();

            if (_schoolYearConfiguration == null)
                _schoolYearConfiguration = await SchoolConfigurationService.Instance.GetSchoolConfiguration();
            _initWeekStart = initDate.StartOfWeek(DayOfWeek.Sunday);
            LoadWeeks();

        }


        private Day CheckIfHoliday(Day day)
        {
            day.IsNotSchoolDay = (day.Date < _schoolYearConfiguration.YearStart);
            day.IsNotSchoolDay |= (day.Date.Date.DayOfWeek == DayOfWeek.Saturday || day.Date.Date.DayOfWeek == DayOfWeek.Sunday);
            day = DateIsWithinRanges(day);
            day = DateIsHoliday(day);
            return day;

        }

        private Day DateIsHoliday(Day day)
        {
            foreach (var specialDay in _schoolYearConfiguration.SpecialDays.Where(h => !h.IsRange))
            {
                if (day.Date.Date == specialDay.Date.Date)
                {
                    
                    day.IsNotSchoolDay = (!specialDay.IsSchoolDay || specialDay.IsEarlyDismissal);
                    day.Description = specialDay.Description;
                }
            }
            return day;
        }

        private Day DateIsWithinRanges(Day day)
        {
            foreach (var specialDay in _schoolYearConfiguration.SpecialDays.Where(h => h.IsRange)) {
                if (day.Date.Date >= specialDay.Date.Date && day.Date.Date <= specialDay.EndDate.Date)
                {
                    day.IsNotSchoolDay = (!specialDay.IsSchoolDay || specialDay.IsEarlyDismissal);
                    day.Description = specialDay.Description;
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
            var endDate = _schoolYearConfiguration.YearEnd;
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



    public class SchoolYearConfiguration
    {
        public SchoolYearConfiguration()
        {
            SpecialDays = new List<SpecialDay>();
            MealWeekMenuRotationSchedule = new List<MealWeekRotation>();
        }
        [Id]
        public string Id { get; set; }
        public DateTime YearStart { get; set; }
        public DateTime YearEnd { get; set; }
        public List<SpecialDay> SpecialDays { get; set; }
        public List<MealWeekRotation> MealWeekMenuRotationSchedule { get; set; }
        public List<HotLunchMenu> HotLunchMenu { get; set; }
    }

    public class SpecialDay
    {
        public SpecialDay()
        {
            EndDate = DateTime.MinValue;
        }
        [Ignored]
        public bool IsRange
        {
            get
            {
                return EndDate > Date;
            }
        }
        public bool IsSchoolDay { get; set; }
        public bool IsEarlyDismissal { get; set; }
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
