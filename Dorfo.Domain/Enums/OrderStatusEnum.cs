using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Enums
{
    public enum OrderStatusEnum
    {
        WAITING_FOR_PAYMENT = 0,
        PENDING = 1,
        IN_PROGRESS = 2,
        COMPLETED = 3,
        CANCELLED = 4,
    }
}
