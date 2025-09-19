using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IMenuCategoryService
    {
        Task<MenuCategoryResponse> CreateAsync(MenuCategoryRequest request);
        Task<MenuCategoryResponse> UpdateAsync(Guid id ,MenuCategoryRequest request);
        Task<MenuCategoryResponse> DeleteAsync(Guid id);
        Task<IEnumerable<MenuCategoryResponse>> GetAllCategoryByMerchantIdAsync(Guid id);
        Task<MenuCategoryResponse> GetCategoryByIdAsync(Guid id);
    }
}
