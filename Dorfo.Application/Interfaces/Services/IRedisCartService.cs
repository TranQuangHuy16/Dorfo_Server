
using Dorfo.Application.DTOs.Responses;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IRedisCartService
    {
        Task SaveCartAsync(CartResponse cart, int expireMinutes = 60);
        Task<CartResponse?> GetCartAsync(Guid userId);
        Task RemoveCartAsync(Guid userId);
        Task<CartResponse?> GetCartByIdAsync(Guid cartId);
    }
}
