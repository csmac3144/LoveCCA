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
                new ServicesModel {Id = 3,  Icon = "🙏", Description = "Prayer Requests", Active = true},
                new ServicesModel {Id = 4,  Icon = "🤒", Description = "Report Absence", Active = true},
                new ServicesModel {Id = 5,  Icon = "🎟", Description = "Buy Tickets", Active = true},
                new ServicesModel {Id = 6,  Icon = "😇", Description = "Donate To CCA!", Active = true}
            };
        }

        public List<ServicesModel> Services { get; private set; }
    }
}
