using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class AddPromotionVM
    {
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
