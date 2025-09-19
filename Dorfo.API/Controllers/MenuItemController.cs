using Dorfo.Application.Interfaces.Services;
using Dorfo.Application;
using Microsoft.AspNetCore.Mvc;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        // GET: api/MenuItem/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItemResponse>> GetMenuItemById(Guid id)
        {
            var result = await _menuItemService.GetMenuItemByIdAsync(id);
            return Ok(result);
        }

        // GET: api/MenuItem/merchant/{merchantId}
        [HttpGet("merchant/{merchantId}")]
        public async Task<ActionResult<IEnumerable<MenuItemResponse>>> GetAllByMerchantId(Guid merchantId)
        {
            var result = await _menuItemService.GetAllMenuItemByMerchantIdAsync(merchantId);
            return Ok(result);
        }

        // GET: api/MenuItem/category/{categoryId}
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<MenuItemResponse>>> GetAllByCategoryId(Guid categoryId)
        {
            var result = await _menuItemService.GetAllMenuItemByCategoryIdAsync(categoryId);
            return Ok(result);
        }

        // POST: api/MenuItem
        [HttpPost]
        public async Task<ActionResult<MenuItemResponse>> Create([FromBody] MenuItemRequest request)
        {
            var result = await _menuItemService.CreateAsync(request);
            return CreatedAtAction(nameof(GetMenuItemById), new { id = result.MenuItemId }, result);
        }

        // PUT: api/MenuItem/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<MenuItemResponse>> Update(Guid id, [FromBody] MenuItemRequest request)
        {
            var result = await _menuItemService.UpdateAsync(id, request);
            return Ok(result);
        }

        // DELETE: api/MenuItem/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<MenuItemResponse>> Delete(Guid id)
        {
            var result = await _menuItemService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
