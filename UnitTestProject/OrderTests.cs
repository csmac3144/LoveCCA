using LoveCCA.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public async Task InitTest()
        {
            var service = new OrderHistoryService();
            await service.LoadOrders();
            Assert.IsTrue(true);
        }
    }
}
