using Dorfo.Domain.Entities;

namespace Dorfo.Application
{
    public class MenuItemOptionValueRequest
    {
        public Guid? OptionId { get; set; } = null;
        public string? ValueName { get; set; }
        public decimal PriceDelta { get; set; }
        public bool IsActive { get; set; }
    }
}