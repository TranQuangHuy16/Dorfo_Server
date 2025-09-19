namespace Dorfo.Application
{
    public class MenuCategoryRequest
    {
        public Guid MerchantId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public int SortOrder { get; set; }
    }
}