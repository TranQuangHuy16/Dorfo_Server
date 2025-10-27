using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IFavoriteShopRepository
    {
        Task<List<FavoriteShop>> GetByCustomerIdAsync(Guid customerId);
        Task<FavoriteShop?> GetByCustomerAndMerchantAsync(Guid customerId, Guid merchantId);
        Task AddAsync(FavoriteShop entity);
        Task SaveChangesAsync();
    }
}
