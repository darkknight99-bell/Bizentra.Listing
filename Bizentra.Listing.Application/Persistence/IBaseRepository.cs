using Bizentra.Listing.Application.Persistence.Helpers;
using Bizentra.Listing.Domain.Common;
using System.Linq.Expressions;

namespace Bizentra.Listing.Application.Persistence
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();

        Task<T> ReadSingleAsync(Guid Id, bool WithTracking = true, string include = null);

        Task<IEnumerable<T>> ReadByIdsAsync(Guid[] Id, bool WithTracking = false);

        Task<IEnumerable<T>> ReadAllAsync(bool WithTracking = false);

        Task<T> CreateAsync(T T);

        Task<bool> CreateMultipleAsync(IEnumerable<T> T);

        Task UpdateAsync(T T);

        Task UpdateApprovedAsync(string T);

        Task UpdateMultipleAsync(IEnumerable<T> T);

        Task<bool> DeleteAsync(Guid Id);

        Task<bool> DeleteApprovedAsync(string Id);

        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate, string[] ChildObjectNamesToInclude = null, bool WithTracking = false);

        Task<T> SingleOrDefault(Expression<Func<T, bool>> predicate, string[] ChildObjectNamesToInclude = null, bool WithTracking = false);

        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate, string[] ChildObjectNamesToInclude = null, bool WithTracking = false);

        IQueryable<T> GetWhereQueryable(Expression<Func<T, bool>> predicate, string[] ChildObjectNamesToInclude = null, bool WithTracking = false);
        Task<decimal?> Sum(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal?>> sumpredicate);

        Task<Paginated<T>> GetWherePaginated(PaginatedQuery<T> query);

        Task<long> CountAll();

        Task<long> CountWhere(Expression<Func<T, bool>> predicate);
    }
}
