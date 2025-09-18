using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.Exceptions;
using Dorfo.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid token");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.CreateOrderAsync(request, userId);

            if (order == null)
                return BadRequest("Cart is empty or user not found.");

            return Ok(order);
        }


        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
                return NotFound();

            return Ok(order);
        }


        [HttpGet("/api/users/{userId:guid}/orders")]
        public async Task<IActionResult> GetOrdersByUser(Guid userId)
        {
            var orders = await _orderService.GetOrdersByUserAsync(userId);
            return Ok(orders);
        }
    }
}
