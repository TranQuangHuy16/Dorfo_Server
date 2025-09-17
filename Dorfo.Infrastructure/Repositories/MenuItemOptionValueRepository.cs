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
    public class MenuItemOptionValueRepository : IMenuItemOptionValueRepository
    {
        private readonly DorfoDbContext _context;

        public MenuItemOptionValueRepository(DorfoDbContext context)
        {
            _context = context;
        }

        public async Task<List<MenuItemOptionValue>> GetByIdsAsync(IEnumerable<Guid> valueIds)
        {
            return await _context.MenuItemOptionValues
                .Where(v => valueIds.Contains(v.OptionValueId))
                .ToListAsync();
        }
    }
}
