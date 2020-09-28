using LoveCCA.Models;
using LoveCCA.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class CalendarTests
    {
        [TestMethod]
        public async Task InitTest()
        {
            var service = new OrderCalendarService(new FakeHolidayService());
            await service.Initialize(DateTime.Now, new Student { FirstName = "Ruby" }, "Milk");
            Assert.IsTrue(service.WeekDays[0].Date.DayOfWeek == DayOfWeek.Sunday);
        }
        [TestMethod]
        public async Task InitChildTest()
        {
            var service = new OrderCalendarService(new FakeHolidayService());
            await service.Initialize(DateTime.Now, new Student { FirstName = "Ruby" }, "Milk");
            Assert.AreEqual(service.Kid, new Student { FirstName = "Ruby" });
        }
        [TestMethod]
        public async Task InitProductTest()
        {
            var service = new OrderCalendarService(new FakeHolidayService());
            await service.Initialize(DateTime.Now, new Student { FirstName = "Ruby" }, "Milk");
            Assert.AreEqual(service.ProductType, "Milk");
        }
        [TestMethod]
        public async Task DayCountTest()
        {
            var service = new OrderCalendarService(new FakeHolidayService());
            await service.Initialize(DateTime.Now, new Student { FirstName = "Ruby" }, "Milk");
            Assert.IsTrue(service.WeekDays.Count > 0);
        }
        [TestMethod]
        public async Task CanGetFirstSchoolDay()
        {
            var service = new OrderCalendarService(new FakeHolidayService());
            await service.Initialize(DateTime.Now, new Student { FirstName = "Ruby" }, "Milk");
            var firstSchoolDay = service.WeekDays.Where(d => !d.IsNotSchoolDay).FirstOrDefault();
            var startDay = service.SchoolYearSettings.YearStart;
            Assert.IsTrue(firstSchoolDay.Date >= startDay);
        }
        [TestMethod]
        public async Task FirstSchoolDayOrderStatusNone()
        {
            var service = new OrderCalendarService(new FakeHolidayService());
            await service.Initialize(DateTime.Now, new Student { FirstName = "Ruby" }, "Milk");
            var firstSchoolDay = service.WeekDays.Where(d => !d.IsNotSchoolDay).FirstOrDefault();
            Assert.AreEqual(firstSchoolDay.OrderStatus, OrderStatus.None);
        }
        [TestMethod]
        public async Task CanSetPendingOrderForDay()
        {
            var service = new OrderCalendarService(new FakeHolidayService());
            await service.Initialize(DateTime.Now, new Student { FirstName = "Ruby" }, "Milk");
            var firstSchoolDay = service.WeekDays.Where(d => !d.IsNotSchoolDay).FirstOrDefault();
            service.CreatePendingOrder(firstSchoolDay);
            Assert.IsTrue(firstSchoolDay.OrderStatus == OrderStatus.Pending &&
                firstSchoolDay.OrderChild == new Student { FirstName = "Ruby" } &&
                firstSchoolDay.OrderProductType == "Milk");
        }

        [TestMethod]
        public async Task CanSetCompletedOrderForDay()
        {
            var service = new OrderCalendarService(new FakeHolidayService());
            await service.Initialize(DateTime.Now, new Student { FirstName = "Ruby" }, "Milk");
            var firstSchoolDay = service.WeekDays.Where(d => !d.IsNotSchoolDay).FirstOrDefault();
            service.CompleteOrder(firstSchoolDay);
            Assert.IsTrue(firstSchoolDay.OrderStatus == OrderStatus.Completed &&
                firstSchoolDay.OrderChild == new Student { FirstName = "Ruby" } &&
                firstSchoolDay.OrderProductType == "Milk");
        }
    }
}
