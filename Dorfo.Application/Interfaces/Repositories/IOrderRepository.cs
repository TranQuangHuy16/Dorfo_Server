using Dorfo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetByUserAsync(Guid userId);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<bool> UpdateStatus(Guid id, OrderStatusEnum status);
        Task<Order> GetOrderByOrderCode(long orderCode);
    }
}
