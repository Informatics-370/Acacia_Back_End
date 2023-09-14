using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Acacia_Back_End.Core.Models.SupplierOrders;

namespace Acacia_Back_End.Infrastructure.Data.Config
{
    public class SupplierOrderConfig : IEntityTypeConfiguration<SupplierOrder>
    {
        public void Configure(EntityTypeBuilder<SupplierOrder> builder)
        {
            builder.Property(s => s.Status).HasConversion(x => x.ToString(),
                    x => (SupplierOrderStatus)Enum.Parse(typeof(SupplierOrderStatus), x));
            builder.HasMany(x => x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
