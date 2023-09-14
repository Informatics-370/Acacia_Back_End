using Acacia_Back_End.Core.Models.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Acacia_Back_End.Infrastructure.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure user roles
            builder.Entity<IdentityUserRole<string>>().HasKey(ur => new { ur.UserId, ur.RoleId });

            // Seed roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Manager", NormalizedName = "MANAGER".ToUpper() },
                new IdentityRole { Id = "2", Name = "Externalcustomer", NormalizedName = "EXTERNALCUSTOMER".ToUpper() },
                new IdentityRole { Id = "3", Name = "Internalcustomer", NormalizedName = "INTERNALCUSTOMER".ToUpper() },
                new IdentityRole { Id = "4", Name = "GroupElephantcompany", NormalizedName = "GROUPELEPHANTCOMPANY".ToUpper() }
            );
        }
    }
}
