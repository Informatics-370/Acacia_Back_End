using Acacia_Back_End.Core.Models.CustomerReturns;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Acacia_Back_End.Core.Models.SupplierReturns;

namespace Acacia_Back_End.Infrastructure.Data.Config
{
    public class SupplierReturnConfig : IEntityTypeConfiguration<SupplierReturn>
    {
        public void Configure(EntityTypeBuilder<SupplierReturn> builder)
        {
            builder.HasOne(x => x.SupplierOrder).WithMany().HasForeignKey(x => x.SupplierOrderId);
            builder.HasMany(x => x.ReturnItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
