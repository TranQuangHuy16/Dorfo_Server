using AutoMapper;
using Dorfo.API.Exceptions;
using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IServiceProviders _serviceProvider;
        private readonly IMapper _mapper;
        public UserController(IServiceProviders serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid token");
            }

            var user = await _serviceProvider.UserService.UpdateAsync(userId, request);
            if (user == null)
                throw new UserNotFoundException($"User not found");

            var userDto = _mapper.Map<UserResponse>(user);

            return Ok(userDto);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedException("Invalid token");
            }

            // Tìm user theo Id
            var user = await _serviceProvider.UserService.GetUserById(userId);


            if (user == null)
                throw new UserNotFoundException($"User not found");

            var userDto = _mapper.Map<UserResponse>(user);

            return Ok(userDto);
        }
    }
}
