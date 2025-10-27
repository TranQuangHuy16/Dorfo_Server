using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class Review
    {
        public Guid ReviewId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid MerchantId { get; set; }
        public int RatingProduct { get; set; }
        public string Comment { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
        public User Customer { get; set; }
        public Merchant Merchant { get; set; }
        public ICollection<ReviewImage> Images { get; set; } = new List<ReviewImage>();

        public ShopReply? ShopReply { get; set; }
    }
}
