namespace Acacia_Back_End.Core.Models.SupplierOrders
{
    public class SupplierProductOrdered
    {
        public SupplierProductOrdered()
        {
        }

        public SupplierProductOrdered(int productItemId, string productName, string productUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            ProductUrl = productUrl;
        }

        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string ProductUrl { get; set; }
    }
}
