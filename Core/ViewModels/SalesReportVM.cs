using Acacia_Back_End.Controllers;
using Acacia_Back_End.Core.Models.CustomerOrders;
using NPOI.SS.Formula.Functions;

namespace Acacia_Back_End.Core.ViewModels
{
    public class SalesReportVM
    {
        public List<ReportCategory> Data { get; set; } = new List<ReportCategory>();
        public decimal DeliveryCosts { get; set; }
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

    public class ReportCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ReportSubCategory> SubCategories { get; set; } = new List<ReportSubCategory>();
        public decimal Total { get => GetTotal(); }
        public decimal GetTotal()
        {
            decimal total = 0;
            foreach(var subcategory in SubCategories)
            {
                total += subcategory.Total;
            }
            return total;
        }
    }

    public class ReportSubCategory
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Total { get; set; }
    }
}
