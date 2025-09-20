using Dorfo.Application.Interfaces.Services;
using Dorfo.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MenuItemOptionValueController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;

        public MenuItemOptionValueController(IServiceProviders serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // POST: api/MenuItemOptionValue
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MenuItemOptionValueRequest request)
        {
            var result = await _serviceProvider.MenuItemOptionValueService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.OptionValueId }, result);
        }

        // GET: api/MenuItemOptionValue/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _serviceProvider.MenuItemOptionValueService.GetMenuItemOptionValueByIdAsync(id);
            return Ok(result);
        }

        // GET: api/MenuItemOptionValue/option/{optionId}
        [HttpGet("option/{optionId:guid}")]
        public async Task<IActionResult> GetAllByOptionId(Guid optionId)
        {
            var result = await _serviceProvider.MenuItemOptionValueService.GetAllMenuItemOptionValueByOptionIdAsync(optionId);
            return Ok(result);
        }

        // PUT: api/MenuItemOptionValue/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MenuItemOptionValueRequest request)
        {
            var result = await _serviceProvider.MenuItemOptionValueService.UpdateAsync(id, request);
            return Ok(result);
        }

        // DELETE: api/MenuItemOptionValue/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _serviceProvider.MenuItemOptionValueService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
