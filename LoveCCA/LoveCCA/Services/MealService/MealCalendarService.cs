using LoveCCA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveCCA.Services.MealService
{
    class MealCalendarService : OrderCalendarService
    {

        public override async Task Initialize(DateTime initDate, Student kid, string productType)
        {
            await base.Initialize(initDate, kid, productType);
            
            UpdateDays();
        }

        public void UpdateDays()
        {

            foreach (var rotation in base.SchoolYearSettings.MealWeekMenuRotationSchedule)
            {
                var weekays = base.WeekDays.Where(d => d.Date >= rotation.Date && d.Date < rotation.Date.AddDays(7));

            }
        }



    }
}
