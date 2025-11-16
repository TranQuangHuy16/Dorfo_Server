using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IFavoriteShopService
    {
        Task<List<FavoriteShop>> GetFavoriteShopsByCustomerAsync(Guid customerId);
        Task<FavoriteShop?> AddFavoriteShopAsync(Guid customerId, Guid merchantId);
        Task<bool> RemoveFavoriteShopAsync(Guid customerId, Guid merchantId);
    }
}
