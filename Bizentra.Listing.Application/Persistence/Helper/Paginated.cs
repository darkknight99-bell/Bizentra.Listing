using System.Linq.Expressions;

namespace Bizentra.Listing.Application.Persistence.Helpers
{
    public class Paginated<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; }
        public int TotalPages => (int)Math.Ceiling(this.TotalCount / (double)this.PageSize);
    }

    public class PaginatedQuery<T>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string OrderColumn { get; set; }
        public Order Order { get; set; } = Order.Ascending;

        public Expression<Func<T, bool>> predicate;
        public string[] ChildObjectNamesToInclude { get; set; }
    }

    public enum Order
    {
        Ascending = 1, Descending
    }


}
