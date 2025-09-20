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
    public class MenuItemOptionValueRepository : GenericRepository<MenuItemOptionValue>, IMenuItemOptionValueRepository
    {
        public MenuItemOptionValueRepository() { }

        public MenuItemOptionValueRepository(DorfoDbContext context) => _context = context;

        public async Task<IEnumerable<MenuItemOptionValue>> GetAllMenuItemOptionValueByOptionIdAsync(Guid id)
        {
           return await _context.MenuItemOptionValues.Where(m => m.OptionId == id && m.IsActive ==  true).ToListAsync(); 
        }

        public async Task<MenuItemOptionValue> GetMenuItemOptionValueByIdAsync(Guid id)
        {
            return await _context.MenuItemOptionValues.FirstOrDefaultAsync(m => m.OptionValueId == id && m.IsActive == true);
        }
    }
}
