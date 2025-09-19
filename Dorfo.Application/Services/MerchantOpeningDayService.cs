using AutoMapper;
using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Exceptions;
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
    public class MerchantOpeningDayService : IMerchantOpeningDayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MerchantOpeningDayService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MerchantOpeningDayResponse> CreateAsync(MerchantOpeningDayRequest request)
        {
            var existing = await _unitOfWork.MerchantOpeningDayRepository
                                     .GetAllOpeningDayAsyncByMerchantId(request.MerchantId);

            var dayConflict = existing.Any(d => d.DayOfWeek == request.DayOfWeek);

            if (dayConflict)
                throw new DuplicateFieldException("DayOfWeek", request.DayOfWeek.ToString());
            var merchantOpeningDay = _mapper.Map<MerchantOpeningDay>(request);
            await _unitOfWork.MerchantOpeningDayRepository.CreateAsync(merchantOpeningDay);
            return _mapper.Map<MerchantOpeningDayResponse>(merchantOpeningDay);
        }

        public async Task<IEnumerable<MerchantOpeningDayResponse>> GetAllOpeningDayAsyncByMerchantId(Guid id)
        {
            var result = await _unitOfWork.MerchantOpeningDayRepository.GetAllOpeningDayAsyncByMerchantId(id);
            return _mapper.Map<IEnumerable<MerchantOpeningDayResponse>>(result);
        }

        public async Task<MerchantOpeningDayResponse> GetByIdAsync(Guid id)
        {
            var result = await _unitOfWork.MerchantOpeningDayRepository.GetByIdAsync(id);
            return _mapper.Map<MerchantOpeningDayResponse>(result);
        }

        public async Task<MerchantOpeningDayResponse> RemoveAsync(Guid id)
        {
            var merchantOpeningDay = await _unitOfWork.MerchantOpeningDayRepository.GetByIdAsync(id);
            if (merchantOpeningDay == null) return null;
            await _unitOfWork.MerchantOpeningDayRepository.RemoveAsync(merchantOpeningDay);
            return _mapper.Map<MerchantOpeningDayResponse>(merchantOpeningDay);
        }

        public async Task<MerchantOpeningDayResponse> UpdateAsync(Guid id, MerchantOpeningDayRequest request)
        {
            var merchantOpeningDay = await _unitOfWork.MerchantOpeningDayRepository.GetByIdAsync(id);
            if (merchantOpeningDay == null) return null;
            _mapper.Map(request, merchantOpeningDay);
            await _unitOfWork.MerchantOpeningDayRepository.UpdateAsync(merchantOpeningDay);
            return _mapper.Map<MerchantOpeningDayResponse>(merchantOpeningDay);
        }
    }
}
