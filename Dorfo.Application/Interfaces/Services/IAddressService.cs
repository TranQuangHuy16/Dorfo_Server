using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IAddressService
    {
        Task<AddressResponse> CreateAddressAsync(Guid userId, AddressRequest addressRequest);
        Task<bool> RemoveAddressAsync(Guid userId, Guid addressId);
        Task<AddressResponse?> UpdateAddressAsync(Guid userId, Guid addressId, AddressRequest addressRequest);
        Task<IEnumerable<AddressResponse>> GetAllAddressesByUserAsync(Guid userId);
    }
}
