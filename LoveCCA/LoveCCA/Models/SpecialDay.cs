using Plugin.CloudFirestore.Attributes;
using System;

namespace LoveCCA.Models
{
    public class SpecialDay
    {
        public SpecialDay()
        {
            EndDate = DateTime.MinValue;
        }
        [Ignored]
        public bool IsRange
        {
            get
            {
                return EndDate > Date;
            }
        }
        public bool IsSchoolDay { get; set; }
        public bool IsEarlyDismissal { get; set; }
        public DateTime Date { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        [Ignored]
        public string DayOfWeekLabel => Date.Date.DayOfWeek.ToString();
        [Ignored]
        public string DateLabel
        {
            get
            {
                if (IsRange)
                    return $"{Date.ToString("M")} until {EndDate.ToString("M")}";
                else
                    return Date.ToString("M");
            }
        }

        public string Glyph { get; set; }
    }
}
