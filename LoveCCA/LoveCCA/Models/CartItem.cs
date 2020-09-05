using Plugin.CloudFirestore.Attributes;

namespace LoveCCA.Models
{
    public class CartItem
    {
        [Id]
        public string Id { get; internal set; }
        public string ProductType { get; set; }
        public string Glyph { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string Child { get; internal set; }
        public string Total { get; internal set; }
        public string QuantityPriceLabel
        {
            get
            {
                return $"{Quantity} @ {Price} ea.";
            }
        }
    }
}
