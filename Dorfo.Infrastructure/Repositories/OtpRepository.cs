using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Domain.Entities;
using Dorfo.Infrastructure.Persistence;
using Dorfo.Infrastructure.Repositories.Basic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Repositories
{
    public class OtpRepository : GenericRepository<OtpCode>, IOtpRepository
    {
        public OtpRepository()
        {
        }

        public OtpRepository(DorfoDbContext context) => _context = context;

        public async Task IsUsedOtp(Guid id)
        {
            var otp = await _context.Otps.FindAsync(id);
            otp.IsUsed = true;
            base.UpdateAsync(otp);
        }

        public async Task CreateAsync(OtpCode otpCode)
        {
            await _context.Otps.AddAsync(otpCode);
            await _context.SaveChangesAsync();
        }

        public async Task<OtpCode> GetOtpByPhoneAndCode(string phone, string code)
        {
            return await _context.Otps.FirstOrDefaultAsync(o => o.PhoneNumber == phone && o.Code == code && o.IsUsed == false);
        }
    }
}
