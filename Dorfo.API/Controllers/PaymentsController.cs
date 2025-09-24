using Dorfo.Application.Exceptions;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Application.Services;
using Dorfo.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text.Json;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IServiceProviders _serviceProviders;
    private readonly PayOS _payOS;

    public PaymentsController(IPaymentService paymentService, PayOS payOS, IServiceProviders serviceProviders)
    {
        _payOS = payOS;
        _paymentService = paymentService;
        _serviceProviders = serviceProviders;
    }

    // POST api/payments/checkout/{merchantId}
    [HttpPost("checkout/{merchantId}")]
    public async Task<IActionResult> Checkout(Guid merchantId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedException("Invalid token");

        var result = await _paymentService.CheckoutAsync(userId, merchantId);
        return Ok(result);
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook([FromBody] WebhookType webhookBody)
    {
        try
        {
            // verifyPaymentWebhookData trả về WebhookData (những trường: orderCode, amount, description, ...)
            WebhookData webhookData = _payOS.verifyPaymentWebhookData(webhookBody);

            // Bạn có thể kiểm tra success ở phần gốc (webhookBody) hoặc kiểm tra webhookData.code == "00"
            if (webhookBody != null && webhookBody.success)
            {
                // truy cập trực tiếp các trường trong WebhookData (camelCase)
                var orderCode = webhookData.orderCode;   // long

                // Cập nhật trạng thái thanh toán trong hệ thống của bạn

                var order = await _serviceProviders.OrderService.GetOrderByOrderCode(orderCode);
                if (order == null)
                {
                    // Log lỗi không tìm thấy đơn hàng hoặc cập nhật thất bại
                    return BadRequest(new { code = "-1", message = "Order not found or update failed" });
                }
                var updateResult = await _serviceProviders.OrderService.UpdateOrderStatusAsync(order.OrderId, OrderStatusEnum.PENDING);
                var updatePayment = await _paymentService.UpdatePaymentStatus(order.OrderId, PaymentStatusEnum.SUCCESS);


            }
            else if (webhookBody != null && !webhookBody.success)
            {
                var orderCode = webhookData.orderCode;
                var order = await _serviceProviders.OrderService.GetOrderByOrderCode(orderCode);
                if (order == null)
                {
                    // Log lỗi không tìm thấy đơn hàng hoặc cập nhật thất bại
                    return BadRequest(new { code = "-1", message = "Order not found or update failed" });
                }
                var updatePayment = await _paymentService.UpdatePaymentStatus(order.OrderId, PaymentStatusEnum.FAILED);

                var updateResult = await _serviceProviders.OrderService.UpdateOrderStatusAsync(order.OrderId, OrderStatusEnum.CANCELLED);

            }

            // trả 200 để payOS biết đã nhận
            return Ok(new { code = "00", message = "processed" });
        }
        catch (Exception ex)
        {
            // SDK có thể throw nếu signature không hợp lệ -> log để debug
            Console.WriteLine(ex);
            return BadRequest(new { code = "-1", message = ex.Message });
        }
    }

    //// POST api/payments/callback
    //[HttpPost("callback")]
    //public async Task<IActionResult> Callback([FromBody] JsonElement payload)
    //{
    //    await _paymentService.HandleCallbackAsync(payload);
    //    return Ok(new { message = "Callback processed" });
    //}
}
