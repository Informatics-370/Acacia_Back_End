using NPOI.SS.Formula.Functions;
using Acacia_Back_End.Core.Models;
using System.Reflection;
using System.Text.Json;
using Acacia_Back_End.Core.Models.Identities;
using Acacia_Back_End.Core.Models.CustomerOrders;

namespace Acacia_Back_End.Infrastructure.Data
{
    public class ContextSeed
    {
        public static async Task SeedAsync(Context context)
        {
            if (!context.Suppliers.Any())
            {
                var supplierData = File.ReadAllText("../Acacia_Back_End/Infrastructure/Data/SeedData/supplier.json");
                var suppliers = JsonSerializer.Deserialize<List<Supplier>>(supplierData);
                context.Suppliers.AddRange(suppliers);
                await context.SaveChangesAsync();
            }
            if (!context.ProductCategories.Any())
            {
                var categoriesData = File.ReadAllText("../Acacia_Back_End/Infrastructure/Data/SeedData/categories.json");
                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);
                context.ProductCategories.AddRange(categories);
                await context.SaveChangesAsync();
            }
            if (!context.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../Acacia_Back_End/Infrastructure/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                context.ProductTypes.AddRange(types);
                await context.SaveChangesAsync();
            }
            if (!context.Products.Any())
            {
                var productsData = File.ReadAllText("../Acacia_Back_End/Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                foreach (var prod in products)
                {
                    prod.PriceHistory = new List<ProductPrice>(); // Initialize PriceHistory list
                    prod.Quantity = 10;
                    prod.TresholdValue = 5;
                    var productPrice = new ProductPrice
                    {
                        ProductId = prod.Id,
                        Price = 100,
                        StartDate = DateTime.Now
                    };

                    prod.PriceHistory.Add(productPrice); // Add ProductPrice to the PriceHistory list

                    context.Products.Add(prod);
                }

                await context.SaveChangesAsync();
            }

            if (!context.OrderTypes.Any())
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
                context.OrderTypes.Add(status1);
                context.OrderTypes.Add(status2);
                context.OrderTypes.Add(status3);
                await context.SaveChangesAsync();
            }

            if (!context.DeliveryMethods.Any())
            {
                var deliveryData = File.ReadAllText("../Acacia_Back_End/Infrastructure/Data/SeedData/delivery.json");
                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                context.DeliveryMethods.AddRange(methods);
                await context.SaveChangesAsync();
            }

            if (!context.Vats.Any())
            {
                context.Vats.Add(new Vat
                {
                    StartDate = DateTime.Now,
                    Percentage = 15,
                    IsActive = true
                });
                await context.SaveChangesAsync();
            }



            if (!context.Company.Any())
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
                context.Company.Add(compnay);
                await context.SaveChangesAsync();
            }

            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}