using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;

namespace Dorfo.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IRedisCartService _redis;
        private readonly IUnitOfWork _unitOfWork;

        public CartService(
                   IRedisCartService redis,
                   IUnitOfWork unitOfWork
               )
        {
            _redis = redis;
            _unitOfWork = unitOfWork;
        }

        public async Task<CartResponse> AddItemsToCartAsync(AddCartItemsRequest request, Guid userId)
        {
            var cart = await _redis.GetCartAsync(userId);
            var merchant = await _unitOfWork.MerchantRepository.GetMerchantByIdAsync(request.MerchantId);
            if (cart == null)
            {
                cart = new CartResponse
                {
                    CartId = Guid.NewGuid(),
                    UserId = userId,
                    Merchant = new MerchantInfo
                    {
                        MerchantId = merchant.MerchantId,
                        MerchantName = merchant.Name,
                    },
                    CreatedAt = DateTime.UtcNow,
                    Items = new List<CartItemResponse>()
                };
            }

            foreach (var dto in request.Items)
            {
                var existingItem = cart.Items.FirstOrDefault(i => i.MenuItemId == dto.MenuItemId);
                if (existingItem != null)
                {
                    existingItem.Quantity += dto.Quantity;
                }
                else
                {
                    var optionResponses = new List<CartItemOptionResponse>();

                    foreach (var opt in dto.Options)
                    {
                        var optionEntity = await _unitOfWork.MenuItemOptionRepository.GetByIdAsync(opt.OptionId);
                        if (optionEntity == null) continue;

                        var valueEntities = await _unitOfWork.MenuItemOptionValueRepository.GetByIdsAsync(opt.SelectedValueIds);

                        optionResponses.Add(new CartItemOptionResponse
                        {
                            OptionId = optionEntity.OptionId,
                            OptionName = optionEntity.OptionName,
                            SelectedValues = valueEntities.Select(v => new CartItemOptionValueResponse
                            {
                                OptionValueId = v.OptionValueId,
                                ValueName = v.ValueName,
                                PriceDelta = v.PriceDelta
                            }).ToList()
                        });
                    }

                    cart.Items.Add(new CartItemResponse
                    {
                        CartItemId = Guid.NewGuid(),
                        MenuItemId = dto.MenuItemId,
                        Quantity = dto.Quantity,
                        MenuItemName = dto.ItemName,
                        PriceAtAdd = dto.PriceAtAdd,
                        Options = optionResponses
                    });
                }
            }

            cart.UpdatedAt = DateTime.UtcNow;

            // phí/giảm giá
            cart.DeliveryFee = request.DeliveryFee;
            cart.ServiceFee = request.ServiceFee;
            cart.Discount = request.Discount;

            // tính toán lại tổng (bao gồm cả PriceDelta từ option values)
            cart.SubTotal = cart.Items.Sum(i =>
                (i.PriceAtAdd + i.Options.SelectMany(o => o.SelectedValues).Sum(v => v.PriceDelta))
                * i.Quantity
            );

            cart.TotalAmount = cart.SubTotal + cart.DeliveryFee + cart.ServiceFee - cart.Discount;

            await _redis.SaveCartAsync(cart);
            return cart;
        }


        public async Task<CartResponse?> GetCartAsync(Guid userId)
        {
            return await _redis.GetCartAsync(userId);
        }

        public async Task<CartResponse?> RemoveItemAsync(Guid userId, Guid cartItemId)
        {
            var cart = await _redis.GetCartAsync(userId);
            if (cart == null) return null;

            cart.Items = cart.Items.Where(i => i.CartItemId != cartItemId).ToList();
            cart.UpdatedAt = DateTime.UtcNow;

            // cập nhật lại tổng
            cart.SubTotal = cart.Items.Sum(i => i.PriceAtAdd * i.Quantity);
            cart.TotalAmount = cart.SubTotal + cart.DeliveryFee + cart.ServiceFee - cart.Discount;

            await _redis.SaveCartAsync(cart);
            return cart;
        }

        public async Task DeleteCartAsync(Guid userId)
        {
            await _redis.RemoveCartAsync(userId);
        }
    }

}