using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Domain.Entities;
using Dorfo.Infrastructure.Persistence;
using Dorfo.Infrastructure.Repositories.Basic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Repositories
{
    public class MerchantCategoryRepository : GenericRepository<MerchantCategory>, IMerchantCategoryRepository
    {
        public MerchantCategoryRepository() { }

        public MerchantCategoryRepository(DorfoDbContext context) => _context = context;

        public async Task<IEnumerable<MerchantCategory>> GetAllMerchantCategoryAsync()
        {
            return await _context.MerchantCategories.Where(mc => mc.IsActive ==  true).ToListAsync();
        }

        public async Task<MerchantCategory> GetMerchantCategoryByIdAsync(int id)
        {
            return await _context.MerchantCategories
                .FirstOrDefaultAsync(mc =>  mc.MerchantCategoryId == id && mc.IsActive == true);
        }
    }
}
