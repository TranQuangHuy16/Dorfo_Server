using Dorfo.Domain.Entities;

namespace Dorfo.Application
{
    public class MenuItemOptionRequest
    {
        public string OptionName { get; set; } = null!;
        public bool IsMultipleChoice { get; set; }
        public bool Required { get; set; }
        public bool IsActive { get; set; }
        public ICollection<MenuItemOptionValueRequest> Values { get; set; } = new List<MenuItemOptionValueRequest>();
    }
}