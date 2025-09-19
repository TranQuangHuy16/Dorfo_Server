namespace Dorfo.Application
{
    public class MenuItemOptionValueResponse
    {
        public Guid OptionValueId { get; set; }
        public Guid OptionId { get; set; }
        public string? ValueName { get; set; }
        public decimal PriceDelta { get; set; }
        public bool IsActive { get; set; }
    }
}