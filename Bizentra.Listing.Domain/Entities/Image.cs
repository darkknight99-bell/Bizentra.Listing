using Bizentra.Listing.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bizentra.Listing.Domain.Entities
{
    public class Image : BaseEntity
    {
        public string CloudId { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public Guid ProductId { get; set; }
        public DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
    }
}
