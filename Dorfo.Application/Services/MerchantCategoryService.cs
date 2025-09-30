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
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Services
{
    public class MerchantCategoryService : IMerchantCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MerchantCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<MerchantCategoryResponse> CreateAsync(MerchantCategoryRequest request)
        {
            var category = _mapper.Map<MerchantCategory>(request);
            category.IsActive = true;
            await _unitOfWork.MerchantCategory.CreateAsync(category);
            return _mapper.Map<MerchantCategoryResponse>(category);

        }

        public async Task<MerchantCategoryResponse> DeleteAsynce(int id)
        {
            var category = await _unitOfWork.MerchantCategory.GetMerchantCategoryByIdAsync(id);
            if(category == null) throw new NotFoundException("Not Found Menu Category");
            category.IsActive = false;
            await _unitOfWork.MerchantCategory.UpdateAsync(category);
            return _mapper.Map<MerchantCategoryResponse>(category);
        }

        public async Task<IEnumerable<MerchantCategoryResponse>> GetAllMerchantCategoryAsync()
        {
            var categories = await _unitOfWork.MerchantCategory.GetAllMerchantCategoryAsync();
            if(categories == null || !categories.Any()) throw new NotFoundException("Not Found Menu Category");
            return _mapper.Map<IEnumerable<MerchantCategoryResponse>>(categories);
        }

        public async Task<MerchantCategoryResponse> GetMerchantCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.MerchantCategory.GetMerchantCategoryByIdAsync(id);
            if (category == null) throw new NotFoundException("Not Found Menu Category");
            return _mapper.Map<MerchantCategoryResponse>(category);
        }

        public async Task<MerchantCategoryResponse> UpdateAsync(int id, MerchantCategoryRequest request)
        {
            var category = await _unitOfWork.MerchantCategory.GetMerchantCategoryByIdAsync(id);
            if (category == null) throw new NotFoundException("Not Found Menu Category");
            _mapper.Map(request, category);
            await _unitOfWork.MerchantCategory.UpdateAsync(category);
            return _mapper.Map<MerchantCategoryResponse>(category);
        }
    }
}
