using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Dorfo.Application.DTOs.Responses;
using Dorfo.Application.Interfaces.Repositories;
using Dorfo.Application.Interfaces.Services;


public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // 1.1. KPI Cards
    public async Task<KpiCardsData> GetKpiCardsDataAsync()
    {
        try
        {
            var userCount = await _unitOfWork.DashboardRepository.GetUserCountByRoleAsync();
            var merchantCount = await _unitOfWork.DashboardRepository.GetActiveMerchantCountAsync();
            var orderCount = await _unitOfWork.DashboardRepository.GetOrderCountsAsync();
            var revenue = await _unitOfWork.DashboardRepository.GetRevenueAsync();
            var cancelRate = await _unitOfWork.DashboardRepository.GetOrderCancellationRateAsync();
            var completeRate = await _unitOfWork.DashboardRepository.GetOrderCompletionRateAsync();
            var processingOrder = await _unitOfWork.DashboardRepository.GetProcessingOrderCountAsync();
            var shipperCount = await _unitOfWork.DashboardRepository.GetActiveShipperCountAsync();

            return new KpiCardsData
            {
                UserCountByRole = userCount,
                ActiveMerchantCount = merchantCount,
                OrderCounts = orderCount,
                Revenue = revenue,
                OrderCancellationRate = cancelRate,
                OrderCompletionRate = completeRate,
                ProcessingOrderCount = processingOrder,
                ActiveShipperCount = shipperCount
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    // 1.2. Biểu đồ Doanh thu
    public async Task<RevenueChartData> GetRevenueChartDataAsync()
    {
        try
        {
            var revenueData = await _unitOfWork.DashboardRepository.GetRevenueAsync();
            var revenueByDay = await _unitOfWork.DashboardRepository.GetRevenueByDayAsync(30);
            var revenueByMonthData = await _unitOfWork.DashboardRepository.GetRevenueByMonthAsync(12);

            var revenueComparison = CalculateMonthlyRevenueComparison(revenueData, revenueByMonthData);

            return new RevenueChartData
            {
                MonthlyRevenueComparison = revenueComparison,
                RevenueByDay = revenueByDay,
                RevenueByMonth = revenueByMonthData
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    // 1.3. Biểu đồ Đơn hàng
    public async Task<OrderChartData> GetOrderChartDataAsync()
    {
        try
        {
            var orderCountByDay = await _unitOfWork.DashboardRepository.GetOrderCountByDayAsync(30);
            var orderStatus = await _unitOfWork.DashboardRepository.GetOrderStatusDistributionAsync();
            var orderCountByHour = await _unitOfWork.DashboardRepository.GetOrderCountByHourAsync(7);

            return new OrderChartData
            {
                OrderCountByDay = orderCountByDay,
                OrderStatusDistribution = orderStatus,
                OrderCountByHour = orderCountByHour
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    // 1.4 & 1.5. Top Lists
    public async Task<TopListData> GetTopListDataAsync()
    {
        try
        {
            var topMerchRevenue = await _unitOfWork.DashboardRepository.GetTopMerchantsByRevenueAsync(10);
            var topMerchOrder = await _unitOfWork.DashboardRepository.GetTopMerchantsByOrderCountAsync(10);
            var topMerchRating = await _unitOfWork.DashboardRepository.GetTopMerchantsByRatingAsync(10);
            var topItemSales = await _unitOfWork.DashboardRepository.GetTopMenuItemsBySalesAsync(10);
            var topItemRevenue = await _unitOfWork.DashboardRepository.GetTopMenuItemsByRevenueAsync(10);

            return new TopListData
            {
                TopMerchantsByRevenue = topMerchRevenue,
                TopMerchantsByOrderCount = topMerchOrder,
                TopMerchantsByRating = topMerchRating,
                TopMenuItemsBySales = topItemSales,
                TopMenuItemsByRevenue = topItemRevenue
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    // 1.6. Thống kê Người dùng
    public async Task<UserStatData> GetUserStatDataAsync()
    {
        try
        {
            var newUserCount = await _unitOfWork.DashboardRepository.GetNewUserCountsAsync();
            var activeUser = await _unitOfWork.DashboardRepository.GetActiveUserCountsAsync();

            return new UserStatData
            {
                NewUserCounts = newUserCount,
                ActiveUserCounts = activeUser
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    // 1.7. Thống kê Thanh toán
    public async Task<PaymentStatData> GetPaymentStatDataAsync()
    {
        try
        {
            var paymentStatus = await _unitOfWork.DashboardRepository.GetPaymentCountByStatusAsync();
            var paymentSuccessRate = await _unitOfWork.DashboardRepository.GetPaymentSuccessRateAsync();
            var totalPaidRevenue = await _unitOfWork.DashboardRepository.GetTotalPaidRevenueAsync();
            var paymentMethodDist = await _unitOfWork.DashboardRepository.GetPaymentMethodDistributionAsync();
            var revenueByPayment = await _unitOfWork.DashboardRepository.GetRevenueByPaymentMethodAsync();
            var recentPayments = await _unitOfWork.DashboardRepository.GetRecentPaymentsAsync(10);

            return new PaymentStatData
            {
                PaymentCountByStatus = paymentStatus,
                PaymentSuccessRate = paymentSuccessRate,
                TotalPaidRevenue = totalPaidRevenue,
                PaymentMethodDistribution = paymentMethodDist,
                RevenueByPaymentMethod = revenueByPayment,
                RecentPayments = recentPayments
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    // 1.8. Đơn hàng gần đây
    public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count = 20)
    {
        try
        {
            return await _unitOfWork.DashboardRepository.GetRecentOrdersAsync(count);
        }
        catch (Exception)
        {
            throw;
        }
    }


    // === Private Helper Function ===
    private RevenueChartData.RevenueComparison CalculateMonthlyRevenueComparison(
        KpiTimeframeAmounts currentRevenue,
        Dictionary<string, decimal> monthlyRevenueHistory)
    {
        var now = DateTime.UtcNow;
        var lastMonthKey = new DateTime(now.Year, now.Month, 1).AddMonths(-1).ToString("yyyy-MM");

        monthlyRevenueHistory.TryGetValue(lastMonthKey, out var previousMonthRevenue);

        var currentMonthRevenue = currentRevenue.ThisMonth;

        decimal changePercentage = 0;
        if (previousMonthRevenue > 0)
        {
            changePercentage = ((currentMonthRevenue - previousMonthRevenue) / previousMonthRevenue) * 100;
        }
        else if (currentMonthRevenue > 0)
        {
            changePercentage = 100;
        }

        return new RevenueChartData.RevenueComparison
        {
            CurrentPeriod = currentMonthRevenue,
            PreviousPeriod = previousMonthRevenue,
            ChangePercentage = changePercentage
        };
    }
}