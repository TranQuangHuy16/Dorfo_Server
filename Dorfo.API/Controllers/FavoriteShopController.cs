using Dorfo.Application.Exceptions;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteShopController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;

        public FavoriteShopController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        // GET: api/favorites/{customerId}
        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid token");
            }

            var favorites = await _serviceProviders.FavoriteShopService.GetFavoriteShopsByCustomerAsync(userId);
            return Ok(favorites.Select(f => new
            {
                f.FavoriteShopId,
                f.MerchantId,
                MerchantName = f.Merchant?.Name,
                f.AddedAt
            }));
        }

        // POST: api/favorites
        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid token");
            }

            var result = await _serviceProviders.FavoriteShopService.AddFavoriteShopAsync(userId, request.MerchantId);
            if (result == null)
                return BadRequest("This shop is already in favorites.");

            return Ok(new
            {
                result.FavoriteShopId,
                result.CustomerId,
                result.MerchantId,
                result.AddedAt
            });
        }
    }

    public class AddFavoriteRequest
    {
        public Guid MerchantId { get; set; }
    }
}
