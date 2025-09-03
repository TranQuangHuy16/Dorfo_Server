using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class LoyaltyAccount
    {
        public Guid LoyaltyAccountId { get; set; }
        public Guid UserId { get; set; }
        public long Points { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User User { get; set; } = null!;
    }
}
