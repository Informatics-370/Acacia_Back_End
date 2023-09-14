namespace Acacia_Back_End.Core.Models.SupplierReturns
{
    public class SupplierReturnItem:BaseEntity
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
