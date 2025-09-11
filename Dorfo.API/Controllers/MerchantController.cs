using AutoMapper;
using Dorfo.API.Exceptions;
using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MerchantController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;
        private readonly IMapper _mapper;

        public MerchantController(IServiceProviders serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }



        // GET: api/merchant
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var merchants = await _serviceProvider.MerchantService.GetAllAsync();
            if (merchants == null || !merchants.Any())
                return Ok(merchants);
            else throw new MerchantException($"Not Found Merchants");
        }

        // GET: api/merchant/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var merchant = await _serviceProvider.MerchantService.GetMerchantByIdAsync(id);
            if (merchant == null)
                return NotFound();

            return Ok(merchant);
        }

        // GET: api/merchant/owner/{ownerId}
        [HttpGet("owner/{ownerId:guid}")]
        public async Task<IActionResult> GetByOwnerId(Guid ownerId)
        {
            var merchants = await _serviceProvider.MerchantService.GetMerchantByOwnerIdAsync(ownerId);

            if (merchants == null || !merchants.Any())
                throw new MerchantException($"Not Found Merchants");

            return Ok(merchants);

        }

        // POST: api/merchant
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MerchantRequest merchant)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _serviceProvider.MerchantService.CreateAsync(merchant);
            if (result != null)
                return Ok(result);
            else throw new MerchantException($"Failed to create merchant");
        }

        // PUT: api/merchant/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MerchantRequest merchant)
        {
            var result = await _serviceProvider.MerchantService.UpdateAsync(id, merchant);
            if (result != null)
                return Ok(result);
            else throw MerchantException.NotFound(id);
        }

        // DELETE: api/merchant/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _serviceProvider.MerchantService.DeleteAsync(id);
            if (result != null)
                return Ok(result);
            else throw MerchantException.NotFound(id);

        }
    }
}
