using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.Exceptions;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{orderId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException("Order not found");

            return Ok(order);
        }

        [HttpGet("/api/Orders/users/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetOrdersByUser(Guid userId)
        {
            var orders = await _orderService.GetOrdersByUserAsync(userId);
            return Ok(orders);
        }

        [HttpPost]
        [Authorize]
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
                throw new NotFoundException("Order not found");

            return Ok(order);
        }

        [HttpPut("{orderId:guid}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromQuery] OrderStatusEnum status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, status);

            if (!result)
                throw new NotFoundException("Order not found");

            return Ok(new { message = "Update successfully" });
        }
    }
}
