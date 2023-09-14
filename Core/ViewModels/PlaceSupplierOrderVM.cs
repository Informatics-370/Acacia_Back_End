using Acacia_Back_End.Core.Models.SupplierOrders;
using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class ConfigureSupplierOrderVM
    {
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public List<SupplierOrderItemVM> OrderItems { get; set; }
    }
}
