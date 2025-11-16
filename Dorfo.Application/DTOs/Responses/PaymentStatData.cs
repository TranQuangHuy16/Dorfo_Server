using Dorfo.Domain.Entities;
using Dorfo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class PaymentStatData
    {
        public Dictionary<PaymentStatusEnum, int> PaymentCountByStatus { get; set; } = new();
        public decimal PaymentSuccessRate { get; set; }
        public decimal TotalPaidRevenue { get; set; }
        public Dictionary<string, int> PaymentMethodDistribution { get; set; } = new();
        public Dictionary<string, decimal> RevenueByPaymentMethod { get; set; } = new();
        public IEnumerable<Payment> RecentPayments { get; set; } = new List<Payment>();
    }
}
