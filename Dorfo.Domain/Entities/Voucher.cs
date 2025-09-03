using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class Voucher
    {
        public Guid VoucherId { get; set; }
        public string Code { get; set; } = null!;
        public Guid? MerchantId { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercent { get; set; }
        public decimal? MinOrderAmount { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public int? UsageLimit { get; set; }
        public DateTime CreatedAt { get; set; }

        public Merchant? Merchant { get; set; }
    }
}
