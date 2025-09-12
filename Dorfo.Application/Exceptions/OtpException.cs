using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Exceptions
{
    public class OtpException : AppException
    {
        public OtpException(string message) : base(404, message)
        {
        }
    }
}
