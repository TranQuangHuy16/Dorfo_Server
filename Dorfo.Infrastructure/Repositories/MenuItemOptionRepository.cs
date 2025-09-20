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
    public class MenuItemOptionRepository : GenericRepository<MenuItemOption>, IMenuItemOptionRepository
    {
        public MenuItemOptionRepository() { }

        public MenuItemOptionRepository(DorfoDbContext context) => _context = context;

        public async Task<IEnumerable<MenuItemOption>> GetAllMenuItemOptionByMenuItemIdAsync(Guid id)
        {
            return await _context.MenuItemOptions.Where(m => m.MenuItemId == id && m.IsActive == true).ToListAsync();
        }

        public async Task<MenuItemOption> GetMenuItemOptionByIdAsync(Guid id)
        {
            return await _context.MenuItemOptions.Include(m => m.Values).FirstOrDefaultAsync(m => m.OptionId == id && m.IsActive == true);
        }
    }
}
