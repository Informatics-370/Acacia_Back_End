using Acacia_Back_End.Core.Models.Identities;

namespace Acacia_Back_End.Core.Models
{
    public class ProductReview : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string CustomerEmail { get; set; }

        public DateTime Date { get; set; }

        private int _rating;
        public int Rating
        {
            get => _rating;
            set => _rating = (value < 6) ? value : _rating;
        }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public ProductReviewStatus Status { get; set; } = ProductReviewStatus.Available;
    }
}
