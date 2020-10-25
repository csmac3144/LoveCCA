using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoveCCA.Models
{
    public class UserProfile
    {
        private string _email;
        public UserProfile()
        {
            FCMTokens = new List<string>();
            Kids = new List<Student>();
        }

        [Id]
        public string Id { get; set; }
        public string Email { 
            get {
                return _email;
            } 
            set {
                _email = value.ToLower();
            }
        }
        public string Name { get; set; }
        public string CellPhone { get; set; }
        public List<Student> Kids { get; set; }
        public bool AllowNotifications { get; set; }
        //public bool UrgentNotificationsOnly { get; internal set; }
        public List<Order> OrderHistory { get; set; }
        public List<string> FCMTokens { get; internal set; }
        public bool IsStaffMember { get; set; }
    }
}
