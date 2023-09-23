using NPOI.SS.Formula.Functions;
using Acacia_Back_End.Core.Models;
using System.Reflection;
using System.Text.Json;
using Acacia_Back_End.Core.Models.Identities;
using Acacia_Back_End.Core.Models.CustomerOrders;
using Microsoft.EntityFrameworkCore;

namespace Acacia_Back_End.Infrastructure.Data
{
    public class ContextSeed
    {
        public static async Task SeedAsync(Context context)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!await context.Suppliers.AnyAsync())
            {
                var supplierData = File.ReadAllText(path + @"\Infrastructure\Data\SeedData\supplier.json");
                var suppliers = JsonSerializer.Deserialize<List<Supplier>>(supplierData);
                await context.Suppliers.AddRangeAsync(suppliers);
                await context.SaveChangesAsync();
            }
            if (!await context.ProductCategories.AnyAsync())
            {
                var categoriesData = File.ReadAllText(path + @"\Infrastructure\Data\SeedData\categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);
                await context.ProductCategories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }
            if (!await context.ProductTypes.AnyAsync())
            {
                var typesData = File.ReadAllText(path + @"\Infrastructure\Data\SeedData\types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                await context.ProductTypes.AddRangeAsync(types);
                await context.SaveChangesAsync();
            }
            if (!await context.Products.AnyAsync())
            {
                var productsData = File.ReadAllText(path + @"\Infrastructure\Data\SeedData\products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                foreach (var prod in products)
                {
                    prod.PriceHistory = new List<ProductPrice>();
                    prod.Quantity = 10;
                    prod.TresholdValue = 5;
                    var productPrice = new ProductPrice
                    {
                        Price = 100,
                        StartDate = DateTime.Now
                    };

                    prod.PriceHistory.Add(productPrice);

                    await context.Products.AddAsync(prod);
                }

                await context.SaveChangesAsync();
            }

            if (!await context.OrderTypes.AnyAsync())
            {
                var status1 = new OrderType
                {
                    Name = "Collection",
                };
                var status2 = new OrderType
                {
                    Name = "Delivery",
                };
                var status3 = new OrderType
                {
                    Name = "Pay-and-Go",
                };
                await context.OrderTypes.AddAsync(status1);
                await context.OrderTypes.AddAsync(status2);
                await context.OrderTypes.AddAsync(status3);
                await context.SaveChangesAsync();
            }

            if (!await context.DeliveryMethods.AnyAsync())
            {
                var deliveryData = File.ReadAllText(path + @"\Infrastructure\Data\SeedData\delivery.json");
                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                await context.DeliveryMethods.AddRangeAsync(methods);
                await context.SaveChangesAsync();
            }

            if (!await context.Vats.AnyAsync())
            {
                await context.Vats.AddAsync(new Vat
                {
                    StartDate = DateTime.Now,
                    Percentage = 15,
                    IsActive = true
                });
                await context.SaveChangesAsync();
            }



            if (!await context.Company.AnyAsync())
            {
                var compnay = new Company
                {
                    VatNumber = 123456789,
                    AddressLine1 = "46 Ingersol Rd",
                    AddressLine2 = "",
                    Suburb = "Lynnwood Glen",
                    City = "Pretoria",
                    Province = "Gauteng",
                    PostalCode = 0081
                };
                await context.Company.AddAsync(compnay);
                await context.SaveChangesAsync();
            }

            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}