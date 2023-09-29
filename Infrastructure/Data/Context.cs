using Microsoft.EntityFrameworkCore;
using Acacia_Back_End.Core.Models;
using System.Reflection;
using Acacia_Back_End.Core.Models.CustomerOrders;
using Acacia_Back_End.Core.Models.SupplierOrders;
using Acacia_Back_End.Core.Models.CustomerReturns;
using Acacia_Back_End.Core.Models.SupplierReturns;
using Acacia_Back_End.Core.ViewModels;

namespace Acacia_Back_End.Infrastructure.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
        public DbSet<Media> MediaItems { get; set; }
        public DbSet<SupplierReturnItem> SupplierReturnItems { get; set; }
        public DbSet<SupplierReturn> SupplierReturns { get; set; }
        public DbSet<SupplierOrder> SupplierOrders { get; set; }
        public DbSet<SupplierOrderItem> SupplierOrderItems { get; set; }
        public DbSet<ReturnItem> ReturnItems { get; set; }
        public DbSet<CustomerReturn> CustomerReturns { get; set; }
        public DbSet<WriteOff> WriteOffs { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<GiftBox> GiftBoxes { get; set; }
        public DbSet<GiftBoxPrice> GiftBoxPrices { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductPrice> ProductPrices{ get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Vat> Vats { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<AuditTrailVM> SupplierOrderCombinedView { get; set; }
        public DbSet<SaleOrderAuditVM> SalesOrderView { get; set; }
        public DbSet<SaleReturnAuditVM> SalesReturnsView { get; set; }
        public DbSet<SupplierReturnAuditVM> SupplierReturnsView { get; set; }
        public DbSet<WriteOffAuditVM> WriteOffsView { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<AuditTrailVM>().ToView("SupplierOrderCombinedView").HasNoKey();
            modelBuilder.Entity<SaleOrderAuditVM>().ToView("SalesOrderView").HasNoKey();
            modelBuilder.Entity<SaleReturnAuditVM>().ToView("SalesReturnsView").HasNoKey();
            modelBuilder.Entity<SupplierReturnAuditVM>().ToView("SupplierReturnsView").HasNoKey();
            modelBuilder.Entity<WriteOffAuditVM>().ToView("WriteOffsView").HasNoKey();
        }
    }
}
