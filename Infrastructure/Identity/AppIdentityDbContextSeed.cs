using Acacia_Back_End.Core.Models.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;

namespace Acacia_Back_End.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!await userManager.Users.AnyAsync())
            {
                var user = new AppUser
                {
                    DisplayName = "Mzamo",
                    Email = "mzamotembe7@gmail.com",
                    UserName = "mzamotembe7@gmail.com",
                    ProfilePicture = "images/users/default.jpg",
                    Address = new User_Address()
                    {
                        StreetAddress = "13 Silverleaf",
                        ComplexName = "Ironwood",
                        Suburb = "Ballito",
                        City = "Durban",
                        Province = "KwaZulu-Natal",
                        PostalCode = "4399"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");

                await userManager.AddToRoleAsync(user, "Manager");
            }
        }
    }
}
