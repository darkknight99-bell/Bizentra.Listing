using Bizentra.Listing.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bizentra.Listing.Domain.Entities
{
    public class Service : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public string? Description { get; set; }
        public ICollection<Image> Images { get; set; }
        public string? Business { get; set; }
        public string? Condition { get; set; }
        public string? Location { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? OtherInformation { get; set; }
        public string? Status { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = default!;
        public bool IsDeleted { get; set; }
    }
}
