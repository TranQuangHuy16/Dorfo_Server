using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;
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

            // Lấy list cart hiện tại
            var json = await _cache.GetStringAsync(GetCartKey(cart.UserId));
            var carts = string.IsNullOrEmpty(json)
                ? new List<CartResponse>()
                : JsonSerializer.Deserialize<List<CartResponse>>(json)!;

            // Thay thế hoặc thêm cart mới theo MerchantId
            var existing = carts.FirstOrDefault(c => c.Merchant.MerchantId == cart.Merchant.MerchantId);
            if (existing != null)
            {
                carts.Remove(existing);
            }
            carts.Add(cart);

            var updatedJson = JsonSerializer.Serialize(carts);
            await _cache.SetStringAsync(GetCartKey(cart.UserId), updatedJson, options);
        }

        public async Task<List<CartResponse>> GetCartsAsync(Guid userId)
        {
            var json = await _cache.GetStringAsync(GetCartKey(userId));
            if (string.IsNullOrEmpty(json)) return new List<CartResponse>();
            return JsonSerializer.Deserialize<List<CartResponse>>(json)!;
        }

        public async Task<CartResponse?> GetCartByIdAsync(Guid userId, Guid cartId)
        {
            var carts = await GetCartsAsync(userId);
            return carts.FirstOrDefault(c => c.CartId == cartId);
        }


        public async Task<CartResponse?> GetCartByMerchantAsync(Guid userId, Guid merchantId)
        {
            var carts = await GetCartsAsync(userId);
            return carts.FirstOrDefault(c => c.Merchant.MerchantId == merchantId);
        }

        //public async Task RemoveCartAsync(Guid userId, Guid merchantId)
        //{
        //    var carts = await GetCartsAsync(userId);
        //    carts = carts.Where(c => c.Merchant.MerchantId != merchantId).ToList();
        //    var json = JsonSerializer.Serialize(carts);
        //    await _cache.SetStringAsync(GetCartKey(userId), json);
        //}

        public async Task RemoveCartAsync(Guid userId, Guid merchantId)
        {
            var carts = await GetCartsAsync(userId);

            // Xoá cart có MerchantId trùng khớp
            carts.RemoveAll(c => c.Merchant.MerchantId == merchantId);

            var json = JsonSerializer.Serialize(carts);
            await _cache.SetStringAsync(GetCartKey(userId), json);
        }


        public async Task DeleteAllCartsAsync(Guid userId)
        {
            await _cache.RemoveAsync(GetCartKey(userId));
        }
    }

}
