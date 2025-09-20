using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IMenuItemOptionService
    {
        Task<MenuItemOptionResponse> CreateAsync(MenuItemOptionRequest request);
        Task<MenuItemOptionResponse> UpdateAsync(Guid id, MenuItemOptionRequest request);
        Task<MenuItemOptionResponse> DeleteAsync(Guid id);
        Task<IEnumerable<MenuItemOptionResponse>> GetAllMenuItemOptionByMenuItemIdAsync(Guid id);
        Task<MenuItemOptionResponse> GetMenuItemOptionByIdAsync(Guid id);
    }
}
