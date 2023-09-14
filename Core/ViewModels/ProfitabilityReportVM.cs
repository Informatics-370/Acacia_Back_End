namespace Acacia_Back_End.Core.ViewModels
{
    public class ProfitabilityReportVM
    {
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public decimal SupplierReturns { get; set; }
        public decimal SalesReturns { get; set; }
        public decimal Profit { get; set; }
    }
}
