using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class MerchantCategory
    {
        public int MerchantCategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Merchant> Merchants { get; set; } = new List<Merchant>();
    }

}
