using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class FavoriteShop
    {
        public Guid FavoriteShopId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid MerchantId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public User Customer { get; set; }
        public Merchant Merchant { get; set; }
    }
}
