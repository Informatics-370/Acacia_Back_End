namespace Acacia_Back_End.Core.Models
{
    public class WishlistItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
    }
}
