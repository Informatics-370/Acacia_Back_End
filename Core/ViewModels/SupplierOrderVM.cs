using Acacia_Back_End.Core.Models.SupplierOrders;
using Acacia_Back_End.Core.Models;

namespace Acacia_Back_End.Core.ViewModels
{
    public class SupplierOrderVM
    {
        public string Id { get; set; }
        public string ManagerEmail { get; set; }
        public DateTime OrderDate { get; set; } 
        public string Status { get; set; }
        public IReadOnlyList<SupplierOrderItemVM> OrderItems { get; set; }
        public decimal Total { get; set; }
        public CompanyVM CompanyDetails { get; set; }
        public SupplierVM Supplier { get; set; }
    }
}
