using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IMerchantCategoryRepository
    {
        Task<int> CreateAsync(MerchantCategory merchantCategory);
        Task<int> UpdateAsync(MerchantCategory merchantCategory);
        Task<IEnumerable<MerchantCategory>> GetAllMerchantCategoryAsync();
        Task<MerchantCategory> GetMerchantCategoryByIdAsync(int id);
    }
}
