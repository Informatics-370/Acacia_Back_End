namespace Acacia_Back_End.Core.ViewModels
{
    public class DashboardReportVM
    {
        public IReadOnlyList<OrderVM> Orders { get; set; }
        public decimal TotalSales { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalItemsSold { get; set; }
        public int PendingSupplierOrders { get; set; }
    }
}
