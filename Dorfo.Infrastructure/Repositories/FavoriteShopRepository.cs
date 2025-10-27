using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Domain.Entities;
using Dorfo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Repositories
{
    public class FavoriteShopRepository : IFavoriteShopRepository
    {
        private readonly DorfoDbContext _context;
        public FavoriteShopRepository(DorfoDbContext context)
        {
            _context = context;
        }

        public async Task<List<FavoriteShop>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.FavoriteShops
                .Include(f => f.Merchant)
                .Where(f => f.CustomerId == customerId)
                .OrderByDescending(f => f.AddedAt)
                .ToListAsync();
        }

        public async Task<FavoriteShop?> GetByCustomerAndMerchantAsync(Guid customerId, Guid merchantId)
        {
            return await _context.FavoriteShops
                .FirstOrDefaultAsync(f => f.CustomerId == customerId && f.MerchantId == merchantId);
        }

        public async Task AddAsync(FavoriteShop entity)
        {
            await _context.FavoriteShops.AddAsync(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
