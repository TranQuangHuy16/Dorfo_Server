using Dorfo.Domain.Entities;

namespace Dorfo.Application
{
    public class MenuItemOptionValueRequest
    {
        public string? ValueName { get; set; }
        public decimal PriceDelta { get; set; }
        public bool IsActive { get; set; }
    }
}