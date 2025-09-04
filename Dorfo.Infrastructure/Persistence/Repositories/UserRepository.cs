using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Domain.Entities;
using DrugPrevention.Repositories.HuyTQ.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository() { }

        public UserRepository(DorfoDbContext context) => _context = context;

        public async Task<int> UpdateAsync(User user)
        {
            return await base.UpdateAsync(user);
        }
        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await base.GetByIdAsync(userId);
        }

    }
}
