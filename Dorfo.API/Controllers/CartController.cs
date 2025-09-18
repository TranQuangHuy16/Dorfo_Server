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
            return cart == null ? NotFound(new { message = "Cart not found" }) : Ok(cart);
        }

        [HttpDelete("items/{cartItemId}/{merchantId}")]
        [Authorize]
        public async Task<IActionResult> RemoveItem(Guid cartItemId, Guid merchantId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            var cart = await _cartService.RemoveItemAsync(userId, cartItemId, merchantId);
            return cart == null ? NotFound(new { message = "Cart not found" }) : Ok(cart);
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
    }
}
