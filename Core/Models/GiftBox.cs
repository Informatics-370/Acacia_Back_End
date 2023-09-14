namespace Acacia_Back_End.Core.Models
{
    public class GiftBox:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string GiftBoxImage { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<GiftBoxPrice> PriceHistory { get; set; }
    }
}
