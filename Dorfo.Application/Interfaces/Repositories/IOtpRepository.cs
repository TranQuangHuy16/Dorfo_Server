using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IOtpRepository
    {
        Task CreateAsync(OtpCode otpCode);
        Task IsUsedOtp(Guid id);
        Task<OtpCode> GetOtpByPhoneAndCode(string phone, string code);

    }
}
