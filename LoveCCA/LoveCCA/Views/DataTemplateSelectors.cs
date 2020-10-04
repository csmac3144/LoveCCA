using LoveCCA.Models;
using LoveCCA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace LoveCCA.Views
{
    public class MilkOrderDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ValidTemplate { get; set; }
        public DataTemplate InvalidTemplate { get; set; }
        public DataTemplate WeekendTemplate { get; set; }
        public DataTemplate PreviouslyOrderedTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var day = (Day)item;
            if (day.OrderStatus == OrderStatus.Completed)
                return PreviouslyOrderedTemplate;
            if (day.Date.DayOfWeek == DayOfWeek.Saturday ||
                day.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                return WeekendTemplate;
            }

            bool notSchoolDay = day.IsNotSchoolDay || day.Date.Date < DateTime.Now.Date;

            return notSchoolDay ? InvalidTemplate : ValidTemplate;
        }
    }
    public class MealOrderDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ValidTemplate { get; set; }
        public DataTemplate InvalidTemplate { get; set; }
        public DataTemplate WeekendTemplate { get; set; }
        public DataTemplate PreviouslyOrderedTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var day = (Day)item;
            if (day.OrderStatus == OrderStatus.Completed)
                return PreviouslyOrderedTemplate;
            if (day.Date.DayOfWeek == DayOfWeek.Saturday ||
                day.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                return WeekendTemplate;
            }
            return day.IsNotSchoolDay || !day.Products.Any() ? InvalidTemplate : ValidTemplate;
        }
    }

    public class CalendarDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SchoolDayTemplate { get; set; }
        public DataTemplate NotSchoolDayTemplate { get; set; }
        public DataTemplate EarlyDismissalTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var day = (SpecialDay)item;
            if (day.IsEarlyDismissal)
            {
                return EarlyDismissalTemplate;
            }
            return day.IsSchoolDay ? SchoolDayTemplate : NotSchoolDayTemplate;
        }
    }


}
