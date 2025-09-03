using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class MenuItemOption
    {
        public Guid OptionId { get; set; }
        public Guid MenuItemId { get; set; }
        public string OptionName { get; set; } = null!;
        public bool IsMultipleChoice { get; set; }
        public bool Required { get; set; }

        public MenuItem MenuItem { get; set; } = null!;
        public ICollection<MenuItemOptionValue> Values { get; set; } = new List<MenuItemOptionValue>();
    }
}
