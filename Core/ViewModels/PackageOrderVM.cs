using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class PackageOrderVM
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
    }
}
