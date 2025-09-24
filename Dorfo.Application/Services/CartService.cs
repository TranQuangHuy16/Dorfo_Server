using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Exceptions;
using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;

namespace Dorfo.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IRedisCartService _redis;
        private readonly IUnitOfWork _unitOfWork;
        public CartService(IRedisCartService redis, IUnitOfWork unitOfWork)
        {
            _redis = redis;
            _unitOfWork = unitOfWork;
        }

        public async Task<CartResponse> AddItemsToCartAsync(AddCartItemsRequest request, Guid userId)
        {
            // Lấy cart của user theo merchant
            var cart = await _redis.GetCartByMerchantAsync(userId, request.MerchantId);
            var merchant = await _unitOfWork.MerchantRepository.GetMerchantByIdAsync(request.MerchantId);
            var menuItems = new MenuItem();
            foreach (var item in merchant.MenuItems)
            {
                foreach (var dto in request.Items)
                {
                    if (item.MenuItemId == dto.MenuItemId)
                    {
                        menuItems = item;
                    }
                }
            }
            if (merchant == null)
            {
                throw new NotFoundException($"Merchant with id {request.MerchantId} not found");
            }

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
                // build options
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

                // chỉ tìm theo MenuItemId
                var existingItem = cart.Items.FirstOrDefault(i => i.MenuItemId == dto.MenuItemId);

                if (existingItem != null)
                {
                    // đã có cartItem -> update
                    existingItem.Quantity += dto.Quantity;

                    // gộp options
                    foreach (var opt in optionResponses)
                    {
                        var existingOption = existingItem.Options.FirstOrDefault(o => o.OptionId == opt.OptionId);
                        if (existingOption != null)
                        {
                            // gộp selected values (chỉ thêm nếu chưa tồn tại)
                            foreach (var val in opt.SelectedValues)
                            {
                                if (!existingOption.SelectedValues.Any(v => v.OptionValueId == val.OptionValueId))
                                {
                                    existingOption.SelectedValues.Add(val);
                                }
                            }
                        }
                        else
                        {
                            existingItem.Options.Add(opt);
                        }
                    }
                }
                else
                {
                    // chưa có cartItem cho MenuItemId này -> tạo mới
                    var menuItem = merchant.MenuItems.First(x => x.MenuItemId == dto.MenuItemId);

                    cart.Items.Add(new CartItemResponse
                    {
                        CartItemId = Guid.NewGuid(),
                        MenuItemId = dto.MenuItemId,
                        Quantity = dto.Quantity,
                        MenuItemName = menuItem.Name,
                        PriceAtAdd = menuItem.Price,
                        Options = optionResponses
                    });
                }
            }



            cart.UpdatedAt = DateTime.UtcNow;

            // Cập nhật phí/giảm giá
            cart.DeliveryFee = request.DeliveryFee;
            cart.ServiceFee = request.ServiceFee;
            cart.Discount = request.Discount;

            // Tính tổng (bao gồm cả PriceDelta từ option values)
            cart.SubTotal = cart.Items.Sum(i =>
                (i.PriceAtAdd + i.Options.SelectMany(o => o.SelectedValues).Sum(v => v.PriceDelta)) * i.Quantity
            );

            cart.TotalAmount = cart.SubTotal + cart.DeliveryFee + cart.ServiceFee - cart.Discount;

            await _redis.SaveCartAsync(cart);
            return cart;
        }

        public async Task<List<CartResponse>> GetAllCartsAsync(Guid userId)
        {
            return await _redis.GetCartsAsync(userId);
        }

        public async Task<CartResponse?> GetCartByMerchantAsync(Guid userId, Guid merchantId)
        {
            return await _redis.GetCartByMerchantAsync(userId, merchantId);
        }

        public async Task<CartResponse?> RemoveItemAsync(Guid userId, Guid cartItemId, Guid merchantId)
        {
            var cart = await _redis.GetCartByMerchantAsync(userId, merchantId);
            if (cart == null) return null;

            cart.Items = cart.Items.Where(i => i.CartItemId != cartItemId).ToList();
            cart.UpdatedAt = DateTime.UtcNow;

            if (!cart.Items.Any())
            {
                // nếu cart rỗng thì xóa luôn cart
                await _redis.RemoveCartAsync(userId, merchantId);
                return null;
            }

            cart.SubTotal = cart.Items.Sum(i => (i.PriceAtAdd + i.Options.SelectMany(o => o.SelectedValues).Sum(v => v.PriceDelta)) * i.Quantity);
            cart.TotalAmount = cart.SubTotal + cart.DeliveryFee + cart.ServiceFee - cart.Discount;

            await _redis.SaveCartAsync(cart);
            return cart;
        }

        public async Task<CartResponse?> RemoveSelectedValueIdsAsync(Guid userId, Guid merchantId, Guid cartItemId, Guid optionId, List<Guid> selectedValueIds)
        {
            var cart = await _redis.GetCartByMerchantAsync(userId, merchantId);
            if (cart == null) return null;

            var cartItem = cart.Items.FirstOrDefault(i => i.CartItemId == cartItemId);
            if (cartItem == null) return cart;

            var option = cartItem.Options.FirstOrDefault(o => o.OptionId == optionId);
            if (option == null) return cart;

            // Remove các SelectedValueIds
            option.SelectedValues = option.SelectedValues
                .Where(v => !selectedValueIds.Contains(v.OptionValueId))
                .ToList();

            // Nếu SelectedValues rỗng → remove luôn option
            if (option.SelectedValues == null || !option.SelectedValues.Any())
            {
                cartItem.Options = cartItem.Options
                    .Where(o => o.OptionId != optionId)
                    .ToList();
            }

            // Update lại totals
            cart.UpdatedAt = DateTime.UtcNow;
            cart.SubTotal = cart.Items.Sum(i =>
                (i.PriceAtAdd + i.Options.SelectMany(o => o.SelectedValues).Sum(v => v.PriceDelta)) * i.Quantity
            );
            cart.TotalAmount = cart.SubTotal + cart.DeliveryFee + cart.ServiceFee - cart.Discount;

            await _redis.SaveCartAsync(cart);
            return cart;
        }


        public async Task RemoveCartByMerchantAsync(Guid userId, Guid merchantId)
        {
            await _redis.RemoveCartAsync(userId, merchantId);
        }

        public async Task DeleteAllCartsAsync(Guid userId)
        {
            await _redis.DeleteAllCartsAsync(userId);
        }

        public async Task<CartResponse?> UpdateItemQuantityAsync(Guid userId, Guid merchantId, Guid cartItemId, int newQuantity)
        {
            var cart = await _redis.GetCartByMerchantAsync(userId, merchantId);
            if (cart == null) return null;

            var cartItem = cart.Items.FirstOrDefault(i => i.CartItemId == cartItemId);
            if (cartItem == null) return cart;

            if (newQuantity <= 0)
            {
                // nếu newQuantity <= 0 thì remove luôn
                cart.Items = cart.Items.Where(i => i.CartItemId != cartItemId).ToList();
            }
            else
            {
                cartItem.Quantity = newQuantity;
            }

            // Update lại totals
            cart.UpdatedAt = DateTime.UtcNow;
            cart.SubTotal = cart.Items.Sum(i =>
                (i.PriceAtAdd + i.Options.SelectMany(o => o.SelectedValues).Sum(v => v.PriceDelta)) * i.Quantity
            );
            cart.TotalAmount = cart.SubTotal + cart.DeliveryFee + cart.ServiceFee - cart.Discount;

            await _redis.SaveCartAsync(cart);
            return cart;
        }

        public async Task<CartResponse?> AddOptionValueAsync(Guid userId, Guid merchantId, Guid cartItemId, Guid optionId, Guid optionValueId)
        {
            var cart = await _redis.GetCartByMerchantAsync(userId, merchantId);
            if (cart == null) return null;

            var cartItem = cart.Items.FirstOrDefault(i => i.CartItemId == cartItemId);
            if (cartItem == null) return cart;

            var option = cartItem.Options.FirstOrDefault(o => o.OptionId == optionId);
            if (option == null)
            {
                var optionEntity = await _unitOfWork.MenuItemOptionRepository.GetByIdAsync(optionId);
                if (optionEntity == null) return cart;

                var valueEntity = await _unitOfWork.MenuItemOptionValueRepository.GetByIdAsync(optionValueId);
                if (valueEntity == null) return cart;

                cartItem.Options.Add(new CartItemOptionResponse
                {
                    OptionId = optionEntity.OptionId,
                    OptionName = optionEntity.OptionName,
                    SelectedValues = new List<CartItemOptionValueResponse>
            {
                new CartItemOptionValueResponse
                {
                    OptionValueId = valueEntity.OptionValueId,
                    ValueName = valueEntity.ValueName,
                    PriceDelta = valueEntity.PriceDelta
                }
            }
                });
            }
            else
            {
                if (!option.SelectedValues.Any(v => v.OptionValueId == optionValueId))
                {
                    var valueEntity = await _unitOfWork.MenuItemOptionValueRepository.GetByIdAsync(optionValueId);
                    if (valueEntity != null)
                    {
                        option.SelectedValues.Add(new CartItemOptionValueResponse
                        {
                            OptionValueId = valueEntity.OptionValueId,
                            ValueName = valueEntity.ValueName,
                            PriceDelta = valueEntity.PriceDelta
                        });
                    }
                }
            }

            // Update totals
            cart.UpdatedAt = DateTime.UtcNow;
            cart.SubTotal = cart.Items.Sum(i =>
                (i.PriceAtAdd + i.Options.SelectMany(o => o.SelectedValues).Sum(v => v.PriceDelta)) * i.Quantity
            );
            cart.TotalAmount = cart.SubTotal + cart.DeliveryFee + cart.ServiceFee - cart.Discount;

            await _redis.SaveCartAsync(cart);
            return cart;
        }

        public async Task<CartResponse?> UpdateSelectedValuesAsync(Guid userId, Guid merchantId, Guid cartItemId, Guid optionId, List<Guid> newSelectedValueIds)
        {
            var cart = await _redis.GetCartByMerchantAsync(userId, merchantId);
            if (cart == null) return null;

            var cartItem = cart.Items.FirstOrDefault(i => i.CartItemId == cartItemId);
            if (cartItem == null) return cart;

            // Lấy Option nếu có
            var option = cartItem.Options.FirstOrDefault(o => o.OptionId == optionId);

            // Lấy danh sách value mới
            var valueEntities = await _unitOfWork.MenuItemOptionValueRepository.GetByIdsAsync(newSelectedValueIds);

            if (option == null)
            {
                // Nếu chưa có option -> tạo mới
                var optionEntity = await _unitOfWork.MenuItemOptionRepository.GetByIdAsync(optionId);
                if (optionEntity == null) return cart;

                option = new CartItemOptionResponse
                {
                    OptionId = optionEntity.OptionId,
                    OptionName = optionEntity.OptionName,
                    SelectedValues = new List<CartItemOptionValueResponse>()
                };

                cartItem.Options.Add(option);
            }

            // Gán lại danh sách SelectedValues
            option.SelectedValues = valueEntities.Select(v => new CartItemOptionValueResponse
            {
                OptionValueId = v.OptionValueId,
                ValueName = v.ValueName,
                PriceDelta = v.PriceDelta
            }).ToList();

            // Update totals
            cart.UpdatedAt = DateTime.UtcNow;
            cart.SubTotal = cart.Items.Sum(i =>
                (i.PriceAtAdd + i.Options.SelectMany(o => o.SelectedValues).Sum(v => v.PriceDelta)) * i.Quantity
            );
            cart.TotalAmount = cart.SubTotal + cart.DeliveryFee + cart.ServiceFee - cart.Discount;

            await _redis.SaveCartAsync(cart);
            return cart;
        }

    }


}
