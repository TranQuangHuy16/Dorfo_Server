using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid MenuItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? OptionsJson { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime? ScheduledFor { get; set; }

        public Order Order { get; set; } = null!;
        public MenuItem MenuItem { get; set; } = null!;
        public ICollection<OrderItemOption> OrderItemOptions { get; set; } = new List<OrderItemOption>();
    }
}
