namespace Acacia_Back_End.Core.Models.CustomerOrders
{
    public class Order:BaseEntity
    {
        public Order()
        {
        }

        public Order(IReadOnlyList<OrderItem> orderItems, string customerEmail, OrderAddress shipToAddress, DeliveryMethod deliveryMethod, int orderTypeId, decimal subTotal)
        {
            CustomerEmail = customerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            SubTotal = subTotal;
            OrderTypeId = orderTypeId;
        }

        public string CustomerEmail { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderAddress ShipToAddress { get; set; }
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GroupElephantDiscount { get; set; } = 0;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public int OrderTypeId { get; set; }
        public OrderType OrderType { get; set; }
        public string PaymentIntentId { get; set; }

        public decimal GetTotal()
        {
            if(GroupElephantDiscount != 0)
            {
                return (SubTotal + DeliveryMethod.Price) * ( 1 - GroupElephantDiscount / 100);
            }
            else
            {
                return SubTotal + DeliveryMethod.Price;
            }

        }
    }
}
