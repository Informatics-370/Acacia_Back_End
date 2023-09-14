namespace Acacia_Back_End.Core.ViewModels
{
    public class SupplierReportVM
    {
        public List<SupplierReportCategory> Data { get; set; } = new List<SupplierReportCategory>();
        public decimal Total { get => GetTotal(); }
        public decimal GetTotal()
        {
            decimal total = 0;
            foreach (var category in Data)
            {
                total += category.GetTotal();
            }
            return total;
        }
    }

    public class SupplierReportCategory
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public List<ReportSubCategory> SubCategories { get; set; } = new List<ReportSubCategory>();
        public decimal Total { get => GetTotal(); }
        public decimal GetTotal()
        {
            decimal total = 0;
            foreach (var subcategory in SubCategories)
            {
                total += subcategory.Total;
            }
            return total;
        }
    }
}
