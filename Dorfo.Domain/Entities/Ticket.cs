using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class Ticket
    {
        public Guid TicketId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid UserId { get; set; }
        public Guid? MerchantId { get; set; }
        public string IssueType { get; set; } = null!;
        public string? Description { get; set; }
        public string Status { get; set; } = "NEW";
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }

        public Order? Order { get; set; }
        public User User { get; set; } = null!;
    }
}
