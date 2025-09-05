using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Services
{
    public class OtpService : IOtpService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;

        public OtpService(IUnitOfWork unitOfWork, IJwtProvider jwtProvider)
        {
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
        }

        public async Task<bool> RequestOtpAsync(string phoneNumber)
        {
            var repoUser = _unitOfWork.UserRepository;
            var user = await repoUser.GetUserByPhoneAsync(phoneNumber);
            if (user == null)
            {
                //throw new Exception("User not exist");
                return false;
            }
            var code = new Random().Next(100000, 999999).ToString();
            var otp = new OtpCode
            {
                PhoneNumber = phoneNumber,
                Code = code,
                ExpiredAt = DateTime.UtcNow.AddMinutes(5)
            };

            await _unitOfWork.OtpRepository.CreateAsync(otp);

            // TODO: call SMS API (VD: Twilio, Zalo…)
            Console.WriteLine($"Send OTP {code} to {phoneNumber}");
            return true;
        }

        public async Task<string> VerifyOtpAsync(string phoneNumber, string code)
        {
            var repo = _unitOfWork.OtpRepository;
            var repoUser = _unitOfWork.UserRepository;
            var user = await repoUser.GetUserByPhoneAsync(phoneNumber);
            var otp = await repo.GetOtpByPhoneAndCode(phoneNumber, code);
            

            if (otp == null || otp.ExpiredAt < DateTime.UtcNow)
                throw new Exception("Invalid or expired OTP");

            await _unitOfWork.OtpRepository.IsUsedOtp(otp.Id);

            // Issue JWT
            return _jwtProvider.GenerateToken(user.UserId);
        }
    }
}
