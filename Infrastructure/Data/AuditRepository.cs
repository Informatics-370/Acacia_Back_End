using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
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

            switch (searchParams.AuditType)
            {
                case "Sale Order":
                    var saleOrders = await _context.Orders
                        .Include(x => x.OrderItems)
                        .Include(x => x.DeliveryMethod)
                        .Where(x => (string.IsNullOrEmpty(searchParams.Search) || x.CustomerEmail.ToLower().Contains(searchParams.Search.ToLower())))
                        .ToListAsync();

                    foreach (var record in saleOrders)
                    {
                        auditTrail.Add(new AuditTrailVM
                        {
                            User = record.CustomerEmail,
                            Type = AuditTypeVM.SaleOrder.ToString(),
                            Date = record.OrderDate,
                            Amount = record.GetTotal(),
                            Quantity = record.OrderItems.Sum(x => x.Quantity),
                        });
                    }
                    break;
                case "Supplier Order":
                    var supplierOrders = await _context.SupplierOrders
                        .Include(x => x.OrderItems)
                        .Where(x => (string.IsNullOrEmpty(searchParams.Search) || x.ManagerEmail.ToLower().Contains(searchParams.Search.ToLower())))
                        .ToListAsync();

                    foreach (var record in supplierOrders)
                    {
                        auditTrail.Add(new AuditTrailVM
                        {
                            User = record.ManagerEmail,
                            Type = AuditTypeVM.SupplierOrder.ToString(),
                            Date = record.OrderDate,
                            Amount = record.Total,
                            Quantity = record.OrderItems.Sum(x => x.Quantity),
                        });
                    }
                    break;
                case "Sale Return":
                    var saleReturnLog = await _context.CustomerReturns
                        .Include(x => x.ReturnItems)
                        .Where(x => (string.IsNullOrEmpty(searchParams.Search) || x.CustomerEmail.ToLower().Contains(searchParams.Search.ToLower()) || x.Description.ToLower().Contains(searchParams.Search.ToLower())))
                        .ToListAsync();

                    foreach (var record in saleReturnLog)
                    {
                        auditTrail.Add(new AuditTrailVM
                        {
                            User = record.CustomerEmail,
                            Type = AuditTypeVM.SaleReturn.ToString(),
                            Date = record.Date,
                            Amount = record.Total,
                            Quantity = record.ReturnItems.Sum(x => x.Quantity),
                        });
                    }
                    break;
                case "Supplier Return":
                    var supplierReturnLog = await _context.SupplierReturns
                        .Include(x => x.ReturnItems)
                        .Where(x => (string.IsNullOrEmpty(searchParams.Search) || x.ManagerEmail.ToLower().Contains(searchParams.Search.ToLower()) || x.Description.ToLower().Contains(searchParams.Search.ToLower())))
                        .ToListAsync();

                    foreach (var record in supplierReturnLog)
                    {
                        auditTrail.Add(new AuditTrailVM
                        {
                            User = record.ManagerEmail,
                            Type = AuditTypeVM.SupplierReturn.ToString(),
                            Date = record.Date,
                            Amount = record.Total,
                            Quantity = record.ReturnItems.Sum(x => x.Quantity),
                        });
                    }
                    break;
                default:
                    var saleOrder = await _context.Orders
                        .Include(x => x.OrderItems)
                        .Include(x => x.DeliveryMethod)
                        .Where(x => (string.IsNullOrEmpty(searchParams.Search) || x.CustomerEmail.ToLower().Contains(searchParams.Search.ToLower())))
                        .ToListAsync();

                    foreach (var record in saleOrder)
                    {
                        auditTrail.Add(new AuditTrailVM
                        {
                            User = record.CustomerEmail,
                            Type = AuditTypeVM.SaleOrder.ToString(),
                            Date = record.OrderDate,
                            Amount = record.GetTotal(),
                            Quantity = record.OrderItems.Sum(x => x.Quantity),
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
                    auditTrail = auditTrail.OrderBy(x => x.Date).ToList();
                    break;
                case "dateDesc":
                    auditTrail = auditTrail.OrderByDescending(x => x.Date).ToList();
                    break;
                default:
                    auditTrail = auditTrail.OrderByDescending(x => x.Date).ToList();
                    break;
            }
            return auditTrail;
        }
    }
}
