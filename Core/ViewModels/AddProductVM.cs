using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class AddProductVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        public int ProductCategoryId { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        public int ProductTypeId { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        public int SupplierId { get; set; }
        public string PictureUrl { get; set; }
        [Required]
        [Range(1, double.MaxValue)]
        public int TresholdValue { get; set; }
    }
}
