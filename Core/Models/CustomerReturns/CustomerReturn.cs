using Acacia_Back_End.Core.Models.CustomerOrders;

namespace Acacia_Back_End.Core.Models.CustomerReturns
{
    public class CustomerReturn : BaseEntity
    {
        public CustomerReturn()
        {
        }

        public CustomerReturn(int orderId, string customerEmail, string description, DateTime date, decimal total, IReadOnlyList<ReturnItem> returnItems)
        {
            OrderId = orderId;
            CustomerEmail = customerEmail;
            Description = description;
            Date = date;
            Total = total;
            ReturnItems = returnItems;
        }

        public int OrderId { get; set; }
        public Order Order { get; set; }
        public string CustomerEmail { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public IReadOnlyList<ReturnItem> ReturnItems { get; set; }
    }
}
