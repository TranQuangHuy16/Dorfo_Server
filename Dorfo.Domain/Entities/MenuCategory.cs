using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class MenuCategory
    {
        public Guid MenuCategoryId { get; set; }
        public Guid MerchantId { get; set; }
        public string Name { get; set; } = null!;
        public int SortOrder { get; set; }
        public Merchant Merchant { get; set; } = null!;
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
