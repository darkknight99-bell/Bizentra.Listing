using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bizentra.Listing.Persistence.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(BizentraListingDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Category>> GetParentCategory(Guid categoryId)
        {
            var parentCategories = await _context.Categories
                .Where(c => c.ParentCategoryId == null)
                .Include(c => c.ChildCategories)
                .ToListAsync();

            return parentCategories;
        }

        public async Task<List<Category>> GetSubCategories(Guid categoryId)
        {
            var subcategories = await _context.Categories
                .Where(c => c.ParentCategoryId == categoryId)
                .Include(c => c.ChildCategories)
                .ToListAsync();

            return subcategories;
        }
    }
}