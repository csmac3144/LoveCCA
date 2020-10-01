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
        private List<Product> _products;
        public MealCalendarService()
        {
        }

        public override async Task Initialize(DateTime initDate, Student kid, string productType)
        {
            await base.Initialize(initDate, kid, productType);
            
            await UpdateDays();
        }

        public async Task UpdateDays()
        {
            if (_products == null)
                _products = await ProductService.LoadProducts();

            foreach (var rotation in base.SchoolYearSettings.MealWeekMenuRotationSchedule)
            {
                var weekDays = base.WeekDays.Where(d => d.Date >= rotation.Date && d.Date < rotation.Date.AddDays(7));
                
                foreach (var day in weekDays)
                {
                    var menu = base.SchoolYearSettings.HotLunchMenu.Where(m => m.MenuNumber == rotation.Menu &&
                            m.DayOfWeek == (int)day.Date.DayOfWeek).FirstOrDefault();

                    if (menu != null)
                    {
                        day.Products = new ObservableCollection<Product>();
                        foreach (int index in menu.ProductOptionIndexes)
                        {
                            var p = _products.FirstOrDefault(pr => pr.MenuIndex == index);
                            if (p != null)
                            {
                                var mo = new Product(day)
                                {
                                    Id = p.Id,
                                    //Description = $"{p.Glyph} {p.Description}",
                                    Description = p.Description,
                                    Price = p.Price,
                                    Glyph = p.Glyph,
                                };
                                day.Products.Add(mo);
                            }
                        }
                        if (!string.IsNullOrEmpty (day.SelectedProductID))
                        {
                            var selected = day.Products.FirstOrDefault(o => o.Id == day.SelectedProductID);
                            if (selected != null)
                            {
                                day.SelectedProduct = selected;
                                selected.SelectionGlyph = "⚫";
                            }
                        }
                    }                         
                }
            }
        }



    }
}
