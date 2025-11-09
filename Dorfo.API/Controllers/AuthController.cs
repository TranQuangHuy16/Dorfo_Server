using AutoMapper;
using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Exceptions;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Application.Services;
using Dorfo.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IServiceProviders _serviceProvider;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMapper _mapper;

        public AuthController(IServiceProviders serviceProviders, IMapper mapper, IJwtProvider jwtProvider, IRefreshTokenService redisService)
        {
            _serviceProvider = serviceProviders;
            _mapper = mapper;
            _jwtProvider = jwtProvider;
            _refreshTokenService = redisService;
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
            var (accessToken, refreshToken) = await _serviceProvider.UserService.Login(login);

            return Ok(new
            {
                accessToken = accessToken,
                refreshToken = refreshToken
            });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            try
            {
                // Lấy userId từ access token (dù đã hết hạn)
                var principal = _jwtProvider.GetPrincipalFromExpiredToken(request.AccessToken);
                var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                // Lấy refresh token từ Redis
                var savedRefreshToken = await _refreshTokenService.GetRefreshTokenAsync(userId);

                if (savedRefreshToken == null || savedRefreshToken != request.RefreshToken)
                {
                    return Unauthorized(new { message = "Invalid refresh token" });
                }

                // Tạo access token mới
                var newAccessToken = _jwtProvider.GenerateToken(userId);

                return Ok(new
                {
                    accessToken = newAccessToken,
                });
            }
            catch (Exception)
            {
                throw new UnauthorizedException("Invalid token");
            }
        }

        [HttpPost("logout")]
        [Authorize] // yêu cầu có access token để biết user nào
        public async Task<IActionResult> Logout()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid token");
            }

            var isDeletedFcmToken = await _serviceProvider.UserService.DeleteFcmToken(userId);

            await _serviceProvider.UserService.Logout(userId);

            return Ok(new { message = "Logged out successfully" });
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
