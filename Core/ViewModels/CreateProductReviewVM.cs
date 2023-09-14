namespace Acacia_Back_End.Core.ViewModels
{
    public class CreateProductReviewVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CustomerEmail { get; set; }
        public int Rating { get; set; }
        public int ProductId { get; set; }
    }
}
