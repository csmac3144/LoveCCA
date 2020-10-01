using LoveCCA.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LoveCCA.Services.MealService
{
    class MealCalendarService : OrderCalendarService
    {
        public MealCalendarService()
        {
        }

        public override async Task Initialize(DateTime initDate, Student kid, string productType)
        {
            await base.Initialize(initDate, kid, productType);
            
            UpdateDays();
        }

        public void UpdateDays()
        {

            foreach (var rotation in base.SchoolYearSettings.MealWeekMenuRotationSchedule)
            {
                var weekDays = base.WeekDays.Where(d => d.Date >= rotation.Date && d.Date < rotation.Date.AddDays(7));
                
                foreach (var day in weekDays)
                {
                    var menu = base.SchoolYearSettings.HotLunchMenu.Where(m => m.MenuNumber == rotation.Menu &&
                            m.DayOfWeek == (int)day.Date.DayOfWeek).FirstOrDefault();

                    if (menu != null)
                    {
                        day.MenuOptions = new ObservableCollection<MenuOption>();
                        foreach (var option in menu.Options)
                        {
                            var mo = new MenuOption(day)
                            {
                                Id = option.Id,
                                Description = option.Description,
                                Price = option.Price,
                                Glyph = "⚪"
                            };
                            day.MenuOptions.Add(mo);
                        }
                        if (!string.IsNullOrEmpty (day.SelectedMenuOptionID))
                        {
                            var selected = day.MenuOptions.FirstOrDefault(o => o.Id == day.SelectedMenuOptionID);
                            if (selected != null)
                            {
                                day.SelectedMenuOption = selected;
                                selected.Glyph = "⚫";
                            }
                        }
                    }                         
                }
            }
        }



    }
}
