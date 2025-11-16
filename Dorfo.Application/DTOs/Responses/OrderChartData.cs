using Dorfo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class OrderChartData
    {
        public Dictionary<DateTime, int> OrderCountByDay { get; set; } = new();
        public Dictionary<OrderStatusEnum, int> OrderStatusDistribution { get; set; } = new();
        public Dictionary<int, int> OrderCountByHour { get; set; } = new();
    }
}
