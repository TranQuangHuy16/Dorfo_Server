using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string email, string otp);
    }
}
