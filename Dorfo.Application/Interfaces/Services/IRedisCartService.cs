
using Dorfo.Application.DTOs.Responses;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IRedisCartService
    {
        Task SaveCartAsync(CartResponse cart, int expireMinutes = 60);
        Task<List<CartResponse>> GetCartsAsync(Guid userId);
        Task<CartResponse?> GetCartByMerchantAsync(Guid userId, Guid merchantId);
        Task RemoveCartAsync(Guid userId, Guid merchantId);
        Task DeleteAllCartsAsync(Guid userId);
        Task<CartResponse?> GetCartByIdAsync(Guid userId, Guid cartId);

    }
}
