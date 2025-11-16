using Dorfo.Application.DTOs.Responses;
using Dorfo.Domain.Entities;
using Dorfo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        // 1.1. KPI Cards
        Task<Dictionary<UserRoleEnum, int>> GetUserCountByRoleAsync();
        Task<int> GetActiveMerchantCountAsync();
        Task<KpiTimeframeCounts> GetOrderCountsAsync();
        Task<KpiTimeframeAmounts> GetRevenueAsync();
        Task<decimal> GetOrderCancellationRateAsync();
        Task<decimal> GetOrderCompletionRateAsync();
        Task<int> GetProcessingOrderCountAsync();
        Task<int> GetActiveShipperCountAsync();

        // 1.2. Biểu đồ Doanh thu
        Task<Dictionary<DateTime, decimal>> GetRevenueByDayAsync(int days);
        Task<Dictionary<string, decimal>> GetRevenueByMonthAsync(int months);

        // 1.3. Biểu đồ Đơn hàng
        Task<Dictionary<DateTime, int>> GetOrderCountByDayAsync(int days);
        Task<Dictionary<OrderStatusEnum, int>> GetOrderStatusDistributionAsync();
        Task<Dictionary<int, int>> GetOrderCountByHourAsync(int lastDays);

        // 1.4. Top Merchants
        Task<IEnumerable<TopMerchantDto>> GetTopMerchantsByRevenueAsync(int count);
        Task<IEnumerable<TopMerchantDto>> GetTopMerchantsByOrderCountAsync(int count);
        Task<IEnumerable<TopMerchantDto>> GetTopMerchantsByRatingAsync(int count);

        // 1.5. Top Menu Items
        Task<IEnumerable<TopMenuItemDto>> GetTopMenuItemsBySalesAsync(int count);
        Task<IEnumerable<TopMenuItemDto>> GetTopMenuItemsByRevenueAsync(int count);

        // 1.6. Thống kê Người dùng
        Task<KpiTimeframeCounts> GetNewUserCountsAsync();
        Task<ActiveUserCounts> GetActiveUserCountsAsync();

        // 1.7. Thống kê Thanh toán
        Task<Dictionary<PaymentStatusEnum, int>> GetPaymentCountByStatusAsync();
        Task<decimal> GetPaymentSuccessRateAsync();
        Task<decimal> GetTotalPaidRevenueAsync();
        Task<Dictionary<string, int>> GetPaymentMethodDistributionAsync();
        Task<Dictionary<string, decimal>> GetRevenueByPaymentMethodAsync();
        Task<IEnumerable<Payment>> GetRecentPaymentsAsync(int count);

        // 1.8. Đơn hàng gần đây
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count);
    }
}
