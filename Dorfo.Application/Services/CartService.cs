using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IRedisCartService _redis;

        public CartService(IRedisCartService redis)
        {
            _redis = redis;
        }

        private CartRequest MapToDto(Cart cart)
        {
            return new CartRequest
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                MerchantId = cart.MerchantId,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                Items = cart.Items.Select(i => new CartItemRequest
                {
                    CartItemId = i.CartItemId,
                    MenuItemId = i.MenuItemId,
                    Quantity = i.Quantity,
                    PriceAtAdd = i.PriceAtAdd,
                    OptionsJson = i.OptionsJson,
                    ScheduledFor = i.ScheduledFor
                }).ToList()
            };
        }

        public async Task<CartRequest> AddItemsToCartAsync(Guid userId, Guid merchantId, AddCartItemsRequest request)
        {
            var cart = await _redis.GetCartAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    CartId = Guid.NewGuid(),
                    UserId = userId,
                    MerchantId = merchantId,
                    CreatedAt = DateTime.UtcNow,
                    Items = new List<CartItem>()
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
                    cart.Items.Add(new CartItem
                    {
                        CartItemId = Guid.NewGuid(),
                        CartId = cart.CartId,
                        MenuItemId = dto.MenuItemId,
                        Quantity = dto.Quantity,
                        PriceAtAdd = dto.PriceAtAdd,
                        OptionsJson = dto.OptionsJson,
                        ScheduledFor = dto.ScheduledFor
                    });
                }
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _redis.SaveCartAsync(cart);

            return MapToDto(cart);
        }


        public async Task<CartRequest?> GetCartAsync(Guid userId)
        {
            var cart = await _redis.GetCartAsync(userId);
            return cart == null ? null : MapToDto(cart);
        }


        public async Task<CartRequest?> RemoveItemAsync(Guid userId, Guid cartItemId)
        {
            var cart = await _redis.GetCartAsync(userId);
            if (cart == null) return null;

            cart.Items = cart.Items.Where(i => i.CartItemId != cartItemId).ToList();
            cart.UpdatedAt = DateTime.UtcNow;

            await _redis.SaveCartAsync(cart);
            return MapToDto(cart);
        }

        public async Task DeleteCartAsync(Guid userId)
        {
            await _redis.RemoveCartAsync(userId);
        }
    }
}

