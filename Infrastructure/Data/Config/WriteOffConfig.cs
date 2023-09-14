using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Acacia_Back_End.Core.Models;
using System.Drawing;

namespace Acacia_Back_End.Infrastructure.Data.Config
{

        public class WriteOffConfig : IEntityTypeConfiguration<WriteOff>
        {
            public void Configure(EntityTypeBuilder<WriteOff> builder)
            {
                builder.Property(s => s.Reason).HasConversion(x => x.ToString(),
                        x => (WriteOffReason)Enum.Parse(typeof(WriteOffReason), x));
            }
        }

}
