namespace Acacia_Back_End.Core.Models
{
    public class Promotion:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public bool IsActive { get; set; } = false;
        public ICollection<Product> Products { get; set; }
    }
}
