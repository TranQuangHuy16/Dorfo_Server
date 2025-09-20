using Dorfo.Domain.Entities;

namespace Dorfo.Application
{
    public class MenuItemResponse
    {
        public Guid MenuItemId { get; set; }
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
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<MenuItemOptionResponse> Options { get; set; } = new List<MenuItemOptionResponse>();
    }
}