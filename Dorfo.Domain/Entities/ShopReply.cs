using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class ShopReply
    {
        public Guid ShopReplyId { get; set; }
        public Guid ReviewId { get; set; }
        public Guid MerchantId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime RepliedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public Review Review { get; set; }
        public Merchant Merchant { get; set; }
    }
}
