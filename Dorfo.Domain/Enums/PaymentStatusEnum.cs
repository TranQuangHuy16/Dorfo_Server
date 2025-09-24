using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Enums
{
    public enum PaymentStatusEnum
    {
        PENDING = 1,      // Chưa thanh toán
        SUCCESS = 2,    // Thanh toán thành công
        FAILED = 3,       // Thanh toán thất bại
        REFUNDED = 4,     // Đã hoàn tiền
        CANCELLED = 5     // Thanh toán đã bị hủy
    }
}
