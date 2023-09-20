using Acacia_Back_End.Core.Models.CustomerOrders;

namespace Acacia_Back_End.Core.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderAddress ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Savings { get; set; }
        public string OrderType { get; set; }
        public IReadOnlyList<OrderItemVM> OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GroupElephantDiscount { get; set; }
        public decimal Total { get; set; }
        public VatVM VAT { get; set; }
        public string Status { get; set; } 
    }
}
