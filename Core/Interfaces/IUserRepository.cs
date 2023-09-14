using Acacia_Back_End.Core.Models.Identities;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;

namespace Acacia_Back_End.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IReadOnlyList<UserVM>> GetUsersAsync(UserSpecParams userParams);

        // Define your Permission functions here (if necessary) then Implement them in the UserRepository.cs file
    }
}
