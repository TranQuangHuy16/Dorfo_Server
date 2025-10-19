using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Domain.Entities;
using Dorfo.Domain.Enums;
using Dorfo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DorfoDbContext _context;
        public OrderRepository(DorfoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        //public async Task<Order?> GetByIdAsync(Guid orderId)
        //{
        //    return await _context.Orders
        //        .Include(o => o.Items).ThenInclude(i => i.OrderItemOptions).ThenInclude(o => o.OrderItemOptionValue)
        //        .FirstOrDefaultAsync(o => o.OrderId == orderId);
        //}

        //public async Task<List<Order>> GetByUserAsync(Guid userId)
        //{
        //    return await _context.Orders
        //        .Where(o => o.UserId == userId)
        //        .Include(o => o.Items)
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<Order>> GetByUserAsync(Guid userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Items)
                    .ThenInclude(i => i.OrderItemOptions)
                        .ThenInclude(oo => oo.OrderItemOptionValue)
                .Include(o => o.Items)
                    .ThenInclude(i => i.OrderItemOptions)
                        .ThenInclude(oo => oo.MenuItemOption)
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .Include(o => o.Merchant)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.OrderItemOptions)
                        .ThenInclude(oo => oo.OrderItemOptionValue)
                .Include(o => o.Items)
                    .ThenInclude(i => i.OrderItemOptions)
                        .ThenInclude(oo => oo.MenuItemOption)
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .Include(o => o.Merchant)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.OrderItemOptions)
                        .ThenInclude(oo => oo.OrderItemOptionValue)
                .Include(o => o.Items)
                    .ThenInclude(i => i.OrderItemOptions)
                        .ThenInclude(oo => oo.MenuItemOption)
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .Include(o => o.Merchant)
                .ToListAsync();
        }

        public async Task<bool> UpdateStatus(Guid id, OrderStatusEnum status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;
            order.Status = status;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Order> GetOrderByOrderCode(long orderCode)
        {
            return await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.OrderItemOptions)
                        .ThenInclude(oo => oo.OrderItemOptionValue)
                .Include(o => o.Items)
                    .ThenInclude(i => i.OrderItemOptions)
                        .ThenInclude(oo => oo.MenuItemOption)
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .Include(o => o.Merchant)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.OrderCode == orderCode);
        }

        public async Task<IEnumerable<Order>> GetOrderByMerchant(Guid merchantId)
        {
            return await _context.Orders
                .Where(o => o.MerchantId == merchantId)
                .Include(o => o.Items)
                    .ThenInclude(i => i.OrderItemOptions)
                        .ThenInclude(oo => oo.OrderItemOptionValue)
                .Include(o => o.Items)
                    .ThenInclude(i => i.OrderItemOptions)
                        .ThenInclude(oo => oo.MenuItemOption)
                .Include(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
                .Include(o => o.Merchant)
                .ToListAsync();
        }
    }
}
