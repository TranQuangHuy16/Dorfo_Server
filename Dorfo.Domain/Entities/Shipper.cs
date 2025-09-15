using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class Shipper
    {
        public Guid ShipperId { get; set; }
        public Guid MerchantId { get; set; }

        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? VehicleType { get; set; }
        public string? LicensePlate { get; set; }

        public string? CccdFrontUrl { get; set; }
        public string? CccdBackUrl { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsOnline { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Merchant Merchant { get; set; } = null!;
    }

}
