using Plugin.CloudFirestore.Attributes;
using System;

namespace LoveCCA.Models
{
    public class AbsenceReport
    {
        [Id]
        public string Id { get; set; }
        public string ParentName { get; set; }
        public string StudentName { get; set; }
        public string Grade { get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
        public string ParentEmail { get; internal set; }
        [Ignored]
        public string ReportedBy {  get
            {
                if (string.IsNullOrEmpty(ParentName))
                    return ParentEmail;
                return $"{ParentName} ({ParentEmail})";
            } 
        }

        public DateTime ReportedDate { get; set; }
        [Ignored]
        public string ReportedDateLabel => ReportedDate.ToLocalTime().ToString("g");
    }
}
