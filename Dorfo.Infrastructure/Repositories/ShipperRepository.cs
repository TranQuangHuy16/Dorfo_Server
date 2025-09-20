using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Domain.Entities;
using Dorfo.Infrastructure.Persistence;
using Dorfo.Infrastructure.Repositories.Basic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Repositories
{
    public class ShipperRepository : GenericRepository<Shipper>, IShipperRepository
    {
        public ShipperRepository() { }

        public ShipperRepository(DorfoDbContext context) => _context = context;

        public async Task<IEnumerable<Shipper>> GetAllShipperByMerchantIdAsync(Guid id)
        {
            return await _context.Shippers.Where(m => m.MerchantId == id && m.IsActive == true).ToListAsync();
        }

        public async Task<Shipper> GetShipperByIdAsync(Guid id)
        {
            return await _context.Shippers.FirstOrDefaultAsync(m => m.ShipperId == id && m.IsActive == true);
        }
    }
}
