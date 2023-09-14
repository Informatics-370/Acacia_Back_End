using System.Security.Claims;

namespace Acacia_Back_End.Extensions
{
    public static class ClaimsPrincipleExtention
    {
        public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email);  
        }   
    }
}
