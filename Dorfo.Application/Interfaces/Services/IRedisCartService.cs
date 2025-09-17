using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IRedisCartService
    {
        Task SaveCartAsync(Cart cart, int expireMinutes = 60);
        Task<Cart?> GetCartAsync(Guid userId);
        Task RemoveCartAsync(Guid userId);
    }
}
