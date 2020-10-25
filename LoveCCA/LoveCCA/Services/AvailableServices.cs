using LoveCCA.Models;
using System.Collections.Generic;

namespace LoveCCA.Services
{
    class AvailableServices
    {
        public AvailableServices()
        {
            Services = new List<ServicesModel>
            {
                new ServicesModel {Id = 0, Icon = "🍕", Description = "Order Hot Lunches", Active = true},
                new ServicesModel {Id = 1,  Icon = "🥛", Description = "Order Milk", Active = true},
                new ServicesModel {Id = 2,  Icon = "📅", Description = "School Calendar", Active = true},
                new ServicesModel {Id = 3,  Icon = "🙏", Description = "Prayer Requests", Active = false},
                new ServicesModel {Id = 4,  Icon = "🤒", Description = "Report Absence", Active = false},
                new ServicesModel {Id = 5,  Icon = "🎟", Description = "Buy Tickets", Active = false},
                new ServicesModel {Id = 6,  Icon = "😇", Description = "Donate To CCA!", Active = false},
                new ServicesModel {Id = 7,  Icon = "📝", Description = "CCA Staff Reports", Active = true}
            };

            Reports = new List<ReportsModel>
            {
                new ReportsModel {Id = 0, Icon = "📋", Description = "Weekly Orders", Active = true},
            };


        }


        public List<ServicesModel> Services { get; private set; }
        public List<ReportsModel> Reports { get; private set; }
    }
}
