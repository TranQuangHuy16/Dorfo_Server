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
    public class MenuCategoryService : IMenuCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<MenuCategoryResponse> CreateAsync(MenuCategoryRequest request)
        {
            var category = _mapper.Map<MenuCategory>(request);
            category.IsActive = true;
            await _unitOfWork.MenuCategoryRepository.CreateAsync(category);
            return _mapper.Map<MenuCategoryResponse>(category);
        }

        public async Task<MenuCategoryResponse> DeleteAsync(Guid id)
        {
            var category = await _unitOfWork.MenuCategoryRepository.GetCategoryByIdAsync(id);
            if (category == null) throw new NotFoundException("Not Found Category");
            category.IsActive = false;
            await _unitOfWork.MenuCategoryRepository.UpdateAsync(category);
            return _mapper.Map<MenuCategoryResponse>(category);
        }

        public async Task<IEnumerable<MenuCategoryResponse>> GetAllCategoryByMerchantIdAsync(Guid id)
        {
            var categories = await _unitOfWork.MenuCategoryRepository.GetAllCategoryByMerchantIdAsync(id);
            if (categories == null || !categories.Any()) throw new NotFoundException("Not Found Category");
            return _mapper.Map<IEnumerable<MenuCategoryResponse>>(categories);
        }

        public async Task<MenuCategoryResponse> GetCategoryByIdAsync(Guid id)
        {
            var category = await _unitOfWork.MenuCategoryRepository.GetCategoryByIdAsync(id);
            if(category == null) throw new NotFoundException("Not Found Category");
            return _mapper.Map<MenuCategoryResponse>(category);
        }

        public async Task<MenuCategoryResponse> UpdateAsync(Guid id, MenuCategoryRequest request)
        {
            var category = await _unitOfWork.MenuCategoryRepository.GetCategoryByIdAsync(id);
            if (category == null) throw new NotFoundException("Not Found Category");
            _mapper.Map(request, category);
            await _unitOfWork.MenuCategoryRepository.UpdateAsync(category);
            return _mapper.Map<MenuCategoryResponse>(category);
        }
    }
}
