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
    }
}
