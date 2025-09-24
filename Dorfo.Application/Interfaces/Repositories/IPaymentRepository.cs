using Dorfo.Domain.Entities;
using Dorfo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> CreateAsync(Payment payment);
        Task<bool> UpdatePaymentStatus(Guid orderId, PaymentStatusEnum status);
    }
}
