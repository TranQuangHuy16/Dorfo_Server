using Dorfo.Application.Interfaces.Services;
using Dorfo.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ShipperController : ControllerBase
    {
        private readonly IShipperService _shipperService;

        public ShipperController(IShipperService shipperService)
        {
            _shipperService = shipperService;
        }

        // POST: api/Shipper
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ShipperRequest request)
        {
            var result = await _shipperService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.ShipperId }, result);
        }

        // GET: api/Shipper/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _shipperService.GetShipperByIdAsync(id);
            return Ok(result);
        }

        // GET: api/Shipper/merchant/{merchantId}
        [HttpGet("merchant/{merchantId:guid}")]
        public async Task<IActionResult> GetAllByMerchantId(Guid merchantId)
        {
            var result = await _shipperService.GetAllShipperByMerchantIdAsync(merchantId);
            return Ok(result);
        }

        // PUT: api/Shipper/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ShipperRequest request)
        {
            var result = await _shipperService.UpdateAsync(id, request);
            return Ok(result);
        }

        // DELETE: api/Shipper/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _shipperService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
