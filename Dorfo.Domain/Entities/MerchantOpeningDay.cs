using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class MerchantOpeningDay
    {
        public Guid MerchantOpeningDayId { get; set; }
        public Guid MerchantId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }

        public Merchant Merchant { get; set; } = null!;
    }
}
