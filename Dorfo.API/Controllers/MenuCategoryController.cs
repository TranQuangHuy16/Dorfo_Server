using Dorfo.Application.Interfaces.Services;
using Dorfo.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MenuCategoryController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;

        public MenuCategoryController(IServiceProviders serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // GET: api/MenuCategory?merchantId
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuCategoryResponse>>> GetAllCategories([FromQuery] Guid merchantId)
        {
            var result = await _serviceProvider.MenuCategoryService.GetAllCategoryByMerchantIdAsync(merchantId);
            return Ok(result);
        }


        // GET: api/MenuCategory/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MenuCategoryResponse>> GetCategoryById(Guid id)
        {
            var result = await _serviceProvider.MenuCategoryService.GetCategoryByIdAsync(id);
            return Ok(result);
        }

        // POST: api/MenuCategory
        [HttpPost]
        public async Task<ActionResult<MenuCategoryResponse>> Create(MenuCategoryRequest request)
        {
            var result = await _serviceProvider.MenuCategoryService.CreateAsync(request);
            return CreatedAtAction(nameof(GetCategoryById), new { id = result.MenuCategoryId }, result);
        }

        // PUT: api/MenuCategory/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<MenuCategoryResponse>> Update(Guid id, MenuCategoryRequest request)
        {
            var result = await _serviceProvider.MenuCategoryService.UpdateAsync(id, request);
            return Ok(result);
        }

        // DELETE: api/MenuCategory/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<MenuCategoryResponse>> Delete(Guid id)
        {
            var result = await _serviceProvider.MenuCategoryService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
