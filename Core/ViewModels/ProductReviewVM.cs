using Acacia_Back_End.Core.Models;

namespace Acacia_Back_End.Core.ViewModels
{
    public class ProductReviewVM
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CustomerEmail { get; set; }
        public int Rating { get; set; }

        public DateTime Date { get; set; }

        public string Product { get; set; }
        public string Status { get; set; }
    }
}
