using Dorfo.Domain.Entities;

namespace Dorfo.Application
{
    public class MenuItemRequest
    {
        public Guid MerchantId { get; set; }
        public Guid? CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? PrepTimeMinutes { get; set; }
        public bool SupportsScheduling { get; set; }
        public TimeSpan? AvailableFrom { get; set; }
        public TimeSpan? AvailableTo { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsSpecial { get; set; }

        public ICollection<MenuItemOptionRequest> Options { get; set; } = new List<MenuItemOptionRequest>();
    }
}