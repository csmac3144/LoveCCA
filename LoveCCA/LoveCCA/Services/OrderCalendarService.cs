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
        Task Initialize(DateTime initDate, string child, string productType);
        void CreatePendingOrder(Day schoolDay);
        void CompleteOrder(Day schoolDay);
        List<Day> WeekDays { get; }
        string Child { get; }
        string ProductType { get; }


    }

    public class OrderCalendarService : IOrderCalendarService
    {
        private DateTime _initWeekStart;
        private SchoolYearSettings _schoolYearSettings;
        private IHolidayService _holidayService;
        public OrderCalendarService() : this(DependencyService.Get<IHolidayService>())
        {
        }

        public SchoolYearSettings SchoolYearSettings => _schoolYearSettings;

        public OrderCalendarService(IHolidayService holidayService)
        {
            _holidayService = holidayService;
            WeekDays = new List<Day>();
        }

        public List<Day> WeekDays { get; private set; }
        public int Index { get; set; }
        public string Child { get; private set; }
        public string ProductType { get; private set; }

        public async Task Initialize(DateTime initDate, string child, string productType)
        {
            Child = child;
            ProductType = productType;
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
            schoolDay.OrderChild = Child;
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
                var day = new Day() { Date = dateToAdd };
                WeekDays.Add(CheckIfHoliday(day));
                dateToAdd = dateToAdd.AddDays(1);
            }
        }

        public void CompleteOrder(Day schoolDay)
        {
            schoolDay.OrderStatus = OrderStatus.Completed;
            schoolDay.OrderChild = Child;
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
