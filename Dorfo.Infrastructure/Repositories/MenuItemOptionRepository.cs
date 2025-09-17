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
    public class MenuItemOptionRepository : IMenuItemOptionRepository
    {
        private readonly DorfoDbContext _context;

        public MenuItemOptionRepository(DorfoDbContext context)
        {
            _context = context;
        }

        public async Task<MenuItemOption?> GetByIdAsync(Guid optionId)
        {
            return await _context.MenuItemOptions
                .Include(o => o.Values) // load luôn danh sách value
                .FirstOrDefaultAsync(o => o.OptionId == optionId);
        }
    }
}
