using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IMenuItemService
    {
        Task<MenuItemResponse> CreateAsync(MenuItemRequest request);
        Task<MenuItemResponse> UpdateAsync(Guid id,MenuItemRequest request);
        Task<MenuItemResponse> DeleteAsync(Guid id);
        Task<IEnumerable<MenuItemResponse>> GetAllMenuItemByMerchantIdAsync(Guid id);
        Task<IEnumerable<MenuItemResponse>> GetAllMenuItemByCategoryIdAsync(Guid id);
        Task<MenuItemResponse> GetMenuItemByIdAsync(Guid id);
    }
}
