using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public Guid? UserId { get; set; }
        public string? AddressLabel { get; set; }
        public string? Building { get; set; }
        public string? Floor { get; set; }
        public decimal? GeoLat { get; set; }  // DECIMAL(9,6)
        public decimal? GeoLng { get; set; }  // DECIMAL(9,6)
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }

        public User? User { get; set; }
    }
}
