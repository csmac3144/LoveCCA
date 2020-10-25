using LoveCCA.Models;
using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;

namespace LoveCCA.Models
{
    public class SchoolYearConfiguration
    {
        public SchoolYearConfiguration()
        {
            SpecialDays = new List<SpecialDay>();
            MealWeekMenuRotationSchedule = new List<MealWeekRotation>();
        }
        [Id]
        public string Id { get; set; }
        public DateTime YearStart { get; set; }
        public DateTime YearEnd { get; set; }
        public List<SpecialDay> SpecialDays { get; set; }
        public List<MealWeekRotation> MealWeekMenuRotationSchedule { get; set; }
        public List<HotLunchMenu> HotLunchMenu { get; set; }
        public List<Grade> Grades { get; set; }
    }
}
