using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IMerchantOpeningDayRepository
    {
        Task<int> CreateAsync(MerchantOpeningDay openingDay);
        Task<int> UpdateAsync(MerchantOpeningDay openingDay);
        Task<bool> RemoveAsync(MerchantOpeningDay openingDay);

        Task<IEnumerable<MerchantOpeningDay>> GetAllOpeningDayAsyncByMerchantId(Guid id);
        Task<MerchantOpeningDay> GetByIdAsync(Guid id);

    }
}
