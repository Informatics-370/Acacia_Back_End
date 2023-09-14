namespace Acacia_Back_End.Core.Models.CustomerOrders
{
    public class OrderItem:BaseEntity
    {
        public OrderItem()
        {
        }

        public OrderItem(ProductOrdered itemOrdered, decimal price, int quantity)
        {
            ItemOrdered = itemOrdered;
            Price = price;
            Quantity = quantity;
        }

        public ProductOrdered ItemOrdered {get; set; }
        public decimal Promotion { get; set; } = 0;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
