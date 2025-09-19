using Dorfo.Domain.Entities;

namespace Dorfo.Application
{
    public class MenuItemOptionResponse
    {
        public Guid OptionId { get; set; }
        public Guid MenuItemId { get; set; }
        public string OptionName { get; set; } = null!;
        public bool IsMultipleChoice { get; set; }
        public bool Required { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<MenuItemOptionValueResponse> Values { get; set; } = new List<MenuItemOptionValueResponse>();
    }
}