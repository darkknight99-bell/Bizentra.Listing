using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bizentra.Listing.Persistence.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(BizentraListingDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Product> GetProductWithPhotos(Guid id)
        {
            var productPhotos = await _context.Products.Include(p => p.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            return productPhotos;
        }

        public async Task<Product> DeleteProductPhoto(string photoId)
        {
            var product = await _context.Products
                 .Include(e => e.Images)
                 .FirstOrDefaultAsync(e => e.Images.Any(p => p.CloudId == photoId));

            var photoToDelete = product.Images.FirstOrDefault(p => p.CloudId == photoId);

            if (photoToDelete != null)
            {
                product.Images.Remove(photoToDelete);
            }

            await _context.SaveChangesAsync();

            return product;
        }
    }
}
