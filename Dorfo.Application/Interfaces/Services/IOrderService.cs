using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderResponse?> CreateOrderAsync(CreateOrderRequest request, Guid userId);
        Task<OrderResponse?> GetOrderByIdAsync(Guid orderId);
        Task<List<OrderResponse>> GetOrdersByUserAsync(Guid userId);
    }
}
