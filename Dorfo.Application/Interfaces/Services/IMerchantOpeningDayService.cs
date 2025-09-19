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
    public interface IMerchantOpeningDayService
    {
        Task<MerchantOpeningDayResponse> CreateAsync(MerchantOpeningDayRequest request);
        Task<MerchantOpeningDayResponse> UpdateAsync(Guid id,MerchantOpeningDayRequest request);
        Task<MerchantOpeningDayResponse> RemoveAsync(Guid id);
        Task<IEnumerable<MerchantOpeningDayResponse>> GetAllOpeningDayAsyncByMerchantId(Guid id);
        Task<MerchantOpeningDayResponse> GetByIdAsync(Guid id);
    }
}
