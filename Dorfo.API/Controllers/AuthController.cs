using AutoMapper;
using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Exceptions;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IServiceProviders _serviceProvider;
        private readonly IMapper _mapper;

        public AuthController(IServiceProviders serviceProviders, IMapper mapper)
        {
            _serviceProvider = serviceProviders;
            _mapper = mapper;
        }

        //[HttpPost("send-otp")]
        //[AllowAnonymous]
        //public async Task<IActionResult> RequestOtp([FromBody] OtpRequest request)
        //{
        //    var check = await _serviceProvider.OtpService.RequestOtpAsync(request.PhoneNumber);
        //    if (!check)
        //    {
        //        throw new UserNotFoundException($"User not found");
        //    }
        //    return Ok(new { Message = "OTP sent" });
        //}

        //[HttpPost("verify-otp")]
        //[AllowAnonymous]
        //public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        //{
        //    var token = await _serviceProvider.OtpService.VerifyOtpAsync(request.PhoneNumber, request.Code);
        //    return Ok(new { Token = token });
        //}

        [HttpPost("register-by-username")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterByUsername([FromBody] UserCreateRequest request)
        {
            var user = await _serviceProvider.UserService.RegisterByUsername(request);
            var userResponse = _mapper.Map<UserCreateResponse>(user);
            return Ok(userResponse);
        }

        [HttpPost("register-by-phone")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterByPhone([FromBody] UserCreateByPhoneRequest request)
        {
            var user = await _serviceProvider.UserService.RegisterByPhone(request);

            var userResponse = _mapper.Map<UserCreateResponse>(user);
            return Ok(userResponse);
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var token = await _serviceProvider.UserService.Login(login);
            return Ok(new { token = token });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await _serviceProvider.AuthService.ForgotPasswordAsync(request);
            return Ok(new { message = "OTP sent to your email." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            await _serviceProvider.AuthService.ResetPasswordAsync(request);
            return Ok(new { message = "Password reset successfully." });
        }
    }
}
