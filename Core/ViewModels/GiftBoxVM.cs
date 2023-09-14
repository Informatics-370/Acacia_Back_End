using Acacia_Back_End.Core.Models;

namespace Acacia_Back_End.Core.ViewModels
{
    public class GiftBoxVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GiftBoxImage { get; set; }
        public ICollection<ProductVM> Products { get; set; }
        public decimal Price { get; set; }
        public decimal PackagingCosts { get; set; }
    }
}
