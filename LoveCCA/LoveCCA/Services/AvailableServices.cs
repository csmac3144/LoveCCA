using LoveCCA.Models;
using System.Collections.Generic;
using System.Linq;

namespace LoveCCA.Services
{
    class AvailableServices
    {
        List<ServicesModel> _services;
        List<ReportsModel> _reports;
        public AvailableServices()
        {
            _services = new List<ServicesModel>
            {
                new ServicesModel {Id = 0, Icon = "🍕", Description = "Order Hot Lunches", Active = true},
                new ServicesModel {Id = 1,  Icon = "🥛", Description = "Order Milk", Active = true},
                new ServicesModel {Id = 2,  Icon = "📅", Description = "School Calendar", Active = true},
                new ServicesModel {Id = 3,  Icon = "🙏", Description = "Prayer Requests", Active = false},
                new ServicesModel {Id = 4,  Icon = "🤒", Description = "Report Absence", Active = true},
                new ServicesModel {Id = 5,  Icon = "🎟", Description = "Buy Tickets", Active = false},
                new ServicesModel {Id = 6,  Icon = "😇", Description = "Donate To CCA!", Active = false},
                new ServicesModel {Id = 7,  Icon = "📋", Description = "CCA Admin Reports", Active = true, IsRestrictedToStaff = true},
                new ServicesModel {Id = 8,  Icon = "👩‍🏫", Description = "CCA Staff", Active = true, IsRestrictedToStaff = true}
            };

            _reports = new List<ReportsModel>
            {
                new ReportsModel {Id = 0, Icon = "📝", Description = "Weekly Orders", Active = true},
            };


        }


        public List<ServicesModel> Services {  get
            {
                bool staff = UserProfileService.Instance.CurrentUserProfile.IsStaffMember;
                if (staff)
                    return _services.Where(s => s.Active).ToList();
                else
                    return _services.Where(s => s.Active && !s.IsRestrictedToStaff).ToList();
            }
        }
        public List<ReportsModel> Reports => _reports;
    }
}
