using System;
using System.Collections.Generic;
using System.Text;

namespace LoveCCA.Models
{
    public class MealWeekRotation
    {
        public MealWeekRotation()
        {

        }
        public MealWeekRotation(DateTime date, int menu)
        {
            Date = date;
            Menu = menu;
        }
        public DateTime Date { get; set; }
        public int Menu { get; set; }
    }
}
