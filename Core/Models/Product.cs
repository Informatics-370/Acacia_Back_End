using System.ComponentModel.DataAnnotations.Schema;

namespace Acacia_Back_End.Core.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public string QRCode { get; set; }
        public int Quantity { get; set; }
        public int TresholdValue { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public int ProductCategoryId { get; set; }
        public int? PromotionId { get; set; }
        public Promotion Promotion { get; set; }
        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public List<GiftBox> GiftBoxes { get; set; }
        public List<ProductPrice> PriceHistory { get; set; }

        public decimal GetPrice()
        {
            return PriceHistory.OrderByDescending(pp => pp.StartDate).First().Price;
        }
    }
}
