using Acacia_Back_End.Core.Models.CustomerOrders;

namespace Acacia_Back_End.Core.Models.SupplierOrders
{
    public class SupplierOrderItem:BaseEntity
    {
        public SupplierOrderItem()
        {
        }

        public SupplierOrderItem(SupplierProductOrdered itemOrdered, decimal price, int quantity)
        {
            ItemOrdered = itemOrdered;
            Price = price;
            Quantity = quantity;
        }
        public SupplierProductOrdered ItemOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
