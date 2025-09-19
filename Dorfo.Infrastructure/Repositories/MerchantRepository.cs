using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Domain.Entities;
using Dorfo.Infrastructure.Persistence;
using Dorfo.Infrastructure.Repositories.Basic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Repositories
{
    public class MerchantRepository : GenericRepository<Merchant>, IMerchantRepository
    {
        public MerchantRepository() { }

        public MerchantRepository(DorfoDbContext context) => _context = context;

        public async Task<bool> DeleteAsync(Guid id)
        {
            var merchant = await _context.Merchants.FindAsync(id);
            merchant.IsActive = false;
            base.UpdateAsync(merchant);
            return true;
        }

        public async Task<IEnumerable<Merchant>> GetAllAsync()
        {
            return await _context.Merchants.OrderByDescending(m => m.IsActive)
                                           .ThenByDescending(m => m.CreatedAt)
                                           .ToListAsync();
        }

        public async Task<Merchant> GetMerchantByIdAsync(Guid id)
        {
            return await _context.Merchants
                .Include(m => m.MerchantAddress)
                .Include(m => m.MerchantSetting)
                .Include(m => m.OpeningDays)
                .FirstOrDefaultAsync(m => m.MerchantId == id && m.IsActive == true);
                                            
        }

        public async Task<IEnumerable<Merchant>> GetMerchantByOwnerIdAsync(Guid id)
        {
            return await _context.Merchants
                .Where(m => m.OwnerUserId == id && m.IsActive == true)   
                .OrderByDescending(m => m.CreatedAt)    
                .ToListAsync();
        }
    }
}
