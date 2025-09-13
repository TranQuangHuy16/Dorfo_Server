using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.Exceptions;
using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokenService _refreshTokenService;
        public UserService(IUnitOfWork unitOfWork, IJwtProvider jwtProvider, IRefreshTokenService redisService)
        {
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
            _refreshTokenService = redisService;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _unitOfWork.UserRepository.GetUserByIdAsync(id);
        }

        private async Task<User> CreateUserAsync(
                                string? username,
                                string password,
                                string? phone,
                                string? email,
                                string? displayName,
                                DateOnly? birthDate,
                                int? gender)
        {
            // Kiểm tra trùng phone
            if (!string.IsNullOrEmpty(phone))
            {
                var existedUserByPhone = await _unitOfWork.UserRepository.GetUserByUsernameOrPhoneOrEmailAsync(phone);
                if (existedUserByPhone != null)
                    throw new DuplicateFieldException("Phone", phone);
            }

            // Kiểm tra trùng email
            if (!string.IsNullOrEmpty(email))
            {
                var existedUserByEmail = await _unitOfWork.UserRepository.GetUserByUsernameOrPhoneOrEmailAsync(email);
                if (existedUserByEmail != null)
                    throw new DuplicateFieldException("Email", email);
            }

            // Kiểm tra trùng username
            if (!string.IsNullOrEmpty(username))
            {
                var existedUserByUsername = await _unitOfWork.UserRepository.GetUserByUsernameOrPhoneOrEmailAsync(username);
                if (existedUserByUsername != null)
                    throw new DuplicateFieldException("Username", username);
            }

            // Tạo user mới
            var passwordHasher = new PasswordHasher<User>();
            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                Username = username,
                DisplayName = displayName,
                Phone = phone,
                Email = email,
                BirthDate = birthDate,
                Gender = gender.HasValue ? gender.Value == 1 : false,
                Role = Domain.Enums.UserRoleEnum.Customer,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            newUser.Password = passwordHasher.HashPassword(newUser, password);

            return await _unitOfWork.UserRepository.CreateAsync(newUser);
        }


        public async Task<User?> RegisterByUsername(UserCreateRequest dto)
        {
            return await CreateUserAsync(
                dto.UserName,
                dto.Password,
                dto.Phone,
                dto.Email,
                dto.DisplayName,
                dto.DateOfBirth,
                dto.Gender
            );
        }

        public async Task<User?> RegisterByPhone(UserCreateByPhoneRequest dto)
        {
            return await CreateUserAsync(
                null, // không bắt buộc username khi đăng ký qua phone
                dto.Password,
                dto.Phone,
                dto.Email,
                dto.DisplayName,
                dto.DateOfBirth,
                dto.Gender
            );
        }


        public Task<User?> RegisterByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> UpdateAsync(Guid id, UserUpdateRequest dto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
            if (user == null) return null;

            user.DisplayName = dto.DisplayName ?? user.DisplayName;
            user.Phone = dto.Phone ?? user.Phone;
            user.Email = dto.Email ?? user.Email;
            user.BirthDate = dto.DateOfBirth ?? user.BirthDate;
            user.AvatarUrl = dto.AvatarUrl ?? user.AvatarUrl;
            user.Gender = dto.Gender.HasValue ? dto.Gender.Value == 1 : user.Gender;

            await _unitOfWork.UserRepository.UpdateAsync(user);
            return user;
        }

        public async Task<(string accessToken, string refreshToken)> Login(LoginRequest login)
        {
            // Lấy user theo Username, Phone hoặc Email
            var user = await _unitOfWork.UserRepository.GetUserByLogin(login.Username);
            if (user == null)
                throw new UserNotFoundException("User not found");

            // Kiểm tra password
            var passwordHasher = new PasswordHasher<User>();
            if (string.IsNullOrEmpty(user.Password))
                throw new UnauthorizedException("User has no password set");

            var result = passwordHasher.VerifyHashedPassword(user, user.Password, login.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedException("Invalid password");

            // Cập nhật thời gian đăng nhập
            user.LastLoginAt = DateTime.UtcNow;
            await _unitOfWork.UserRepository.UpdateAsync(user);

            // Sinh Access Token & Refresh Token
            var accessToken = _jwtProvider.GenerateToken(user.UserId);
            var refreshToken = _jwtProvider.GenerateRefreshToken();

            // Lưu refresh token vào Redis
            await _refreshTokenService.SaveRefreshTokenAsync(user.UserId, refreshToken);

            return (accessToken, refreshToken);
        }

        public async Task<string> RefreshAccessToken(Guid userId, string refreshToken)
        {
            var storedToken = await _refreshTokenService.GetRefreshTokenAsync(userId);

            if (storedToken == null || storedToken != refreshToken)
            {
                throw new UnauthorizedException("Invalid refresh token");
            }

            // Sinh access token mới
            var newAccessToken = _jwtProvider.GenerateToken(userId);

            return newAccessToken;
        }

        public async Task Logout(Guid userId)
        {
            await _refreshTokenService.RemoveRefreshTokenAsync(userId);
        }

    }
}
