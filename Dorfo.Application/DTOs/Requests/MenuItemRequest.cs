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
        public string? ImgUrl { get; set; }
        public int? PrepTimeMinutes { get; set; }
        public bool SupportsScheduling { get; set; }
        public TimeSpan? AvailableFrom { get; set; }
        public TimeSpan? AvailableTo { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsSpecial { get; set; }

        public ICollection<MenuItemOptionReq> Options { get; set; } = new List<MenuItemOptionReq>();
    }

    public class MenuItemOptionReq
    {
        public string OptionName { get; set; } = null!;
        public bool IsMultipleChoice { get; set; }
        public bool Required { get; set; }
        public bool IsActive { get; set; }
        public ICollection<MenuItemOptionValueReq> Values { get; set; } = new List<MenuItemOptionValueReq>();
    }

    public class MenuItemOptionValueReq
    {
        public string? ValueName { get; set; }
        public decimal PriceDelta { get; set; }
        public bool IsActive { get; set; }
    }
}