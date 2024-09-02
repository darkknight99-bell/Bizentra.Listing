using Bizentra.Listing.Domain.Entities;

namespace Bizentra.Listing.Application.Persistence
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<List<Category>> GetSubCategories(Guid categoryId);
    }
}
