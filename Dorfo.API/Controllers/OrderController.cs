using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.Exceptions;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;
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
        private readonly IServiceProviders _serviceProviders;
        private readonly INotificationService _notificationService;

        public OrdersController(IServiceProviders serviceProviders, INotificationService notificationService)
        {
            _serviceProviders = serviceProviders;
            _notificationService = notificationService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _serviceProviders.OrderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{orderId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await _serviceProviders.OrderService.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException("Order not found");

            return Ok(order);
        }

        [HttpGet("/api/Orders/users/{userId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetOrdersByUser(Guid userId)
        {
            var orders = await _serviceProviders.OrderService.GetOrdersByUserAsync(userId);
            return Ok(orders);
        }

        [HttpGet("/api/Orders/merchant/{merchantId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetOrdersByMerchant(Guid merchantId)
        {
            var orders = await _serviceProviders.OrderService.GetOrderByMerchantAsync(merchantId);
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

            var order = await _serviceProviders.OrderService.CreateOrderAsync(request, userId);

            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }
            else
            {
                var user = await _serviceProviders.UserService.GetUserById(userId);
                await _notificationService.SendNotificationAsync(user.FcmToken, "Thanh Toán", $"Đơn hàng đã được tạo. Xin vui lòng chờ quán xác nhận");

            }

            return Ok(order);
        }

        [HttpPut("{orderId:guid}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromQuery] OrderStatusEnum status)
        {
            var result = await _serviceProviders.OrderService.UpdateOrderStatusAsync(orderId, status);

            if (!result)
                throw new NotFoundException("Order not found");

            var order = await _serviceProviders.OrderService.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                var address = await _serviceProviders.AddressService.GetAddressByAddressId(order.AddressId);
                var user = await _serviceProviders.UserService.GetUserById(address.UserId);
                var userMerchant = await _serviceProviders.UserService.GetUserByMerchantId(order.MerchantId);

                switch (status)
                {
                    case OrderStatusEnum.IN_PROGRESS:
                        await _notificationService.SendNotificationAsync(user.FcmToken, "Cập Nhật Đơn Hàng", $"Quán đã xác nhận đơn. Đang chuẩn bị món.");
                        break;
                    case OrderStatusEnum.DELIVERING:
                        await _notificationService.SendNotificationAsync(user.FcmToken, "Cập Nhật Đơn Hàng", $"Đơn đang trên đường giao");
                        break;
                    case OrderStatusEnum.COMPLETED:
                        await _notificationService.SendNotificationAsync(user.FcmToken, "Cập Nhật Đơn Hàng", $"Đơn đã giao thành công. Chúc bạn ngon miệng!");
                        await _notificationService.SendNotificationAsync(userMerchant.FcmToken, "Cập Nhật Đơn Hàng", $"Đơn {order.OrderId} đã giao thành công");
                        break;
                    case OrderStatusEnum.CANCELLED:
                        await _notificationService.SendNotificationAsync(user.FcmToken, "Cập Nhật Đơn Hàng", $"Đơn đã bị hủy. Vui lòng liên hệ quán để biết thêm chi tiết.");
                        await _notificationService.SendNotificationAsync(userMerchant.FcmToken, "Cập Nhật Đơn Hàng", $"Đơn {order.OrderId} đã bị hủy.");
                        break;
                }

            }

            return Ok(new { message = "Update successfully" });
        }
    }
}
