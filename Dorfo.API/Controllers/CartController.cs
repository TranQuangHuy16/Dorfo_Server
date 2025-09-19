using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.Exceptions;
using Dorfo.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("items")]
        [Authorize]
        public async Task<IActionResult> AddItemsToCart([FromBody] AddCartItemsRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            var cart = await _cartService.AddItemsToCartAsync(request, userId);
            return Ok(cart);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCarts()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            var carts = await _cartService.GetAllCartsAsync(userId);
            return Ok(carts);
        }

        [HttpGet("{merchantId}")]
        [Authorize]
        public async Task<IActionResult> GetCartByMerchant(Guid merchantId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            var cart = await _cartService.GetCartByMerchantAsync(userId, merchantId);
            return cart == null ? throw new NotFoundException("Not Found Cart") : Ok(cart);
        }

        [HttpDelete("items/{cartItemId}/{merchantId}")]
        [Authorize]
        public async Task<IActionResult> RemoveItem(Guid cartItemId, Guid merchantId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            var cart = await _cartService.RemoveItemAsync(userId, cartItemId, merchantId);
            return cart == null ? null : Ok(cart);
        }

        [HttpDelete("{merchantId}")]
        [Authorize]
        public async Task<IActionResult> RemoveCartByMerchant(Guid merchantId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            await _cartService.RemoveCartByMerchantAsync(userId, merchantId);
            return Ok(new { message = "Cart removed successfully" });
        }

        [HttpPost("{merchantId}/remove-selected-values")]
        public async Task<IActionResult> RemoveSelectedValues(Guid merchantId, [FromBody] RemoveSelectedValueRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");
            var cart = await _cartService.RemoveSelectedValueIdsAsync(
                userId,
                merchantId,
                request.CartItemId,
                request.OptionId,
                request.SelectedValueIds
            );

            if (cart == null) throw new NotFoundException("Not Found Cart");

            return Ok(cart);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteAllCarts()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            await _cartService.DeleteAllCartsAsync(userId);
            return Ok(new { message = "All carts deleted successfully" });
        }

        [HttpPut("{merchantId}/items/{cartItemId}/quantity")]
        [Authorize]
        public async Task<IActionResult> UpdateItemQuantity(Guid merchantId, Guid cartItemId, [FromBody] int newQuantity)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            var cart = await _cartService.UpdateItemQuantityAsync(userId, merchantId, cartItemId, newQuantity);
            return cart == null ? throw new NotFoundException("Not Found Cart") : Ok(cart);
        }

        [HttpPost("{merchantId}/items/{cartItemId}/options/{optionId}/add-value/{optionValueId}")]
        [Authorize]
        public async Task<IActionResult> AddOptionValue(Guid merchantId, Guid cartItemId, Guid optionId, Guid optionValueId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            var cart = await _cartService.AddOptionValueAsync(userId, merchantId, cartItemId, optionId, optionValueId);
            return cart == null ? throw new NotFoundException("Not Found Cart") : Ok(cart);
        }

        [HttpPut("{merchantId}/update-selected-values")]
        [Authorize]
        public async Task<IActionResult> UpdateSelectedValues(Guid merchantId, [FromBody] UpdateSelectedValueRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            var cart = await _cartService.UpdateSelectedValuesAsync(
                userId,
                merchantId,
                request.CartItemId,
                request.OptionId,
                request.SelectedValueIds
            );

            return cart == null ? NotFound(new { message = "Cart not found" }) : Ok(cart);
        }


    }
}
