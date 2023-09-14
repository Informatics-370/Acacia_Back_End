using Acacia_Back_End.Core.Models;

namespace Acacia_Back_End.Core.Interfaces
{
    public interface IWishlistRepository
    {
        Task<CustomerWishlist> GetWishlistAsync(string wishlistId);
        Task<CustomerWishlist> UpdateWishlistAsync(CustomerWishlist wishlist);
        Task<bool> DeleteWishlistAsync(string wishlistId);
    }
}
