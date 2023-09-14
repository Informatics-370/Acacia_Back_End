namespace Acacia_Back_End.Core.ViewModels
{
    public class ListGiftBoxVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GiftBoxImage { get; set; }
        public decimal Price { get; set; }
        public decimal PackagingCosts { get; set; }
    }
}
