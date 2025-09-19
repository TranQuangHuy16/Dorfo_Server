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
    public class MerchantOpeningDayRepository : GenericRepository<MerchantOpeningDay>, IMerchantOpeningDayRepository
    {
        public MerchantOpeningDayRepository() { }

        public MerchantOpeningDayRepository(DorfoDbContext context) => _context = context;

        public async Task<IEnumerable<MerchantOpeningDay>> GetAllOpeningDayAsyncByMerchantId(Guid id)
        {
            return await _context.MerchantOpeningDays.Where(op => op.MerchantId == id).ToListAsync();
        }
    }
}
