using Dorfo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class KpiCardsData
    {
        public Dictionary<UserRoleEnum, int> UserCountByRole { get; set; } = new();
        public int ActiveMerchantCount { get; set; }
        public KpiTimeframeCounts OrderCounts { get; set; } = new();
        public KpiTimeframeAmounts Revenue { get; set; } = new();
        public decimal OrderCancellationRate { get; set; }
        public decimal OrderCompletionRate { get; set; }
        public int ProcessingOrderCount { get; set; }
        public int ActiveShipperCount { get; set; }
    }
}
