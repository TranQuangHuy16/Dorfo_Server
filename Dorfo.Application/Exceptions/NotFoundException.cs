using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base(404, message)
        {
        }
    }
}
