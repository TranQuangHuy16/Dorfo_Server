using Dorfo.Application.DTOs.Responses;
using Dorfo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponse> CheckoutAsync(Guid userId, Guid merchantId);
        Task<bool> UpdatePaymentStatus(Guid orderId, PaymentStatusEnum status);
        //Task HandleCallbackAsync(JsonElement payload);
    }

}
