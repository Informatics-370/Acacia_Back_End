using Acacia_Back_End.Core.Models.CustomerOrders;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Acacia_Back_End.Core.Models.CustomerReturns;

namespace Acacia_Back_End.Infrastructure.Data.Config
{
    public class CustomerReturnConfig : IEntityTypeConfiguration<CustomerReturn>
    {
        public void Configure(EntityTypeBuilder<CustomerReturn> builder)
        {
            builder.HasOne(x => x.Order).WithMany().HasForeignKey(x => x.OrderId);
            builder.HasMany(x => x.ReturnItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
