using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IMenuItemOptionRepository
    {
        Task<int> CreateAsync(MenuItemOption itemOption);
        Task<int> UpdateAsync(MenuItemOption itemOption);
        Task<IEnumerable<MenuItemOption>> GetAllMenuItemOptionByMenuItemIdAsync(Guid id);
        Task<MenuItemOption> GetMenuItemOptionByIdAsync(Guid id);
    }
}
