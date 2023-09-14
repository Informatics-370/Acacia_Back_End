namespace Acacia_Back_End.Core.Models.CustomerOrders
{
    public class ProductOrdered
    {
        public ProductOrdered()
        {
        }

        public ProductOrdered(int productItemId, string productName, string productUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            ProductUrl = productUrl;
        }

        public int? PromotionId { get; set; }
        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string ProductUrl { get; set; }
    }
}
