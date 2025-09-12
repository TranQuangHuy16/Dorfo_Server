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

namespace Dorfo.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(IOtpService otpService, IEmailService emailService, IUnitOfWork unitOfWork)
        {
            _otpService = otpService;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
                throw new UserNotFoundException("User Not Found");

            var otp = new Random().Next(100000, 999999).ToString();

            await _otpService.SaveOtpAsync(user.Email, otp);

            await _emailService.SendOtpEmailAsync(user.Email, otp);
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest request)
        {
            // Kiểm tra OTP trong Redis
            var storedOtp = await _otpService.GetOtpAsync(request.Email);
            if (storedOtp == null || storedOtp != request.Otp)
                throw new Exception("Invalid or expired OTP");

            // Lấy user từ DB
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("User not found");

            // Hash mật khẩu mới
            var hashedPassword = _passwordHasher.HashPassword(user, request.NewPassword);

            user.Password = hashedPassword;
            // Update mật khẩu
            await _unitOfWork.UserRepository.UpdateAsync(user);

            // Xóa OTP
            await _otpService.RemoveOtpAsync(request.Email);
        }
    }
}
