using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class MerchantResponse
    {
        public Guid MerchantId { get; set; }
        public Guid? OwnerUserId { get; set; }
        public int? MerchantCategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? ImgUrl { get; set; }
        public string? Description { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public decimal CommissionRate { get; set; }

        public MerchantAddressResponse Address { get; set; }
        public MerchantSettingResponse Setting { get; set; }
    }
}
