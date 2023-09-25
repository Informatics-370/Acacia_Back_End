using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Acacia_Back_End.Infrastructure.Data
{
    public class AuditRepository: IAuditRepository
    {
        private readonly Context _context;

        public AuditRepository(Context context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<AuditTrailVM>> GetAuditTrailAsync(AuditSpecParams searchParams)
        {
            List<AuditTrailVM> auditTrail = new List<AuditTrailVM>();
            if (string.IsNullOrEmpty(searchParams.Search))
            {
                searchParams.Search = ""; 
            }


            switch (searchParams.AuditType)
            {
                case "Sale Order":
                    var saleOrdersSP = _context.SalesOrderView
                        .FromSqlRaw("EXEC GetOrdersBySearch @Search", new SqlParameter("@Search", searchParams.Search))
                        .ToList();
                    foreach (var record in saleOrdersSP)
                    {
                        auditTrail.Add(new AuditTrailVM
                        {
                            Email = record.Email,
                            Type = AuditTypeVM.SaleOrder.ToString(),
                            TransactionDate = record.TransactionDate,
                            Amount = record.Amount,
                            Quantity = record.Quantity,
                        });
                    }
                    break;

                case "Supplier Order":
                    var supplierOrdersSP = _context.SupplierOrderCombinedView
                        .FromSqlRaw("EXEC GetSupplierOrdersBySearch @Search", new SqlParameter("@Search", searchParams.Search))
                        .ToList();
                    foreach (var record in supplierOrdersSP)
                    {
                        auditTrail.Add(record);
                    }
                    break;

                case "Sale Return":
                    var saleReturnLogSP = await _context.SalesReturnsView
                        .FromSqlRaw("EXEC GetCustomerReturnsBySearch @Search", new SqlParameter("@Search", searchParams.Search))
                        .ToListAsync();
                    foreach (var record in saleReturnLogSP)
                    {
                        auditTrail.Add(new AuditTrailVM
                        {
                            Email = record.Email,
                            Type = AuditTypeVM.SaleReturn.ToString(),
                            TransactionDate = record.TransactionDate,
                            Amount = record.Amount,
                            Quantity = record.Quantity,
                        });
                    }
                    break;

                case "Supplier Return":
                    var supplierReturnLogSP = await _context.SupplierReturnsView
                        .FromSqlRaw("EXEC GetSupplierReturnsBySearch @Search", new SqlParameter("@Search", searchParams.Search))
                        .ToListAsync();
                    foreach (var record in supplierReturnLogSP)
                    {
                        auditTrail.Add(new AuditTrailVM
                        {
                            Email = record.Email,
                            Type = AuditTypeVM.SupplierReturn.ToString(),
                            TransactionDate = record.TransactionDate,
                            Amount = record.Amount,
                            Quantity = record.Quantity,
                        });
                    }
                    break;

                case "Write Off":
                    var writeOffsLogSP = await _context.WriteOffsView
                        .FromSqlRaw("EXEC GetWriteOffsBySearch @Search", new SqlParameter("@Search", searchParams.Search))
                        .ToListAsync();
                    foreach (var record in writeOffsLogSP)
                    {
                        auditTrail.Add(new AuditTrailVM
                        {
                            Email = record.Email,
                            Type = AuditTypeVM.WriteOff.ToString(),
                            TransactionDate = record.TransactionDate,
                            Amount = record.Amount,
                            Quantity = record.Quantity,
                        });
                    }
                    break;

                default:
                    var DefaultSP = _context.SalesOrderView
                       .FromSqlRaw("EXEC GetOrdersBySearch @Search", new SqlParameter("@Search", searchParams.Search))
                       .ToList();
                    foreach (var record in DefaultSP)
                    {
                        auditTrail.Add(new AuditTrailVM
                        {
                            Email = record.Email,
                            Type = AuditTypeVM.SaleOrder.ToString(),
                            TransactionDate = record.TransactionDate,
                            Amount = record.Amount,
                            Quantity = record.Quantity,
                        });
                    }
                    break;
            }

            switch (searchParams.sort)
            {
                case "amountAsc":
                    auditTrail = auditTrail.OrderBy(x => x.Amount).ToList();
                    break;
                case "amountDesc":
                    auditTrail = auditTrail.OrderByDescending(x => x.Amount).ToList();
                    break;
                case "quantityAsc":
                    auditTrail = auditTrail.OrderBy(x => x.Quantity).ToList();
                    break;
                case "quantityDesc":
                    auditTrail = auditTrail.OrderByDescending(x => x.Quantity).ToList();
                    break;
                case "dateAsc":
                    auditTrail = auditTrail.OrderBy(x => x.TransactionDate).ToList();
                    break;
                case "dateDesc":
                    auditTrail = auditTrail.OrderByDescending(x => x.TransactionDate).ToList();
                    break;
                default:
                    auditTrail = auditTrail.OrderByDescending(x => x.TransactionDate).ToList();
                    break;
            }
            return auditTrail;
        }
    }
}
