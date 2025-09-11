using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<User?> GetUserByPhoneAsync(string phone);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUsernameOrPhoneOrEmailAsync(string email);
        Task<User?> GetUserByLogin(string username);
        Task<int> UpdateAsync(User user);
        Task<User> CreateAsync(User newUser);
        Task<User?> GetUserByUsernameAsync(string username);


    }
}
