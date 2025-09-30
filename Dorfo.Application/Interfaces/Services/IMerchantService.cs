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
    public interface IMerchantService
    {
        Task<MerchantResponse> CreateAsync(MerchantRequest merchantRequest);
        Task<MerchantResponse> UpdateAsync(Guid id, MerchantRequest merchantRequest);
        Task<MerchantResponse> DeleteAsync(Guid id);
        Task<IEnumerable<MerchantResponse>> GetAllAsync();
        Task<MerchantResponse> GetMerchantByIdAsync(Guid id);
        Task<IEnumerable<MerchantResponse>> GetMerchantByOwnerIdAsync(Guid id);
        Task<IEnumerable<MerchantResponse>> GetAllMerchantByMerchantCategoryIdAsync(int id);
    }
}
