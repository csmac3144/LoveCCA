using Plugin.CloudFirestore.Attributes;

namespace LoveCCA.Models
{
    public class Student
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Grade Grade { get; set; }
        [Ignored]
        public string Name => $"{FirstName} {LastName}";
        [Ignored]
        public string GradeName => Grade?.Name;
    }
}
