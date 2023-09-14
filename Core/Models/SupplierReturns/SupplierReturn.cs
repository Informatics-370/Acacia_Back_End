using Acacia_Back_End.Core.Models.CustomerReturns;
using Acacia_Back_End.Core.Models.SupplierOrders;

namespace Acacia_Back_End.Core.Models.SupplierReturns
{
    public class SupplierReturn:BaseEntity
    {
        public SupplierReturn()
        {
        }

        public SupplierReturn(int supplierOrderId, string managerEmail, string description, DateTime date, decimal total, IReadOnlyList<SupplierReturnItem> returnItems)
        {
            SupplierOrderId = supplierOrderId;
            ManagerEmail = managerEmail;
            Description = description;
            Date = date;
            Total = total;
            ReturnItems = returnItems;
        }

        public int SupplierOrderId { get; set; }
        public SupplierOrder SupplierOrder { get; set; }
        public string ManagerEmail { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public IReadOnlyList<SupplierReturnItem> ReturnItems { get; set; }
    }
}
