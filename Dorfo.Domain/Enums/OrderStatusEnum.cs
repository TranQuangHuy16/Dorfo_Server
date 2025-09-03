using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Enums
{
    public enum OrderStatusEnum
    {
        Created = 1,
        Paid = 2,
        PendingCOD = 3,
        Accepted = 4,
        Preparing = 5,
        ReadyForPickup = 6,
        OutForDelivery = 7,
        Delivered = 8,
        Completed = 9,
        PaymentFailed = 10,
        Cancelled = 11,
        Refunded = 12,
        Dispute = 13,
        Returned = 14,
        Rejected = 15
    }
}
