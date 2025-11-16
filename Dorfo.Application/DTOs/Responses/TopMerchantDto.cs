using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class TopMerchantDto
    {
        public Guid MerchantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; } // Có thể là doanh thu, số đơn, hoặc rating
    }
}
