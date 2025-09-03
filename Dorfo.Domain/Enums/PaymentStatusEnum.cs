using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Enums
{
    public enum PaymentStatusEnum
    {
        Pending = 1,      // Chưa thanh toán
        Completed = 2,    // Thanh toán thành công
        Failed = 3,       // Thanh toán thất bại
        Refunded = 4,     // Đã hoàn tiền
        Cancelled = 5     // Thanh toán đã bị hủy
    }
}
