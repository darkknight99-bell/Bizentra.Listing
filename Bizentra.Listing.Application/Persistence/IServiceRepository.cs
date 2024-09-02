using Bizentra.Listing.Domain.Entities;

namespace Bizentra.Listing.Application.Persistence
{
    public interface IServiceRepository : IBaseRepository<Service>
    {
        Task<Service> GetServiceWithPhotos(Guid id);
        Task<Service> DeleteServicePhoto(string photoId);
    }
}
