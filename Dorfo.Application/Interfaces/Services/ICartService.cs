using Dorfo.Application.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<CartRequest> AddItemsToCartAsync(Guid userId, Guid merchantId, AddCartItemsRequest request);
        Task<CartRequest?> GetCartAsync(Guid userId);
        Task<CartRequest?> RemoveItemAsync(Guid userId, Guid menuItemId);
        Task DeleteCartAsync(Guid userId);
    }
}
