using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class MerchantSetting
    {
        public Guid MerchantSettingId { get; set; }
        public Guid MerchantId { get; set; }
        public bool SupportsScheduling { get; set; }
        public decimal? FreeShipThreshold { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public int PrepWindowMinutes { get; set; }
        public int DeliveryRadiusMeters { get; set; }
        public DateTime CreatedAt { get; set; }

        public Merchant Merchant { get; set; } = null!;
    }
}
