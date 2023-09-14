using Microsoft.AspNetCore.Identity;
using NPOI.SS.Formula.Functions;

namespace Acacia_Back_End.Core.Models.Identities
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public string? ProfilePicture { get; set; }

        public User_Address Address { get; set; }
    }
}
