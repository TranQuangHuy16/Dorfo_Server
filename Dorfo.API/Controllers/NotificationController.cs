using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IServiceProviders _serviceProviders;

        public NotificationController(INotificationService notificationService, IServiceProviders serviceProviders)
        {
            _notificationService = notificationService;
            _serviceProviders = serviceProviders;
        }

        [HttpPost("send-to-user")]
        public async Task<IActionResult> SendToUser([FromBody] UserNotificationRequest request)
        {
            var user = _serviceProviders.UserService.GetUserById(request.UserId).Result;

            var result = await _notificationService.SendNotificationAsync(user.FcmToken, request.Title, request.Body);
            return result ? Ok("Notification sent to user") : NotFound("User or token not found");
        }

        [HttpPost("send-to-merchant")]
        public async Task<IActionResult> SendToMerchant([FromBody] MerchantNotificationRequest request)
        {
            var user = await _serviceProviders.UserService.GetUserByMerchantId(request.MerchantId);

            var result = await _notificationService.SendNotificationAsync(user.FcmToken, request.Title, request.Body);
            return result ? Ok("Notification sent to merchant") : NotFound("Merchant or token not found");
        }
    }
}
