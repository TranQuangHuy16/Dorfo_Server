using Dorfo.Application.DTOs.Requests;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Domain.Enums;
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
        Task<IEnumerable<OrderResponse>> GetOrdersByUserAsync(Guid userId);
        Task<IEnumerable<OrderResponse>> GetAllAsync();
        Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatusEnum status);
        Task<Order> GetOrderByOrderCode(long orderCode);
    }
}
