using Dorfo.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Services
{
    public class RedisOtpService : IOtpService
    {
        private readonly IDistributedCache _cache;

        public RedisOtpService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SaveOtpAsync(string email, string otp, int expireMinutes = 5)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expireMinutes)
            };

            await _cache.SetStringAsync($"otp:{email}", otp, options);
        }

        public async Task<string?> GetOtpAsync(string email)
        {
            return await _cache.GetStringAsync($"otp:{email}");
        }

        public async Task RemoveOtpAsync(string email)
        {
            await _cache.RemoveAsync($"otp:{email}");
        }
    }
}
