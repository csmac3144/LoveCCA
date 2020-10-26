using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
