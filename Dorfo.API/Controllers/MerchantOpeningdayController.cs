using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Exceptions;
using Dorfo.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MerchantOpeningDayController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;

        public MerchantOpeningDayController(IServiceProviders serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }



        // GET: api/MerchantOpeningDay/{id}
        [HttpGet("by_merchant_id/{id}")]
        public async Task<ActionResult<MerchantOpeningDayResponse>> GetById(Guid id)
        {
            var result = await _serviceProvider.MerchantOpeningDayService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // GET: api/MerchantOpeningDay/merchant/{merchantId}
        [HttpGet("merchant/{merchantId}")]
        public async Task<ActionResult<IEnumerable<MerchantOpeningDayResponse>>> GetAllByMerchantId(Guid merchantId)
        {
            var result = await _serviceProvider.MerchantOpeningDayService.GetAllOpeningDayAsyncByMerchantId(merchantId);
            return Ok(result);
        }

        // POST: api/MerchantOpeningDay
        [HttpPost]
        public async Task<ActionResult<MerchantOpeningDayResponse>> Create([FromBody] MerchantOpeningDayRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var created = await _serviceProvider.MerchantOpeningDayService.CreateAsync(request);
            if (created != null)
                return Ok(created);
            else throw new MerchantException($"Failed to create merchant");
        }

        // PUT: api/MerchantOpeningDay/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<MerchantOpeningDayResponse>> Update(Guid id, [FromBody] MerchantOpeningDayRequest request)
        {
            var updated = await _serviceProvider.MerchantOpeningDayService.UpdateAsync(id, request);
            if (updated == null) throw new NotFoundException("Not Found MerchantOpeningDay");
            return Ok(updated);
        }

        // DELETE: api/MerchantOpeningDay/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<MerchantOpeningDayResponse>> Delete(Guid id)
        {
            var deleted = await _serviceProvider.MerchantOpeningDayService.RemoveAsync(id);
            if (deleted == null) throw new NotFoundException("Not Found MerchantOpeningDay");
            return Ok(deleted);
        }
    }
}
