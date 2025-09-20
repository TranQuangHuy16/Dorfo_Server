using Dorfo.Domain.Entities;

namespace Dorfo.Application
{
    public class MenuCategoryResponse
    {
        public Guid MenuCategoryId { get; set; }
        public Guid MerchantId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int SortOrder { get; set; }

        public IEnumerable<MenuItemResponse> MenuItems { get; set; } = new List<MenuItemResponse>();
    }
}