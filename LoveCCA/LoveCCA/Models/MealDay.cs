using System;
using System.Collections.Generic;
using System.Text;

namespace LoveCCA.Models
{
    class MealDay : Day
    {
        public int SelectedOption { get; set; }
        public string Option1Description { get; set; }
        public string Option1Price { get; set; }
        public string Option2Description { get; set; }
        public string Option2Price { get; set; }

    }
}
