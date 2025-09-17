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
        Task<CartResponse?> GetCartAsync(Guid userId);
        Task<CartResponse?> RemoveItemAsync(Guid userId, Guid cartItemId);
        Task DeleteCartAsync(Guid userId);
    }
}
