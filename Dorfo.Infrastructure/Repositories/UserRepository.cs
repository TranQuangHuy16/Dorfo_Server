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
            return await GetByIdAsync(userId);
        }

        public async Task<User?> GetUserByPhoneAsync(string phone)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Phone == phone);
        }

        public async Task<User> CreateAsync(User newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<User?> GetUserByUsernameOrPhoneOrEmailAsync(string search)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Username == search || x.Phone == search || x.Email == search);
        }

        public Task<User?> GetUserByLogin(string username)
        {
            return _context.Users.FirstOrDefaultAsync(x => (x.Username == username || x.Phone == username) && x.IsActive == true);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByMerchantId(Guid merchantId)
        {
            var userId = await _context.Merchants
                .Where(m => m.MerchantId == merchantId)
                .Select(m => m.OwnerUserId)
                .FirstOrDefaultAsync();

            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<bool> DeleteFcmToken(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            user.FcmToken = null;

            return true;
        }
    }
}