using Acacia_Back_End.Core.Models;

namespace Acacia_Back_End.Core.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Promotion { get; set; }
        public string ProductCategory { get; set; }
        public string ProductType { get; set; }
        public string Supplier { get; set; }
        public string PictureUrl { get; set; }
        public string QRCode { get; set; }
        public int Quantity { get; set; }
        public int TresholdValue { get; set; }
    }
}
