using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Requests
{
    public class MerchantRequest
    {
        public Guid? OwnerUserId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Phone { get; set; }
        public string? ImgUrl { get; set; }
        public decimal CommissionRate { get; set; }
        public MerchantAddressRequest Address { get; set; }
        public MerchantSettingRequest Setting { get; set; }
    }
}
