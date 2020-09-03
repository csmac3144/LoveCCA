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
        public List<string> Kids { get; set; }
        public bool AllowNotifications { get; set; }

    }
}
