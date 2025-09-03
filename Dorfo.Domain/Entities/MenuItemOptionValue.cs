using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class MenuItemOptionValue
    {
        public Guid OptionValueId { get; set; }
        public Guid OptionId { get; set; }
        public string? ValueName { get; set; }
        public decimal PriceDelta { get; set; }

        public MenuItemOption Option { get; set; } = null!;
    }
}
