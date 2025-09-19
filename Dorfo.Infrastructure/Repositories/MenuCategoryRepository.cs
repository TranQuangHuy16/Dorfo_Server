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
    public class MenuCategoryRepository : GenericRepository<MenuCategory>, IMenuCategoryRepository
    {
        public MenuCategoryRepository() { }

        public MenuCategoryRepository(DorfoDbContext context) => _context = context;

        public async Task<MenuCategory> GetCategoryByIdAsync(Guid id)
        {
            return await _context.MenuCategories
                .Include(m => m.MenuItems)
                .ThenInclude(m => m.Options)
                .ThenInclude(m => m.Values)
                .FirstOrDefaultAsync(m => m.MenuCategoryId == id);
        }

        public async Task<IEnumerable<MenuCategory>> GetAllCategoryByMerchantIdAsync(Guid id)
        {
            return await _context.MenuCategories.Where(m => m.MerchantId == id).ToListAsync();
        }
    }
}
