using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class OrderItemOption
    {
        public Guid OrderItemOptionId { get; set; }
        public Guid OrderItemId { get; set; }
        public Guid MenuItemOptionId { get; set; }
        public string OptionName { get; set; } = null!; // snapshot

        public OrderItem OrderItem { get; set; } = null!;
        public MenuItemOption MenuItemOption { get; set; } = null!;
        public ICollection<OrderItemOptionValue> OrderItemOptionValue { get; set; } = new List<OrderItemOptionValue>();
    }
}
