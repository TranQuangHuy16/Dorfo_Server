using Dorfo.Application.DTOs.Requests;
using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<User?> UpdateAsync(Guid id, UserUpdateRequest user);
        Task<User?> GetUserById(Guid id);
        Task<User?> RegisterByUsername(UserCreateRequest user);
        Task<User?> RegisterByPhone(UserCreateByPhoneRequest user);
        Task<string> Login(LoginRequest login);
    }
}
