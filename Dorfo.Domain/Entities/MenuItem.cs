using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class MenuItem
    {
        public Guid MenuItemId { get; set; }
        public Guid MerchantId { get; set; }
        public Guid? CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImgUrl { get; set; }
        public int? PrepTimeMinutes { get; set; }
        public bool SupportsScheduling { get; set; }
        public TimeSpan? AvailableFrom { get; set; }
        public TimeSpan? AvailableTo { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsSpecial { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public Merchant Merchant { get; set; } = null!;
        public MenuCategory? Category { get; set; }
        public ICollection<MenuItemOption> Options { get; set; } = new List<MenuItemOption>();
    }
}
