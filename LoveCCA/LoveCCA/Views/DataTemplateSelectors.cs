using LoveCCA.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LoveCCA.Views
{
    public class PersonDataTemplateSelector : DataTemplateSelector
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
            return ((Day)item).IsNotSchoolDay ? InvalidTemplate : ValidTemplate;
        }
    }
}
