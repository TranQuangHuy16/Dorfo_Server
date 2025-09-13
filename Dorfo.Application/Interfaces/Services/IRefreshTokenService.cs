using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IRefreshTokenService
    {
        Task SaveRefreshTokenAsync(Guid userId, string refreshToken, int expireMinutes = 10080); // default 7 ngày
        Task<string?> GetRefreshTokenAsync(Guid userId);
        Task RemoveRefreshTokenAsync(Guid userId);
    }
}
