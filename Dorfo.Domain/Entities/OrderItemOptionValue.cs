using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class OrderItemOptionValue
    {
        public Guid OrderItemOptionValueId { get; set; }
        public Guid OrderItemOptionId { get; set; }
        public Guid MenuItemOptionValueId { get; set; }
        public string ValueName { get; set; } = null!; // snapshot
        public decimal PriceDelta { get; set; }

        public OrderItemOption OrderItemOption { get; set; } = null!;
        public MenuItemOptionValue MenuItemOptionValue { get; set; } = null!;
    }
}
