using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAllAddressByUserAsync(Guid userId);
        Task<Address> CreateAddressAsync(Address address);
        Task<Address> UpdateAddressAsync(Address address);
        Task<bool> RemoveAddressAsync(Guid id);
        Task<Address?> GetByIdAsync(Guid id);
    }
}
