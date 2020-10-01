using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoveCCA.Models
{
    public class HotLunchMenu
    {
        public HotLunchMenu()
        {
            ProductOptions = new List<Product>();
            ProductOptionIndexes = new List<int>();
        }
        public int MenuNumber { get; set; }
        public int DayOfWeek { get; set; }
        [Ignored]
        public List<Product> ProductOptions { get; set; }
        public List<int> ProductOptionIndexes { get; set; }
    }


}
