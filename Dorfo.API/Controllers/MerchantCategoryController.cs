using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dorfo.Application.Services;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MerchantCategoryController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;

        public MerchantCategoryController(IServiceProviders serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // GET: api/MerchantCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MerchantCategoryResponse>>> GetAll()
        {
            var result = await _serviceProvider.MerchantCategoryService.GetAllMerchantCategoryAsync();
            return Ok(result);
        }

        // GET: api/MerchantCategory/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MerchantCategoryResponse>> GetById(int id)
        {
            var result = await _serviceProvider.MerchantCategoryService.GetMerchantCategoryByIdAsync(id);
            return Ok(result);
        }

        // POST: api/MerchantCategory
        [HttpPost]
        public async Task<ActionResult<MerchantCategoryResponse>> Create([FromBody] MerchantCategoryRequest request)
        {
            var result = await _serviceProvider.MerchantCategoryService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.MerchantCategoryId }, result);
        }

        // PUT: api/MerchantCategory/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<MerchantCategoryResponse>> Update(int id, [FromBody] MerchantCategoryRequest request)
        {
            var result = await _serviceProvider.MerchantCategoryService.UpdateAsync(id, request);
            return Ok(result);
        }

        // DELETE: api/MerchantCategory/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<MerchantCategoryResponse>> Delete(int id)
        {
            var result = await _serviceProvider.MerchantCategoryService.DeleteAsynce(id);
            return Ok(result);
        }
    }
}
