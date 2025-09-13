using Dorfo.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Services.Redis
{
    public class RedisRefreshTokenService : IRefreshTokenService
    {
        private readonly IDistributedCache _cache;

        public RedisRefreshTokenService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SaveRefreshTokenAsync(Guid userId, string refreshToken, int expireMinutes = 60 * 24 * 30)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expireMinutes)
            };

            await _cache.SetStringAsync($"refresh:{userId}", refreshToken, options);
        }

        public async Task<string?> GetRefreshTokenAsync(Guid userId)
        {
            return await _cache.GetStringAsync($"refresh:{userId}");
        }

        public async Task RemoveRefreshTokenAsync(Guid userId)
        {
            await _cache.RemoveAsync($"refresh:{userId}");
        }
    }
}
