using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Application.Interfaces.Services;
using Dorfo.Domain.Entities;
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

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _unitOfWork.UserRepository.GetUserByIdAsync(id);
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

            await _unitOfWork.UserRepository.UpdateAsync(user);
            return user;
        }
    }
}
