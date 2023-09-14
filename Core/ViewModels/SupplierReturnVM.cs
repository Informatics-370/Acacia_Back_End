using Acacia_Back_End.Core.Models.SupplierOrders;
using Acacia_Back_End.Core.Models.SupplierReturns;

namespace Acacia_Back_End.Core.ViewModels
{
    public class SupplierReturnVM
    {
        public int Id { get; set; }
        public int SupplierOrderId { get; set; }
        public string ManagerEmail { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public IReadOnlyList<SupplierReturnItemVM> ReturnItems { get; set; }
    }
}
