using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AddressResponse> CreateAddressAsync(Guid userId, AddressRequest addressRequest)
        {
            var address = new Address
            {
                AddressId = Guid.NewGuid(),
                UserId = userId,
                AddressLabel = addressRequest.AddressLabel,
                Street = addressRequest.Street,
                Ward = addressRequest.Ward,
                District = addressRequest.District,
                City = addressRequest.City,
                Country = addressRequest.Country,
                IsDefault = addressRequest.IsDefault
            };

            await _unitOfWork.AddressRepository.CreateAddressAsync(address);

            return MapToResponse(address);
        }

        public async Task<AddressResponse?> UpdateAddressAsync(Guid userId, Guid addressId, AddressRequest addressRequest)
        {
            var address = await _unitOfWork.AddressRepository.GetByIdAsync(addressId);
            if (address == null || address.UserId != userId || address.IsActive == false)
            {
                return null;
            }

            address.AddressLabel = addressRequest.AddressLabel;
            address.Street = addressRequest.Street;
            address.Ward = addressRequest.Ward;
            address.District = addressRequest.District;
            address.City = addressRequest.City;
            address.Country = addressRequest.Country;

            if (addressRequest.IsDefault == false)
            {
                address.IsDefault = addressRequest.IsDefault;
            }
            else
            {
                address.IsDefault = addressRequest.IsDefault;
                var addresses = await _unitOfWork.AddressRepository.GetAllAddressByUserAsync(userId);
                foreach (var addr in addresses)
                {
                    if (addr.IsDefault && addr.AddressId != address.AddressId)
                    {
                        addr.IsDefault = false;
                        await _unitOfWork.AddressRepository.UpdateAddressAsync(addr);
                    }
                }
            }


            await _unitOfWork.AddressRepository.UpdateAddressAsync(address);

            return MapToResponse(address);
        }

        public async Task<bool> RemoveAddressAsync(Guid userId, Guid addressId)
        {
            var address = await _unitOfWork.AddressRepository.GetByIdAsync(addressId);
            if (address == null || address.UserId != userId || address.IsActive == false)
            {
                return false;
            }

            return await _unitOfWork.AddressRepository.RemoveAddressAsync(addressId);
        }

        public async Task<IEnumerable<AddressResponse>> GetAllAddressesByUserAsync(Guid userId)
        {
            var addresses = await _unitOfWork.AddressRepository.GetAllAddressByUserAsync(userId);
            return addresses.Select(MapToResponse).ToList();
        }

        private AddressResponse MapToResponse(Address address)
        {
            return new AddressResponse
            {
                AddressId = address.AddressId,
                AddressLabel = address.AddressLabel,
                Street = address.Street,
                Ward = address.Ward,
                District = address.District,
                City = address.City,
                Country = address.Country,
                IsDefault = address.IsDefault,
                CreatedAt = address.CreatedAt
            };
        }
    }
}
