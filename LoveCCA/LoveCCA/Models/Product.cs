using Plugin.CloudFirestore.Attributes;
using System;

namespace LoveCCA.Models
{
    public class Product
    {
        [Id]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Glyph { get; set; }
        public decimal Price { get; set; }
        public bool Taxable { get; set; }
        public int DayOfWeek { get; set; }
        public int Option { get; set; }
    }
}
