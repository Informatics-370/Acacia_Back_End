using Acacia_Back_End.Core.Models.CustomerOrders;

namespace Acacia_Back_End.Core.Models.SupplierOrders
{
    public class SupplierOrder:BaseEntity
    {
        public SupplierOrder()
        {
        }

        public SupplierOrder(IReadOnlyList<SupplierOrderItem> orderItems, string managerEmail, Company companyDetails, Supplier supplier, decimal total)
        {
            ManagerEmail = managerEmail;
            OrderItems = orderItems;
            CompanyDetails = companyDetails;
            Supplier = supplier;
            Total = total;
        }

        public string ManagerEmail { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public SupplierOrderStatus Status { get; set; } = SupplierOrderStatus.Pending;
        public IReadOnlyList<SupplierOrderItem> OrderItems { get; set; }
        public decimal Total { get; set; }
        public string InvoiceUrl { get; set; }
        public string ProofOfPaymentUrl { get; set; }
        public Company CompanyDetails { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}
