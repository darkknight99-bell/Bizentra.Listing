using Bizentra.Listing.Domain.Common;

namespace Bizentra.Listing.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Guid? ParentCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Category? ParentCategory { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Category>? ChildCategories { get; set; }
        public ICollection<Service>? Services { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
