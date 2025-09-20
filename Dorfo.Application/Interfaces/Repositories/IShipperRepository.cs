using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IShipperRepository
    {
        Task<int> CreateAsync(Shipper shipper);
        Task<int> UpdateAsync(Shipper shipper);
        Task<IEnumerable<Shipper>> GetAllShipperByMerchantIdAsync(Guid id);
        Task<Shipper> GetShipperByIdAsync(Guid id);
    }
}
