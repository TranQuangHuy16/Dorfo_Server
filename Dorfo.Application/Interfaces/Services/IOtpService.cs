using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IOtpService
    {
        Task SaveOtpAsync(string email, string otp, int expireMinutes = 5);
        Task<string?> GetOtpAsync(string email);
        Task RemoveOtpAsync(string email);
    }
}
