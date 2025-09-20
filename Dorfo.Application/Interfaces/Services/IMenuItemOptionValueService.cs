using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IMenuItemOptionValueService
    {
        Task<MenuItemOptionValueResponse> CreateAsync(MenuItemOptionValueRequest request);
        Task<MenuItemOptionValueResponse> UpdateAsync(Guid id, MenuItemOptionValueRequest request);
        Task<MenuItemOptionValueResponse> DeleteAsync(Guid id);
        Task<IEnumerable<MenuItemOptionValueResponse>> GetAllMenuItemOptionValueByOptionIdAsync(Guid id);
        Task<MenuItemOptionValueResponse> GetMenuItemOptionValueByIdAsync(Guid id);
    }
}
