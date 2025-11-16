using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Domain.Entities;
using Dorfo.Domain.Enums;
using Dorfo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dorfo.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DorfoDbContext _context;

        public DashboardRepository(DorfoDbContext context)
        {
            _context = context;
        }

        // ========== 1.1. KPI Cards ==========

        public async Task<Dictionary<UserRoleEnum, int>> GetUserCountByRoleAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .GroupBy(u => u.Role)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<int> GetActiveMerchantCountAsync()
        {
            return await _context.Merchants
                .AsNoTracking()
                .CountAsync(m => m.IsActive);
        }

        public async Task<KpiTimeframeCounts> GetOrderCountsAsync()
        {
            var now = DateTime.UtcNow;
            var todayStart = now.Date;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var yearStart = new DateTime(now.Year, 1, 1);

            var orders = _context.Orders.AsNoTracking();

            int today = await orders.CountAsync(o => o.CreatedAt >= todayStart);
            int thisMonth = await orders.CountAsync(o => o.CreatedAt >= monthStart);
            int thisYear = await orders.CountAsync(o => o.CreatedAt >= yearStart);

            return new KpiTimeframeCounts { Today = today, ThisMonth = thisMonth, ThisYear = thisYear };
        }

        public async Task<KpiTimeframeAmounts> GetRevenueAsync()
        {
            var now = DateTime.UtcNow;
            var todayStart = now.Date;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var yearStart = new DateTime(now.Year, 1, 1);

            var completedOrders = _context.Orders
                .AsNoTracking()
                .Where(o => o.Status == OrderStatusEnum.COMPLETED);

            decimal today = await completedOrders
                .Where(o => o.CreatedAt >= todayStart)
                .SumAsync(o => o.TotalAmount);

            decimal thisMonth = await completedOrders
                .Where(o => o.CreatedAt >= monthStart)
                .SumAsync(o => o.TotalAmount);

            decimal thisYear = await completedOrders
                .Where(o => o.CreatedAt >= yearStart)
                .SumAsync(o => o.TotalAmount);

            return new KpiTimeframeAmounts { Today = today, ThisMonth = thisMonth, ThisYear = thisYear };
        }

        public async Task<decimal> GetOrderCancellationRateAsync()
        {
            int totalOrders = await _context.Orders.AsNoTracking().CountAsync();
            if (totalOrders == 0) return 0;

            int cancelledOrders = await _context.Orders
                .AsNoTracking()
                .CountAsync(o => o.Status == OrderStatusEnum.CANCELLED);

            return (decimal)cancelledOrders / totalOrders * 100;
        }

        public async Task<decimal> GetOrderCompletionRateAsync()
        {
            int totalOrders = await _context.Orders.AsNoTracking().CountAsync();
            if (totalOrders == 0) return 0;

            int completedOrders = await _context.Orders
                .AsNoTracking()
                .CountAsync(o => o.Status == OrderStatusEnum.COMPLETED);

            return (decimal)completedOrders / totalOrders * 100;
        }

        public async Task<int> GetProcessingOrderCountAsync()
        {
            var processingStatuses = new[]
            {
                OrderStatusEnum.PENDING,
                OrderStatusEnum.IN_PROGRESS,
                OrderStatusEnum.DELIVERING
            };

            return await _context.Orders
                .AsNoTracking()
                .CountAsync(o => processingStatuses.Contains(o.Status));
        }

        public async Task<int> GetActiveShipperCountAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .CountAsync(u => u.Role == UserRoleEnum.SHIPPER && u.IsActive);
        }

        // ========== 1.2. Biểu đồ Doanh thu ==========

        public async Task<Dictionary<DateTime, decimal>> GetRevenueByDayAsync(int days)
        {
            var startDate = DateTime.UtcNow.Date.AddDays(-days + 1);

            var dailyRevenueList = await _context.Orders
                .AsNoTracking()
                .Where(o => o.Status == OrderStatusEnum.COMPLETED && o.CreatedAt >= startDate)
                // 1. Group by các thành phần của ngày (SQL dịch được)
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month, o.CreatedAt.Day })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .ThenBy(g => g.Key.Day)
                .Select(g => new
                {
                    // 2. Tái tạo lại ngày trong C#
                    Date = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
                    Revenue = g.Sum(o => o.TotalAmount)
                })
                .ToListAsync(); // 3. Lấy dữ liệu về List

            // 4. Chuyển sang Dictionary trong bộ nhớ
            return dailyRevenueList.ToDictionary(r => r.Date, r => r.Revenue);
        }

        // DashboardRepository.cs

        public async Task<Dictionary<string, decimal>> GetRevenueByMonthAsync(int months)
        {
            var startDate = DateTime.UtcNow.Date.AddMonths(-months + 1);
            startDate = new DateTime(startDate.Year, startDate.Month, 1);

            // 1. Chỉ lấy dữ liệu thô (Year, Month, Revenue) từ DB
            var monthlyRevenueList = await _context.Orders
                .AsNoTracking()
                .Where(o => o.Status == OrderStatusEnum.COMPLETED && o.CreatedAt >= startDate)
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                .Select(g => new
                {
                    g.Key.Year,  
                    g.Key.Month, 
                    Revenue = g.Sum(o => o.TotalAmount)
                })
                .ToListAsync();

            return monthlyRevenueList
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToDictionary(
                    x => $"{x.Year}-{x.Month:D2}",
                    x => x.Revenue
                );
        }

        // ========== 1.3. Biểu đồ Đơn hàng ==========

        public async Task<Dictionary<DateTime, int>> GetOrderCountByDayAsync(int days)
        {
            var startDate = DateTime.UtcNow.Date.AddDays(-days + 1);

            var dailyCounts = await _context.Orders
                .AsNoTracking()
                .Where(o => o.CreatedAt >= startDate)
                .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month, o.CreatedAt.Day }) // <-- SỬA
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .ThenBy(g => g.Key.Day)
                .Select(g => new
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
                    Count = g.Count()
                })
                .ToListAsync();

            return dailyCounts.ToDictionary(r => r.Date, r => r.Count);
        }

        public async Task<Dictionary<OrderStatusEnum, int>> GetOrderStatusDistributionAsync()
        {
            return await _context.Orders
                .AsNoTracking()
                .GroupBy(o => o.Status)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<int, int>> GetOrderCountByHourAsync(int lastDays)
        {
            var startDate = DateTime.UtcNow.AddDays(-lastDays);

            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.CreatedAt >= startDate)
                .GroupBy(o => o.CreatedAt.Hour)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        // ========== 1.4. Top Merchants ==========

        public async Task<IEnumerable<TopMerchantDto>> GetTopMerchantsByRevenueAsync(int count)
        {
            // 1. Tính toán trên bảng Orders TRƯỚC
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.Status == OrderStatusEnum.COMPLETED)
                .GroupBy(o => o.MerchantId)
                .Select(g => new
                {
                    MerchantId = g.Key,
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .Take(count)
                // 2. Join với bảng Merchants để lấy tên
                .Join(_context.Merchants.AsNoTracking(),
                      orderStats => orderStats.MerchantId,
                      merchant => merchant.MerchantId,
                      (orderStats, merchant) => new TopMerchantDto
                      {
                          MerchantId = merchant.MerchantId,
                          Name = merchant.Name,
                          Value = orderStats.TotalRevenue
                      })
                .ToListAsync();
        }

        public async Task<IEnumerable<TopMerchantDto>> GetTopMerchantsByOrderCountAsync(int count)
        {
            // 1. Tính toán trên bảng Orders TRƯỚC
            return await _context.Orders
                .AsNoTracking()
                .GroupBy(o => o.MerchantId)
                .Select(g => new
                {
                    MerchantId = g.Key,
                    TotalOrders = g.Count()
                })
                .OrderByDescending(x => x.TotalOrders)
                .Take(count)
                // 2. Join với bảng Merchants để lấy tên
                .Join(_context.Merchants.AsNoTracking(),
                      orderStats => orderStats.MerchantId,
                      merchant => merchant.MerchantId,
                      (orderStats, merchant) => new TopMerchantDto
                      {
                          MerchantId = merchant.MerchantId,
                          Name = merchant.Name,
                          Value = orderStats.TotalOrders
                      })
                .ToListAsync();
        }

        public async Task<IEnumerable<TopMerchantDto>> GetTopMerchantsByRatingAsync(int count)
        {
            return await _context.Reviews
                .AsNoTracking()
                .GroupBy(r => r.MerchantId)
                .Select(g => new
                {
                    MerchantId = g.Key,
                    AverageRating = g.Average(r => r.RatingProduct)
                })
                .OrderByDescending(x => x.AverageRating)
                .Take(count)
                .Join(_context.Merchants.AsNoTracking(),
                      reviewStats => reviewStats.MerchantId,
                      merchant => merchant.MerchantId,
                      (reviewStats, merchant) => new TopMerchantDto
                      {
                          MerchantId = merchant.MerchantId,
                          Name = merchant.Name,
                          Value = (decimal)reviewStats.AverageRating
                      })
                .ToListAsync();
        }

        // ========== 1.5. Top Menu Items ==========

        public async Task<IEnumerable<TopMenuItemDto>> GetTopMenuItemsBySalesAsync(int count)
        {
            return await _context.OrderItems
                .AsNoTracking()
                .Where(oi => oi.Order.Status == OrderStatusEnum.COMPLETED)
                .GroupBy(oi => oi.MenuItemId)
                .Select(g => new
                {
                    MenuItemId = g.Key,
                    TotalSales = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(x => x.TotalSales)
                .Take(count)
                .Join(_context.MenuItems.AsNoTracking(),
                      itemStats => itemStats.MenuItemId,
                      menuItem => menuItem.MenuItemId,
                      (itemStats, menuItem) => new TopMenuItemDto
                      {
                          MenuItemId = menuItem.MenuItemId,
                          Name = menuItem.Name,
                          Value = itemStats.TotalSales
                      })
                .ToListAsync();
        }

        public async Task<IEnumerable<TopMenuItemDto>> GetTopMenuItemsByRevenueAsync(int count)
        {
            return await _context.OrderItems
                .AsNoTracking()
                .Where(oi => oi.Order.Status == OrderStatusEnum.COMPLETED)
                .GroupBy(oi => oi.MenuItemId)
                .Select(g => new
                {
                    MenuItemId = g.Key,
                    TotalRevenue = g.Sum(oi => oi.Quantity * oi.UnitPrice)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .Take(count)
                .Join(_context.MenuItems.AsNoTracking(),
                      itemStats => itemStats.MenuItemId,
                      menuItem => menuItem.MenuItemId,
                      (itemStats, menuItem) => new TopMenuItemDto
                      {
                          MenuItemId = menuItem.MenuItemId,
                          Name = menuItem.Name,
                          Value = itemStats.TotalRevenue
                      })
                .ToListAsync();
        }

        // ========== 1.6. Thống kê Người dùng ==========

        public async Task<KpiTimeframeCounts> GetNewUserCountsAsync()
        {
            var now = DateTime.UtcNow;
            var todayStart = now.Date;
            var weekStart = now.Date.AddDays(-(int)now.DayOfWeek);
            var monthStart = new DateTime(now.Year, now.Month, 1);

            var users = _context.Users.AsNoTracking();

            int today = await users.CountAsync(u => u.CreatedAt >= todayStart);
            int thisWeek = await users.CountAsync(u => u.CreatedAt >= weekStart);
            int thisMonth = await users.CountAsync(u => u.CreatedAt >= monthStart);

            return new KpiTimeframeCounts { Today = today, ThisMonth = thisMonth, ThisYear = thisWeek };
        }

        public async Task<ActiveUserCounts> GetActiveUserCountsAsync()
        {
            var now = DateTime.UtcNow;
            var last24h = now.AddHours(-24);
            var last7d = now.AddDays(-7);
            var last30d = now.AddDays(-30);

            var users = _context.Users.AsNoTracking().Where(u => u.LastLoginAt.HasValue);

            int count24h = await users.CountAsync(u => u.LastLoginAt >= last24h);
            int count7d = await users.CountAsync(u => u.LastLoginAt >= last7d);
            int count30d = await users.CountAsync(u => u.LastLoginAt >= last30d);

            return new ActiveUserCounts { Last24h = count24h, Last7d = count7d, Last30d = count30d };
        }

        // ========== 1.7. Thống kê Thanh toán ==========

        public async Task<Dictionary<PaymentStatusEnum, int>> GetPaymentCountByStatusAsync()
        {
            return await _context.Payments
                .AsNoTracking()
                .GroupBy(p => p.Status)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<decimal> GetPaymentSuccessRateAsync()
        {
            int totalPayments = await _context.Payments.AsNoTracking().CountAsync();
            if (totalPayments == 0) return 0;

            int successfulPayments = await _context.Payments
                .AsNoTracking()
                .CountAsync(p => p.Status == PaymentStatusEnum.SUCCESS);

            return (decimal)successfulPayments / totalPayments * 100;
        }

        public async Task<decimal> GetTotalPaidRevenueAsync()
        {
            return await _context.Payments
                .AsNoTracking()
                .Where(p => p.Status == PaymentStatusEnum.SUCCESS)
                .SumAsync(p => p.Amount);
        }

        public async Task<Dictionary<string, int>> GetPaymentMethodDistributionAsync()
        {
            return await _context.Payments
                .AsNoTracking()
                .Include(p => p.PaymentMethod)
                .GroupBy(p => p.PaymentMethod.DisplayName)
                .ToDictionaryAsync(g => g.Key, g => g.Count());
        }

        public async Task<Dictionary<string, decimal>> GetRevenueByPaymentMethodAsync()
        {
            return await _context.Payments
                .AsNoTracking()
                .Where(p => p.Status == PaymentStatusEnum.SUCCESS)
                .Include(p => p.PaymentMethod)
                .GroupBy(p => p.PaymentMethod.DisplayName)
                .ToDictionaryAsync(g => g.Key, g => g.Sum(p => p.Amount));
        }

        public async Task<IEnumerable<Payment>> GetRecentPaymentsAsync(int count)
        {
            return await _context.Payments
                .AsNoTracking()
                .Include(p => p.Order)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        // ========== 1.8. Đơn hàng gần đây ==========

        public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.Merchant)
                .OrderByDescending(o => o.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}