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
    public class MenuItemService : IMenuItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MenuItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<MenuItemResponse> CreateAsync(MenuItemRequest request)
        {
            var menuItem = _mapper.Map<MenuItem>(request);
            menuItem.IsActive = true;
            await _unitOfWork.MenuItemRepository.CreateAsync(menuItem);
            return _mapper.Map<MenuItemResponse>(menuItem);
        }

        public async Task<MenuItemResponse> DeleteAsync(Guid id)
        {
            var menuItem = await _unitOfWork.MenuItemRepository.GetMenuItemByIdAsync(id);
            if (menuItem == null) throw new NotFoundException("Not Found MenuItem");
            menuItem.IsActive = false;
            await _unitOfWork.MenuItemRepository.UpdateAsync(menuItem);
            return _mapper.Map<MenuItemResponse>(menuItem);
        }

        public async Task<IEnumerable<MenuItemResponse>> GetAllMenuItemByCategoryIdAsync(Guid id)
        {
            var menuItems = await _unitOfWork.MenuItemRepository.GetAllMenuItemByCategoryIdAsync(id);
            if (menuItems == null || !menuItems.Any()) throw new NotFoundException("Not Found MenuItem");
            return _mapper.Map<IEnumerable<MenuItemResponse>>(menuItems);
        }

        public async Task<IEnumerable<MenuItemResponse>> GetAllMenuItemByMerchantIdAsync(Guid id)
        {
            var menuItems = await _unitOfWork.MenuItemRepository.GetAllMenuItemByMerchantIdAsync(id);
            if (menuItems == null || !menuItems.Any()) throw new NotFoundException("Not Found MenuItem");
            return _mapper.Map<IEnumerable<MenuItemResponse>>(menuItems);
        }

        public async Task<MenuItemResponse> GetMenuItemByIdAsync(Guid id)
        {
            var menuItem = await _unitOfWork.MenuItemRepository.GetMenuItemByIdAsync(id);
            if (menuItem == null) throw new NotFoundException("Not Found MenuItem");
            return _mapper.Map<MenuItemResponse>(menuItem);
        }

        public async Task<MenuItemResponse> UpdateAsync(Guid id,MenuItemRequest request)
        {
            var menuItem = await _unitOfWork.MenuItemRepository.GetMenuItemByIdAsync(id);
            if (menuItem == null) throw new NotFoundException("Not Found MenuItem");
            _mapper.Map(request, menuItem);
            await _unitOfWork.MenuItemRepository.UpdateAsync(menuItem);
            return _mapper.Map<MenuItemResponse>(menuItem);
        }
    }
}
