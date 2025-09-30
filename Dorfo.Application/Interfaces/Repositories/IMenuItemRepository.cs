using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IMenuItemRepository
    {
        Task<int> CreateAsync(MenuItem menuItem);
        Task<int> UpdateAsync(MenuItem menuItem);
        Task<IEnumerable<MenuItem>> GetAllMenuItemByMerchantIdAsync(Guid id);
        Task<IEnumerable<MenuItem>> GetAllMenuItemByCategoryIdAsync(Guid id);
        Task<MenuItem> GetMenuItemByIdAsync(Guid id);
        Task<IEnumerable<MenuItem>> GetAllMenuItemAsync();
    }
}
