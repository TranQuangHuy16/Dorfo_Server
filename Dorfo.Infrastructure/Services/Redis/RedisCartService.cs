using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Dorfo.Infrastructure.Services.Redis
{
    public class RedisCartService : IRedisCartService
    {
        private readonly IDistributedCache _cache;

        public RedisCartService(IDistributedCache cache)
        {
            _cache = cache;
        }

        private string GetCartKey(Guid userId) => $"cart:{userId}";

        public async Task SaveCartAsync(CartResponse cart, int expireMinutes = 60)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expireMinutes)
            };

            var json = JsonSerializer.Serialize(cart);
            await _cache.SetStringAsync(GetCartKey(cart.UserId), json, options);
        }

        public async Task<CartResponse?> GetCartAsync(Guid userId)
        {
            var json = await _cache.GetStringAsync(GetCartKey(userId));
            if (string.IsNullOrEmpty(json)) return null;

            return JsonSerializer.Deserialize<CartResponse>(json);
        }

        public async Task RemoveCartAsync(Guid userId)
        {
            await _cache.RemoveAsync(GetCartKey(userId));
        }

        public async Task<CartResponse?> GetCartByIdAsync(Guid cartId)
        {
            var json = await _cache.GetStringAsync($"cartid:{cartId}");
            if (string.IsNullOrEmpty(json)) return null;
            return JsonSerializer.Deserialize<CartResponse>(json);
        }
    }


}
