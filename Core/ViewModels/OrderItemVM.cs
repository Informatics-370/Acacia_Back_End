namespace Acacia_Back_End.Core.ViewModels
{
    public class OrderItemVM
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string PictureUrl { get; set; }

        public decimal Promotion { get; set; } = 0;

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
