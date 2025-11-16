using Dorfo.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dorfo.API.Controllers
{
    [ApiController]
    [Route("api/admin/dashboard")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;

        public DashboardController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        [HttpGet("kpi-cards")]
        public async Task<IActionResult> GetKpiCards()
        {
            var data = await _serviceProviders.DashboardService.GetKpiCardsDataAsync();
            return Ok(data);
        }

        [HttpGet("revenue-charts")]
        public async Task<IActionResult> GetRevenueCharts()
        {
            var data = await _serviceProviders.DashboardService.GetRevenueChartDataAsync();
            return Ok(data);
        }

        [HttpGet("order-charts")]
        public async Task<IActionResult> GetOrderCharts()
        {
            var data = await _serviceProviders.DashboardService.GetOrderChartDataAsync();
            return Ok(data);
        }

        [HttpGet("top-lists")]
        public async Task<IActionResult> GetTopLists()
        {
            var data = await _serviceProviders.DashboardService.GetTopListDataAsync();
            return Ok(data);
        }

        [HttpGet("user-stats")]
        public async Task<IActionResult> GetUserStats()
        {
            var data = await _serviceProviders.DashboardService.GetUserStatDataAsync();
            return Ok(data);
        }

        [HttpGet("payment-stats")]
        public async Task<IActionResult> GetPaymentStats()
        {
            var data = await _serviceProviders.DashboardService.GetPaymentStatDataAsync();
            return Ok(data);
        }

        [HttpGet("recent-orders")]
        public async Task<IActionResult> GetRecentOrders([FromQuery] int count = 20)
        {
            var data = await _serviceProviders.DashboardService.GetRecentOrdersAsync(count);
            return Ok(data);
        }
    }
}
