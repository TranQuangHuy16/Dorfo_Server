using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task<bool> SendNotificationAsync(string deviceToken, string title, string body);

    }
}
