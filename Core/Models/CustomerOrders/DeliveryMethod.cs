namespace Acacia_Back_End.Core.Models.CustomerOrders
{
    public class DeliveryMethod : BaseEntity
    {
        public string Name { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

    }
}
