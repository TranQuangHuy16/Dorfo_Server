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
    public class MenuItemOptionValueService : IMenuItemOptionValueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuItemOptionValueService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MenuItemOptionValueResponse> CreateAsync(MenuItemOptionValueRequest request)
        {
            var value = _mapper.Map<MenuItemOptionValue>(request);
            await _unitOfWork.MenuItemOptionValueRepository.CreateAsync(value);
            return _mapper.Map<MenuItemOptionValueResponse>(value);
        }

        public async Task<MenuItemOptionValueResponse> DeleteAsync(Guid id)
        {
            var value = await _unitOfWork.MenuItemOptionValueRepository.GetMenuItemOptionValueByIdAsync(id);
            if (value == null) throw new NotFoundException("Not Found Menu Item Option Value");
            value.IsActive = false;
            await _unitOfWork.MenuItemOptionValueRepository.UpdateAsync(value);
            return _mapper.Map<MenuItemOptionValueResponse>(value);
        }

        public async Task<IEnumerable<MenuItemOptionValueResponse>> GetAllMenuItemOptionValueByOptionIdAsync(Guid id)
        {
            var values = await _unitOfWork.MenuItemOptionValueRepository.GetAllMenuItemOptionValueByOptionIdAsync(id);
            if (values == null) throw new NotFoundException("Not Found Menu Item Option Value");
            return _mapper.Map<IEnumerable<MenuItemOptionValueResponse>>(values);
        }

        public async Task<MenuItemOptionValueResponse> GetMenuItemOptionValueByIdAsync(Guid id)
        {
            var value = await _unitOfWork.MenuItemOptionValueRepository.GetMenuItemOptionValueByIdAsync(id);
            if (value == null) throw new NotFoundException("Not Found Menu Item Option Value");
            return _mapper.Map<MenuItemOptionValueResponse>(value);
        }

        public async Task<MenuItemOptionValueResponse> UpdateAsync(Guid id, MenuItemOptionValueRequest request)
        {
            var value = await _unitOfWork.MenuItemOptionValueRepository.GetMenuItemOptionValueByIdAsync(id);
            if (value == null) throw new NotFoundException("Not Found Menu Item Option Value");
            await _unitOfWork.MenuItemOptionValueRepository.UpdateAsync(value);
            return _mapper.Map<MenuItemOptionValueResponse>(value);
        }
    }
}
