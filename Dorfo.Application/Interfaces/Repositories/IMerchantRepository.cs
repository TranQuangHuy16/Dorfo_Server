using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IMerchantRepository
    {
        Task<int> CreateAsync(Merchant merchant);
        Task<int> UpdateAsync(Merchant merchant);
        Task<bool> DeleteAsync(Guid id);
        Task <IEnumerable<Merchant>> GetAllAsync();
        Task <Merchant> GetMerchantByIdAsync(Guid id);
        Task <IEnumerable<Merchant>> GetMerchantByOwnerIdAsync(Guid id);
    }
}
