using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.Exceptions;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly IServiceProviders _serviceProviders;

        public CartController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        [HttpPost("/items/{merchantId}")]
        [Authorize]
        public async Task<IActionResult> AddItemsToCart(Guid merchantId, [FromBody] AddCartItemsRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid token");
            }

            var cart = await _serviceProviders.CartService.AddItemsToCartAsync(userId, merchantId, request);
            return Ok(cart);
        }

        [HttpGet()]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid token");
            }

            var cart = await _serviceProviders.CartService.GetCartAsync(userId);
            return cart == null ? NotFound(new { message = "Not found" }) : Ok(cart);
        }

        [HttpDelete("items/{cartItemId}")]
        [Authorize]
        public async Task<IActionResult> RemoveItem(Guid cartItemId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid token");
            }


            var cart = await _serviceProviders.CartService.RemoveItemAsync(userId, cartItemId);
            return cart == null ? throw new NotFoundException("Not found") : Ok(cart);
        }

        [HttpDelete()]
        [Authorize]
        public async Task<IActionResult> DeleteCart()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid token");
            }

            await _serviceProviders.CartService.DeleteCartAsync(userId);
            return Ok();
        }
    }
}

