using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Acacia_Back_End.Infrastructure.Data
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly IDatabase _database;
        public WishlistRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<CustomerWishlist> GetWishlistAsync(string wishlistId)
        {
            var data = await _database.StringGetAsync(wishlistId);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerWishlist>(data);
        }

        public async Task<CustomerWishlist> UpdateWishlistAsync(CustomerWishlist wishlist)
        {

            var created = await _database.StringSetAsync(wishlist.Id, JsonSerializer.Serialize(wishlist), TimeSpan.FromDays(90));

            if (!created) return null;

            return await GetWishlistAsync(wishlist.Id);

        }

        public async Task<bool> DeleteWishlistAsync(string wishlistId)
        {
            return await _database.KeyDeleteAsync(wishlistId);
        }
    }
}
