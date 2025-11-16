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
    public class FavoriteShopService : IFavoriteShopService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FavoriteShopService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FavoriteShop>> GetFavoriteShopsByCustomerAsync(Guid customerId)
        {
            return await _unitOfWork.FavoriteShopRepository.GetByCustomerIdAsync(customerId);
        }

        public async Task<FavoriteShop?> AddFavoriteShopAsync(Guid customerId, Guid merchantId)
        {
            // kiểm tra trùng
            var existing = await _unitOfWork.FavoriteShopRepository.GetByCustomerAndMerchantAsync(customerId, merchantId);
            if (existing != null)
                return null;

            var favorite = new FavoriteShop
            {
                FavoriteShopId = Guid.NewGuid(),
                CustomerId = customerId,
                MerchantId = merchantId,
                AddedAt = DateTime.UtcNow
            };

            await _unitOfWork.FavoriteShopRepository.AddAsync(favorite);
            await _unitOfWork.FavoriteShopRepository.SaveChangesAsync();

            return favorite;
        }

        public async Task<bool> RemoveFavoriteShopAsync(Guid customerId, Guid merchantId)
        {
            var existing = await _unitOfWork.FavoriteShopRepository
                .GetByCustomerAndMerchantAsync(customerId, merchantId);

            if (existing == null)
                return false;

            await _unitOfWork.FavoriteShopRepository.RemoveAsync(existing);
            await _unitOfWork.FavoriteShopRepository.SaveChangesAsync();

            return true;
        }

    }
}
