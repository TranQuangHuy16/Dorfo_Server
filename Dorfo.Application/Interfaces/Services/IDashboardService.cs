using Dorfo.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        // 1.1. KPI Cards
        Task<KpiCardsData> GetKpiCardsDataAsync();

        // 1.2. Biểu đồ Doanh thu
        Task<RevenueChartData> GetRevenueChartDataAsync();

        // 1.3. Biểu đồ Đơn hàng
        Task<OrderChartData> GetOrderChartDataAsync();

        // 1.4 & 1.5. Top Lists
        Task<TopListData> GetTopListDataAsync();

        // 1.6. Thống kê Người dùng
        Task<UserStatData> GetUserStatDataAsync();

        // 1.7. Thống kê Thanh toán
        Task<PaymentStatData> GetPaymentStatDataAsync();

        // 1.8. Đơn hàng gần đây
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 20);
    }
}
