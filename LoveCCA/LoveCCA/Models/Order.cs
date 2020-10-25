using Plugin.CloudFirestore.Attributes;
using System;

namespace LoveCCA.Models
{
    public class Order
    {
        [Id]
        public string Id { get; set; }
        public string Email { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int Status { get; set; }
        public Student Kid { get; set; }
        public string ProductType { get; set; }
        public string ProductDescription { get; set; }
        public int Quantity { get; set; }
        public string SelectedProductID { get; set; }
        public string GradeId { get; set; }
    }
}
