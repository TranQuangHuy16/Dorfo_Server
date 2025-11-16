using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class UserStatData
    {
        public KpiTimeframeCounts NewUserCounts { get; set; } = new();
        // Phân bổ user theo role đã có trong KpiCardsData.UserCountByRole
        public ActiveUserCounts ActiveUserCounts { get; set; } = new();
    }
}
