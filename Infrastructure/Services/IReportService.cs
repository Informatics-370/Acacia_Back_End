using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.Models.CustomerOrders;
using Acacia_Back_End.Core.Models.Identities;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;

namespace Acacia_Back_End.Infrastructure.Services
{
    public interface IReportService
    {
        // Define your functions here and Implement them in the ReportService.cs file
        Task<IReadOnlyList<UserVM>> GetUsersListAsync(ReportParams specParams);
        Task<IReadOnlyList<Product>> GetProductsListAsync(ReportParams specParams);
        Task<IReadOnlyList<Supplier>> GetSupplierListAsync(ReportParams specParams);
        Task<ReportsVM> GetPromotionsReportAsync(ReportParams specParams);
        Task<ReportsVM> GetProductsReportAsync(ReportParams specParams);
        Task<SalesReportVM> GetSaleOrdersReportAsync(ReportParams specParams);

        Task<SupplierReportVM> GetSupplierOrdersReportAsync(ReportParams specParams);

        Task<ProfitabilityReportVM> GetProfitabilityReportAsync(ReportParams specParams);

        Task<DashboardReportVM> GetDashboardReportAsync();
    }
}
