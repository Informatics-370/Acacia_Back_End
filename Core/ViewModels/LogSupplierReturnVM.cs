using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class LogSupplierReturnVM
    {
        [Required]
        public int SupplierOrderId { get; set; }
        [Required]
        public string ManagerEmail { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IReadOnlyList<SupplierReturnItemVM> ReturnItems { get; set; }
    }
}
