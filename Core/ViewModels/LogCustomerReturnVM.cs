using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class LogCustomerReturnVM
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IReadOnlyList<ReturnItemVM> ReturnItems { get; set; }
    }
}
