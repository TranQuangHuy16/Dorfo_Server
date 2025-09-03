using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class MerchantAddress
    {
        public Guid MerchantAddressId { get; set; }
        public Guid MerchantId { get; set; }
        public string? Address { get; set; }
        public string? Building { get; set; }
        public decimal? GeoLat { get; set; }
        public decimal? GeoLng { get; set; }

        public Merchant Merchant { get; set; } = null!;
    }
}
