using Bizentra.Listing.Domain.Common;

namespace Bizentra.Listing.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public string? Description { get; set; }
        public ICollection<Image> Images { get; set; }
        public string? Business { get; set; }
        public string? Location { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? OtherInformation { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = default!;
        public bool IsDeleted { get; set; }
    }
}
