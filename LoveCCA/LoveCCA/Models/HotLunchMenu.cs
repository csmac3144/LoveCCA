using System;
using System.Collections.Generic;
using System.Text;

namespace LoveCCA.Models
{
    public class HotLunchMenu
    {
        public HotLunchMenu()
        {
            Options = new List<MenuOption>();
        }
        public int MenuNumber { get; set; }
        public int DayOfWeek { get; set; }
        public List<MenuOption> Options { get; set; }
    }


}
