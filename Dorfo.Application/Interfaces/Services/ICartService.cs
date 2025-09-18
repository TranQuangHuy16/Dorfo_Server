using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<CartResponse> AddItemsToCartAsync(AddCartItemsRequest request, Guid userId);
        Task<List<CartResponse>> GetAllCartsAsync(Guid userId);
        Task<CartResponse?> GetCartByMerchantAsync(Guid userId, Guid merchantId);
        Task<CartResponse?> RemoveItemAsync(Guid userId, Guid cartItemId, Guid merchantId);
        Task RemoveCartByMerchantAsync(Guid userId, Guid merchantId);
        Task DeleteAllCartsAsync(Guid userId);

    }
}
