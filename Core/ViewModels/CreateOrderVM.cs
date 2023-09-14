using Acacia_Back_End.Core.Models.CustomerOrders;

namespace Acacia_Back_End.Core.ViewModels
{
    public class CreateOrderVM
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public int OrderTypeId { get; set; }
        public OrderAddressVM ShipToAddress { get; set; }
    }
}
