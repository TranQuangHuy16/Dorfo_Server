using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Exceptions;
using Dorfo.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;

        public AddressController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        // GET: api/users/{userId}/address
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AddressResponse>>> GetAll()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            var addresses = await _serviceProviders.AddressService.GetAllAddressesByUserAsync(userId);

            return Ok(addresses);
        }

        // POST: api/users/{userId}/address
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<AddressResponse>> Create([FromBody] AddressRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            var created = await _serviceProviders.AddressService.CreateAddressAsync(userId, request);

            return CreatedAtAction(
                nameof(GetAll),
                new { userId = userId },
                created
            );
        }

        // PUT: api/users/{userId}/address/{addressId}
        [HttpPut("{addressId:guid}")]
        [Authorize]
        public async Task<ActionResult<AddressResponse>> Update(Guid addressId, [FromBody] AddressRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            var updated = await _serviceProviders.AddressService.UpdateAddressAsync(userId, addressId, request);
            if (updated == null) throw new NotFoundException("Address not found");

            return Ok(updated);
        }

        // DELETE: api/users/{userId}/address/{addressId}
        [HttpDelete("{addressId:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid addressId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                throw new UnauthorizedException("Invalid token");

            var success = await _serviceProviders.AddressService.RemoveAddressAsync(userId, addressId);
            if (!success) throw new NotFoundException("Address not found");

            return Ok("Delete successfully");
        }
    }
}
