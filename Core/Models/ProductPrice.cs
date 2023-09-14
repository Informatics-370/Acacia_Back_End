namespace Acacia_Back_End.Core.Models
{
    public class ProductPrice:BaseEntity
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
