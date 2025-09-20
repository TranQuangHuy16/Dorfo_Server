using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IShipperService
    {
        Task<ShipperResponse> CreateAsync(ShipperRequest request);
        Task<ShipperResponse> UpdateAsync(Guid id, ShipperRequest request);
        Task<ShipperResponse> DeleteAsync(Guid id);
        Task<IEnumerable<ShipperResponse>> GetAllShipperByMerchantIdAsync(Guid id);
        Task<ShipperResponse> GetShipperByIdAsync(Guid id);
    }
}
