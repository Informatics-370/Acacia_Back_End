using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class UpdatePromotionVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Percentage { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public ICollection<ProductVM> Products { get; set; }
    }
}
