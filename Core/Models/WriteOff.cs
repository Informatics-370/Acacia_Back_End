namespace Acacia_Back_End.Core.Models
{
    public class WriteOff:BaseEntity
    {
        public string ManagerEmail { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public WriteOffReason Reason { get; set; }
    }
}
