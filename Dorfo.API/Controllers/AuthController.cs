using Dorfo.API.Exceptions;
using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IServiceProviders _serviceProvider;

        public AuthController(IServiceProviders serviceProviders)
        {
            _serviceProvider = serviceProviders;
        }

        [HttpPost("send-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestOtp([FromBody] OtpRequest request)
        {
            var check = await _serviceProvider.OtpService.RequestOtpAsync(request.PhoneNumber);
            if (!check)
            {
                throw new UserNotFoundException($"User not found");
            }
            return Ok(new { Message = "OTP sent" });
        }

        [HttpPost("verify-otp")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var token = await _serviceProvider.OtpService.VerifyOtpAsync(request.PhoneNumber, request.Code);
            return Ok(new { Token = token });
        }
    }



}
