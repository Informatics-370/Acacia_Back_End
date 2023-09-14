using Acacia_Back_End.Core.Models.CustomerOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acacia_Back_End.Infrastructure.Data.Config
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(x => x.ShipToAddress, a => { a.WithOwner(); });
            builder.Navigation(s => s.ShipToAddress).IsRequired();
            builder.Property(s => s.Status).HasConversion(x => x.ToString(),
                    x => (OrderStatus)Enum.Parse(typeof(OrderStatus), x));

            builder.HasMany(x => x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
