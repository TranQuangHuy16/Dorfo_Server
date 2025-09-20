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
    public class MenuItemRepository : GenericRepository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository() { }

        public MenuItemRepository(DorfoDbContext context) => _context = context;

        public async Task<IEnumerable<MenuItem>> GetAllMenuItemByCategoryIdAsync(Guid id)
        {
            return await _context.MenuItems.Where(m => m.CategoryId == id && m.IsActive == true).ToListAsync();
        }

        public async Task<IEnumerable<MenuItem>> GetAllMenuItemByMerchantIdAsync(Guid id)
        {
            return await _context.MenuItems.Where(m => m.MerchantId == id && m.IsActive == true).ToListAsync();
        }

        public async Task<MenuItem> GetMenuItemByIdAsync(Guid id)
        {
            return await _context.MenuItems
                .Include(m => m.Options)
                .ThenInclude(m => m.Values)
                .FirstOrDefaultAsync(m => m.MenuItemId == id);
        }
    }
}
