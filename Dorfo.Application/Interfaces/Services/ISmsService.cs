using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface ISmsService
    {
        Task SendOtpAsync(string phoneNumber, string message);
    }
}
