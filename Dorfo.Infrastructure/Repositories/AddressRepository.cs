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
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository() { }

        public AddressRepository(DorfoDbContext context) => _context = context;

        public async Task<IEnumerable<Address>> GetAllAddressByUserAsync(Guid userId)
        {
            return await _context.Addresses.Where(a => a.UserId == userId && a.IsActive == true).ToListAsync();
        }

        public async Task<Address> GetByIdAsync(Guid id)
        {
            return await _context.Addresses.FirstOrDefaultAsync(a => a.AddressId == id && a.IsActive == true);
        }

        public async Task<Address?> CreateAddressAsync(Address address)
        {
            address.IsActive = true;
            address.CreatedAt = DateTime.UtcNow;
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Address> UpdateAddressAsync(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<bool> RemoveAddressAsync(Guid id)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.AddressId == id);
            if (address == null)
            {
                return false;
            }
            address.IsActive = false;
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
