using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.Models.CustomerOrders;
using Acacia_Back_End.Core.Models.Identities;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using static iTextSharp.text.pdf.AcroFields;

namespace Acacia_Back_End.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly Context _context;
        private readonly UserManager<AppUser> _usermanager;
        private readonly IConfiguration _config;

        public ReportService(Context context, UserManager<AppUser> usermanager, IConfiguration config)
        {
            _context = context;
            _usermanager = usermanager;
            _config = config;
        }

        public async Task<ReportsVM> GetProductsReportAsync(ReportParams specParams)
        {
            // Calculate total sales (Price * Quantity) for each product
            var productSalesQuery = _context.OrderItems
                .Include(x => x.ItemOrdered)
                .GroupBy(x => x.ItemOrdered.ProductItemId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalSales = g.Sum(item => item.Price * item.Quantity * (1 - item.Promotion / 100)),
                    ProductCategoryId = 0
                });

            if (specParams.CategoryId.HasValue)
            {
                productSalesQuery = productSalesQuery
                    .Join(_context.Products,
                        ps => ps.ProductId,
                        p => p.Id,
                        (ps, p) => new
                        {
                            ProductId = ps.ProductId,
                            TotalSales = ps.TotalSales,
                            ProductCategoryId = p.ProductCategoryId
                        })
                    .Where(x => x.ProductCategoryId == specParams.CategoryId);
            }

            var productSales = await productSalesQuery.ToListAsync();

            // Fetch product data (including ProductName and PromotionId) from the database
            var productsData = await _context.Products
                .Where(p => productSales.Select(ps => ps.ProductId).Contains(p.Id))
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.PromotionId
                })
                .ToListAsync();

            // Join the product data with the total sales
            var productDataWithSales = productsData
                .Join(productSales,
                    pData => pData.Id,
                    ps => ps.ProductId,
                    (pData, sales) => new
                    {
                        ProductName = pData.Name,
                        PromotionId = pData.PromotionId,
                        TotalSales = sales.TotalSales
                    })
                .ToList();

            // Sort the products by total sales in descending order
            var sortedProducts = productDataWithSales
                .OrderByDescending(pd => pd.TotalSales)
                .ToList();

            // Select the product names and total sales for the report
            var reportData = new ReportsVM
            {
                Labels = sortedProducts.Select(pd => pd.ProductName).Take(5).ToList(),
                Data = sortedProducts.Select(pd => (decimal)pd.TotalSales).Take(5).ToList()
            };

            return reportData;
        }



        public async Task<ReportsVM> GetPromotionsReportAsync(ReportParams specParams)
        {
            var appliedPromotionCounts = await _context.OrderItems.ToListAsync();
            if (specParams.CategoryId.HasValue)
            {
                var filteredPromotionCounts = new List<OrderItem>();
                foreach (var item in appliedPromotionCounts)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == item.ItemOrdered.ProductItemId);
                    if (product != null && product.ProductCategoryId == specParams.CategoryId)
                    {
                        filteredPromotionCounts.Add(item);
                    }
                }
                appliedPromotionCounts = filteredPromotionCounts;
            }

            var promotionStats = appliedPromotionCounts
                .Where(x => x.Promotion > 0)
                .GroupBy(x => x.ItemOrdered.PromotionId)
                .Select(g => new
                {
                    PromotionId = g.Key,
                    TimesApplied = g.Count()
                })
                .ToList();


            var promotionIds = promotionStats.Select(pc => pc.PromotionId).ToList();
            var promotions = await _context.Promotions
                .Where(p => promotionIds.Contains(p.Id))
                .ToListAsync();

            var reportData = new ReportsVM
            {
                Labels = new List<string>(),
                Data = new List<decimal>()
            };

            foreach (var promotion in promotions)
            {
                var timesApplied = promotionStats.FirstOrDefault(pc => pc.PromotionId == promotion.Id)?.TimesApplied ?? 0;
                reportData.Labels.Add(promotion.Name);
                reportData.Data.Add((decimal)timesApplied);
            }
            return reportData;
        }

        public async Task<SalesReportVM> GetSaleOrdersReportAsync(ReportParams specParams)
        {
            var orders = await _context.Orders
                .Include(x => x.OrderItems)
                .Include(x => x.DeliveryMethod)
                .Where(x => string.IsNullOrEmpty(specParams.StartDate.ToString()) || (x.OrderDate >= specParams.StartDate && x.OrderDate <= specParams.EndDate))
                .ToListAsync();

            SalesReportVM salesVM = new SalesReportVM();
            foreach (var order in orders)
            {

                foreach (var orderItem in order.OrderItems)
                {
                    var product = await _context.Products.Where(x => x.Id == orderItem.ItemOrdered.ProductItemId).Include(x => x.ProductCategory).Include(x => x.ProductType).FirstOrDefaultAsync();
                    decimal productSaleTotal = 0;
                    if (orderItem.Promotion != 0)
                    {
                        productSaleTotal = (orderItem.Price * orderItem.Quantity) * (1 - orderItem.Promotion / 100);
                    }
                    else
                    {
                        productSaleTotal = (orderItem.Price * orderItem.Quantity);
                    }


                    var category = salesVM.Data.Where(x => x.CategoryId == product.ProductCategoryId).FirstOrDefault();
                    if (category != null)
                    {
                        var subcategory = category.SubCategories.Where(x => x.ProductId == product.Id).FirstOrDefault();
                        if (subcategory!= null)
                        {
                            subcategory.Total += productSaleTotal;
                        }
                        else
                        {
                            category.SubCategories.Add(new ReportSubCategory
                            {
                                ProductId = product.Id,
                                ProductName = product.Name,
                                Total = productSaleTotal
                            });
                        }
                        salesVM.DeliveryCosts += order.DeliveryMethod.Price;
                    }
                    else
                    {
                        // Creating product
                        var subcategory = new ReportSubCategory
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            Total = productSaleTotal
                        };

                        // Creating Category

                        var NewCategory = new ReportCategory
                        {
                            CategoryId = product.ProductCategoryId,
                            CategoryName = product.ProductCategory.Name,
                        };

                        NewCategory.SubCategories.Add(subcategory);
                        salesVM.Data.Add(NewCategory);
                        salesVM.DeliveryCosts += order.DeliveryMethod.Price;
                    }
                }
            }
            return salesVM;
        }

        public async Task<SupplierReportVM> GetSupplierOrdersReportAsync(ReportParams specParams)
        {
            var orders = await _context.SupplierOrders
                .Include(x => x.OrderItems)
                .Where(x => string.IsNullOrEmpty(specParams.StartDate.ToString()) || (x.OrderDate >= specParams.StartDate && x.OrderDate <= specParams.EndDate))
                .ToListAsync();

            SupplierReportVM supplierOrderVM = new SupplierReportVM();
            foreach (var order in orders)
            {

                foreach (var orderItem in order.OrderItems)
                {
                    var product = await _context.Products.Where(x => x.Id == orderItem.ItemOrdered.ProductItemId).Include(x => x.Supplier).FirstOrDefaultAsync();
                    decimal productOrderTotal = (orderItem.Price * orderItem.Quantity);


                    var supplier = supplierOrderVM.Data.Where(x => x.SupplierId == product.SupplierId).FirstOrDefault();
                    if (supplier != null)
                    {
                        var subcategory = supplier.SubCategories.Where(x => x.ProductId == product.Id).FirstOrDefault();
                        if (subcategory != null)
                        {
                            subcategory.Total += productOrderTotal;
                        }
                        else
                        {
                            supplier.SubCategories.Add(new ReportSubCategory
                            {
                                ProductId = product.Id,
                                ProductName = product.Name,
                                Total = productOrderTotal
                            });
                        }
                    }
                    else
                    {
                        // Creating product
                        var subcategory = new ReportSubCategory
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            Total = productOrderTotal
                        };

                        // Creating Supplier

                        var NewSupplier = new SupplierReportCategory
                        {
                            SupplierId = product.Supplier.Id,
                            SupplierName = product.Supplier.Name,
                        };

                        NewSupplier.SubCategories.Add(subcategory);
                        supplierOrderVM.Data.Add(NewSupplier);
                    }
                }
            }
            return supplierOrderVM;
        }

        public async Task<IReadOnlyList<Product>> GetProductsListAsync(ReportParams specParams)
        {
            var products =  await _context.Products.Include(x => x.ProductCategory).Include(x => x.ProductType).Include(x => x.Promotion).Include(x => x.PriceHistory).ToListAsync();

            switch (specParams.sort)
            {
                case "priceAsc":
                    products = products.OrderBy(p => p.GetPrice()).ToList();
                    break;
                case "priceDesc":
                    products = products.OrderByDescending(p => p.GetPrice()).ToList();
                    break;
                case "nameAsc":
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
                case "nameDesc":
                    products = products.OrderByDescending(p => p.Name).ToList();
                    break;
                default:
                    products = products.OrderBy(n => n.Name).ToList();
                    break;
            }

            return products;
        }

        public async Task<IReadOnlyList<Supplier>> GetSupplierListAsync(ReportParams specParams)
        {
            var suppliers =  await _context.Suppliers.ToListAsync();

            switch (specParams.sort)
            {
                case "nameAsc":
                    suppliers = suppliers.OrderBy(p => p.Name).ToList();
                    break;
                case "nameDesc":
                    suppliers = suppliers.OrderByDescending(p => p.Name).ToList();
                    break;
                case "emailAsc":
                    suppliers = suppliers.OrderBy(p => p.Email).ToList();
                    break;
                case "emailDesc":
                    suppliers = suppliers.OrderByDescending(p => p.Email).ToList();
                    break;
                default:
                    suppliers = suppliers.OrderBy(n => n.Name).ToList();
                    break;
            }

            return suppliers;
        }

        public async Task<IReadOnlyList<UserVM>> GetUsersListAsync(ReportParams specParams)
        {
            var users = await _usermanager.Users.ToListAsync();

            switch (specParams.sort)
            {
                case "nameAsc":
                    users = users.OrderBy(p => p.DisplayName).ToList();
                    break;
                case "nameDesc":
                    users = users.OrderByDescending(p => p.DisplayName).ToList();
                    break;
                case "emailAsc":
                    users = users.OrderBy(p => p.Email).ToList();
                    break;
                case "emailDesc":
                    users = users.OrderByDescending(p => p.Email).ToList();
                    break;
                default:
                    users = users.OrderBy(n => n.DisplayName).ToList();
                    break;
            }

            return users.Select(async user => new UserVM
            {
                Email = user.Email,
                Roles = await _usermanager.GetRolesAsync(user),
                DisplayName = user.DisplayName,
                ProfilePicture = _config["ApiUrl"] + "/" + user.ProfilePicture
            }).Select(t => t.Result).ToList();


        }

        public async Task<ProfitabilityReportVM> GetProfitabilityReportAsync(ReportParams specParams)
        {
            decimal income = 0;
            decimal expenses = 0;
            decimal supplierReturns = 0;
            decimal salesReturns = 0;

            var orders = _context.Orders
                .Include(x => x.OrderItems)
                .Include(x => x.DeliveryMethod)
                .Where(x => string.IsNullOrEmpty(specParams.StartDate.ToString()) || (x.OrderDate >= specParams.StartDate && x.OrderDate <= specParams.EndDate))
                .ToList();  
            foreach(var order in orders)
            {
                income += order.OrderItems.Sum(x => x.Price * x.Quantity);
                expenses += order.DeliveryMethod.Price;
            }

            var supplierOrders = await _context.SupplierOrders
                .Include(x => x.OrderItems)
                .Where(x => string.IsNullOrEmpty(specParams.StartDate.ToString()) || (x.OrderDate >= specParams.StartDate && x.OrderDate <= specParams.EndDate))
                .ToListAsync();
            foreach(var supOrder in supplierOrders)
            {
                expenses += supOrder.OrderItems.Sum(x => x.Price * x.Quantity);
            }

            var returns = await _context.CustomerReturns
                .Include(x => x.ReturnItems)
                .Where(x => string.IsNullOrEmpty(specParams.StartDate.ToString()) || (x.Date >= specParams.StartDate && x.Date <= specParams.EndDate))
                .ToListAsync();
            foreach(var cusReturn in returns)
            {
                salesReturns += cusReturn.ReturnItems.Sum(x => x.Price * x.Quantity);
            }

            var supReturns = await _context.SupplierReturns
                .Include(x => x.ReturnItems)
                .Where(x => string.IsNullOrEmpty(specParams.StartDate.ToString()) || (x.Date >= specParams.StartDate && x.Date <= specParams.EndDate))
                .ToListAsync();
            foreach (var supReturn in returns)
            {
                supplierReturns += supReturn.ReturnItems.Sum(x => x.Price * x.Quantity);
            }


            return new ProfitabilityReportVM()
            {
                Income = income,
                Expenses = expenses,
                SupplierReturns = supplierReturns,
                SalesReturns = salesReturns,
                Profit = (income + supplierReturns + salesReturns) - (expenses)
            };
        }
    }
}