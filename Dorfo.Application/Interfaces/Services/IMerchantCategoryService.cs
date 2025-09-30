using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IMerchantCategoryService
    {
        Task<MerchantCategoryResponse> CreateAsync(MerchantCategoryRequest request);
        Task<MerchantCategoryResponse> UpdateAsync(int id, MerchantCategoryRequest request);
        Task<MerchantCategoryResponse> DeleteAsynce(int id);
        Task<IEnumerable<MerchantCategoryResponse>> GetAllMerchantCategoryAsync();
        Task<MerchantCategoryResponse> GetMerchantCategoryByIdAsync(int id);
    }
}
