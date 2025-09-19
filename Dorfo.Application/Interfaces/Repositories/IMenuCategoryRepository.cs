using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IMenuCategoryRepository
    {
        Task<int> CreateAsync(MenuCategory category);
        Task<int> UpdateAsync(MenuCategory category);
        Task<IEnumerable<MenuCategory>> GetAllCategoryByMerchantIdAsync(Guid id);
        Task<MenuCategory> GetCategoryByIdAsync(Guid id);
    }
}
