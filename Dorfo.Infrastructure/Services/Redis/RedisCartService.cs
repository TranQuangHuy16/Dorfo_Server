using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

        public async Task SaveCartAsync(Cart cart, int expireMinutes = 60)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expireMinutes)
            };

            var json = JsonSerializer.Serialize(cart);
            await _cache.SetStringAsync(GetCartKey(cart.UserId), json, options);
        }

        public async Task<Cart?> GetCartAsync(Guid userId)
        {
            var json = await _cache.GetStringAsync(GetCartKey(userId));
            if (string.IsNullOrEmpty(json)) return null;
            return JsonSerializer.Deserialize<Cart>(json);
        }

        public async Task RemoveCartAsync(Guid userId)
        {
            await _cache.RemoveAsync(GetCartKey(userId));
        }
    }
}

