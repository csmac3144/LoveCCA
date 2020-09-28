using System;
using System.Collections.Generic;
using System.Text;

namespace LoveCCA.Models
{
    class Menu
    {
        public Menu()
        {
            Options = new List<MenuOption>();
        }
        public DayOfWeek DayOfWeek { get; set; }
        public List<MenuOption> Options { get; set; }
    }

    class MenuOption { 
        public string Description { get; set; }
        public decimal Price { get; set; }
    }


}
