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
    public class MenuItemOptionService : IMenuItemOptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuItemOptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MenuItemOptionResponse> CreateAsync(MenuItemOptionRequest request)
        {
            var option = _mapper.Map<MenuItemOption>(request);
            await _unitOfWork.MenuItemOptionRepository.CreateAsync(option);
            return _mapper.Map<MenuItemOptionResponse>(option);
        }

        public async Task<MenuItemOptionResponse> DeleteAsync(Guid id)
        {
            var option = await _unitOfWork.MenuItemOptionRepository.GetMenuItemOptionByIdAsync(id);
            if (option == null) throw new NotFoundException("Not found MenuItemOption");
            option.IsActive = false;
            await _unitOfWork.MenuItemOptionRepository.UpdateAsync(option);
            return _mapper.Map<MenuItemOptionResponse>(option);

        }

        public async Task<IEnumerable<MenuItemOptionResponse>> GetAllMenuItemOptionByMenuItemIdAsync(Guid id)
        {
            var options = await _unitOfWork.MenuItemOptionRepository.GetAllMenuItemOptionByMenuItemIdAsync(id);
            if (options == null || !options.Any()) throw new NotFoundException("Not found MenuItemOption");
            return _mapper.Map<IEnumerable<MenuItemOptionResponse>>(options);
        }

        public async Task<MenuItemOptionResponse> GetMenuItemOptionByIdAsync(Guid id)
        {
            var option = await _unitOfWork.MenuItemOptionRepository.GetMenuItemOptionByIdAsync(id);
            if (option == null) throw new NotFoundException("Not found MenuItemOption");
            return _mapper.Map<MenuItemOptionResponse>(option);
        }

        public async Task<MenuItemOptionResponse> UpdateAsync(Guid id, MenuItemOptionRequest request)
        {
            var option = await _unitOfWork.MenuItemOptionRepository.GetMenuItemOptionByIdAsync(id);
            if (option == null) throw new NotFoundException("Not found MenuItemOption");
            await _unitOfWork.MenuItemOptionRepository.UpdateAsync(option);
            return _mapper.Map<MenuItemOptionResponse>(option);
        }
    }
}
