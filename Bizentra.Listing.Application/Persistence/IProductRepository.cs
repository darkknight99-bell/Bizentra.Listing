using Bizentra.Listing.Domain.Entities;

namespace Bizentra.Listing.Application.Persistence
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product> GetProductWithPhotos(Guid id);
        Task<Product> DeleteProductPhoto(string photoId);
    }
}
