using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class RevenueChartData
    {
        // Dùng cho so sánh kỳ trước
        public class RevenueComparison
        {
            public decimal CurrentPeriod { get; set; }
            public decimal PreviousPeriod { get; set; }
            public decimal ChangePercentage { get; set; }
        }

        public RevenueComparison MonthlyRevenueComparison { get; set; } = new();
        public Dictionary<DateTime, decimal> RevenueByDay { get; set; } = new();
        public Dictionary<string, decimal> RevenueByMonth { get; set; } = new();
    }
}
