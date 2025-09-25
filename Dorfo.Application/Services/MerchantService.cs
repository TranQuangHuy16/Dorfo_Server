using AutoMapper;
using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MerchantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MerchantResponse> CreateAsync(MerchantRequest merchantRequest)
        {
            var merchant = _mapper.Map<Merchant>(merchantRequest);
            merchant.IsActive = true;
            await _unitOfWork.MerchantRepository.CreateAsync(merchant);
            return _mapper.Map<MerchantResponse>(merchant);
        }

        public async Task<MerchantResponse> DeleteAsync(Guid id)
        {
            var merchant = await _unitOfWork.MerchantRepository.GetMerchantByIdAsync(id);
            if (merchant == null)
            {
                return null;
            }
            await _unitOfWork.MerchantRepository.DeleteAsync(id);
            return _mapper.Map<MerchantResponse>(merchant);

        }

        public async Task<IEnumerable<MerchantResponse>> GetAllAsync()
        {
            var merchants = await _unitOfWork.MerchantRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MerchantResponse>>(merchants);
        }

        public async Task<MerchantResponse> GetMerchantByIdAsync(Guid id)
        {
            var merchant = await _unitOfWork.MerchantRepository.GetMerchantByIdAsync(id);
            return _mapper.Map<MerchantResponse>(merchant);
        }

        public async Task<IEnumerable<MerchantResponse>> GetMerchantByOwnerIdAsync(Guid id)
        {
            var merchants = await _unitOfWork.MerchantRepository.GetMerchantByOwnerIdAsync(id);
            return _mapper.Map<IEnumerable<MerchantResponse>>(merchants);
        }

        public async Task<MerchantResponse> UpdateAsync(Guid id, MerchantRequest merchantRequest)
        {
            var merchant = await _unitOfWork.MerchantRepository.GetMerchantByIdAsync(id);
            if(merchant == null)
            {
                return null;
            }

            _mapper.Map(merchantRequest, merchant);

            await _unitOfWork.MerchantRepository.UpdateAsync(merchant);

            return _mapper.Map<MerchantResponse>(merchant);

        }
    }
}
