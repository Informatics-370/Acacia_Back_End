using Acacia_Back_End.Core.Models.Identities;

namespace Acacia_Back_End.Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
