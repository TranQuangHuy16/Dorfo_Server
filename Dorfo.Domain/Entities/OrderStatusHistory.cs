using Dorfo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class OrderStatusHistory
    {
        public Guid HistoryId { get; set; }
        public Guid OrderId { get; set; }
        public int? FromStatusId { get; set; }
        public int ToStatusId { get; set; }
        public Guid? ChangedByUserId { get; set; }
        public string? Note { get; set; }
        public DateTime ChangedAt { get; set; }

        public Order Order { get; set; } = null!;
        public OrderStatusEnum FromStatus { get; set; }
        public OrderStatusEnum? ToStatus { get; set; } = null!;
        public User? ChangedByUser { get; set; }
    }
}
