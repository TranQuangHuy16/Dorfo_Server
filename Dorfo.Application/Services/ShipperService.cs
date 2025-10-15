using AutoMapper;
using Dorfo.Application.Exceptions;
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
    public class ShipperService : IShipperService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShipperService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ShipperResponse> CreateAsync(ShipperRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.UserId);
            user.Role = Domain.Enums.UserRoleEnum.SHIPPER;
            await _unitOfWork.UserRepository.UpdateAsync(user);
            var shipper = _mapper.Map<Shipper>(request);
            await _unitOfWork.ShipperRepository.CreateAsync(shipper);
            return _mapper.Map<ShipperResponse>(request);
        }

        public async Task<ShipperResponse> DeleteAsync(Guid id)
        {
            var shipper = await _unitOfWork.ShipperRepository.GetShipperByIdAsync(id);
            if (shipper == null) throw new NotFoundException("Not Found Shipper");
            shipper.IsActive = true;
            await _unitOfWork.ShipperRepository.UpdateAsync(shipper);
            return _mapper.Map<ShipperResponse>(shipper);
        }

        public async Task<IEnumerable<ShipperResponse>> GetAllShipperByMerchantIdAsync(Guid id)
        {
            var shippers = await _unitOfWork.ShipperRepository.GetAllShipperByMerchantIdAsync(id);
            if (shippers == null) throw new NotFoundException("Not Found Shipper");
            return _mapper.Map<IEnumerable<ShipperResponse>>(shippers);
        }

        public async Task<ShipperResponse> GetShipperByIdAsync(Guid id)
        {
            var shipper = await _unitOfWork.ShipperRepository.GetShipperByIdAsync(id);
            if (shipper == null) throw new NotFoundException("Not Found Shipper");
            return _mapper.Map<ShipperResponse>(shipper);
        }

        public async Task<ShipperResponse> UpdateAsync(Guid id, ShipperRequest request)
        {
            var shipper = await _unitOfWork.ShipperRepository.GetShipperByIdAsync(id);
            if (shipper == null) throw new NotFoundException("Not Found Shipper");
            await _unitOfWork.ShipperRepository.UpdateAsync(shipper);
            return _mapper.Map<ShipperResponse>(shipper);
        }
    }
}
