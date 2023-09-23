using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Acacia_Back_End.Core.Models;

namespace Acacia_Back_End.Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Quantity).IsRequired();
            builder.Property(p => p.TresholdValue).IsRequired();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.PictureUrl).IsRequired();
            builder.HasOne(b => b.ProductCategory).WithMany().HasForeignKey(p => p.ProductCategoryId);
            builder.HasOne(b => b.ProductType).WithMany().HasForeignKey(p => p.ProductTypeId);
            builder.HasMany(b => b.PriceHistory).WithOne().HasForeignKey(pp => pp.ProductId);
        }

        public void Configure(EntityTypeBuilder<GiftBox> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.HasMany(b => b.PriceHistory).WithOne().HasForeignKey(pp => pp.GiftBoxId);
        }
    }
}
