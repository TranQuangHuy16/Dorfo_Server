using Dorfo.Application.Interfaces.Services;
using Dorfo.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MenuItemOptionController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;

        public MenuItemOptionController(IServiceProviders serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // POST: api/MenuItemOption
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MenuItemOptionRequest request)
        {
            var result = await _serviceProvider.MenuItemOptionService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.OptionId }, result);
        }

        // GET: api/MenuItemOption/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _serviceProvider.MenuItemOptionService.GetMenuItemOptionByIdAsync(id);
            return Ok(result);
        }

        // GET: api/MenuItemOption/menuitem/{menuItemId}
        [HttpGet("menuitem/{menuItemId:guid}")]
        public async Task<IActionResult> GetAllByMenuItemId(Guid menuItemId)
        {
            var result = await _serviceProvider.MenuItemOptionService.GetAllMenuItemOptionByMenuItemIdAsync(menuItemId);
            return Ok(result);
        }

        // PUT: api/MenuItemOption/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MenuItemOptionRequest request)
        {
            var result = await _serviceProvider.MenuItemOptionService.UpdateAsync(id, request);
            return Ok(result);
        }

        // DELETE: api/MenuItemOption/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _serviceProvider.MenuItemOptionService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
