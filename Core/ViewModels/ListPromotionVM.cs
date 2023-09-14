namespace Acacia_Back_End.Core.ViewModels
{
    public class ListPromotionVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public bool IsActive { get; set; }
    }
}
