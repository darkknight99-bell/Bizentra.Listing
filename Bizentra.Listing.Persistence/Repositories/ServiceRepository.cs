using Bizentra.Listing.Application.Persistence;
using Bizentra.Listing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bizentra.Listing.Persistence.Repositories
{
    public class ServiceRepository : BaseRepository<Service>, IServiceRepository
    {
      
        public ServiceRepository(BizentraListingDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Service> GetServiceWithPhotos(Guid id)
        {
            var servicePhotos = await _context.Services.Include(p => p.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            return servicePhotos;
        }

        public async Task<Service> DeleteServicePhoto(string photoId)
        {
            var service = await _context.Services
                    .Include(e => e.Images)
                    .FirstOrDefaultAsync(e => e.Images.Any(p => p.CloudId == photoId));

            var photoToDelete = service.Images.FirstOrDefault(p => p.CloudId == photoId);

            if (photoToDelete != null)
            {
                service.Images.Remove(photoToDelete);
            }

            await _context.SaveChangesAsync();

            return service;
        }
    }    
}
