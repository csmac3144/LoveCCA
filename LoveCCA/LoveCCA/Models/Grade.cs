using Plugin.CloudFirestore.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoveCCA.Models
{
    public class Grade
    {
        public string GradeId { get; set; }
        public string Name { get; set; }
        public List<string> Grades { get; set; }
        public string Teacher { get; set; }
        [Ignored]
        public string GradesDescription {  get
            {
                return String.Join(", ", Grades.ToArray());
            } 
        }
        public override string ToString()
        {
            return $"{Teacher} ({Name})";
        }
    }
}
